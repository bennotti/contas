using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contas.Core.ViewModels.Authentication
{
    public class AutenticacaoRequestViewModel
    {
        public AutenticacaoRequestViewModel()
        {

        }

        public AutenticacaoRequestViewModel(string usuario, string senha)
        {
            this.usuario = usuario;
            this.senha = senha;
        }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string usuario { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string senha { get; set; }
    }
}
