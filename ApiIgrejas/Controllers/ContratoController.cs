using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerResponse(201, "Novo contrato", typeof(Result<Contrato>))]
        [ProducesResponseType(typeof(Result<Contrato>), 201)]
        public async Task<IActionResult> NovoContrato(ContratoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _contratoRepository.NovoContrato(dto);

            if (result.Succeeded)
                return Created("Result", result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("listar-contratos")]
        [SwaggerResponse(200, "Paginação de contratos", typeof(Result<Paginacao<Contrato>>))]
        [ProducesResponseType(typeof(Result<Paginacao<Contrato>>), 200)]
        public async Task<IActionResult> ListarContratos(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _contratoRepository.Paginacao(wrapper);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("update/{id}")]
        [SwaggerResponse(200, "Atualização do contrato", typeof(Result<Contrato>))]
        [ProducesResponseType(typeof(Result<Contrato>), 200)]
        public async Task<IActionResult> UpdateContrato(ContratoDto dto, int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _contratoRepository.Update(dto, id);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }


        [HttpGet("contratos-ativos")]
        [SwaggerResponse(200, "ALista de contratos ativos contrato", typeof(Result<Contrato>))]
        [ProducesResponseType(typeof(Result<Contrato>), 200)]
        public async Task<IActionResult> ContratosAtivos()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _contratoRepository.GetList();

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
