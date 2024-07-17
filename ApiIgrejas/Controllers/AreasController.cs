using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpPost("novo")]
        [SwaggerResponse(201, "Nova área", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 201)]
        public async Task<IActionResult> Novo(AreaDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Novo(dto, isToken.Email!);

            if (result.Succeeded)
                return CreatedAtAction(nameof(Novo), result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("listar")]
        [SwaggerResponse(200, "Paginação das áreas.", typeof(Result<Paginacao<Area>>))]
        [ProducesResponseType(typeof(Result<Paginacao<Area>>), 200)]
        public async Task<IActionResult> Paginado(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Paginacao(wrapper, isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("editar")]
        [SwaggerResponse(200, "Editar área", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> Editar(Area area)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Editar(area, isToken.Email);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("detalhar/{id}")]
        [SwaggerResponse(200, "Buscar área", typeof(Result<Area>))]
        [ProducesResponseType(typeof(Result<Area>), 200)]
        public async Task<IActionResult> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _areasRepository.Detalhar(id, isToken.Email);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("getAreas/{token}")]
        [SwaggerResponse(200, "Lista de áreas", typeof(Result<List<Area>>))]
        [ProducesResponseType(typeof(Result<List<Area>>), 200)]
        public async Task<Result<List<Area>>> getAreas(string token)
        {
            return await _areasRepository.GetAll(token);
        }
    }
}
