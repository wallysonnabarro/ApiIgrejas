
namespace Domain.Dominio
{
    public class Lanchonete
    {
        public int Id { get; set; }
        public string Forma { get; set; }
        public string Descricao { get; set; }
        public Usuario Usuario { get; set; }
        public decimal Valor { get; set; }
        public Evento Evento { get; set; }
    }
}
