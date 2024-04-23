using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class AreasRepository : IAreasRepository
    {
        private readonly ContextDb _db;
        private readonly IContratoRepository _contratoRepository;

        public AreasRepository(ContextDb db, IContratoRepository contratoRepository)
        {
            _db = db;
            _contratoRepository = contratoRepository;
        }

        public async Task<Result<Area>> Detalhar(int id, string email)
        {

            try
            {
                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var existe = await _db.AreasSet.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Id == id && x.Contrato.Id == contrato.Dados.Id);
                    if (existe == null)
                    {
                        return Result<Area>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Área não localizado.", ocorrencia = "", versao = "V1" } });
                    }
                    else
                    {
                        return Result<Area>.Sucesso(existe);
                    }
                }
                else
                {
                    return Result<Area>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Area>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Editar(Area area, string email)
        {
            try
            {
                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var existe = await _db.AreasSet.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Id == area.Id && x.Contrato.Id == contrato.Dados.Id);
                    if (existe == null)
                    {
                        return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Área não localizado.", ocorrencia = "", versao = "V1" } });
                    }
                    else
                    {
                        existe.Nome = area.Nome;
                        await _db.SaveChangesAsync();
                        return Result<bool>.Sucesso(true);
                    }
                }
                else
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<List<Area>>> GetAll(string token)
        {
            try
            {
                var contrato = await _db.Eventos.Include(c => c.Contrato)
                    .FirstOrDefaultAsync(x => x.Token.Equals(token));
                if (contrato == null) return Result<List<Area>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Entre em contato com o responsável do evento. Não foi localizado as áreas para este evento.", ocorrencia = "", versao = "V1" } });
                return Result<List<Area>>.Sucesso(await _db.AreasSet.ToListAsync());
            }
            catch (Exception ex)
            {
                return Result<List<Area>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Novo(AreaDto dto, string email)
        {
            try
            {
                var existe = await _db.AreasSet.FirstOrDefaultAsync(x => x.Nome.Equals(dto.Nome));
                if (existe != null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Área de atuação já registrada.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    var contrato = await _contratoRepository.GetResult(email);

                    if (contrato.Succeeded)
                    {
                        var area = new Area
                        {
                            Nome = dto.Nome,
                            Contrato = contrato.Dados
                        };
                        _db.Add(area);
                        await _db.SaveChangesAsync();
                        return Result<bool>.Sucesso(true);
                    }
                    else
                    {
                        return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                    }
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<Area>>> Paginacao(PageWrapper wrapper, string email)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var lista = await _db.AreasSet
                        .Include(x => x.Contrato)
                        .Where(x => x.Contrato.Id == contrato.Dados.Id)
                        .Skip(page * wrapper.PageSize)
                        .Take(wrapper.PageSize)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();

                    return Result<Paginacao<Area>>.Sucesso(new Paginacao<Area>
                    {
                        Dados = lista,
                        Count = await Count(),
                        PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                        PageSize = wrapper.PageSize
                    });
                }
                else
                {
                    return Result<Paginacao<Area>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Paginacao<Area>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        private async Task<int> Count()
        {
            return await _db.Eventos.CountAsync();
        }
    }
}
