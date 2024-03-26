namespace Domain.Dominio
{
    public class Result<T>
    {
        private static readonly Result<T> _success = new Result<T> { Succeeded = true };
        private readonly List<Erros> _errors = new List<Erros>();
        private T? _dados { get; set; }


        public IEnumerable<Erros> Errors => _errors;
        public bool Succeeded { get; protected set; }
        public static Result<T> Success => _success;
        public T Dados => _dados!;

        /// <summary>
        /// Retorno os dados com sucesso
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public static Result<T> Sucesso(T dados)
        {
            var result = new Result<T> { Succeeded = true };

            if (dados != null)
            {
                result._dados = dados;
            }
            return result;
        }

        /// <summary>
        /// Retorna uma lista de erros personalizadas
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<T> Failed(List<Erros> errors)
        {
            var result = new Result<T> { Succeeded = false };

            if (errors != null)
            {
                result._errors.AddRange(errors);
            }

            return result;
        }
    }
}
