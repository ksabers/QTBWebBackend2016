using QTBWebBackend.Models;

namespace QTBWebBackend.Aeroporti
{
    public interface IAeroportiRepository
    {
        Task<List<AeroportoViewModel>> GetAeroportiAsync();
        Task<AeroportoViewModel?> GetAeroportoByIdAsync(long idAeroporto);

        Task<List<TipoAeroportoViewModel>> GetTipiAeroportiAsync();
        Task<TipoAeroportoViewModel?> GetTipoAeroportoByIdAsync(long idTipoAeroporto);

        Task<Aeroporto?> AddAeroportoAsync(AeroportoViewModel aeroporto);
    }
}
