using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly ISiaoRepository _siaoRepository;
        private readonly IAuthorization authorization;

        public EventoController(ISiaoRepository siaoRepository, IAuthorization authorization)
        {
            _siaoRepository = siaoRepository;
            this.authorization = authorization;
        }

        [Authorize]
        [HttpPost("novo")]
        public async Task<Result<string>> Novo(SiaoNovoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await _siaoRepository.Novo(dto, isToken.Result.Email!);
            }
            else
            {
                return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpPost("listar")]
        public async Task<Result<Paginacao<Evento>>> Paginado(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await _siaoRepository.Paginacao(wrapper, isToken.Result.Email!);
            }
            else
            {
                return Result<Paginacao<Evento>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpPost("editar")]
        public async Task<Result<bool>> Editar(Evento siao)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await _siaoRepository.Editar(siao, isToken.Result.Email!);
            }
            else
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpPost("{id}/editar-status/{status}")]
        public async Task<Result<bool>> EditarStatus(int id, int status)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await _siaoRepository.EditarStatus(id, status, isToken.Result.Email!);
            }
            else
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpPost("detalhar/{id}")]
        public async Task<Result<Evento>> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await _siaoRepository.Detalhar(id, isToken.Result.Email!);
            }
            else
            {
                return Result<Evento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpGet("evento-andamento")]
        public async Task<Result<List<EventosAtivosDto>>> SiaoEmAndamento()
        {
            return await _siaoRepository.GetEmIniciado();
        }

        [HttpPost("confirmar-token")]
        public async Task<Result<EventosAtivosDto>> confirmarToken(ConfirmarTokenDto dto)
        {
            return await _siaoRepository.ConfirmarToken(dto);
        }

        //[HttpGet("gerarToken")]
        //public async Task<string> Gerar()
        //{
        //    return await TokenEvento.GenerateToken();
        //}
    }
}
