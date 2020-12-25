using System;
using System.Collections.Generic;
using System.Text;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JI = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Contas.Core.ViewModels.Contas
{
    public class ContasCollectionResultViewModel : ResultBaseViewModel
    {
        [J("totalRegistros")]
        public int TotalRegistros { get; set; }
        [J("contas")]
        public ICollection<ContaResultViewModel> Contas { get; set; }
    }
}
