using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Contas.Core.Services.Interfaces;
using Contas.Core.ViewModels.Usuarios;
using Contas.Core.Extensions;
using Contas.Core.Models;
using Contas.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Contas.Core.ViewModels.Email;

namespace Contas.Core.Services.Commands
{
    public class UsuariosServices : IUsuariosServices
    {
        private ApplicationDbContext _db;
        private readonly IEmailServices _emailServices;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment env;

        public UsuariosServices(ApplicationDbContext db,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment _env,
            IEmailServices emailServices)
        {
            _db = db;
            _emailServices = emailServices;
            this.env = _env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<MeusDadosResultViewModel> ObterUsuarioLogado()
        {
            return await _db.AspNetUsers
                .Where(x => x.Id.Equals(_httpContextAccessor.GetUserId()))
                .Select(x => new MeusDadosResultViewModel{

                    Result = true,
                    Msg = "Usuário encontrado",
                    Dados = new { Nome = x.FullName, Usuario = x.UserName }

                }).FirstOrDefaultAsync();
        }

    }
}
