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
        [SwaggerResponse(201, "Novo conectado", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 201)]
        public async Task<Result<bool>> novoConectado(FichaConectadoDto dto)
        {
            return await _fichaRepository.NovoConectado(dto);
        }

        [HttpPost("novo-lider")]
        [SwaggerResponse(201, "Novo voluntário", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 201)]
        public async Task<Result<bool>> novolider(FichaLiderDto dto)
        {
            return await _fichaRepository.NovoLider(dto);
        }

        [HttpPost("lista-inscricoes")]
        [SwaggerResponse(200, "Buscar ficha", typeof(Result<FichaPagamento>))]
        [ProducesResponseType(typeof(Result<FichaPagamento>), 200)]
        public async Task<IActionResult> GetFichasInscricoes(FichaParametros parametros)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _fichaRepository.GetFichasInscricoes(parametros);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("lista-inscricoes-nao-confirmados")]
        [SwaggerResponse(200, "Buscar fichas não confirmados", typeof(Result<FichaPagamento>))]
        [ProducesResponseType(typeof(Result<FichaPagamento>), 200)]
        public async Task<IActionResult> GetFichasInscricoesNaoConfirmados(FichaParametros parametros)
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _fichaRepository.GetFichasInscricoesNaoconfirmados(parametros);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
