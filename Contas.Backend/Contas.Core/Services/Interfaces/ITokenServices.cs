using Contas.Core.ViewModels.Authentication;
using Contas.Core.ViewModels.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Services.Interfaces
{
    public interface ITokenServices
    {
        Task<string> GenerateToken(TokenRequestViewModel user, string secret);
    }
}
