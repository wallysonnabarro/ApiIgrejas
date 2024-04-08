using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FichaController : ControllerBase
    {
        private readonly IFichaRepository _fichaRepository;

        public FichaController(IFichaRepository fichaRepository)
        {
            _fichaRepository = fichaRepository;
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
        public async Task<Result<FichaPagamento>> GetFichasInscricoes(FichaParametros parametros)
        {
            return await _fichaRepository.GetFichasInscricoes(parametros);
        }

        [Authorize]
        [HttpPost("lista-inscricoes-nao-confirmados")]
        public async Task<Result<FichaPagamento>> GetFichasInscricoesNaoConfirmados(FichaParametros parametros)
        {
            return await _fichaRepository.GetFichasInscricoesNaoconfirmados(parametros);
        }
    }
}
