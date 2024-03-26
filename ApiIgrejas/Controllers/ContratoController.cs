using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
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
    }
}
