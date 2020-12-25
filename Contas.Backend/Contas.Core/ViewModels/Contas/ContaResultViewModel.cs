using System;
using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JI = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Contas.Core.ViewModels.Contas
{
    public class ContaResultViewModel : ResultBaseViewModel
    {
        [J("contaId")]
        public int ContaId { get; set; }
        [J("nome")]
        public string Nome { get; set; }
        [J("valorOriginal")]
        public double ValorOriginal { get; set; }
        [J("valorMulta")]
        public double ValorMulta { get; set; }
        [J("valorFinal")]
        public double ValorFinal { get; set; }
        [J("vencimento")]
        public string Vencimento { get; set; }
        [J("pagamento")]
        public string Pagamento { get; set; }
        [J("qntDiasAtraso")]
        public int QntDiasAtraso { get; set; }
        [J("loading")]
        public bool Loading { get; set; }
    }
}
