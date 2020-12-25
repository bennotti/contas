using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contas.Core.ViewModels.Contas
{
    public class CalcularMultaRequestModel
    {
        [Display(Name = "Vencimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Vencimento { get; set; }

        [Display(Name = "Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Pagamento { get; set; }
        
        [Display(Name = "ValorOriginal")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double ValorOriginal { get; set; }
    }
}
