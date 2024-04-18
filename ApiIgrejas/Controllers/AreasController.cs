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
    public class AreasController : ControllerBase
    {
        private readonly IAreasRepository _areasRepository;
        private readonly IAuthorization authorization;

        public AreasController(IAreasRepository areasRepository, IAuthorization authorization)
        {
            _areasRepository = areasRepository;
            this.authorization = authorization;
        }

        [Authorize]
        [HttpPost("novo")]
        public async Task<IActionResult> Novo(AreaDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Novo(dto, isToken.Email!);

            if (result.Succeeded)
                return CreatedAtAction(nameof(Novo), result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("listar")]
        public async Task<IActionResult> Paginado(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Paginacao(wrapper, isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("editar")]
        public async Task<IActionResult> Editar(Area area)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Editar(area, isToken.Email);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [Authorize]
        [HttpPost("detalhar/{id}")]
        public async Task<IActionResult> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Detalhar(id, isToken.Email);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("getAreas")]
        public async Task<Result<List<Area>>> getAreas()
        {
            return await _areasRepository.GetAll();
        }
    }
}
