using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<Identidade> Delete(int id);
        Task<Role> Get(int id);
        Task<Role> Get(string name);
        Task<Identidade> Insert(PerfilDto roler);
        Task<Identidade> Insert(int tipo);
        Task<List<Role>> List();
        Task<Identidade> Update(Role roler);
        Task<bool> IsValid(string v);
        Task<Result<Paginacao<PerfilListaPaginadaDto>>> Paginacao(PageWrapper wrapper);
    }
}
