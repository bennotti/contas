using System;
using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JI = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Contas.Core.ViewModels
{
    public class ResultBaseViewModel
    {
        [J("result")]
        public bool Result { get; set; }

        [J("msg")]
        public string Msg { get; set; }
    }
}
