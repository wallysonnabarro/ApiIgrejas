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
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IAuthorization authorization;

        public TransacaoController(ITransacaoRepository transacaoRepository, IAuthorization authorization)
        {
            _transacaoRepository = transacaoRepository;
            this.authorization = authorization;
        }

        [HttpPost("registrar-transacao")]
        [SwaggerResponse(201, "Nova transação", typeof(Result<Transacao>))]
        [ProducesResponseType(typeof(Result<Transacao>), 201)]
        public async Task<IActionResult> Registrar(TransacaoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _transacaoRepository.novo(dto);

            if (result.Succeeded)
                return Created("Registrar", result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("listar-transacao")]
        [SwaggerResponse(200, "Lista transação", typeof(Result<Paginacao<TransacaoListDto>>))]
        [ProducesResponseType(typeof(Result<Transacao>), 200)]
        public async Task<IActionResult> ListarTransacao(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _transacaoRepository.Paginacao(wrapper);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
