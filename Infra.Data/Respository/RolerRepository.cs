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

        public async Task<Result<List<PerfilSelectedDto>>> GetList(string email)
        {
            try
            {
                var contrato = await contratoRepository.GetResult(email);

                var roles = await _context.Roles
                        .Include(x => x.Contrato)
                        .Where(x => x.Contrato.Id == contrato.Dados.Id && !x.Nome.Equals("DESENVOLVEDOR"))
                        .Select(x => new PerfilSelectedDto { Id = x.Id, Nome = x.Nome })
                        .ToListAsync();                

                if (roles != null)
                    return Result<List<PerfilSelectedDto>>.Sucesso(roles);
                else return Result<List<PerfilSelectedDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Perfil não encotrado", ocorrencia = "", versao = "" } });
            }
            catch (Exception ex)
            {
                return Result<List<PerfilSelectedDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<int>> Insert(PerfilDto roler, string email)
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

                    return Result<int>.Success;
                }
                else
                {
                    return Result<int>.Failed(new List<Erros>{ new Erros {
                        mensagem = "Contrato não localizado."
                    }});
                }
            }
            catch (Exception e)
            {
                return Result<int>.Failed(new List<Erros>{ new Erros {
                    mensagem = e.Message,
                    }});
            }
        }

        public async Task<Result<int>> Insert(PerfilNovoDto roler, string email)
        {
            try
            {
                var contrato = await contratoRepository.GetResult(roler.contratoSelecionadoId);

                if (contrato.Succeeded)
                {
                    Role perfil = new Role() { Contrato = contrato.Dados, Nome = roler.perfilName, Status = roler.statusSelecionadoId };

                    List<Transacao> transacao = new List<Transacao>();
                    if (roler.tribosEquipes == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Tribos/Equipes")));
                    }
                    if (roler.membros == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Membros")));
                    }
                    if (roler.cadastroEvento == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Cadastro Sião")));
                    }
                    if (roler.eventosSele == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Sião")));
                    }
                    if (roler.area == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Área")));
                    }
                    if (roler.inscricoes == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Inscricões")));
                    }
                    if (roler.inscricoesVoluntarios == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Inscrições Voluntários")));
                    }
                    if (roler.administracoe == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Administração")));
                    }
                    if (roler.novoUsuario == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Novo Usuário")));
                    }
                    if (roler.redefinirSenha == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Redefinir Senha")));
                    }
                    if (roler.redefinirAcesso == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Redefinir Acesso")));
                    }
                    if (roler.fechamentoPagamentos == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Fechamento/Pagamentos")));
                    }
                    if (roler.fechamentoEvento == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Fechamento - Sião")));
                    }
                    if (roler.SaidaPagamentos == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Saída - Pagamentos")));
                    }
                    if (roler.ofertasEvento == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Descontos - Pagamentos")));
                    }
                    if (roler.lanchonete == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Lançamentos Lanchonete")));
                    }
                    if (roler.financeiro == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Financeiro")));
                    }
                    if (roler.registrarFinanceiro == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Registrar Financeiro")));
                    }
                    if (roler.despesasObrigações == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Despesas e Obrigações")));
                    }
                    if (roler.visualizarFinanceiro == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Visualizar Financeiro")));
                    }
                    if (roler.tiposSaida == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Tipos de Saída")));
                    }
                    if (roler.logout == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Logout")));
                    }
                    if (roler.login == true)
                    {
                        transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Login")));
                    }

                    perfil.Transacoes = transacao;

                    _context.Roles.Add(perfil);
                    await _context.SaveChangesAsync();

                    return Result<int>.Sucesso(perfil.Id);

                }
                else
                {
                    return Result<int>.Failed(new List<Erros>{ new Erros {
                        mensagem = "Contrato não localizado."
                    }});
                }
            }
            catch (Exception e)
            {
                return Result<int>.Failed(new List<Erros>{ new Erros {
                    mensagem = e.Message,
                    }});
            }
        }

        public async Task<Result<int>> Insert(int tipo, string email)
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

            return Result<int>.Success;
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
                            Status = x.Status,
                            Transacoes = x.Transacoes!
                        })
                        .Skip(page * wrapper.PageSize)
                        .Take(wrapper.PageSize)
                        .ToListAsync();

                    var dese = lista.FirstOrDefault(x => x.Nome.Equals("DESENVOLVEDOR"));

                    if (dese != null)
                        lista.Remove(dese);

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

        public async Task<Result<bool>> Update(UpdatePerfilDto roler)
        {
            try
            {
                var role = await _context.Roles
                    .Include(x => x.Transacoes)
                    .FirstOrDefaultAsync(r => r.Id == roler.Id);

                if (role == null) return Result<bool>.Failed(new List<Erros>() { new Erros { mensagem = "Perfil não localizado." } });

                var perfis = await _context.Transacaos.Where(x => roler.Transacoes.Contains(x.Id)).ToListAsync();

                role.Nome = roler.Nome;
                role.Status = roler.Status;
                role.Transacoes = perfis;

                _context.Roles.Update(role);
                _context.SaveChanges();

                return Result<bool>.Sucesso(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Failed(new List<Erros>() { new Erros { mensagem = e.Message } });
            }
        }

        public async Task<Result<bool>> Update(PerfilAtualizarDto roler, string email)
        {
            try
            {
                var role = await _context.Roles
                    .Include(x => x.Transacoes)
                    .FirstOrDefaultAsync(r => r.Id == roler.Id);

                var contrato = await contratoRepository.GetResult(roler.contratoSelecionadoId);

                if (role == null) return Result<bool>.Failed(new List<Erros>() { new Erros { mensagem = "Perfil não localizado." } });

                List<Transacao> transacao = new List<Transacao>();
                if (roler.tribosEquipes == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Tribos/Equipes")));
                }
                if (roler.membros == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Membros")));
                }
                if (roler.cadastroEvento == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Cadastro Sião")));
                }
                if (roler.eventosSele == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Sião")));
                }
                if (roler.area == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Área")));
                }
                if (roler.inscricoes == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Inscricões")));
                }
                if (roler.inscricoesVoluntarios == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Inscrições Voluntários")));
                }
                if (roler.administracoe == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Administração")));
                }
                if (roler.novoUsuario == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Novo Usuário")));
                }
                if (roler.redefinirSenha == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Redefinir Senha")));
                }
                if (roler.redefinirAcesso == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Redefinir Acesso")));
                }
                if (roler.fechamentoPagamentos == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Fechamento/Pagamentos")));
                }
                if (roler.fechamentoEvento == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Fechamento - Sião")));
                }
                if (roler.SaidaPagamentos == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Saída - Pagamentos")));
                }
                if (roler.ofertasEvento == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Descontos - Pagamentos")));
                }
                if (roler.lanchonete == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Lançamentos Lanchonete")));
                }
                if (roler.financeiro == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Financeiro")));
                }
                if (roler.registrarFinanceiro == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Registrar Financeiro")));
                }
                if (roler.despesasObrigações == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Despesas e Obrigações")));
                }
                if (roler.visualizarFinanceiro == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Visualizar Financeiro")));
                }
                if (roler.tiposSaida == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Tipos de Saída")));
                }
                if (roler.logout == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Logout")));
                }
                if (roler.login == true)
                {
                    transacao.Add(await _context.Transacaos.FirstOrDefaultAsync(x => x.Nome.Equals("Login")));
                }

                var remover = role.Transacoes.Except(transacao);


                role.Nome = roler.perfilName;
                role.Contrato = contrato.Dados;
                role.Status = roler.statusSelecionadoId;
                role.Transacoes = transacao;

                _context.Roles.Update(role);
                _context.SaveChanges();

                return Result<bool>.Sucesso(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Failed(new List<Erros>() { new Erros { mensagem = e.Message } });
            }
        }
    }
}
