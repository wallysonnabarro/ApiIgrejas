using Domain.Dominio;

namespace Service.Interface
{
    public interface IAuthenticationServices
    {
        public Task<Identidade> VerifyPassword(string senha, string hash, string salt);
    }
}
