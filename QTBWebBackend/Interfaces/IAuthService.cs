
using QTBWebBackend.Authorization;

namespace QTBWebBackend.Interfaces
{
    public interface IAuthService
    {
        Task<RispostaLogin?> LoginAsync(RichiestaLogin request);
        Task<long?> GetVoloInCorsoAsync(long personaId);
    }
}
