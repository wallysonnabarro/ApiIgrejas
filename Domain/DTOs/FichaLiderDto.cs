namespace Domain.DTOs
{
    public class FichaLiderDto
    {
        public int Tribo { get; set; }
        public required string Nome { get; set; }
        public int Sexo { get; set; }
        public int Siao { get; set; }
        public int Area { get; set; }
    }

    public class SiaoDto
    {
        public int Id { get; set; }
    }
}
