using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerResponse(200, "Buscar Lista de voluntários", typeof(Result<DadosRelatorio<List<CheckInReports>>>))]
        [ProducesResponseType(typeof(Result<DadosRelatorio<List<CheckInReports>>>), 200)]
        public async Task<IActionResult> GetListHomens(ParametrosEvento dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _repository.GetByIdHomens(dto);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("get-lista-conectados")]
        [SwaggerResponse(200, "Buscar Lista de conectados", typeof(Result<FichasDto<List<CheckInReports>>>))]
        [ProducesResponseType(typeof(Result<FichasDto<List<CheckInReports>>>), 200)]
        public async Task<IActionResult> GetListConectados(ParametrosConectados dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _repository.GetByConectados(dto);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("reader-conectado-novo")]
        [SwaggerResponse(201, "Novo conectados", typeof(Result<DadosReaderRelatorio>))]
        [ProducesResponseType(typeof(Result<DadosReaderRelatorio>), 201)]
        public async Task<IActionResult> NovoConectado(DadosReaderRelatorioDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _repository.NovoConectadoReader(dto);

            if (result.Succeeded)
                return Created("NovoConectado", result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
