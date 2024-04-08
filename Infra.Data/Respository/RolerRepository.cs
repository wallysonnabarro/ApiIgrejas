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
        private readonly IContratoRepository contratoRepository;

        public RolerRepository(ContextDb context, IContratoRepository contratoRepository)
        {
            _context = context;
            this.contratoRepository = contratoRepository;
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

        public async Task<Result<PerfilListaPaginadaDto>> Get(string name, string email)
        {
            try
            {
                var roles = await _context.Roles
                        .Include(x => x.Transacoes)
                        .Select(x => new PerfilListaPaginadaDto
                        {
                            Nome = x.Nome,
                            Id = x.Id,
                            Transacoes = x.Transacoes!
                        }).FirstOrDefaultAsync(r => r.Nome == name);
                if (roles != null)
                    return Result<PerfilListaPaginadaDto>.Sucesso(roles);
                else return Result<PerfilListaPaginadaDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Perfil não encotrado", ocorrencia = "", versao = "" } });
            }
            catch (Exception ex)
            {
                return Result<PerfilListaPaginadaDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Identidade> Insert(PerfilDto roler, string email)
        {
            try
            {
                var contrato = await contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var listaTransacao = await _context.Transacaos.Where(x => roler.Transacoes.Contains(x.Id)).ToListAsync();

                    var novo = new Role
                    {
                        Nome = roler.Nome,
                        Status = 1,
                        Transacoes = listaTransacao,
                        Contrato = contrato.Dados,
                    };

                    _context.Roles.Add(novo);
                    _context.SaveChanges();

                    return Identidade.Success;
                }
                else
                {
                    return Identidade.Failed(new IdentidadeError { Code = "", Description = contrato.Errors.Min(x => x.mensagem) });
                }
            }
            catch (Exception e)
            {
                return Identidade.Failed(new IdentidadeError { Code = e.HResult.ToString(), Description = e.Message });
            }
        }

        public async Task<Identidade> Insert(int tipo, string email)
        {
            var contrato = await contratoRepository.GetResult(email);

            if (contrato.Succeeded)
            {
                if (tipo == 0)
                {
                    _context.Roles.Add(new Role() { Nome = "DESENVOLVEDOR", Contrato = contrato.Dados });
                    await _context.SaveChangesAsync();
                }
            }
            //if (tipo == 1)
            //{
            //    _context.Roles.Add(new Role() { Nome = "CLIENTE", Contrato = contrato.Dados  });
            //    await _context.SaveChangesAsync();
            //}

            //if (tipo == 2)
            //{
            //    _context.Roles.Add(new Role() { Nome = "CORRETOR", Contrato = contrato.Dados  });
            //    await _context.SaveChangesAsync();
            //}

            return Identidade.Success;
        }

        public async Task<bool> IsValid(string v)
        {
            return await _context.Roles.AnyAsync(r => r.Nome.Equals(v));
        }

        public async Task<List<Role>> List() => await _context.Roles.ToListAsync();

        public async Task<Result<Paginacao<PerfilListaPaginadaDto>>> Paginacao(PageWrapper wrapper, string email)
        {
            try
            {
                var contrato = await contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                    var lista = await _context.Roles
                        .Include(x => x.Contrato)
                        .Include(x => x.Transacoes)
                        .Where(x => x.Contrato.Id == contrato.Dados.Id)
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
                else
                {
                    return Result<Paginacao<PerfilListaPaginadaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Paginacao<PerfilListaPaginadaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Identidade> Update(UpdatePerfilDto roler)
        {
            try
            {
                var role = await _context.Roles
                    .Include(x => x.Transacoes)
                    .FirstOrDefaultAsync(r => r.Id == roler.Id);

                if (role == null) return Identidade.Failed(new IdentidadeError() { Code = "error", Description = "Perfil não encontrado." });

                var perfis = await _context.Transacaos.Where(x => roler.Transacoes.Contains(x.Id)).ToListAsync();

                role.Nome = roler.Nome;
                role.Status = roler.Status;
                role.Transacoes = perfis;

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
