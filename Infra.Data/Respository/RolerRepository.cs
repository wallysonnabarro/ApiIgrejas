using Domain.Dominio;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class RolerRepository : IRoleRepository
    {
        private readonly ContextDb _context;

        public RolerRepository(ContextDb context)
        {
            _context = context;
        }

        public async Task<Identidade> Delete(int id)
        {
            try
            {
                var roler = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
                _context.Roles.Remove(roler);
                _context.SaveChanges();
                return Identidade.Success;
            }
            catch (Exception e)
            {
                return Identidade.Failed(new IdentidadeError { Code = e.HResult.ToString(), Description = e.Message });
            }
        }

        public async Task<Role> Get(int id) => await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Role> Get(string name) => await _context.Roles.FirstOrDefaultAsync(r => r.Nome == name);

        public async Task<Identidade> Insert(Role roler)
        {
            try
            {
                _context.Roles.Add(roler);
                _context.SaveChanges();

                return Identidade.Success;
            }
            catch (Exception e)
            {
                return Identidade.Failed(new IdentidadeError { Code = e.HResult.ToString(), Description = e.Message });
            }
        }

        public async Task<Identidade> Insert(int tipo)
        {
            if (tipo == 0)
            {
                _context.Roles.Add(new Role() { Nome = "DESENVOLVEDOR" });
                await _context.SaveChangesAsync();
            }

            //if (tipo == 1)
            //{
            //    _context.Roles.Add(new Role() { Nome = "CLIENTE" });
            //    await _context.SaveChangesAsync();
            //}

            //if (tipo == 2)
            //{
            //    _context.Roles.Add(new Role() { Nome = "CORRETOR" });
            //    await _context.SaveChangesAsync();
            //}

            return Identidade.Success;
        }

        public async Task<bool> IsValid(string v)
        {
            return await _context.Roles.AnyAsync(r => r.Nome.Equals(v));
        }

        public async Task<List<Role>> List() => await _context.Roles.ToListAsync();

        public async Task<Identidade> Update(Role roler)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roler.Id);
                role.Nome = roler.Nome;

                _context.Roles.Update(role);
                _context.SaveChanges();

                return Identidade.Success;
            }
            catch (Exception e)
            {
                return Identidade.Failed(new IdentidadeError() { Code = e.HResult.ToString(), Description = e.Message });
            }
        }
    }
}
