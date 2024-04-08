namespace Domain.Dominio
{
    public class Pagamento : IEntity
    {
        public int Id { get; set; }
        public decimal? Dinheiro { get; set; }
        public decimal? Debito { get; set; }
        public decimal? Credito { get; set; }
        public decimal? CreditoParcelado { get; set; }
        public int Parcelas { get; set; }
        public decimal? Pix { get; set; }
        public int Desistente { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal? Receber { get; set; }
        public decimal? Descontar { get; set; }
        public string? Observacao { get; set; }
        public int Cancelado { get; set; }
        public int Transferido { get; set; }
        public DateTime? DtTransferencia { get; set; }
        public string? ObsTransferencia { get; set; }
        public FichaConectado? FichaConsumidor { get; set; }
        public FichaLider? Voluntario { get; set; }
        public required Evento Evento { get; set; }
        public required Usuario Usuario { get; set; }
    }
}
