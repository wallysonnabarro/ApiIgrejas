using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<Identidade> Delete(int id);
        Task<Role> Get(int id);
        Task<Result<PerfilListaPaginadaDto>> Get(string name, string email);
        Task<Result<int>> Insert(PerfilDto roler, string email);
        Task<Result<int>> Insert(PerfilNovoDto roler, string email);
        Task<Result<int>> Insert(int tipo, string email);
        Task<List<Role>> List();
        Task<Result<bool>> Update(UpdatePerfilDto roler);
        Task<Result<bool>> Update(PerfilAtualizarDto roler, string email);
        Task<bool> IsValid(string v);
        Task<Result<Paginacao<PerfilListaPaginadaDto>>> Paginacao(PageWrapper wrapper, string email);
    }
}
