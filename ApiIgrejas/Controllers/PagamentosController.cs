using Domain.Dominio;
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
        public async Task<IActionResult> Confirmar(PagamentoDto dto)
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.Confirmar(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("trasnferir")]
        public async Task<IActionResult> Transferir(TransferenciaDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.Transferidor(dto.IdRecebedor, dto.IdTransferidor, dto.Tipo, isToken.Result.Email!, dto.Siao, dto.Obs);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("cancelar")]
        public async Task<IActionResult> Cancelar(PagamentoCancelarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.Cancelar(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("buscar-pagamento")]
        public async Task<IActionResult> Buscar(PagamentoCancelarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await pagamentoRepository.BuscarAtualizar(dto);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("atualizar-pagamento")]
        public async Task<IActionResult> Atualizar(PagamentoAtualizarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await pagamentoRepository.Atualizar(dto);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("buscar-pagamentos/{id}")]
        public async Task<IActionResult> GetPagamentos(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await pagamentoRepository.GetPagamento(id);

            if (result.Succeeded)
                return Ok(resultToken);
            else return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
