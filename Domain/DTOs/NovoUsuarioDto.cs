
namespace Domain.DTOs
{
    public class NovoUsuarioDto
    {
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public int Role { get; set; }
        public int Contrato { get; set; }
        public int Tribo { get; set; }
    }
}
