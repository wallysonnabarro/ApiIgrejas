using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerResponse(201, "Novo perfil", typeof(Result<int>))]
        [ProducesResponseType(typeof(Result<int>), 201)]
        public async Task<IActionResult> NovaPermissao(PerfilDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var identidade = await _roleRepository.Insert(dto, isToken.Email!);

            if (identidade.Succeeded)
                return CreatedAtAction(nameof(NovaPermissao), new { id = identidade.Dados });
            else
                return BadRequest(new { mensagem = identidade.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("perfil-novo")]
        [SwaggerResponse(201, "Novo perfil", typeof(Result<int>))]
        [ProducesResponseType(typeof(Result<int>), 201)]
        public async Task<IActionResult> NovoPerfil(PerfilNovoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var identidade = await _roleRepository.Insert(dto, isToken.Email!);

            if (identidade.Succeeded)
                return CreatedAtAction(nameof(NovaPermissao), new { id = identidade.Dados });
            else
                return BadRequest(new { mensagem = identidade.Errors.Min(x => x.mensagem) });
        }


        [HttpPost("perfil-atualizar")]
        [SwaggerResponse(201, "Novo perfil", typeof(Result<int>))]
        [ProducesResponseType(typeof(Result<int>), 201)]
        public async Task<IActionResult> AtualizarPerfil(PerfilAtualizarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var identidade = await _roleRepository.Update(dto, isToken.Email!);

            if (identidade.Succeeded)
                return CreatedAtAction(nameof(NovaPermissao), new { id = identidade.Dados });
            else
                return BadRequest(new { mensagem = identidade.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("lista-paginada")]
        [SwaggerResponse(200, "Lista perfil", typeof(Result<Paginacao<PerfilListaPaginadaDto>>))]
        [ProducesResponseType(typeof(Result<Paginacao<PerfilListaPaginadaDto>>), 200)]
        public async Task<IActionResult> ListaPaginada(PageWrapper dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var paginacao = await _roleRepository.Paginacao(dto, isToken.Email!);

            if (paginacao.Succeeded)
                return Ok(paginacao);
            else
                return BadRequest(new { mensagem = paginacao.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("update-perfil")]
        [SwaggerResponse(200, "Atualizar perfil", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> UpdatePerfil(UpdatePerfilDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var result = await _roleRepository.Update(dto);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("perfil")]
        [SwaggerResponse(200, "Buscar perfil", typeof(Result<PerfilListaPaginadaDto>))]
        [ProducesResponseType(typeof(Result<PerfilListaPaginadaDto>), 200)]
        public async Task<IActionResult> Perfil(PerfilUnicoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _roleRepository.Get(dto.Perfils, isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("perfil-listaSelected")]
        [SwaggerResponse(200, "Buscar lista de perfil", typeof(Result<PerfilSelectedDto>))]
        [ProducesResponseType(typeof(Result<PerfilSelectedDto>), 200)]
        public async Task<IActionResult> Perfil()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this.authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = await authorization.DadosToken(token);

            var result = await _roleRepository.GetList(isToken.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }
    }
}
