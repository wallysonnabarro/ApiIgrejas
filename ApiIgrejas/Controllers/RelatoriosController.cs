using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatoriosRepository _repository;
        private readonly IRelatorioServices _services;
        private readonly IAuthorization authorization;

        public RelatoriosController(IRelatoriosRepository repository, IRelatorioServices services, IAuthorization authorization)
        {
            _repository = repository;
            _services = services;
            this.authorization = authorization;
        }

        [HttpPost("get-lista-voluntarios")]
        public async Task<IActionResult> GetListHomens(ParametrosEvento dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _repository.GetByIdHomens(dto);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("get-lista-conectados")]
        public async Task<IActionResult> GetListConectados(ParametrosConectados dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _repository.GetByConectados(dto);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("reader-conectado-novo")]
        public async Task<IActionResult> NovoConectado(DadosReaderRelatorioDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _repository.NovoConectadoReader(dto);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
