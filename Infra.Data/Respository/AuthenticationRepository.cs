using Domain.Dominio;
using Infra.Data.Interfaces;
using Service.Interface;

namespace Infra.Data.Respository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IUsuarioRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationServices _authenticationService;
        private readonly ITokenService _tokenService;

        public AuthenticationRepository(IUsuarioRepository userRepository, IAuthenticationServices authenticationService, ITokenService tokenService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _roleRepository = roleRepository;
        }

        public async Task<Token> AuthenticateUser(string email, string senha)
        {
            var user = await _userRepository.GetUserByEmail(email);

            Token token = new();

            if (user.SignInResultado.Succeeded)
            {
                var passValid = await _authenticationService.VerifyPassword(senha, user.User!.PasswordHash, user.User.Salt);

                if (user.User.Tentativas != null && user.User.Tentativas == 3)
                {
                    token.Resultado = SignInResultado.LockedOut;
                }
                else if (user.User.EmailConfirmed == false)
                {
                    token.Resultado = SignInResultado.EmailConfirmRequired;
                }
                else if (passValid.Succeeded)
                {
                    var role = await _roleRepository.Get(user.Role!.Id);

                    token = await _tokenService.GenerateToken(user.User, role);
                    token.Resultado = SignInResultado.Success;
                }
                else
                {
                    var locked = await _userRepository.UpdateAcessLock(user.User.Id);
                    if (locked.Succeeded)
                    {
                        token.Resultado = SignInResultado.NotAllowed;
                    }
                }
            }

            return token;
        }
    }
}
