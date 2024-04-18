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
    public class FichaController : ControllerBase
    {
        private readonly IFichaRepository _fichaRepository;
        private readonly IAuthorization authorization;

        public FichaController(IFichaRepository fichaRepository, IAuthorization authorization)
        {
            _fichaRepository = fichaRepository;
            this.authorization = authorization;
        }

        [HttpPost("novo-conectado")]
        public async Task<Result<bool>> novoConectado(FichaConectadoDto dto)
        {
            return await _fichaRepository.NovoConectado(dto);
        }

        [HttpPost("novo-lider")]
        public async Task<Result<bool>> novolider(FichaLiderDto dto)
        {
            return await _fichaRepository.NovoLider(dto);
        }

        [Authorize]
        [HttpPost("lista-inscricoes")]
        public async Task<IActionResult> GetFichasInscricoes(FichaParametros parametros)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _fichaRepository.GetFichasInscricoes(parametros);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("lista-inscricoes-nao-confirmados")]
        public async Task<IActionResult> GetFichasInscricoesNaoConfirmados(FichaParametros parametros)
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _fichaRepository.GetFichasInscricoesNaoconfirmados(parametros);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
