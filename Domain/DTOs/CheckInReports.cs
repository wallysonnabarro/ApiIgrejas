namespace Domain.DTOs
{
    public class CheckInReports
    {
        public required string Tribo { get; set; }
        public required string Nome { get; set; }
        public string? Area { get; set; }
        public int Sexo { get; set; }
        public int Confirmado { get; set; }
    }
}
