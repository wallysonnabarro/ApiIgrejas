using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposSaidasController : ControllerBase
    {
        private readonly IConfiguracoes _configuration;
        private readonly IAuthorization authorization;

        public TiposSaidasController(IConfiguracoes configuration, IAuthorization authorization)
        {
            _configuration = configuration;
            this.authorization = authorization;
        }

        [HttpGet("novo/{tipo}")]
        [SwaggerResponse(201, "Nova tipo de saída", typeof(Result<string>))]
        [ProducesResponseType(typeof(Result<string>), 201)]
        public async Task<IActionResult> Novo(string tipo)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _configuration.Novo(tipo, isToken.Email!);

            if (result.Succeeded)
                return CreatedAtAction(nameof(Novo), result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }


        [HttpPost("listar")]
        [SwaggerResponse(200, "Paginação das tipos de saída.", typeof(Result<Paginacao<TiposSaidaDto>>))]
        [ProducesResponseType(typeof(Result<Paginacao<TiposSaidaDto>>), 200)]
        public async Task<IActionResult> Paginado(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _configuration.Paginacao(wrapper, isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("listar-todos")]
        [SwaggerResponse(200, "Lista tipos de saída.", typeof(Result<List<TiposSaidaDto>>))]
        [ProducesResponseType(typeof(Result<List<TiposSaidaDto>>), 200)]
        public async Task<IActionResult> ListarTodos()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _configuration.ListarTodos(isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("editar")]
        [SwaggerResponse(200, "Editar tipo de saída", typeof(Result<string>))]
        [ProducesResponseType(typeof(Result<string>), 200)]
        public async Task<IActionResult> Editar(TiposSaidaDto area)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _configuration.Editar(area, isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("detalhar/{id}")]
        [SwaggerResponse(200, "Buscar tipo de saída", typeof(Result<TiposSaidaDto>))]
        [ProducesResponseType(typeof(Result<TiposSaidaDto>), 200)]
        public async Task<IActionResult> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _configuration.Detalhar(id, isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
