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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportServices _reportServices;

        public PagamentosController(IAuthorization authorization, IPagamentoRepository pagamentoRepository, IWebHostEnvironment webHostEnvironment, IReportServices reportServices)
        {
            this.authorization = authorization;
            this.pagamentoRepository = pagamentoRepository;
            _webHostEnvironment = webHostEnvironment;
            _reportServices = reportServices;
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
        [SwaggerResponse(200, "Buscar pagamento", typeof(Result<List<PagamentosDto>>))]
        [ProducesResponseType(typeof(Result<List<PagamentosDto>>), 200)]
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

        [HttpPost("registra-lista-saida/{id}")]
        [SwaggerResponse(201, "Novo lista de saida de pagamentos", typeof(Result<string>))]
        [ProducesResponseType(typeof(Result<string>), 201)]
        public async Task<IActionResult> RegistrarListaSaida(int id, List<ItemPagamentoSaidaDto> dto)
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await pagamentoRepository.RegistrarListaSaida(dto, isToken.Result.Email!, id);

            if (result.Succeeded)
                return Created("Confirmar", result);
            else return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }


        [HttpGet("pagamentos-voluntarios-excel/{id}")]
        [SwaggerResponse(201, "Excel com dados dos pagamentos", typeof(Result<byte[]>))]
        [ProducesResponseType(typeof(Result<byte[]>), 201)]
        public async Task<IActionResult> ExcelReportPgamentosVoluntarios(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            //Pegar a lista vinculada ao envento com todos os inscritos e os pagamentos
            var lista = await pagamentoRepository.ListaPagamentoVoluntariosExcel(id);

            if (lista.Succeeded) return Ok(lista.Dados);
            else return BadRequest(new { mensagem = lista.Errors.Min(x => x.mensagem) });
        }


        [HttpGet("pagamentos-conectados-excel/{id}")]
        [SwaggerResponse(201, "Excel com dados dos pagamentos", typeof(Result<byte[]>))]
        [ProducesResponseType(typeof(Result<byte[]>), 201)]
        public async Task<IActionResult> ExcelReportPgamentosConectados(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            //Pegar a lista vinculada ao envento com todos os inscritos e os pagamentos
            var lista = await pagamentoRepository.ListaPagamentoConcetadosExcel(id);

            if (lista.Succeeded) return Ok(lista.Dados);
            else return BadRequest(new { mensagem = lista.Errors.Min(x => x.mensagem) });
        }
    }
}
