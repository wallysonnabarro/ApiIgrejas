using Domain.Dominio;
using Domain.DTOs;
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

        private async Task<int> Count()
        {
            return await _context.Roles.CountAsync();
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

        public async Task<Identidade> Insert(PerfilDto roler)
        {
            try
            {
                var listaTransacao = await _context.Transacaos.Where(x => roler.Transacoes.Contains(x.Id)).ToListAsync();

                var novo = new Role
                {
                    Nome = roler.Nome,
                    Transacoes = listaTransacao,
                };

                _context.Roles.Add(novo);
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

        public async Task<Result<Paginacao<PerfilListaPaginadaDto>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var lista = await _context.Roles
                    .Include(x => x.Transacoes)
                    .Select(x => new PerfilListaPaginadaDto
                    {
                        Nome = x.Nome,
                        Id = x.Id,
                        Transacoes = x.Transacoes!
                    })
                    .Skip(page * wrapper.PageSize)
                    .Take(wrapper.PageSize)
                    .ToListAsync();

                int count = await Count();

                return Result<Paginacao<PerfilListaPaginadaDto>>.Sucesso(new Paginacao<PerfilListaPaginadaDto>
                {
                    Count = count,
                    Dados = lista,
                    PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                    PageSize = wrapper.PageSize
                });
            }
            catch (Exception ex)
            {
                return Result<Paginacao<PerfilListaPaginadaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

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
