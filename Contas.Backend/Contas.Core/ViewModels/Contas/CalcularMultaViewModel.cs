using System;
using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JI = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Contas.Core.ViewModels.Contas
{
    public class CalcularMultaViewModel : ResultBaseViewModel
    {
        [J("valorOriginal")]
        public double ValorOriginal { get; set; }
        [J("valorMulta")]
        public double ValorMulta { get; set; }
        [J("valorFinal")]
        public double ValorFinal { get; set; }
        [J("qntDiasAtraso")]
        public int QntDiasAtraso { get; set; }
    }
}
