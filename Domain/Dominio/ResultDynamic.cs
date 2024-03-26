namespace Domain.Dominio
{
    public class ResultDynamic
    {
        public Usuario? User { get; set; }
        public required SignInResultado SignInResultado { get; set; }
    }
}
