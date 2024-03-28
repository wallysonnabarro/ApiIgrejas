using Domain.Dominio.menus;
using Domain.Dominio.menus.Submenus;
using Domain.DTOs;
using Infra.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuSubmenuController : ControllerBase
    {
        private readonly ContextDb _db;

        public MenuSubmenuController(ContextDb db)
        {
            _db = db;
        }

        [HttpPost("menus")]
        //[Authorize("DESENVOLVEDOR")]
        public async Task<IActionResult> RegistrarMenus(RegistrarMenusDto dto)
        {
            var menus = new Menus { Label = dto.Label, Route = dto.Route };
            _db.Add(menus);
            await _db.SaveChangesAsync();
            return Ok(menus);
        }

        [HttpGet("listar-menus")]
        //[Authorize("DESENVOLVEDOR")]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _db.Menus.ToListAsync());
        }

        [HttpPost("submenus")]
        //[Authorize("DESENVOLVEDOR")]
        public async Task<IActionResult> RegistrarSubMenus(RegistrarSubmenuDto dto)
        {
            var submenus = new Submenu { Label = dto.Label, Route = dto.Route };
            _db.Add(submenus);
            await _db.SaveChangesAsync();
            return Ok(submenus);
        }

        [HttpGet("listar-submenu")]
        //[Authorize("DESENVOLVEDOR")]
        public async Task<IActionResult> ListarSub()
        {
            return Ok(await _db.Submenus.ToListAsync());
        }
    }
}
