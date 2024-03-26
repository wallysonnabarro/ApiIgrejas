using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Domain.Command
{
    public abstract class Command
    {
        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        public virtual bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
