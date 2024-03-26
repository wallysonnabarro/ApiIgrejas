using Domain.Dominio;
using Service.Interface;
using System.Security.Cryptography;

namespace Service.Services
{
    public class UserService : IUserService
    {
        public async Task<Identidade> ValidarLogin(string username, string password)
        {
            if (password.Length < 10)
            {
                return Identidade.Failed(new IdentidadeError() { Code = "205", Description = "A senha ser maior que 10" });
            }
            else if (!password.Any(char.IsPunctuation))
            {
                return Identidade.Failed(new IdentidadeError() { Code = "205", Description = "A senha não contem um caracter especial" });
            }
            else if (username.Equals(""))
            {
                return Identidade.Failed(new IdentidadeError() { Code = "205", Description = "O usuário não foi informado" });
            }
            else
            {
                return Identidade.Success;
            }
        }

        public async Task<byte[]> GeneratePasswordHash(string password, byte[] salt)
        {
            return await Task.Run(() =>
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Settings.ITERATIONS, HashAlgorithmName.SHA256))
                {
                    return pbkdf2.GetBytes(Settings.BASE64);
                }
            });
        }

        public async Task<byte[]> GenerateSalt()
        {
            return await Task.Run(() =>
            {
                byte[] salt = new byte[Settings.SALTVALUE];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                return salt;
            });
        }
    }
}
