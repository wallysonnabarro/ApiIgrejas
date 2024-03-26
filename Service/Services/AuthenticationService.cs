using Domain.Dominio;
using Service.Interface;
using System.Security.Cryptography;

namespace Service.Services
{
    public class AuthenticationService : IAuthenticationServices
    {
        public async Task<Identidade> VerifyPassword(string senha, string hash, string salt)
        {
            return await Task.Run(() =>
            {
                byte[] saltBytes = Convert.FromBase64String(salt);

                using Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(senha, saltBytes, Settings.ITERATIONS, HashAlgorithmName.SHA256);

                byte[] computedHash = pbkdf2.GetBytes(Settings.BASE64);

                string computedHashBase64 = Convert.ToBase64String(computedHash);

                if (hash.Equals(computedHashBase64)) return Identidade.Success;
                else return Identidade.Failed(new IdentidadeError { Code = "403", Description = "Token Invalid" });
            });
        }
    }
}