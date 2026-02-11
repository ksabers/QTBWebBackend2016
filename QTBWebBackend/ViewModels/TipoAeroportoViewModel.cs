using System.ComponentModel.DataAnnotations;

namespace QTBWebBackend.ViewModels
{
    public class TipoAeroportoViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required] 
        public string? Descrizione { get; set; }

    }
}
