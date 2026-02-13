using QTBWebBackend.ViewModels;

namespace QTBWebBackend.Authorization
{
    public class InfoUtente
    {
        public long LoginId { get; set; }                 // Id della tabella Login
        public long PersonaId { get; set; }          // Id persona anagrafica (comodo per API future)
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cognome { get; set; } = string.Empty;
        public long? VoloInCorsoId { get; set; }

        // ⭐ Lista di ruoli, non più una stringa singola
        public List<RuoloViewModel> Ruoli { get; set; } = [];

    }
}