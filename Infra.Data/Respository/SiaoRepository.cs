using AutoMapper;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class SiaoRepository : ISiaoRepository
    {
        private readonly ContextDb _db;
        private readonly Mapper _mapper;

        public SiaoRepository(ContextDb db)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContratoProfile>());
            _mapper = new Mapper(config);
            _db = db;
        }

        public async Task<Result<Siao>> Detalhar(int id)
        {
            try
            {
                var existe = await _db.Siaos.FirstOrDefaultAsync(x => x.Id == id);
                if (existe == null)
                {
                    return Result<Siao>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento não localizado.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    return Result<Siao>.Sucesso(existe);
                }
            }
            catch (Exception ex)
            {
                return Result<Siao>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Editar(Siao siao)
        {
            try
            {
                var existe = await _db.Siaos.FirstOrDefaultAsync(x => x.Id == siao.Id);
                if (existe == null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento não localizado.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    existe.Status = siao.Status;
                    existe.Evento = siao.Evento;
                    existe.Coordenadores = siao.Coordenadores;
                    existe.Inicio = siao.Inicio;
                    existe.Termino = siao.Termino;
                    existe.Descricao = siao.Descricao;
                    await _db.SaveChangesAsync();
                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> EditarStatus(int id, int status)
        {
            try
            {
                var existe = await _db.Siaos.FirstOrDefaultAsync(x => x.Id == id);
                if (existe == null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento não localizado.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    existe.Status = status;
                    await _db.SaveChangesAsync();
                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Novo(SiaoNovoDto dto)
        {
            try
            {
                var existe = await _db.Siaos.FirstOrDefaultAsync(x => x.Evento.Equals(dto.Evento));
                if (existe != null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento já registrado.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    var siao = new Siao
                    {
                        Coordenadores = dto.Coordenadores,
                        Evento = dto.Evento,
                        Descricao = dto.Descricao,
                        Inicio = dto.Inicio,
                        Status = dto.Status,
                        Termino = dto.Termino
                    };
                    _db.Add(siao);
                    await _db.SaveChangesAsync();
                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<Siao>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var lista = await _db.Siaos
                    .Skip(page * wrapper.PageSize)
                    .Take(wrapper.PageSize)
                    .ToListAsync();

                return Result<Paginacao<Siao>>.Sucesso(new Paginacao<Siao>
                {
                    Dados = lista,
                    Count = await Count(),
                    PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                    PageSize = wrapper.PageSize
                });
            }
            catch (Exception ex)
            {
                return Result<Paginacao<Siao>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        private async Task<int> Count()
        {
            return await _db.Siaos.CountAsync();
        }
    }
}
