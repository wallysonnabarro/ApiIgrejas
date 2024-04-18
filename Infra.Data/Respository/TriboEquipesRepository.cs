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
        private readonly IContratoRepository _contratoRepository;

        public TriboEquipesRepository(ContextDb db, IContratoRepository contratoRepository)
        {
            _db = db;
            _contratoRepository = contratoRepository;
        }

        public async Task<Result<TriboEquipe>> Detalhar(int id, string email)
        {
            try
            {
                var contrato = await getContrato(email);

                if (contrato.Succeeded)
                {
                    var tribo = await _db.TribosEquipes
                    .Include(x => x.Contrato)
                    .FirstOrDefaultAsync(x => x.Id == id && x.Contrato.Id == contrato.Dados.Id);
                    if (tribo != null)
                    {
                        return Result<TriboEquipe>.Sucesso(tribo);
                    }
                    else
                    {
                        return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Tribo/equipe não encontrada.", ocorrencia = "", versao = "V1" } });
                    }
                }
                else
                {
                    return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
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

        public async Task<Result<List<TriboSelectede>>> ListaSelected(string email)
        {
            var contrato = await getContrato(email);

            if (contrato.Succeeded)
            {
                var tribo = await _db.TribosEquipes
                    .Include(x => x.Contrato)
                    .Where(x => x.Contrato.Id == contrato.Dados.Id)
                    .Select(r => new TriboSelectede { Nome = r.Nome, Id = r.Id }).ToListAsync();
                if (tribo != null)
                {
                    return Result<List<TriboSelectede>>.Sucesso(tribo);
                }
                else
                {
                    return Result<List<TriboSelectede>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Não foram localizadas tribos ou equipes. Por favor, entre em contato com o resposável do evento.", ocorrencia = "", versao = "V1" } });
                }
            }
            else
            {
                return Result<List<TriboSelectede>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<TriboEquipe>> Novo(TriboNovoDto dto, string email)
        {
            var contrato = await getContrato(email);

            if (contrato.Succeeded)
            {
                var tribo = await _db.TribosEquipes
                    .Include(x => x.Contrato)
                    .FirstOrDefaultAsync(x => x.Nome.Equals(dto.Nome) && x.Contrato.Id == contrato.Dados.Id);

                if (tribo == null)
                {
                    var novaTribo = new TriboEquipe { Nome = dto.Nome, Status = 1, Contrato = contrato.Dados };//atribuir o contrato na tribo
                    _db.Add(novaTribo);
                    await _db.SaveChangesAsync();
                    return Result<TriboEquipe>.Sucesso(novaTribo);
                }
                else
                {
                    return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Tribo já cadastrada.", ocorrencia = "", versao = "V1" } });
                }
            }
            else
            {
                return Result<TriboEquipe>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<TriboEquipe>>> Paginacao(PageWrapper wrapper, string email)
        {
            try
            {
                var contrato = await getContrato(email);

                if (contrato.Succeeded)
                {
                    var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                    var lista = await _db.TribosEquipes
                        .Include(x => x.Contrato)
                        .Where(x => x.Contrato.Id == contrato.Dados.Id)
                        .Skip(page * wrapper.PageSize)
                        .Take(wrapper.PageSize)
                        .ToListAsync();

                    return Result<Paginacao<TriboEquipe>>.Sucesso(new Paginacao<TriboEquipe>
                    {
                        Dados = lista,
                        Count = await Count(contrato.Dados),
                        PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                        PageSize = wrapper.PageSize
                    });
                }
                else
                {
                    return Result<Paginacao<TriboEquipe>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Paginacao<TriboEquipe>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        private async Task<int> Count(Contrato contrato)
        {
            return await _db.TribosEquipes
                .Where(x => x.Contrato.Id == contrato.Id)
                .CountAsync();
        }

        private async Task<Result<Contrato>> getContrato(string email)
        {
            return await _contratoRepository.GetResult(email);
        }
    }
}
