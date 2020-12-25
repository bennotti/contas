using System.Collections.Generic;
using System.Threading.Tasks;
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Usuarios;

namespace Contas.Core.Services.Interfaces
{
    public interface IUsuariosServices
    {
        Task<MeusDadosResultViewModel> ObterUsuarioLogado();
    }
}
