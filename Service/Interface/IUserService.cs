using Domain.Dominio;
using Domain.DTOs;

namespace Service.Interface
{
    public interface IUserService
    {
        Task<byte[]> GenerateSalt();
        Task<byte[]> GeneratePasswordHash(string password, byte[] salt);
        Task<Identidade> ValidarLogin(string username, string password);
    }
}
