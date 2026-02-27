using QTBWebBackend.Models;

namespace QTBWebBackend.Aeroporti
{
    public class AeroportiService(IAeroportiRepository repository) : IAeroportiService
    {
        public Task<List<AeroportoViewModel>> GetAeroportiAsync()
            => repository.GetAeroportiAsync();

        public Task<AeroportoViewModel?> GetAeroportoByIdAsync(long idAeroporto)
            => repository.GetAeroportoByIdAsync(idAeroporto);

        public Task<List<TipoAeroportoViewModel>> GetTipiAeroportiAsync()
            => repository.GetTipiAeroportiAsync();

        public Task<TipoAeroportoViewModel?> GetTipoAeroportoByIdAsync(long idTipoAeroporto)
            => repository.GetTipoAeroportoByIdAsync(idTipoAeroporto);

        public Task<Aeroporto?> CreateAeroportoAsync(AeroportoViewModel aeroporto)
            => repository.AddAeroportoAsync(aeroporto);
    }
}