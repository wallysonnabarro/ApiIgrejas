using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        public async Task<IActionResult> Novo(SiaoNovoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Novo(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Created("Novo", result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("listar")]
        public async Task<IActionResult> Paginado(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Paginacao(wrapper, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("editar")]
        public async Task<IActionResult> Editar(Evento siao)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Editar(siao, isToken.Result.Email!);

            if (result.Succeeded) return Ok(result);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("{id}/editar-status/{status}")]
        public async Task<IActionResult> EditarStatus(int id, int status)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.EditarStatus(id, status, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("detalhar/{id}")]
        public async Task<IActionResult> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _siaoRepository.Detalhar(id, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpGet("evento-andamento")]
        public async Task<IActionResult> SiaoEmAndamento()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _siaoRepository.GetEmIniciado();

            if (result.Succeeded)
                return Ok(result);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("confirmar-token")]
        public async Task<IActionResult> confirmarToken(ConfirmarTokenDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _siaoRepository.ConfirmarToken(dto);

            if (result.Succeeded)
                return Ok(result);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        //[HttpGet("gerarToken")]
        //public async Task<string> Gerar()
        //{
        //    return await TokenEvento.GenerateToken();
        //}
    }
}
