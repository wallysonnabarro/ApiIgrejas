namespace Domain.Dominio
{
    public class Evento
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Coordenadores { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public string? Descricao { get; set; }
        public int Status { get; set; }
        public required string Token { get; set; }
        public required Contrato Contrato { get; set; }
    }
}
