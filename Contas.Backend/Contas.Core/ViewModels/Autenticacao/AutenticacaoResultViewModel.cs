using System;
using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JI = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Contas.Core.ViewModels.Authentication
{
    public class AutenticacaoResultViewModel : ResultBaseViewModel
    {
        [J("token")]
        public string Token { get; set; }
    }
}
