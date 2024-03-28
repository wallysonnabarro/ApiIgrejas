using AutoMapper;
using Domain.Dominio;
using Domain.Dominio.menus;
using Domain.Dominio.menus.Submenus;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interface;

namespace Infra.Data.Respository
{
    public class GrupoRepository : IGrupoRepository
    {
        private readonly IGrupoService grupoService;
        private readonly Mapper _mapper;
        private readonly ContextDb _contextDb;

        public GrupoRepository(IGrupoService grupoService, ContextDb contextDb)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GruposProfile>());
            _mapper = new Mapper(config);
            this.grupoService = grupoService;
            _contextDb = contextDb;
        }

        public async Task<Result<bool>> Atualizar(GrupoAtualizarDto dto)
        {
            try
            {
                if (await _contextDb.Grupos.AnyAsync(x => x.Id == dto.Id))
                {
                    var grupo = await _contextDb.Grupos.FirstOrDefaultAsync(x => x.Id == dto.Id);
                    grupo!.NomeUsuarioCriacao = dto.NomeUsuarioCriacao;
                    grupo.DataModificacao = DateTime.Now;
                    grupo.JustificativaModificacao = dto.JustificativaModificacao;
                    grupo.IdUsuarioCriacao = dto.IdUsuarioCriacao;
                    grupo.Grupo = dto.Grupo;
                    await _contextDb.SaveChangesAsync();
                    return Result<bool>.Success;
                }
                else
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Dados não localizados.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Grupos>> BuscarPorId(int id)
        {
            try
            {
                if (await _contextDb.Grupos.AnyAsync(x => x.Id == id))
                {
                    var grupo = await _contextDb.Grupos.FirstOrDefaultAsync(x => x.Id == id);

                    return Result<Grupos>.Sucesso(grupo!);
                }
                else
                {
                    return Result<Grupos>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Código do grupo incorreto.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Grupos>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Novo(GrupoDto dto)
        {
            try
            {
                if (grupoService.Validacao(dto))
                {
                    List<Menus> menus = new();
                    List<Submenu> subMenus = new();

                    if (dto.SubmenuDto != null)
                    {
                        subMenus = await _contextDb.Submenus
                            .Where(x => dto.SubmenuDto.Any(d => x.Id == d.Id)).ToListAsync();
                    }

                    menus = await _contextDb.Menus
                        .Where(x => dto.MenusDto.Any(d => x.Id == d.Id)).ToListAsync();

                    var grupo = new Grupos
                    {
                        DtCriacao = DateTime.Now,
                        Grupo = dto.Grupo,
                        NomeUsuarioCriacao = dto.NomeUsuarioCriacao,
                        IdUsuarioCriacao = _contextDb.Users.FirstOrDefault(x => x.Nome.Equals(dto.NomeUsuarioCriacao))!.Id,
                        GroupMenus = menus,
                        GruposSubmenus = subMenus
                    };

                    _contextDb.Add(grupo);
                    await _contextDb.SaveChangesAsync();

                    return new Result<bool>();
                }
                else
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Campos inválidos.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<GrupoPaginado>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var lista = await _contextDb.Grupos
                    .Select(x => new GrupoPaginado
                    {
                        Grupo = x.Grupo,
                        NomeUsuarioCriacao = x.NomeUsuarioCriacao,
                        DataModificacao = x.DataModificacao,
                        Id = x.Id,
                        JustificativaModificacao = x.JustificativaModificacao
                    })
                    .ToListAsync();

                return Result<Paginacao<GrupoPaginado>>.Sucesso(new Paginacao<GrupoPaginado>
                {
                    Dados = lista,
                    Count = lista.Count,
                    PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                    PageSize = wrapper.PageSize
                });
            }
            catch (Exception ex)
            {
                return Result<Paginacao<GrupoPaginado>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }
    }
}
