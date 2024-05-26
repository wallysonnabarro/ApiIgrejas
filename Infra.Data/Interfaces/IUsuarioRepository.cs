using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<Identidade> AddUserWithSecurePassword(UsuarioDto user);
        public Task<SigniInUsuarioDto> GetUserByEmail(string email);
        public Task<ResultDynamic> GetUserByCpfCnpj(string cpfCnpj);
        public Task<ResultDynamic> GetUserByNome(string nome);
        public void AddRoleToUser(decimal userId, Role role);

        public Task<Identidade> UpdateAcessLock(int id);
        Task<Result<Paginacao<UsuarioListDto>>> Paginacao(PageWrapper wrapper, string email);
        Task<Result<Paginacao<UsuarioListaPaginadaDto>>> PaginacaoLista(PageWrapper wrapper, string email);
        Task<Result<bool>> Novo(NovoUsuarioDto dto, string email);
        Task<Result<bool>> RedefinirSenha(LoginDTO dto);
        Task<Result<UsuarioDetalharDto>> UserDetalhe(int id);
    }
}
