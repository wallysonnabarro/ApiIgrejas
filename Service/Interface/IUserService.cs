using Domain.Dominio;

namespace Service.Interface
{
    public interface IUserService
    {
        Task<byte[]> GenerateSalt();
        Task<byte[]> GeneratePasswordHash(string password, byte[] salt);

        Task<Identidade> ValidarLogin(string username, string password);
    }
}
