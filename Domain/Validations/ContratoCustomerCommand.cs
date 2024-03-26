using Domain.Command;
using Domain.Util;
using FluentValidation;

namespace Domain.Validations
{
    public class ContratoCustomerCommand<T> : AbstractValidator<T> where T : ContratoCommand
    {
        protected void Validate()
        {
            RuleFor(b => b.Empresa)
                .NotNull()
                .WithMessage("O campo empresa não pode estar vazio.");

            RuleFor(b => b.RazaoSocia)
                .NotNull()
                .WithMessage("O campo Razão Social não pode estar vazio.");

            RuleFor(b => b.Responsavel)
                .NotNull()
                .WithMessage("O campo Responsável não pode estar vazio.");

            RuleFor(b => b.Telefone)
                .NotNull()
                .WithMessage("O campo Telefone não pode estar vazio.");

            RuleFor(b => b.Cep)
                .NotNull()
                .WithMessage("O campo CEP não pode estar vazio.");

            RuleFor(b => b.Logradouro)
                .NotNull()
                .WithMessage("O campo Logradouro não pode estar vazio.");

            RuleFor(b => b.Bairro)
                .NotNull()
                .WithMessage("O campo Bairro não pode estar vazio.");

            RuleFor(b => b.Localidade)
                .NotNull()
                .WithMessage("O campo Localidade não pode estar vazio.");

            RuleFor(b => b.Uf)
                .NotNull()
                .WithMessage("O campo Uf não pode estar vazio.");

            RuleFor(b => b.CNPJ)
                .NotNull()
                .When(b => !new ValidarCnpjCPF().Cnpj(b.CNPJ))
                .WithMessage("O campo CNPJ não pode estar vazio.");
        }
    }
}
