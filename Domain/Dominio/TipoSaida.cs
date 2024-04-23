namespace Domain.Dominio
{
    public class TipoSaida
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required Contrato Contrato { get; set; }
    }
}
