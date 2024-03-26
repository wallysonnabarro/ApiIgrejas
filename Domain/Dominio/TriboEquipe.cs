using System.ComponentModel.DataAnnotations;

namespace Domain.Dominio
{
    public class TriboEquipe : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Tribo / Equipe")]
        public required string Nome { get; set; }
        public int? Status { get; set; }

        public TriboEquipe UpperCase()
        {
            TriboEquipe triboEquipe = new()
            {
                Nome = Nome.ToUpper(),
            };
            return triboEquipe;
        }

        public TriboEquipe UpperCaseEdit()
        {
            TriboEquipe triboEquipe = new()
            {
                Nome = Nome.ToUpper(),
            };
            return triboEquipe;
        }
    }
}
