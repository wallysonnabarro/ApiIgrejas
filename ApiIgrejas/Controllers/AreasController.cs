using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreasRepository _areasRepository;

        public AreasController(IAreasRepository areasRepository)
        {
            _areasRepository = areasRepository;
        }

        [Authorize]
        [HttpPost("novo")]
        public async Task<Result<bool>> Novo(AreaDto dto)
        {
            return await _areasRepository.Novo(dto);
        }

        [Authorize]
        [HttpPost("listar")]
        public async Task<Result<Paginacao<Area>>> Paginado(PageWrapper wrapper)
        {
            return await _areasRepository.Paginacao(wrapper);
        }

        [Authorize]
        [HttpPost("editar")]
        public async Task<Result<bool>> Editar(Area area)
        {
            return await _areasRepository.Editar(area);
        }

        [Authorize]
        [HttpPost("detalhar/{id}")]
        public async Task<Result<Area>> Detalhar(int id)
        {
            return await _areasRepository.Detalhar(id);
        }

        [HttpGet("getAreas")]
        public async Task<Result<List<Area>>> getAreas()
        {
            return await _areasRepository.GetAll();
        }
    }
}
