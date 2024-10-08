﻿using Domain.Dominio;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Service.Services
{
    public class TokenService : ITokenService
    {
        public async Task<Token> GenerateToken(Usuario user, Role role)
        {
            return await Task.Run(() =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(Settings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Nome),
                        new Claim(ClaimTypes.Role, role.Nome),
                        new Claim(ClaimTypes.Email, user.Email)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new Token
                {
                    Toke = tokenHandler.WriteToken(token),
                };
            });
        }

        public async Task<TokenGerenciar> TryValidateToken(string token)
        {
            return await Task.Run(() =>
            {
                ClaimsPrincipal claimsPrincipal;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Settings.Secret);
                TokenGerenciar tokenGerenciado = new TokenGerenciar();

                try
                {
                    SecurityToken validatedToken;
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };

                    claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                    if (claimsPrincipal == null) throw new Exception("Error null");

                    tokenGerenciado = new TokenGerenciar
                    {
                        Nome = claimsPrincipal.FindFirst(ClaimTypes.Name)!.Value,
                        Email = claimsPrincipal.FindFirst(ClaimTypes.Email)!.Value,
                        Role = claimsPrincipal.FindFirst(ClaimTypes.Role)!.Value,
                        IdentidadeResultado = Identidade.Success,
                    };

                    return Task.FromResult(tokenGerenciado);
                }
                catch (Exception ex)
                {
                    tokenGerenciado = new TokenGerenciar
                    {
                        Email = "",
                        Nome = "",
                        IdentidadeResultado = Identidade.Failed(new IdentidadeError { Code = "403", Description = "Token Invalido. Mensagem: " + ex.Message })
                    };
                    return Task.FromResult<TokenGerenciar>(tokenGerenciado);
                }
            });
        }
    }
}