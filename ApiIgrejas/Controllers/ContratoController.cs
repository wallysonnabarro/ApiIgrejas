using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Authorize(Roles = "DESENVOLVEDOR")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IAuthorization authorization;

        public ContratoController(IContratoRepository contratoRepository, IAuthorization authorization)
        {
            _contratoRepository = contratoRepository;
            this.authorization = authorization;
        }

        [HttpPost("novo")]
        public async Task<IActionResult> NovoContrato(ContratoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _contratoRepository.NovoContrato(dto);

            if (result.Succeeded)
                return Created("Result", result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("listar-contratos")]
        public async Task<IActionResult> ListarContratos(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _contratoRepository.Paginacao(wrapper);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateContrato(ContratoDto dto, int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _contratoRepository.Update(dto, id);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
