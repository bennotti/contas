using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.Models
{
    public class TbBase
    {
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public bool Ativo { get; set; }
        public TbBase()
        {
            Ativo = true;
            DataCadastro = DateTime.Now;
            DataUltimaAlteracao = DateTime.Now;
        }
    }
}
