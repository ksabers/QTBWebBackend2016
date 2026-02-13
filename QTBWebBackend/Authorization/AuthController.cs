using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTBWebBackend.Authorization;
using QTBWebBackend.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QTBWebBackend.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        /// <summary>
        /// Autentica un utente e restituisce JWT token + stato volo pilota
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RichiestaLogin richiesta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(richiesta.Username) || string.IsNullOrWhiteSpace(richiesta.Password))
                return BadRequest(new { message = "Email e password obbligatori" });

            var risposta = await authService.LoginAsync(richiesta);

            if (risposta == null)
                return Unauthorized(new { message = "Credenziali non valide" });


            return Ok(risposta);
        }

        public class GenerateHashRequest
        {
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("generate-hash")]
        [AllowAnonymous]
        public IActionResult GenerateHash([FromBody] GenerateHashRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Password required");

            byte[] salt = new byte[32];
            RandomNumberGenerator.Fill(salt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(request.Password),
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32
            );

            string saltB64 = Convert.ToBase64String(salt);
            string hashB64 = Convert.ToBase64String(hash);
            string dbHash = $"{saltB64}.{hashB64}";

            Console.WriteLine($"GENERATED: {dbHash}");

            return Ok(new
            {
                password = request.Password,
                salt = saltB64,
                hash = hashB64,
                dbHash  // ← QUESTO va nel DB!
            });
        }

        /*        /// <summary>
                /// Informazioni utente loggato + stato volo in corso
                /// </summary>
                [HttpGet("me")]
                [Authorize]
                public async Task<IActionResult> GetCurrentUser()
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (!int.TryParse(userIdClaim, out var userId))
                        return Unauthorized();

                    var userInfo = await authService.GetUserInfoAsync(userId);
                    if (userInfo == null)
                        return Unauthorized();

                    // ⭐ Controllo volo in corso
                    var voloInCorso = await authService.HasVoloInCorsoAsync(userInfo.PersonaId);
                    userInfo.VoloInCorso = voloInCorso;

                    return Ok(userInfo);
                }

                /// <summary>
                /// Logout (client-side)
                /// </summary>
                [HttpPost("logout")]
                [Authorize]
                public IActionResult Logout()
                {
                    // JWT stateless: client cancella solo il token
                    return Ok(new { message = "Logout effettuato" });
                }*/
    }
}
