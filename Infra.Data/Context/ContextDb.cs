using Domain.Dominio;
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
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Transacao> Transacaos { get; set; }
        public DbSet<Siao> Siaos { get; set; }
    }
}
