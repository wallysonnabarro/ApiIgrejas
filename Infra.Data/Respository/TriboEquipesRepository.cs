using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class TriboEquipesRepository : ITriboEquipesRepository
    {
        private readonly ContextDb _db;

        public TriboEquipesRepository(ContextDb db)
        {
            _db = db;
        }

        public async Task<Result<TriboEquipe>> Detalhar(int id)
        {
            try
            {
                var tribo = await _db.TribosEquipes.FirstOrDefaultAsync(x => x.Id == id);
                if (tribo != null)
                {
                    return Result<TriboEquipe>.Sucesso(tribo);
                }
                else
                {
                    return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Tribo/equipe não encontrada.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<TriboEquipe>> Editar(TriboEquipe dto)
        {
            var tribo = await _db.TribosEquipes.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (tribo == null)
            {
                return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Tribo não encontrada.", ocorrencia = "", versao = "V1" } });
            }
            else
            {
                tribo.Status = dto.Status;
                tribo.Nome = dto.Nome;
                await _db.SaveChangesAsync();

                return Result<TriboEquipe>.Success;
            }
        }

        public async Task<Result<TriboEquipe>> Novo(TriboNovoDto dto)
        {
            var tribo = await _db.TribosEquipes.FirstOrDefaultAsync(x => x.Nome.Equals(dto.Nome));
            if (tribo == null)
            {
                var novaTribo = new TriboEquipe { Nome = dto.Nome, Status = 1 };
                _db.Add(novaTribo);
                await _db.SaveChangesAsync();
                return Result<TriboEquipe>.Sucesso(novaTribo);
            }
            else
            {
                return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Tribo já cadastrada.", ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<TriboEquipe>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var lista = await _db.TribosEquipes
                    .Skip(page * wrapper.PageSize)
                    .Take(wrapper.PageSize)
                    .ToListAsync();

                var qto = await Count();

                return Result<Paginacao<TriboEquipe>>.Sucesso(new Paginacao<TriboEquipe>
                {
                    Dados = lista,
                    Count = await Count(),
                    PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                    PageSize = wrapper.PageSize
                });
            }
            catch (Exception ex)
            {
                return Result<Paginacao<TriboEquipe>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        private async Task<int> Count()
        {
            return await _db.TribosEquipes.CountAsync();
        }
    }
}
