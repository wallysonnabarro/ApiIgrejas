using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly ISiaoRepository _siaoRepository;

        public EventoController(ISiaoRepository siaoRepository)
        {
            _siaoRepository = siaoRepository;
        }

        [Authorize]
        [HttpPost("novo")]
        public async Task<Result<bool>> Novo(SiaoNovoDto dto)
        {
            return await _siaoRepository.Novo(dto);
        }

        [Authorize]
        [HttpPost("listar")]
        public async Task<Result<Paginacao<Siao>>> Paginado(PageWrapper wrapper)
        {
            return await _siaoRepository.Paginacao(wrapper);
        }

        [Authorize]
        [HttpPost("editar")]
        public async Task<Result<bool>> Editar(Siao siao)
        {
            return await _siaoRepository.Editar(siao);
        }

        [Authorize]
        [HttpPost("{id}/editar-status/{status}")]
        public async Task<Result<bool>> EditarStatus(int id, int status)
        {
            return await _siaoRepository.EditarStatus(id, status);
        }

        [Authorize]
        [HttpPost("detalhar/{id}")]
        public async Task<Result<Siao>> Detalhar(int id)
        {
            return await _siaoRepository.Detalhar(id);
        }

        [HttpGet("evento-andamento")]
        public async Task<Result<List<EventosAtivosDto>>> SiaoEmAndamento()
        {
            return await _siaoRepository.GetEmIniciado();
        }
    }
}
