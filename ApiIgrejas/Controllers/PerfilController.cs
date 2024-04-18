using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorization authorization;

        public PerfilController(IRoleRepository roleRepository, IAuthorization authorization)
        {
            _roleRepository = roleRepository;
            this.authorization = authorization;
        }

        [HttpPost("novo-perfil")]
        public async Task<IActionResult> NovaPermissao(PerfilDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var identidade = await _roleRepository.Insert(dto, isToken.Email!);

            if (identidade.Succeeded)
                return CreatedAtAction(nameof(NovaPermissao), new { id = identidade.Dados });
            else
                return StatusCode(500, new { mensagem = identidade.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("lista-paginada")]
        public async Task<IActionResult> ListaPaginada(PageWrapper dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var paginacao = await _roleRepository.Paginacao(dto, isToken.Email!);

            if (paginacao.Succeeded)
                return Ok(paginacao);
            else
                return StatusCode(500, new { mensagem = paginacao.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("update-perfil")]
        public async Task<IActionResult> UpdatePerfil(UpdatePerfilDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _roleRepository.Update(dto);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("perfil")]
        public async Task<IActionResult> Perfil(PerfilUnicoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return BadRequest(string.Empty);

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _roleRepository.Get(dto.Perfils, isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return StatusCode(500, new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
