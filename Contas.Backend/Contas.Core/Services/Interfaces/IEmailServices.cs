using Contas.Core.ViewModels.Commons;
using Contas.Core.ViewModels.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Services.Interfaces
{
    public interface IEmailServices
    {
        Task<SendEmailResult> EnviarEmail(string email, string assunto, string mensagem);
    }
}
