using Domain.Dominio;

namespace Infra.Data.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Identidade> Delete(int id);
        public Task<Role> Get(int id);
        public Task<Role> Get(string name);
        public Task<Identidade> Insert(Role roler);
        public Task<Identidade> Insert(int tipo);
        public Task<List<Role>> List();
        public Task<Identidade> Update(Role roler);
        public Task<bool> IsValid(string v);
    }
}
