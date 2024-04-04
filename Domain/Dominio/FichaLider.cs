namespace Domain.Dominio
{
    public class FichaLider
    {
        public int Id { get; set; }
        public required TriboEquipe Tribo { get; set; }
        public required string Nome { get; set; }
        public int Sexo { get; set; }
        public required Siao Siao { get; set; }
    }
}
