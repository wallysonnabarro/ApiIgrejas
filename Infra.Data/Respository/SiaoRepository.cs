using AutoMapper;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interface;
using Service.Utilitarios;

namespace Infra.Data.Respository
{
    public class SiaoRepository : ISiaoRepository
    {
        private readonly ContextDb _db;
        private readonly Mapper _mapper;
        private readonly IEventoServices _eventoServices;
        private readonly IContratoRepository _contratoRepository;

        public SiaoRepository(ContextDb db, IEventoServices eventoServices, IContratoRepository contratoRepository)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContratoProfile>());
            _mapper = new Mapper(config);
            _db = db;
            _eventoServices = eventoServices;
            _contratoRepository = contratoRepository;
        }

        public async Task<Result<EventosAtivosDto>> ConfirmarToken(ConfirmarTokenDto dto)
        {
            try
            {
                var isValid = await _db.Eventos.FirstOrDefaultAsync(x => x.Token.Equals(dto.Token));
                if (isValid == null)
                {
                    return Result<EventosAtivosDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "O token informado é inválido.", ocorrencia = "", versao = "" } });
                }
                else
                {
                    return Result<EventosAtivosDto>.Sucesso(new EventosAtivosDto { Nome = isValid.Nome, Id = isValid.Id });
                }
            }
            catch (Exception ex)
            {
                return Result<EventosAtivosDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<Evento>> Detalhar(int id, string email)
        {
            try
            {

                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var existe = await _db.Eventos.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Id == id && x.Contrato.Id == contrato.Dados.Id);
                    if (existe == null)
                    {
                        return Result<Evento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento não localizado.", ocorrencia = "", versao = "V1" } });
                    }
                    else
                    {
                        return Result<Evento>.Sucesso(existe);
                    }
                }
                else
                {
                    return Result<Evento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Evento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Editar(Evento siao, string email)
        {
            try
            {

                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var existe = await _db.Eventos.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Id == siao.Id && x.Contrato.Id == contrato.Dados.Id);
                    if (existe == null)
                    {
                        return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento não localizado.", ocorrencia = "", versao = "V1" } });
                    }
                    else
                    {
                        existe.Status = siao.Status;
                        existe.Nome = siao.Nome;
                        existe.Coordenadores = siao.Coordenadores;
                        existe.Inicio = siao.Inicio;
                        existe.Termino = siao.Termino;
                        existe.Descricao = siao.Descricao;
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

        public async Task<Result<bool>> EditarStatus(int id, int status, string email)
        {
            try
            {

                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var existe = await _db.Eventos.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Id == id && x.Contrato.Id == contrato.Dados.Id);
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

        public async Task<Result<List<EventosAtivosDto>>> GetEmIniciado()
        {
            try
            {
                var siao = await _db.Eventos.Where(s => (s.Status == 1 || s.Status == 4))
                    .Select(s => new EventosAtivosDto { Nome = s.Nome, Id = s.Id }).ToListAsync();
                if (siao == null)
                {
                    return Result<List<EventosAtivosDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Nenhum evento ativo.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    return Result<List<EventosAtivosDto>>.Sucesso(siao);
                }
            }
            catch (Exception ex)
            {
                return Result<List<EventosAtivosDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<string>> Novo(SiaoNovoDto dto, string email)
        {
            try
            {

                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var existe = await _db.Eventos.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Nome.Equals(dto.Evento) && x.Contrato.Id == contrato.Dados.Id);
                    if (existe != null)
                    {
                        return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento já registrado.", ocorrencia = "", versao = "V1" } });
                    }
                    else
                    {
                        var token = "";

                        var isValide = false;

                        do
                        {
                            var t = await _eventoServices.GerarTokenEvento();
                            if (!await _db.Eventos.AnyAsync(x => x.Token.Equals(t)))
                            {
                                isValide = true;
                                token = t;
                            }
                        } while (isValide == false);

                        var siao = new Evento
                        {
                            Coordenadores = dto.Coordenadores,
                            Nome = dto.Evento,
                            Descricao = dto.Descricao,
                            Inicio = dto.Inicio,
                            Status = dto.Status,
                            Termino = dto.Termino,
                            Token = token,
                            Contrato = contrato.Dados
                        };
                        _db.Add(siao);
                        await _db.SaveChangesAsync();
                        return Result<string>.Sucesso(token);
                    }
                }
                else
                {
                    return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<Evento>>> Paginacao(PageWrapper wrapper, string email)
        {
            try
            {
                var contrato = await _contratoRepository.GetResult(email);

                if (contrato.Succeeded)
                {
                    var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                    var lista = await _db.Eventos
                        .Include(x => x.Contrato)
                        .Where(x => x.Contrato.Id == contrato.Dados.Id)
                        .Skip(page * wrapper.PageSize)
                        .Take(wrapper.PageSize)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();

                    return Result<Paginacao<Evento>>.Sucesso(new Paginacao<Evento>
                    {
                        Dados = lista,
                        Count = await Count(),
                        PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                        PageSize = wrapper.PageSize
                    });
                }
                else
                {
                    return Result<Paginacao<Evento>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = contrato.Errors.Min(x => x.mensagem), ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Paginacao<Evento>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        private async Task<int> Count()
        {
            return await _db.Eventos.CountAsync();
        }
    }
}
