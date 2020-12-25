using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Helpers
{
    public class RegraNegocioHelper
    {
        public static int QuantidadeDias(DateTime vencimento, DateTime pagamento)
        {
            if (vencimento >= pagamento) return 0;

            return ((TimeSpan)(pagamento - vencimento)).Days;
        }
        public static double CalcularMulta(double valor, int tempo)
        {
            if (tempo == 0) return 0;

            double taxa = 0;
            if (tempo <= 3)
            {
                taxa = 0.1;
            }else if (tempo > 3 && tempo <= 5)
            {
                taxa = 0.2;
            }else
            {
                taxa = 0.3;
            }

            double juros = (taxa / 100) * tempo;

            return valor * juros;
        }
    }
}
