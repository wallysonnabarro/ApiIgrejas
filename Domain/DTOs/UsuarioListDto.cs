using Domain.Dominio;
using Microsoft.AspNetCore.Identity;

namespace Domain.DTOs
{
    public class UsuarioListDto
    {
        public required string Nome { get; set; }
        public required string Cpf { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public required TriboEquipe TriboEquipe { get; set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        [ProtectedPersonalData]
        public virtual required string UserName { get; set; }
        [ProtectedPersonalData]
        public virtual required string Email { get; set; }

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        [ProtectedPersonalData]
        public virtual string? PhoneNumber { get; set; }
    }
}
