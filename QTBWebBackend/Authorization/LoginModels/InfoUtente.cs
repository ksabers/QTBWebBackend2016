namespace QTBWebBackend.Authorization.LoginModels
{
    public class InfoUtente
    {
        public int LoginId { get; set; }                 // Id della tabella Login
        public int PersonaId { get; set; }          // Id persona anagrafica (comodo per API future)
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;

        // ⭐ Lista di ruoli, non più una stringa singola
        public List<string> Ruoli { get; set; } = new();

        public List<int> AereiPosseduti { get; set; } = new();
    }
}