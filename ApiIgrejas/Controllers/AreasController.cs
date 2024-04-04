using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreasRepository _areasRepository;

        public AreasController(IAreasRepository areasRepository)
        {
            _areasRepository = areasRepository;
        }

        [HttpPost("novo")]
        public async Task<Result<bool>> Novo(AreaDto dto)
        {
            return await _areasRepository.Novo(dto);
        }

        [HttpPost("listar")]
        public async Task<Result<Paginacao<Area>>> Paginado(PageWrapper wrapper)
        {
            return await _areasRepository.Paginacao(wrapper);
        }

        [HttpPost("editar")]
        public async Task<Result<bool>> Editar(Area area)
        {
            return await _areasRepository.Editar(area);
        }

        [HttpPost("detalhar/{id}")]
        public async Task<Result<Area>> Detalhar(int id)
        {
            return await _areasRepository.Detalhar(id);
        }
    }
}
