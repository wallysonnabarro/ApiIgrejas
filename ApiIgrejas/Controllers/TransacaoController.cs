using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Registrar(TransacaoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _transacaoRepository.novo(dto);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("listar-transacao")]
        public async Task<IActionResult> ListarTransacao(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _transacaoRepository.Paginacao(wrapper);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
