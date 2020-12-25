using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.ViewModels.Email
{
    public class NovoEmail
    {
        public string Subject { get; set; } 
        public string To { get; set; }
        public string Message { get; set; }
    }
}
