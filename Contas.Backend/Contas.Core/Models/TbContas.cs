using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.Models
{
    public class TbContas : TbBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int ContaId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ValorOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ValorMulta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ValorFinal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Vencimento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Pagamento { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int QntDiasAtraso { get; set; }

        public TbContas() {
            
        }
    }
}
