using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TribosController : ControllerBase
    {
        private readonly ITriboEquipesRepository _triboRepository;

        public TribosController(ITriboEquipesRepository triboRepository)
        {
            _triboRepository = triboRepository;
        }

        [Authorize]
        [HttpPost("getAll")]
        public async Task<Result<Paginacao<TriboEquipe>>> GetAll(PageWrapper wrapper)
        {
            return await _triboRepository.Paginacao(wrapper);
        }

        [Authorize]
        [HttpPost("novo")]
        public async Task<Result<TriboEquipe>> Novo(TriboNovoDto dto)
        {
            return await _triboRepository.Novo(dto);
        }

        [Authorize]
        [HttpPost("detalhar/{id}")]
        public async Task<Result<TriboEquipe>> Detalhar(int id)
        {
            return await _triboRepository.Detalhar(id);
        }

        [Authorize]
        [HttpPost("editar")]
        public async Task<Result<TriboEquipe>> Editar(TriboEquipe tribo)
        {
            return await _triboRepository.Editar(tribo);
        }

        [HttpGet("lista-selected")]
        public async Task<Result<List<TriboSelectede>>> ListaSelected()
        {
            return await _triboRepository.ListaSelected();
        }
    }
}
