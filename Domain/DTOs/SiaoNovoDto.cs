namespace Domain.DTOs
{
    public class SiaoNovoDto
    {
        public required string Evento { get; set; }
        public required string Coordenadores { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public string? Descricao { get; set; }
        public int Status { get; set; }
    }
}
