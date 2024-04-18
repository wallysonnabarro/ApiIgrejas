namespace Domain.Dominio
{
    public class TriboEquipe : IEntity
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public int? Status { get; set; }
        public required Contrato Contrato { get; set; }
    }
}
