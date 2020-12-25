using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contas.Core.Models;
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Authentication;
using Contas.Core.ViewModels.Usuarios;

namespace Contas.Core.Services.Interfaces
{
    public interface IAutenticacaoServices
    {
        Task<ResultBaseViewModel> Login(AutenticacaoRequestViewModel model, string ipAddress);
    }
}
