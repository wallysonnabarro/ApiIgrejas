using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TribosController : ControllerBase
    {
        private readonly ITriboEquipesRepository _triboRepository;
        private readonly IAuthorization authorization;

        public TribosController(ITriboEquipesRepository triboRepository, IAuthorization authorization)
        {
            _triboRepository = triboRepository;
            this.authorization = authorization;
        }

        [Authorize]
        [HttpPost("getAll")]
        public async Task<IActionResult> GetAll(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _triboRepository.Paginacao(wrapper, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
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
