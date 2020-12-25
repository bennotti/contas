using System.Globalization;

namespace Contas.Core.Extensions
{
    public static class GlobalVars
    {
        public static CultureInfo Culture(string aTipoCultura)
        {
            return new CultureInfo(aTipoCultura);
        }
        public static CultureInfo Culture()
        {
            return new CultureInfo("pt-BR");
        }
    }
}
