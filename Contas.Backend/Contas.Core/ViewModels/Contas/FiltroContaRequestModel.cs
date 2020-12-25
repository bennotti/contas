using Contas.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contas.Core.ViewModels.Contas
{
    public class FiltroContaRequestModel
    {
        public static FiltroContaRequestModel Default {
            get
            {
                return new FiltroContaRequestModel();
            }
        }
        public string Nome { get; set; }
        public double? Valor { get; set; }
        public TipoComparacao TipoComparacaoValor { get; set; }
    }
}
