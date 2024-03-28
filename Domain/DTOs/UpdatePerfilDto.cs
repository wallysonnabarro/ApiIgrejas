namespace Domain.DTOs
{
    public class UpdatePerfilDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required int Status { get; set; }
        public required List<int> Transacoes { get; set; }
    }
}
