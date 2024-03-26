using Domain.Dominio;

namespace Infra.Data.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<Token> AuthenticateUser(string email, string senha);
    }
}
