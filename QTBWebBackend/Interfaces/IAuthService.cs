
using QTBWebBackend.Authorization;
using QTBWebBackend.Authorization.LoginModels;

namespace QTBWebBackend.Interfaces
{
    public interface IAuthService
    {
        Task<RispostaLogin?> LoginAsync(RichiestaLogin request);
    }
}
