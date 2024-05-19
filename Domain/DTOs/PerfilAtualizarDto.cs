
namespace Domain.DTOs
{
    public class PerfilAtualizarDto
    {
        public int Id { get; set; }
        public string perfilName { get; set; }
        public int statusSelecionadoId { get; set; }
        public int contratoSelecionadoId { get; set; }
        public bool tribosEquipes { get; set; }
        public bool membros { get; set; }
        public bool cadastroEvento { get; set; }
        public bool eventosSele { get; set; }
        public bool area { get; set; }
        public bool inscricoes { get; set; }
        public bool inscricoesVoluntarios { get; set; }
        public bool administracoe { get; set; }
        public bool novoUsuario { get; set; }
        public bool redefinirSenha { get; set; }
        public bool redefinirAcesso { get; set; }
        public bool fechamentoPagamentos { get; set; }
        public bool fechamentoEvento { get; set; }
        public bool SaidaPagamentos { get; set; }
        public bool ofertasEvento { get; set; }
        public bool lanchonete { get; set; }
        public bool financeiro { get; set; }
        public bool registrarFinanceiro { get; set; }
        public bool despesasObrigações { get; set; }
        public bool visualizarFinanceiro { get; set; }
        public bool tiposSaida { get; set; }
        public bool logout { get; set; }
        public bool login { get; set; }
    }
}
