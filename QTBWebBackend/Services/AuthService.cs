using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QTBWebBackend.Authorization;
using QTBWebBackend.Interfaces;
using QTBWebBackend.Models;
using QTBWebBackend.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QTBWebBackend.Services
{
    public class AuthService(QTBWebDBContext contesto, IConfiguration configurazione) : IAuthService
    {
        public async Task<RispostaLogin?> LoginAsync(RichiestaLogin richiesta)
        {
            // 1. Cerca l'utente nel database
            var login = await contesto.Login
                .Include(l => l.Persona)
                .Include(l => l.Ruolo)
                .FirstOrDefaultAsync(l => l.Username == richiesta.Username);

            if (login == null || !login.Attivo)
            {
                return null; // Utente non trovato o disabilitato
            }

            // 2. Verifica la password (hashed)
            if (!VerificaPassword(richiesta.Password, login.PasswordHash))
            {
                return null; // Password errata
            }

            // 3. Aggiorna ultimo login
            login.UltimoLogin = DateTime.UtcNow;
            await contesto.SaveChangesAsync();

            // 4. Genera il JWT token
            var (Token, ExpiresAt) = GeneraTokenJWT(login);

            return new RispostaLogin
            {
                Token = Token,
                Scadenza = ExpiresAt,
                Utente = new InfoUtente
                {
                    LoginId = login.Id,
                    PersonaId = login.PersonaId,
                    Username = login.Username,
                    Email = login.Persona.Email,
                    Nome = login.Persona.Nome,
                    Cognome = login.Persona.Cognome,
                    VoloInCorsoId = await GetVoloInCorsoAsync(login.PersonaId),
                    Ruoli = [.. login.Ruolo.Select(r => new RuoloViewModel { Id = r.Id, Descrizione = r.Descrizione })]
                }
            };
        }

        public async Task<long?> GetVoloInCorsoAsync(long personaId)
        {
            return await contesto.Voli
                .Where(v => v.PilotaId == personaId && v.OraAtterraggio == null)
                .Select(v => v.Id)
                .FirstOrDefaultAsync();
        }
        private static bool VerificaPassword(string password, string hashedPassword)
        {
            try
            {
                var parts = hashedPassword.Split('.');
                if (parts.Length != 2) return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHash = Convert.FromBase64String(parts[1]);

                byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    iterations: 100000,
                    HashAlgorithmName.SHA256,
                    256 / 8
                );

                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }
            catch
            {
                return false;
            }
        }

        private (string Token, DateTime ExpiresAt) GeneraTokenJWT(Login login)
        {
            var segreto = configurazione["AppSettings:Secret"]
                ?? throw new InvalidOperationException("JWT Secret non configurato");

            var chiave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(segreto));
            var credenziali = new SigningCredentials(chiave, SecurityAlgorithms.HmacSha256);

            var scadenza = DateTime.UtcNow.AddHours(8); // Token valido 8 ore

            // ⭐ CLAIMS - Informazioni nell'JWT
            var claims = new List<Claim> {
                new("LoginId", login.Id.ToString()),
                new("PersonaId", login.PersonaId.ToString()),
                new("Email", login.Persona.Email ?? ""),
                new("Nome", login.Persona.Nome ?? ""),
                new("Cognome", login.Persona.Cognome ?? ""),
                new("Username", login.Username),      
                new("Pilota", login.Persona.Pilota.ToString() ?? "false")
            };
            foreach (var ruolo in login.Ruolo)
            {
                claims.Add(new Claim("RuoloID", ruolo.Id.ToString()));
                claims.Add(new Claim("Ruolo", ruolo.Descrizione));
            }

            var token = new JwtSecurityToken(
                issuer: configurazione["AppSettings:Issuer"] ?? "QTBWebAPI",
                audience: configurazione["AppSettings:Audience"] ?? "QTBWebClient",
                claims: claims,
                expires: scadenza,
                signingCredentials: credenziali
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, scadenza);
        }
    }
}
