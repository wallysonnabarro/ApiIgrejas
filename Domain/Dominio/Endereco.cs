namespace Domain.Dominio
{
    public class Endereco : IEntity
    {
        public int Id { get; set; }
        public required string Cep { get; set; }
        public required string logradouro { get; set; }
        public string? complemento { get; set; }
        public required string bairro { get; set; }
        public required string localidade { get; set; }
        public required string uf { get; set; }
        public string? ibge { get; set; }
        public string? gia { get; set; }
        public string? ddd { get; set; }
        public string? siafi { get; set; }
    }
}
