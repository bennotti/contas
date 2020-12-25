using System;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JI = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Contas.Core.ViewModels.Usuarios
{
    public class MeusDadosResultViewModel: ResultBaseViewModel
    {
        [J("dados")]
        public object Dados { get; set; }
    }
}
