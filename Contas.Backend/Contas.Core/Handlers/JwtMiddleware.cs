using Contas.Core.Models;
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Authentication;
using Contas.Core.ViewModels.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Handlers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenConfigurations _tokenConfig;

        public JwtMiddleware(RequestDelegate next,
            TokenConfigurations tokenConfig)
        {
            _tokenConfig = tokenConfig;
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await validateTokenDb(context, dataContext, token);

            await _next(context);
        }

        private async Task validateTokenDb(HttpContext context, ApplicationDbContext dataContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_tokenConfig.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // definir clockskew para zero para que os tokens expirem exatamente no tempo de expiração do token(em vez de 5 minutos depois)
                    ClockSkew = TimeSpan.Zero,
                    SaveSigninToken = true
                }, out SecurityToken validatedToken);

                //validar token no banco de dados
                var jwtTokenBd = await dataContext.JwtTokens.FirstOrDefaultAsync(x => x.Token.Equals(token)
                    && x.DataExpiracao >= DateTime.UtcNow
                    && x.Ativo);

                if (jwtTokenBd != null){
                    // anexar conta ao contexto na validação jwt bem-sucedida
                    context.Items["IsValidToken"] = true;
                }
            }
            catch
            {
                // não faça nada se a validação jwt falhar
                // conta não está anexada ao contexto, então a solicitação não terá acesso a rotas seguras
            }
        }
    }
}
