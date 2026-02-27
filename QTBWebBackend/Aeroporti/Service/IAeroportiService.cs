using QTBWebBackend.Models;

namespace QTBWebBackend.Aeroporti
{
    public interface IAeroportiService
    {
        Task<List<AeroportoViewModel>> GetAeroportiAsync();
        Task<AeroportoViewModel?> GetAeroportoByIdAsync(long idAeroporto);

        Task<List<TipoAeroportoViewModel>> GetTipiAeroportiAsync();
        Task<TipoAeroportoViewModel?> GetTipoAeroportoByIdAsync(long idTipoAeroporto);

        Task<Aeroporto?> CreateAeroportoAsync(AeroportoViewModel aeroporto);
    }
}