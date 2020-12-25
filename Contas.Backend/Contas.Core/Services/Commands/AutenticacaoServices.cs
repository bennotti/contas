using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Contas.Core.Extensions;
using Contas.Core.Models;
using Contas.Core.Services.Interfaces;
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Authentication;
using Contas.Core.ViewModels.Usuarios;
using Contas.Core.ViewModels.Email;
using System.Threading;
using System.Security.Cryptography;
using Contas.Core.ViewModels.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Contas.Core.Services.Commands
{
    public class AutenticacaoServices : IAutenticacaoServices
    {
        private ApplicationDbContext _db;
        private readonly IEmailServices _emailServices;
        private readonly ITokenServices _tokenServices;
        private readonly IOptions<TokenConfigurations> _tokenConfigurations;

        public AutenticacaoServices(ApplicationDbContext db,
            IEmailServices emailServices,
            ITokenServices tokenServices,
            IOptions<TokenConfigurations> tokenConfigurations)
        {
            _emailServices = emailServices;
            _tokenServices = tokenServices;
            _tokenConfigurations = tokenConfigurations;
            _db = db;
        }

        public async Task<ResultBaseViewModel> Login(AutenticacaoRequestViewModel model, string ipAddress)
        {
            ResultBaseViewModel result = null;
            var token = string.Empty;
            try
            {
                var user = await _db.AspNetUsers
                    .Include(t => t.AspNetUserRoles)
                    .Include("AspNetUserRoles.Role")
                    .FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(model.usuario.ToLower()) && x.Ativo);
                if (user == null)
                    throw new Exception($"Login inválido!");

                var loginResult = new PasswordHasher<Models.AspNetUsers>().VerifyHashedPassword(user, user.PasswordHash, model.senha);
                if (PasswordVerificationResult.Failed == loginResult)
                    throw new Exception("Login Inválido");

                token = await _tokenServices.GenerateToken(new TokenRequestViewModel(
                        user.Id, user.AspNetUserRoles.Select(x => x.Role.Name).FirstOrDefault(), ipAddress),
                        _tokenConfigurations.Value.Secret);
                
                result = new AutenticacaoResultViewModel{
                    Result = true,
                    Msg = "Usuário autenticado",
                    Token = token
                };
            }
            catch //(Exception ex)
            {
                //throw ex;
            }

            return result;

        }

    }
}
