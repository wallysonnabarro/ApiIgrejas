namespace Domain.Dominio
{
    public class Contrato : IEntity
    {
        public int Id { get; set; }
        public required string Empresa { get; set; }
        public required string RazaoSocia { get; set; }
        public required string CNPJ { get; set; }
        public required string Responsavel { get; set; }
        public required string Telefone { get; set; }
        public required DateTime Registro { get; set; }
        public required DateTime Vencimento { get; set; }
        public required int Status { get; set; }


        //navegação
        public required Endereco Endereco { get; set; }
    }
}
