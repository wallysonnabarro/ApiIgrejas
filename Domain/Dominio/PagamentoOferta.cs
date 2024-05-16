namespace Domain.Dominio
{
    public class PagamentoOferta
    {
        public int Id { get; set; }
        public string Forma { get; set; }
        public Usuario Usuario  { get; set; }
        public decimal Valor { get; set; }
        public Evento Evento { get; set; }
    }
}
