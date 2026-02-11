using QTBWebBackend.Models;
using QTBWebBackend.ViewModels;

namespace QTBWebBackend.Interfaces
{
    public interface IAeroportiService
    {
        Task<List<AeroportoViewModel>> GetAeroportiAsync();
        Task<AeroportoViewModel?> GetAeroportoByIdAsync(long idAeroporto);

        Task<List<TipoAeroportoViewModel>> GetTipiAeroportiAsync();
        Task<TipoAeroportoViewModel?> GetTipoAeroportoByIdAsync(long idTipoAeroporto);

        Task<Aeroporti?> CreateAeroportoAsync(AeroportoViewModel aeroporto);
    }
}