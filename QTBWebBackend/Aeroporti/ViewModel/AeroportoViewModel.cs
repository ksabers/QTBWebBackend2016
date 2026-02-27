namespace QTBWebBackend.Aeroporti
{
    public class AeroportoViewModel
    {
        public long Id { get; set; }
        public string? Nome { get; set; }
        public string? Denominazione { get; set; }
        public long IdTipoAeroporto { get; set; }
        public string? TipoAeroporto { get; set; }
        public string? Identificativo { get; set; }
        public string? Coordinate { get; set; }
        public string? Icao { get; set; }
        public string? Iata { get; set; }
        public int? QNH { get; set; }
        public string? QFU { get; set; }
        public bool Asfalto { get; set; }
        public int? Lunghezza { get; set; }
        public string? Radio { get; set; }
        public string? Indirizzo { get; set; }
        public string? CAP { get; set; }
        public string? Citta { get; set; } 
        public string? Provincia { get; set; }
        public string? Nazione { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Web { get; set; }
        public string? Note { get; set; }

    }
}
