using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.ViewModels.Commons
{
    public class SendEmailResult
    {
        public SendEmailResult()
        {
            this.Success = true;
        }

        public SendEmailResult(string message)
        {
            this.Success = true;
            this.Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
