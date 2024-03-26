using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<Identidade> AddUserWithSecurePassword(UsuarioDto user);
        public Task<ResultDynamic> GetUserByEmail(string email);
        public Task<ResultDynamic> GetUserByCpfCnpj(string cpfCnpj);
        public Task<ResultDynamic> GetUserByNome(string nome);
        public void AddRoleToUser(decimal userId, Role role);

        public Task<Identidade> UpdateAcessLock(int id);
    }
}
