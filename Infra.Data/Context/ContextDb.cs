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
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Area> AreasSet { get; set; }
        public DbSet<FichaConectado> FichasConectados { get; set; }
        public DbSet<FichaLider> FichasLider { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<DadosReaderRelatorio> DadosReaderRelatorios { get; set; }
        public DbSet<TipoSaida> TiposSaidas { get; set; }
        public DbSet<PagamentoSaida> PagamentoSaidas { get; set; }
    }
}
