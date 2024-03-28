using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoController(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        [HttpPost("registrar-transacao")]
        public async Task<IActionResult> Registrar(TransacaoDto dto)
        {
            return Accepted(await _transacaoRepository.novo(dto));
        }

        [HttpPost("listar-transacao")]
        public async Task<IActionResult> ListarTransacao(PageWrapper wrapper)
        {
            return Ok(await _transacaoRepository.Paginacao(wrapper));
        }
    }
}
