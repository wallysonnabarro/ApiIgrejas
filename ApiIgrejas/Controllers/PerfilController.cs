using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIgrejas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public PerfilController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpPost("novo-perfil")]
        public async Task<IActionResult> NovaPermissao(PerfilDto dto)
        {
            return Accepted(await _roleRepository.Insert(dto));
        }

        [HttpPost("lista-paginada")]
        public async Task<IActionResult> ListaPaginada(PageWrapper dto)
        {
            return Accepted(await _roleRepository.Paginacao(dto));
        }

        [HttpPost("update-perfil")]
        public async Task<IActionResult> UpdatePerfil(UpdatePerfilDto dto)
        {
            return Accepted(await _roleRepository.Update(dto));
        }

        [HttpPost("perfil")]
        public async Task<IActionResult> Perfil(PerfilUnicoDto dto)
        {
            return Accepted(await _roleRepository.Get(dto.Perfils));
        }
    }
}
