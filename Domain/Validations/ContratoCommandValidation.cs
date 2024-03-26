using Domain.Command;

namespace Domain.Validations
{
    public class ContratoCommandValidation : ContratoCustomerCommand<ContratoCommand>
    {
        public ContratoCommandValidation()
        {
            Validate();
        }
    }
}
