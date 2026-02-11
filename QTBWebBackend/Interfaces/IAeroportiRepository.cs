using QTBWebBackend.Models;
using QTBWebBackend.ViewModels;

namespace QTBWebBackend.Interfaces
{
    public interface IAeroportiRepository
    {
        Task<List<AeroportoViewModel>> GetAeroportiAsync();
        Task<AeroportoViewModel?> GetAeroportoByIdAsync(long idAeroporto);

        Task<List<TipoAeroportoViewModel>> GetTipiAeroportiAsync();
        Task<TipoAeroportoViewModel?> GetTipoAeroportoByIdAsync(long idTipoAeroporto);

        Task<Aeroporti?> AddAeroportoAsync(AeroportoViewModel aeroporto);
    }
}
