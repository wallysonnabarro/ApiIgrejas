namespace Domain.DTOs
{
    public class ContratoDto
    {
        public required string Empresa { get; set; }
        public required string RazaoSocia { get; set; }
        public required string CNPJ { get; set; }
        public required string Responsavel { get; set; }
        public required string Telefone { get; set; }
        public required string Cep { get; set; }
        public required string Logradouro { get; set; }
        public string? Complemento { get; set; }
        public required string Bairro { get; set; }
        public required string Localidade { get; set; }
        public required string Uf { get; set; }
        public string? Ibge { get; set; }
        public string? Gia { get; set; }
        public string? Ddd { get; set; }
        public string? Siafi { get; set; }
    }
}
