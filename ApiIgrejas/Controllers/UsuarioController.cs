using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ContextDb _db;
        private readonly IRoleRepository _roleRepository;
        private readonly IUsuarioRepository _userManager;

        public UsuarioController(ContextDb db, IRoleRepository roleRepository, IUsuarioRepository userManager)
        {
            _db = db;
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Este endpoint, deverá ser removido na publicação.
        /// </summary>
        [HttpGet("cadastro-develope")]
        public async Task<Identidade> CadastroDeveloper()
        {
            if (await _db.Users.AnyAsync(x => x.Cpf.Equals("009.873.571-31")))
            {
                return Identidade.Failed(new IdentidadeError { Code = "", Description = "" });
            }

            var contrato = _db.Contratos.FirstOrDefault(x => x.Id == 1);

            if(contrato == null) return Identidade.Failed(new IdentidadeError { Code = "", Description = "" });
            
            if (!await _roleRepository.IsValid("DESENVOLVEDOR")) await _roleRepository.Insert(0);

            var role = await _roleRepository.Get("DESENVOLVEDOR");

            var tribo = _db.TribosEquipes.FirstOrDefaultAsync(x => x.Nome.Equals("MAANAIM"));

            if (tribo == null) return Identidade.Failed(new IdentidadeError { Code = "", Description = "" });

            var triboEquipe = new TriboEquipe { Nome = "MAANAIM", Status = 1 };

            _db.Add(triboEquipe);
            _db.SaveChanges();

            var user = new UsuarioDto()
            {
                Cpf = "009.873.571-31",
                Email = "wallyson.a3@gmail.com",
                NormalizedEmail = "WALLYSON.A3@GMAIL.COM",
                Nome = "Wallyson Lopes",
                NormalizedUserName = "WALLYSON LOPES",
                Contrato = contrato,
                Password = "389419wE1e@",
                Role = role,
                TriboEquipe = triboEquipe,
                UserName = "wallyson.a3@gmail.com",
                TwoFactorEnabled = false,
            };

            return await _userManager.AddUserWithSecurePassword(user);
        }
    }
}
