namespace QTBWebBackend.Authorization
{
    public interface IAuthService
    {
        Task<RispostaLogin?> LoginAsync(RichiestaLogin request);
        Task<long?> GetVoloInCorsoAsync(long personaId);
    }
}
