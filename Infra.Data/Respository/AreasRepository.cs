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

        public AreasRepository(ContextDb db)
        {
            _db = db;
        }

        public async Task<Result<Area>> Detalhar(int id)
        {

            try
            {
                var existe = await _db.AreasSet.FirstOrDefaultAsync(x => x.Id == id);
                if (existe == null)
                {
                    return Result<Area>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Área não localizado.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    return Result<Area>.Sucesso(existe);
                }
            }
            catch (Exception ex)
            {
                return Result<Area>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Editar(Area area)
        {
            try
            {
                var existe = await _db.AreasSet.FirstOrDefaultAsync(x => x.Id == area.Id);
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
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Novo(AreaDto dto)
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
                    var area = new Area
                    {
                        Nome = dto.Nome
                    };
                    _db.Add(area);
                    await _db.SaveChangesAsync();
                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<Area>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var lista = await _db.AreasSet
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
            catch (Exception ex)
            {
                return Result<Paginacao<Area>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }
        private async Task<int> Count()
        {
            return await _db.Siaos.CountAsync();
        }
    }
}
