using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiIgrejas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly IAuthorization authorization;
        private readonly IPagamentoRepository pagamentoRepository;

        public PagamentosController(IAuthorization authorization, IPagamentoRepository pagamentoRepository)
        {
            this.authorization = authorization;
            this.pagamentoRepository = pagamentoRepository;
        }

        [HttpPost("confirmar")]
        [SwaggerResponse(201, "Novo pagamento", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 201)]
        public async Task<IActionResult> Confirmar(PagamentoDto dto)
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.Confirmar(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Created("Confirmar", result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("trasnferir")]
        [SwaggerResponse(200, "Transferir pagamento", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> Transferir(TransferenciaDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.Transferidor(dto.IdRecebedor, dto.IdTransferidor, dto.Tipo, isToken.Result.Email!, dto.Siao, dto.Obs);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("cancelar")]
        [SwaggerResponse(200, "Cancelar pagamento", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> Cancelar(PagamentoCancelarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.Cancelar(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("buscar-pagamento")]
        [SwaggerResponse(200, "Buscar pagamento", typeof(Result<PagamentoAtualizarDto>))]
        [ProducesResponseType(typeof(Result<PagamentoAtualizarDto>), 200)]
        public async Task<IActionResult> Buscar(PagamentoCancelarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await pagamentoRepository.BuscarAtualizar(dto);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("atualizar-pagamento")]
        [SwaggerResponse(200, "Atualizar pagamento", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> Atualizar(PagamentoAtualizarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await pagamentoRepository.Atualizar(dto);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("buscar-pagamentos/{id}")]
        [SwaggerResponse(200, "Buscar pagamento", typeof(Result<PagamentoDto>))]
        [ProducesResponseType(typeof(Result<PagamentoDto>), 200)]
        public async Task<IActionResult> GetPagamentos(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await pagamentoRepository.GetPagamento(id);

            if (result.Succeeded)
                return Ok(result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("registra-lista-saida")]
        [SwaggerResponse(201, "Novo lista de saida de pagamentos", typeof(Result<string>))]
        [ProducesResponseType(typeof(Result<string>), 201)]
        public async Task<IActionResult> RegistrarListaSaida(List<ItemPagamentoSaidaDto> dto)
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.RegistrarListaSaida(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Created("Confirmar", result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
