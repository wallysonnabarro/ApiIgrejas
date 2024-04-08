using Domain.Dominio;
using Microsoft.AspNetCore.Identity;

namespace Domain.DTOs
{
    public class UsuarioDto
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

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        public virtual required string NormalizedUserName { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        [ProtectedPersonalData]
        public virtual required string Email { get; set; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        public virtual required string NormalizedEmail { get; set; }


        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public virtual required string Password { get; set; }


        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        [ProtectedPersonalData]
        public virtual string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        /// <value>True if the telephone number has been confirmed, otherwise false.</value>
        [PersonalData]
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        /// <value>True if 2fa is enabled, otherwise false.</value>
        [PersonalData]
        public virtual bool TwoFactorEnabled { get; set; }

        //Relacionamentos
        public required int Role { get; set; }

        public required Contrato Contrato { get; set; }
    }
}
