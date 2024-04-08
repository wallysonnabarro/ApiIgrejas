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
        public async Task<Identidade> NovaPermissao(PerfilDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = await authorization.DadosToken(token);

            if (isToken.IdentidadeResultado!.Succeeded)
            {
                return await _roleRepository.Insert(dto, isToken.Email!);
            }
            else
            {
                return Identidade.Failed(new IdentidadeError { Description = isToken.IdentidadeResultado.Errors.Min(x => x.Description) });
            }
        }

        [HttpPost("lista-paginada")]
        public async Task<Result<Paginacao<PerfilListaPaginadaDto>>> ListaPaginada(PageWrapper dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = await authorization.DadosToken(token);

            if (isToken.IdentidadeResultado!.Succeeded)
            {
                return await _roleRepository.Paginacao(dto, isToken.Email!);
            }
            else
            {
                return Result<Paginacao<PerfilListaPaginadaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = isToken.IdentidadeResultado.Errors.Min(x => x.Description), ocorrencia = "", versao = "" } });
            }
        }

        [HttpPost("update-perfil")]
        public async Task<IActionResult> UpdatePerfil(UpdatePerfilDto dto)
        {
            return Accepted(await _roleRepository.Update(dto));
        }

        [HttpPost("perfil")]
        public async Task<Result<PerfilListaPaginadaDto>> Perfil(PerfilUnicoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = await authorization.DadosToken(token);

            if (isToken.IdentidadeResultado!.Succeeded)
            {
                return await _roleRepository.Get(dto.Perfils, isToken.Email!);
            }
            else
            {
                return Result<PerfilListaPaginadaDto>.Failed(new List<Erros> {
                    new Erros { codigo = "", mensagem = isToken.IdentidadeResultado.Errors.Min(x => x.Description), ocorrencia = "", versao = "" } });
            }
        }
    }
}
