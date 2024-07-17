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
    public class TribosController : ControllerBase
    {
        private readonly ITriboEquipesRepository _triboRepository;
        private readonly IAuthorization authorization;

        public TribosController(ITriboEquipesRepository triboRepository, IAuthorization authorization)
        {
            _triboRepository = triboRepository;
            this.authorization = authorization;
        }

        [HttpPost("getAll")]
        [SwaggerResponse(200, "Lista Tribos e equipes", typeof(Result<Paginacao<TriboEquipe>>))]
        [ProducesResponseType(typeof(Result<Paginacao<TriboEquipe>>), 200)]
        public async Task<IActionResult> GetAll(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _triboRepository.Paginacao(wrapper, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("novo")]
        [SwaggerResponse(200, "Nova tribo / equipe", typeof(Result<TriboEquipe>))]
        [ProducesResponseType(typeof(Result<TriboEquipe>), 200)]
        public async Task<IActionResult> Novo(TriboNovoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _triboRepository.Novo(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Created("Novo", result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("detalhar/{id}")]
        [SwaggerResponse(200, "Buscar tribo/Equipe", typeof(Result<TriboEquipe>))]
        [ProducesResponseType(typeof(Result<TriboEquipe>), 200)]
        public async Task<IActionResult> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _triboRepository.Detalhar(id, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("editar")]
        [SwaggerResponse(200, "Atualizar tribo/Equipe", typeof(Result<TriboEquipe>))]
        [ProducesResponseType(typeof(Result<TriboEquipe>), 200)]
        public async Task<IActionResult> Editar(TriboEquipe tribo)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _triboRepository.Editar(tribo);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("lista-selected/{token}")]
        [SwaggerResponse(200, "Lista tribo/Equipe", typeof(Result<List<TriboEquipe>>))]
        [ProducesResponseType(typeof(Result<List<TriboEquipe>>), 200)]
        public async Task<IActionResult> ListaSelected(string token)
        {
            var result = await _triboRepository.ListaSelected(token);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(result.Errors);
        }


        [HttpGet("lista-selected-all")]
        [SwaggerResponse(200, "Lista tribo/Equipe", typeof(Result<List<TriboEquipe>>))]
        [ProducesResponseType(typeof(Result<List<TriboEquipe>>), 200)]
        public async Task<IActionResult> ListaSelected()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = authorization.DadosToken(token);

            var result = await _triboRepository.ListaSelectedAll(isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(result.Errors);
        }
    }
}
