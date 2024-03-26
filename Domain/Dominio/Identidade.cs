using System.Globalization;
namespace Domain.Dominio
{
    public class Identidade
    {
        private static readonly Identidade _success = new Identidade { Succeeded = true };

        private readonly List<IdentidadeError> _errors = new List<IdentidadeError>();

        public IEnumerable<IdentidadeError> Errors => _errors;
        public bool Succeeded { get; protected set; }
        public static Identidade Success => _success;

        public static Identidade Failed(params IdentidadeError[] errors)
        {
            var result = new Identidade { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        public override string ToString()
        {
            return Succeeded ?
            "Succeeded" :
                   string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}
