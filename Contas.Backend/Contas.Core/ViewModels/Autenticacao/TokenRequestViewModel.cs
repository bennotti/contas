using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.ViewModels.Authentication
{
    public class TokenRequestViewModel
    {
        public TokenRequestViewModel() { }

        public TokenRequestViewModel (string nameIdentifier, string role = "", string ipAddress = "")
        {
            NameIdentifier = nameIdentifier;
            Role = role;
            IpAdress = ipAddress;
        }

        public string NameIdentifier { get; set; }
        public string Role { get; set; }
        public string IpAdress { get; set; }
    }
}
