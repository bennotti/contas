using Contas.Core.Models;
using Contas.Core.Services.Interfaces;
using Contas.Core.ViewModels.Authentication;
using Contas.Core.ViewModels.Commons;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Services.Commands
{
    public class TokenServices : ITokenServices
    {
        private ApplicationDbContext _db;

        public TokenServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<string> GenerateToken(TokenRequestViewModel user, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var dataExpiracao = DateTime.UtcNow.AddHours(12);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.NameIdentifier),
                    new Claim(ClaimTypes.Role, !string.IsNullOrEmpty(user.Role) ? user.Role : ""),
                }),
                Expires = dataExpiracao,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //salvar token no banco
            _db.JwtTokens.Add(new JwtTokens
            {
                UsuarioId = user.NameIdentifier,
                Token = tokenHandler.WriteToken(token),
                DataExpiracao = dataExpiracao,
                IpCriacao = user.IpAdress
            });
            await _db.SaveChangesAsync();

            return tokenHandler.WriteToken(token);
        }
    }
}
