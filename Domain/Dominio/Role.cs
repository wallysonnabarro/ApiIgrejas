namespace Domain.Dominio
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public int Status { get; set; }

        //navegação
        public virtual ICollection<Transacao> Transacoes { get; set; }
    }
}
