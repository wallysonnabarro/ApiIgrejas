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
    public class TribosController : ControllerBase
    {
        private readonly ITriboEquipesRepository _triboRepository;

        public TribosController(ITriboEquipesRepository triboRepository)
        {
            _triboRepository = triboRepository;
        }

        [HttpPost("getAll")]
        public async Task<Result<Paginacao<TriboEquipe>>> GetAll(PageWrapper wrapper)
        {
            return await _triboRepository.Paginacao(wrapper);
        }

        [HttpPost("novo")]
        public async Task<Result<TriboEquipe>> Novo(TriboNovoDto dto)
        {
            return await _triboRepository.Novo(dto);
        }

        [HttpPost("detalhar/{id}")]
        public async Task<Result<TriboEquipe>> Detalhar(int id)
        {
            return await _triboRepository.Detalhar(id);
        }

        [HttpPost("editar")]
        public async Task<Result<TriboEquipe>> Editar(TriboEquipe tribo)
        {
            return await _triboRepository.Editar(tribo);
        }
    }
}
