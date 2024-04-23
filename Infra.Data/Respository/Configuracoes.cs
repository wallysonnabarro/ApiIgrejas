using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class Configuracoes : IConfiguracoes
    {
        private readonly ContextDb _db;
        private readonly IContratoRepository _contratoRepository;

        public Configuracoes(ContextDb db, IContratoRepository contratoRepository)
        {
            _db = db;
            _contratoRepository = contratoRepository;
        }

        public async Task<Result<TiposSaidaDto>> Detalhar(int id, string email)
        {
            try
            {
                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var detalhe = await _db.TiposSaidas.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Contrato.Id == contrato.Dados.Id && x.Id == id);
                    return Result<TiposSaidaDto>.Sucesso(new TiposSaidaDto { Nome = detalhe!.Nome, Id = detalhe.Id });
                }
                else
                {
                    return Result<TiposSaidaDto>.Failed(new List<Erros> { new Erros { mensagem = "Algo de errado no cadastro, entre em contato com o responsável do contrato." } });
                }
            }
            catch (Exception e)
            {
                return Result<TiposSaidaDto>.Failed(new List<Erros> { new Erros { mensagem = e.Message } });
            }
        }

        public async Task<Result<string>> Editar(TiposSaidaDto area, string email)
        {
            try
            {
                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var tipo = await _db.TiposSaidas.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Id == area.Id && x.Contrato.Id == contrato.Dados.Id);

                    if (tipo == null)
                    {
                        return Result<string>.Failed(new List<Erros> { new Erros { mensagem = "O tipo de saída não foi localizado." } });
                    }
                    else
                    {
                        tipo.Nome = area.Nome;
                        await _db.SaveChangesAsync();

                        return Result<string>.Sucesso("Atualizado com sucesso.");
                    }
                }
                else
                {
                    return Result<string>.Failed(new List<Erros> { new Erros { mensagem = "Algo de errado no cadastro, entre em contato com o responsável do contrato." } });
                }
            }
            catch (Exception e)
            {
                return Result<string>.Failed(new List<Erros> { new Erros { mensagem = e.Message } });
            }
        }

        public async Task<Result<string>> Novo(string dto, string email)
        {
            try
            {
                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    if (await _db.TiposSaidas.Include(x => x.Contrato).AnyAsync(x => x.Nome.Equals(dto) && x.Contrato.Id == contrato.Dados.Id))
                    {
                        return Result<string>.Failed(new List<Erros> { new Erros { mensagem = "Tipo já registrado." } });
                    }
                    else
                    {
                        var novo = new TipoSaida { Contrato = contrato.Dados, Nome = dto };

                        _db.TiposSaidas.Add(novo);
                        await _db.SaveChangesAsync();

                        return Result<string>.Sucesso("Registrado com sucesso.");
                    }
                }
                else
                {
                    return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception e)
            {
                return Result<string>.Failed(new List<Erros> { new Erros { mensagem = e.Message } });
            }
        }

        public async Task<Result<Paginacao<TiposSaidaDto>>> Paginacao(PageWrapper wrapper, string email)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var lista = await _db.TiposSaidas
                        .Include(x => x.Contrato)
                        .Where(x => x.Contrato.Id == contrato.Dados.Id)
                        .Skip(page * wrapper.PageSize)
                        .Take(wrapper.PageSize)
                        .Select(s => new TiposSaidaDto { Nome = s.Nome, Id = s.Id })
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();

                    return Result<Paginacao<TiposSaidaDto>>.Sucesso(new Paginacao<TiposSaidaDto>
                    {
                        Dados = lista,
                        Count = await Count(),
                        PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                        PageSize = wrapper.PageSize
                    });
                }
                else
                {
                    return Result<Paginacao<TiposSaidaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Paginacao<TiposSaidaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        private async Task<int> Count()
        {
            return await _db.TiposSaidas.CountAsync();
        }
    }
}
