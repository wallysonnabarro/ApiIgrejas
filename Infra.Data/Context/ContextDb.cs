using Domain.Dominio;
using Domain.Dominio.menus;
using Domain.Dominio.menus.Submenus;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) : base(options)
        {
        }
        
        public DbSet<Usuario> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TriboEquipe> TribosEquipes { get; set; }
        public DbSet<RoleGroup> RolesGroups { get; set; }
        public DbSet<Grupos> Grupos { get; set; }
        public DbSet<GroupMenu> GroupMenu { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Submenu> Submenus { get; set; }
        public DbSet<MenuSubmenu> MenusSubmenus { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
    }
}
