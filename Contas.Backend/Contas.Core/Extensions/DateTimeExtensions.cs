using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contas.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static TimeSpan? ToTimeSpan(this string strHora)
        {
            if (string.IsNullOrEmpty(strHora)) return null;
            try
            {
                if (strHora.IndexOf(":") < 0)
                {
                    var hora = strHora.Substring(0, 2);
                    var minuto = strHora.Substring(2, 2);
                    var segundo = "00";
                    if (strHora.Length > 4) segundo = strHora.Substring(4, 2);
                    strHora = hora + ":" + minuto + ":" + segundo;
                }

                var arrHora = strHora.Split(':');

                var valorHora = Convert.ToInt32(arrHora[0].Substring(0, arrHora[0].Length));
                var valorMinuto = Convert.ToInt32(arrHora[1].Substring(0, arrHora[1].Length));
                var valorSegundo = 0;
                if (arrHora.Length == 3)
                {
                    valorSegundo = Convert.ToInt32(arrHora[2].Substring(0, arrHora[2].Length));
                }

                var horaTimeSpan = new TimeSpan(
                    valorHora,
                    valorMinuto,
                    valorSegundo);
                return horaTimeSpan;
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? ToDatetime(this string strData, string hora = "00:00:00")
        {
            if (string.IsNullOrEmpty(strData)) return null;
            try
            {
                if (string.IsNullOrEmpty(hora)) hora = "00:00:00";
                var timeSpan = hora.ToTimeSpan().Value;

                if (strData.IndexOf("/") < 0 && strData.IndexOf("-") < 0)
                {
                    var dia = strData.Substring(0, 2);
                    var mes = strData.Substring(2, 2);
                    var ano = strData.Substring(4, 4);
                    strData = dia + "/" + mes + "/" + ano;
                }
                strData = strData.Replace("-", "/");

                var arrData = strData.ToLower().Split(' ')[0].Split('t')[0].Split('/');

                var data = new DateTime(
                    Convert.ToInt32(arrData[2].Substring(0, arrData[2].Length > 4 ? 4 : arrData[2].Length)),
                    Convert.ToInt32(arrData[1].Substring(0, arrData[1].Length)),
                    Convert.ToInt32(arrData[0].Substring(0, arrData[0].Length)),
                    timeSpan.Hours,
                    timeSpan.Minutes,
                    timeSpan.Seconds);
                return data;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Retorna o total de meses entre as duas datas, não leva em consideração se a diferença entre os dias entre as duas datas gerou um novo mês.
        /// </summary>
        /// <param name="dataFim"></param>
        /// <param name="dataInicio"></param>
        /// <returns></returns>
        public static int TotalMeses(DateTime dataFim, DateTime dataInicio)
        {
            return ((dataFim.Year - dataInicio.Year) * 12) + dataFim.Month - dataInicio.Month;
        }

        /// <summary>
        /// Retorna a lista de feriados nacionais
        /// </summary>
        /// <param name="yearParameter"></param>
        /// <returns></returns>
        public static IList<DateTime> ObterFeriadosNacionaisDoAnoCorrente(int? yearParameter = null)
        {
            var holidayList = new List<DateTime>();
            var year = DateTime.Now.Year;

            if (yearParameter != null)
                year = yearParameter.Value;

            holidayList.Add(new DateTime(year, 1, 1)); //Ano novo 
            holidayList.Add(new DateTime(year, 4, 21));  //Tiradentes
            holidayList.Add(new DateTime(year, 5, 1)); //Dia do trabalho
            holidayList.Add(new DateTime(year, 9, 7)); //Dia da Independência do Brasil
            holidayList.Add(new DateTime(year, 10, 12));  //Nossa Senhora Aparecida
            holidayList.Add(new DateTime(year, 11, 2)); //Finados
            holidayList.Add(new DateTime(year, 11, 15)); //Proclamação da República
            holidayList.Add(new DateTime(year, 12, 25)); //Natal

            #region FeriadosMóveis

            int x, y;
            int a, b, c, d, e;
            int day, month;

            if (year >= 1900 & year <= 2099)
            {
                x = 24;
                y = 5;
            }
            else
                if (year >= 2100 & year <= 2199)
            {
                x = 24;
                y = 6;
            }
            else
                    if (year >= 2200 & year <= 2299)
            {
                x = 25;
                y = 7;
            }
            else
            {
                x = 24;
                y = 5;
            }

            a = year % 19;
            b = year % 4;
            c = year % 7;
            d = (19 * a + x) % 30;
            e = (2 * b + 4 * c + 6 * d + y) % 7;

            if ((d + e) > 9)
            {
                day = (d + e - 9);
                month = 4;
            }

            else
            {
                day = (d + e + 22);
                month = 3;
            }

            var pascoa = new DateTime(year, month, day);
            var sextaSanta = pascoa.AddDays(-2);
            var carnaval = pascoa.AddDays(-47);
            var corpusChristi = pascoa.AddDays(60);

            holidayList.Add(pascoa);
            holidayList.Add(sextaSanta);
            holidayList.Add(carnaval);
            holidayList.Add(corpusChristi);

            #endregion

            return holidayList;
        }

        /// <summary>
        /// Retorna o dia útil a partir da data prevista informada
        /// </summary>
        /// <param name="dataPrevista"></param>
        /// <returns></returns>
        public static DateTime ObterDiaUtil(DateTime dataPrevista)
        {
            var listaFeriadosNacionais = ObterFeriadosNacionaisDoAnoCorrente().ToList();

            while (listaFeriadosNacionais.Contains(dataPrevista) 
                || dataPrevista.DayOfWeek == DayOfWeek.Sunday 
                || dataPrevista.DayOfWeek == DayOfWeek.Saturday)
            {
                //valida se a data prevista passou do mês previsto para o pagamento, caso isso acontece, pega o ultimo dia util do mes                        
                if (dataPrevista.AddDays(+1).Month != dataPrevista.Month)
                {
                    while (listaFeriadosNacionais.Contains(dataPrevista)
                        || dataPrevista.DayOfWeek == DayOfWeek.Sunday
                        || dataPrevista.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dataPrevista = dataPrevista.AddDays(-1);
                    }
                }
                else
                {
                    dataPrevista = dataPrevista.AddDays(+1);
                }
            }

            return dataPrevista;
        }
    }
}
