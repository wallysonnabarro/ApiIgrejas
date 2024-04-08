using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<Identidade> Delete(int id);
        Task<Role> Get(int id);
        Task<Result<PerfilListaPaginadaDto>> Get(string name, string email);
        Task<Identidade> Insert(PerfilDto roler, string email);
        Task<Identidade> Insert(int tipo, string email);
        Task<List<Role>> List();
        Task<Identidade> Update(UpdatePerfilDto roler);
        Task<bool> IsValid(string v);
        Task<Result<Paginacao<PerfilListaPaginadaDto>>> Paginacao(PageWrapper wrapper, string email);
    }
}
