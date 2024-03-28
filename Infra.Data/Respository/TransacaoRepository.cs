using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly ContextDb _db;

        public TransacaoRepository(ContextDb db)
        {
            _db = db;
        }

        public async Task<Result<Transacao>> novo(TransacaoDto dto)
        {
            try
            {
                var transacao = new Transacao
                {
                    Descricao = dto.Descricao,
                    Nome = dto.Nome,
                    Rota = dto.Rota,
                    DataCadastro = dto.DataCadastro,
                    IdTransacaoPai = dto.IdTransacaoPai,
                    Ordenacao = dto.Ordenacao,
                    Status = dto.Status,
                    StControle = dto.StControle,
                    StFormulario = dto.StFormulario,
                    StFuncao = dto.StFuncao,
                    StMenu = dto.StMenu
                };

                _db.Add(transacao);
                await _db.SaveChangesAsync();

                return Result<Transacao>.Success;
            }
            catch (Exception ex)
            {
                return Result<Transacao>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        private async Task<int> Count()
        {
            return await _db.Transacaos.CountAsync();
        }

        public async Task<Result<Paginacao<TransacaoListDto>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var lista = await _db.Transacaos
                    .Select(x => new TransacaoListDto
                    {
                        Id = x.Id,
                        Descricao = x.Descricao,
                        Nome = x.Nome,
                        IdTransacaoPai = x.IdTransacaoPai
                    })
                    .Skip(page * wrapper.PageSize)
                    .Take(wrapper.PageSize)
                    .ToListAsync();

                return Result<Paginacao<TransacaoListDto>>.Sucesso(new Paginacao<TransacaoListDto>
                {
                    Dados = lista,
                    Count = await Count(),
                    PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                    PageSize = wrapper.PageSize
                });
            }
            catch (Exception ex)
            {
                return Result<Paginacao<TransacaoListDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }
    }
}
