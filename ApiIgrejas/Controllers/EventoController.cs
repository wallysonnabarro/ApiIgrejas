using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpPost("novo")]
        [SwaggerResponse(201, "Novo evento", typeof(Result<string>))]
        [ProducesResponseType(typeof(Result<string>), 201)]
        public async Task<IActionResult> Novo(SiaoNovoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Novo(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Created("Novo", result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("listar")]
        [SwaggerResponse(200, "Paginação dos eventos", typeof(Result<Paginacao<Evento>>))]
        [ProducesResponseType(typeof(Result<Paginacao<Evento>>), 200)]
        public async Task<IActionResult> Paginado(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Paginacao(wrapper, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("editar")]
        [SwaggerResponse(200, "Editar Evento", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> Editar(Evento siao)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Editar(siao, isToken.Result.Email!);

            if (result.Succeeded) return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("{id}/editar-status/{status}")]
        [HttpGet("getAreas")]
        [SwaggerResponse(200, "Lista de eventos", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> EditarStatus(int id, int status)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.EditarStatus(id, status, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("detalhar/{id}")]
        [SwaggerResponse(200, "Busca de eventos", typeof(Result<Evento>))]
        [ProducesResponseType(typeof(Result<Evento>), 200)]
        public async Task<IActionResult> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Detalhar(id, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("evento-andamento")]
        [SwaggerResponse(200, "Lista de eventos", typeof(Result<List<Evento>>))]
        [ProducesResponseType(typeof(Result<List<Evento>>), 200)]
        public async Task<IActionResult> SiaoEmAndamento()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _siaoRepository.GetEmIniciado();

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("confirmar-token")]
        [SwaggerResponse(200, "Confirmação do token do evento", typeof(Result<List<Evento>>))]
        [ProducesResponseType(typeof(Result<List<Evento>>), 200)]
        public async Task<IActionResult> confirmarToken(ConfirmarTokenDto dto)
        {
            var result = await _siaoRepository.ConfirmarToken(dto);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        //[HttpGet("gerarToken")]
        //public async Task<string> Gerar()
        //{
        //    return await TokenEvento.GenerateToken();
        //}
    }
}
