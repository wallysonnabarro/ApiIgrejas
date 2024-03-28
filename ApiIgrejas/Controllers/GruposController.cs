using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly IGrupoRepository _grupoRepository;

        public GruposController(IGrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }

        [HttpPost("novo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize]
        public async Task<IActionResult> Novo(GrupoDto dto)
        {
            if (dto != null)
            {
                return Accepted(await _grupoRepository.Novo(dto));
            }
            else
            {
                return BadRequest("Os dados não pode ser nulo ou vázio.");
            }
        }

        [HttpPost("paginacao")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize]
        public async Task<IActionResult> Paginacao(PageWrapper dto)
        {
            if (dto != null)
            {
                return Accepted(await _grupoRepository.Paginacao(dto));
            }
            else
            {
                return BadRequest("Os dados não pode ser nulo ou vázio.");
            }
        }


        [HttpGet("get/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            if (id != 0)
            {
                return Accepted(await _grupoRepository.BuscarPorId(id));
            }
            else
            {
                return BadRequest("Os dados não pode ser nulo ou vázio.");
            }
        }

        [HttpGet("atualizar")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize]
        public async Task<IActionResult> Atualizar(GrupoAtualizarDto dto)
        {
            if (dto != null)
            {
                return Accepted(await _grupoRepository.Atualizar(dto));
            }
            else
            {
                return BadRequest("Os dados não pode ser nulo ou vázio.");
            }
        }
    }
}
