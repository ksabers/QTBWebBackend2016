using QTBWebBackend.Models;

namespace QTBWebBackend.Authorization
{
    public class RispostaLogin
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Scadenza { get; set; }
        public Persone Utente { get; set; } = null!;
    }
}
