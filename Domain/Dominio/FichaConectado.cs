﻿namespace Domain.Dominio
{
    public class FichaConectado
    {
        public int Id { get; set; }
        public required TriboEquipe Tribo { get; set; }
        public required string Lider { get; set; }
        public required string Cep { get; set; }
        public required string Endereco { get; set; }
        public required string Nome { get; set; }
        public int Sexo { get; set; }
        public int EstadoCivil { get; set; }
        public DateTime Nascimento { get; set; }
        public required string Telefone { get; set; }
        public required string Email { get; set; }
        public string? ContatoEmergencial { get; set; }
        public bool Crianca { get; set; }
        public bool Cuidados { get; set; }
        public int Idade { get; set; }
        public string? DescricaoCuidados { get; set; }
        public required Siao Siao { get; set; }
        public int Confirmacao { get; set; }
    }
}
