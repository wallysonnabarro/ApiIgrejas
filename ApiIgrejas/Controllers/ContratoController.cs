using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    [Authorize(Roles = "DESENVOLVEDOR")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly IContratoRepository _contratoRepository;

        public ContratoController(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        [HttpPost("novo")]
        public async Task<IActionResult> NovoContrato(ContratoDto dto)
        {
            var result = await _contratoRepository.NovoContrato(dto);

            return Created("Result", result);
        }

        [HttpPost("listar-contratos")]
        public async Task<IActionResult> ListarContratos(PageWrapper wrapper)
        {
            return Accepted(await _contratoRepository.Paginacao(wrapper));
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateContrato(ContratoDto dto, int id)
        {
            return Accepted(await _contratoRepository.Update(dto, id));
        }
    }
}
