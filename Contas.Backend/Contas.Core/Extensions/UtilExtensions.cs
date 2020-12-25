using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Contas.Core.Extensions
{
    public static class UtilExtensions
    {

        public static string ToEmail(this System.Security.Claims.ClaimsPrincipal user)
        {
            return user.FindFirst(System.Security.Claims.ClaimTypes.Email).Value;
        }

        public static string ToCpf(this System.Security.Claims.ClaimsPrincipal user)
        {
            return user.FindFirst("cpf") != null ? user.FindFirst("cpf").Value : string.Empty;
        }
        public static string TextoSigla(this string texto)
        {
            string retorno = "";
            var palavras = texto.Split(' ');
            foreach (var item in palavras)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    retorno += item.Substring(0, 1).ToUpper();
            }
            return retorno;
        }
        public static string FormatCelular(this string _texto)
        {
            if (!string.IsNullOrEmpty(_texto))
            {
                _texto = _texto.SomenteNumeros();
                if (_texto.Length == 11)
                    return string.Format(@"{0:(##) # ####-####}", Convert.ToDouble(_texto));
                else
                    return string.Format(@"{0:(##) ####-####}", Convert.ToDouble(_texto));
            }
            return _texto;
        }
        public static string FormatarMoeda(this decimal? pValor)
        {
            if (pValor.HasValue)
                return String.Format(GlobalVars.Culture(), "{0:C}", pValor).Replace("R$", "R$ ");
            else
                return String.Format(GlobalVars.Culture(), "{0:C}", 0).Replace("R$", "R$ ");
        }

        public static string FormatarMoeda(this string pValor)
        {
            if (string.IsNullOrEmpty(pValor)) pValor = "0";
            var value = decimal.Parse(pValor.Replace(".", ","));
            return String.Format(GlobalVars.Culture(), "{0:C}", value).Replace("R$", "R$ ");
        }
        public static string FormatarMoedaGerencianet(this int value, int installment)
        {
            decimal price = Convert.ToDecimal(value) / 100;
            return String.Format(GlobalVars.Culture(), "{0:C}", (price * installment)).Replace("R$", "R$ ");
        }

        public static string DashColorProgress(this decimal value)
        {
            if (value > 60)
                return "progress-bar-success";
            else if (value > 30 && value < 60)
                return "progress-bar-warning";
            else
                return "progress-bar-danger";
        }
        public static string FormatarMoedaGerencianet(this int value)
        {
            decimal price = Convert.ToDecimal(value) / 100;
            return String.Format(GlobalVars.Culture(), "{0:C}", price).Replace("R$", "R$ ");
        }
        public static string CurrencyPagSeguro(this string pValor)
        {
            if (string.IsNullOrEmpty(pValor)) pValor = "0";
            var value = decimal.Parse(pValor.Replace(".", ","));
            return String.Format(GlobalVars.Culture(), "{0:C}", value).Replace("R$", "").Replace(",", ".");
        }

        public static string FormatarMoeda(this decimal pValor)
        {
            return String.Format(GlobalVars.Culture(), "{0:C}", pValor).Replace("R$", "R$ ");
        }

        public static string FormatarMoedaJS(this decimal val)
        {
            return val.ToString().Replace(".", ",");
        }

        public static int FormatarMoedaGerenciaNet(this decimal pValor)
        {
            return Convert.ToInt32(pValor * 100);
        }

        public static string FormatarMoedaSemCifrao(this decimal pValor)
        {
            return String.Format(GlobalVars.Culture(), "{0:C}", pValor).Replace("R$", "");
        }

        public static string FormatarPercentual(this decimal pValor)
        {
            return String.Format(GlobalVars.Culture(), "{0:C}", pValor).Replace("R$", "") + " %";

        }
        public static string FormatarPercentualSemCasas(this decimal pValor)
        {
            return String.Format(GlobalVars.Culture(), "{0}", Convert.ToInt32(pValor)) + " %";

        }

        public static int ToCieloAmount(this decimal? val)
        {
            return int.Parse(val.Value.ToString("C2").SomenteNumeros());
        }
        public static int ToCieloAmount(this decimal val)
        {
            return int.Parse(val.ToString("C2").SomenteNumeros());
        }
        public static string DataExtenso(this DateTime aData)
        {
            try
            {
                CultureInfo culture = GlobalVars.Culture();
                DateTimeFormatInfo dtfi = culture.DateTimeFormat;

                int dia = aData.Day;
                int ano = aData.Year;
                string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(aData.Month));
                string diasemana = culture.TextInfo.ToTitleCase(dtfi.GetDayName(aData.DayOfWeek));
                string data = diasemana + ", " + dia + " de " + mes + " de " + ano;
                return data;
            }
            catch { return ""; }
        }

        public static string DataDiaSemana(this DateTime aData)
        {
            try
            {
                CultureInfo culture = GlobalVars.Culture();
                DateTimeFormatInfo dtfi = culture.DateTimeFormat;

                string diasemana = culture.TextInfo.ToTitleCase(dtfi.GetAbbreviatedDayName(aData.DayOfWeek));

                return diasemana.ToUpper();
            }
            catch { return ""; }
        }
        public static string DataNomeMes(this DateTime aData)
        {
            try
            {
                CultureInfo culture = GlobalVars.Culture();
                DateTimeFormatInfo dtfi = culture.DateTimeFormat;

                string diasemana = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(aData.Month));

                return diasemana;
            }
            catch { return ""; }
        }
        public static string GetDescription(this System.Enum enumVal)
        {
            var type = enumVal.GetType();
            var attribute = type.GetMember(enumVal.ToString())[0]
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .SingleOrDefault();

            var description = (attribute != null) ? attribute.Description : string.Empty;

            return description;
        }
        public static string RemoveAcents(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public static string FormatarCpfCnpj(this string _texto)
        {
            if (!string.IsNullOrEmpty(_texto))
            {
                _texto = SomenteNumeros(_texto);
                if (!string.IsNullOrEmpty(_texto) || _texto.Length > 6)
                {

                    if (_texto.Length > 11)
                    {
                        return string.Format(@"{0:00\.000\.000\/0000\-00}", Convert.ToDouble(_texto));
                    }
                    else
                    {
                        return string.Format(@"{0:000\.000\.000\-00}", Convert.ToDouble(_texto));
                    }
                }
                else _texto = "";
            }
            else _texto = "";

            return _texto;
        }
        public static string GeneratePassword(int lowercase, int uppercase, int numerics, int specialCharacters)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";
            string special = "!@#$%&?";

            Random random = new Random();

            string generated = string.Empty;
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            for (int i = 1; i <= specialCharacters; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    special[random.Next(special.Length - 1)].ToString()
                );

            return generated;

        }
        /// <summary>
        /// Gera uma senha aleatória com 8 caracteres
        /// </summary>
        /// <returns></returns>
        public static string GeneratePassword()
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";
            string special = "!@#$%&?";

            Random random = new Random();

            string generated = string.Empty;
            for (int i = 1; i <= 2; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= 2; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= 2; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            for (int i = 1; i <= 2; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    special[random.Next(special.Length - 1)].ToString()
                );

            return generated;

        }
        public static string SomenteNumeros(this string Texto)
        {
            string numeros = "";
            try
            {
                if (string.IsNullOrEmpty(Texto)) return Texto;

                for (int i = 0; i < Texto.Length; i++)
                {
                    if (char.IsNumber(Texto, i))
                    {
                        numeros += Texto.Substring(i, 1);
                    }
                }
                return numeros;
            }
            catch
            {
                return Texto;
            }
        }
        public static string OnlyNumbers(this string Texto)
        {
            string numeros = "";
            try
            {
                for (int i = 0; i < Texto.Length; i++)
                {
                    if (char.IsNumber(Texto, i))
                    {
                        numeros += Texto.Substring(i, 1);
                    }
                }
                return numeros;
            }
            catch// (Exception ex)
            {
                return Texto;
            }
        }

        public static bool IsCpf(this string cpf)
        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;
                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");
                if (cpf.Length != 11)
                    return false;
                tempCpf = cpf.Substring(0, 9);
                soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = digito + resto.ToString();
                return cpf.EndsWith(digito);
            }
            catch { return false; }
        }

        public static string isPhone(this string val)
        {
            val = val.SomenteNumeros();
            if (val.Length == 10)
                return string.Format(@"{0:(##) ####-####}", Convert.ToDouble(val));
            else
                return string.Format(@"{0:(##) #####-####}", Convert.ToDouble(val));
        }

        public static string ToFormatDateTime(this DateTime val)
        {
            return val.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static string ToFormatDate(this DateTime val)
        {
            return val.ToString("dd/MM/yyyy");
        }
        public static string ToChartDate(this DateTime val)
        {
            return val.ToString("dd/MM");
        }

        public static string ToFormatDate(this DateTime? val)
        {
            return val.HasValue ?  val.Value.ToString("dd/MM/yyyy") : "-";
        }

        public static string ToDecimalUS(this decimal val)
        {
            return val.ToString().Replace(",", ".");
        }

        public static string ListToString(this List<string> lista)
        {
            var mensagem = "";
            foreach (var item in lista)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (!mensagem.Contains(item))
                        mensagem += $"\n {item}";
                }
            }
            return mensagem;
        }
        public static string StripTags(this string HTML)
        {
            // Removes tags from passed HTML            
            System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex("<[^>]*>");

            return objRegEx.Replace(HTML, "");
        }
        public static string ToSessionDateLabel(this DateTime val)
        {
            string _date = val.ToString("dd-MMM-yyyy HH:mm").ToUpper();
            if (val.Subtract(DateTime.Now).TotalDays > 5)
                return $"<i class='label label-default font-df'>{_date}</i>";
            else if (val.Subtract(DateTime.Now).TotalDays >= 3)
                return $"<i class='label label-warning font-df'>{_date}</i>";
            else if (val.Subtract(DateTime.Now).TotalDays >= 1)
                return $"<i class='label label-danger font-df'>{_date}</i>";

            return $"<i class='label label-success font-df'>{_date}</i>";
        }
        public static string ToFormatDateTime(this DateTime? val)
        {
            return val.HasValue ? val.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
        }

        public static string isStatus(this bool val)
        {
            return val ? "Ativo" : "Inativo";
        }
        public static string isStatusSendMail(this DateTime? val)
        {
            return val.HasValue ? "<i style='font-size:22px;' class='ion-checkmark-circled text-success'></i>" : "<img src='../../images/load_mail.svg' class='img-sendmail' />";
        }
        public static string toExtractToCurrency(this decimal val)
        {
            if (val == 0)
                return $"<span class=''>{val.FormatarMoeda()}</span>";
            else if (val < 0)
                return $"<span class='text-danger'>{val.FormatarMoeda()}</span>";
            else return $"<span class='text-success'>{val.FormatarMoeda()}</span>";
        }
        public static string isStatusLabel(this bool val)
        {
            return val ? "<span class='badge badge-success'>ATIVO</span>" : "<span class='badge badge-danger'>INATIVO</span>";
        }
        public static string isStatusValidationLabel(this bool val)
        {
            return val ? "<span class='label label-warning'>AGUARDANDO REVISÃO</span>" : "<span class='label label-success'>REVISADO</span>";
        }
        public static string isStatusCartLabel(this bool val, DateTime? dtCompra)
        {
            return !val ? dtCompra.HasValue ? "<span class='label label-success'>FINALIZADO</span>" : "<span class='label label-danger'>COMPRA NÃO REALIZADA</span>" : "<span class='label label-warning'>EM ABERTO</span>";
        }

        public static string isStatusPaymentLabel(this int val, string status)
        {
            switch (val)
            {
                case 20:
                case 30:
                case 35:
                case 21:
                    return $"<span class='text-success'><b>{status.ToUpper()}</b></span>";
                case 19:
                case 18:
                case 28:
                case 29:
                    return $"<span class='text-warning'><b>{status.ToUpper()}</b></span>";
                case 22:
                case 26:
                case 27:
                case 33:
                    return $"<span class='text-purple'><b>{status.ToUpper()}</b></span>";
                default:
                    return $"<span class='text-danger'><b>{status.ToUpper()}</b></span>";
            }

        }
        public static string isStatusPaymentBadge(this int val, string status)
        {
            switch (val)
            {
                case 20:
                case 30:
                case 35:
                case 21:
                    return $"<span class='badge badge-success'><b>{status.ToUpper()}</b></span>";
                case 19:
                case 18:
                case 28:
                case 29:
                    return $"<span class='badge badge-warning'><b>{status.ToUpper()}</b></span>";
                case 22:
                case 26:
                case 27:
                case 33:
                    return $"<span class='badge badge-purple'><b>{status.ToUpper()}</b></span>";
                default:
                    return $"<span class='badge badge-danger'><b>{status.ToUpper()}</b></span>";
            }

        }

        public static string isStatus(this bool? val)
        {
            return val.HasValue ? val.Value ? "Ativo" : "Inativo" : "Inativo";
        }
        public static string isYesNo(this bool val)
        {
            return val ? "Sim" : "Não";
        }
        public static string isYesNo(this bool? val)
        {
            return val.HasValue ? val.Value ? "Sim" : "Não" : "Não";
        }
        public static string isPrivate(this bool val)
        {
            return val ? "Privado" : "Público";
        }
        public static string isIcon(this bool val)
        {
            return val ? "<i class='fa fa-check-circle color-success'></i>" : "<i class='fa fa-times-circle color-danger'></i>";
        }
        public static string isToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
        public static string isCheckLabel(this string val, bool status)
        {
            return status ? $"<span>{val} <i class='ion-checkmark-circled text-success'></i></span>" : $"<span>{val} <i class='ion-close-circled text-danger'></i></span>";
        }
        public static bool IsCnpj(this string cnpj)
        {
            try
            {
                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int soma;
                int resto;
                string digito;
                string tempCnpj;
                cnpj = cnpj.Trim();
                cnpj = cnpj.SomenteNumeros();
                if (cnpj.Length != 14)
                    return false;
                tempCnpj = cnpj.Substring(0, 12);
                soma = 0;
                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = resto.ToString();
                tempCnpj = tempCnpj + digito;
                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = digito + resto.ToString();
                return cnpj.EndsWith(digito);
            }
            catch
            {
                return false;
            }

        }

        public static T JsonDeserialize<T>(this string jsonObj)
        {
            if (string.IsNullOrEmpty(jsonObj))
                return Activator.CreateInstance<T>();

            return JsonConvert.DeserializeObject<T>(jsonObj as string);
        }

        public static string JsonSerialize<T>(this T jsonObj)
        {
            return JsonConvert.SerializeObject(jsonObj);
        }
        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        public static DateTime DataConvert(this string aData, bool Parse)
        {
            if (!Parse)
            {
                var xDia = int.Parse(aData.Substring(0, 2));
                var xMes = int.Parse(aData.Substring(3, 2));
                var xAno = int.Parse(aData.Substring(6, 4));
                return new DateTime(xAno, xMes, xDia);
            }
            else
            {
                return DateTime.Parse(aData, GlobalVars.Culture());
            }
        }
        public static string GetStringNoAccents(this string str)
        {
            /** Troca os caracteres acentuados por não acentuados **/
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (int i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcento[i]);
            }
            /** Troca os caracteres especiais da string por "" **/
            string[] caracteresEspeciais = { "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°", "^", "~", "`", "'", "%", "#", "@", "!", "?", "*", "+", ";", "<", ">", "$" };
            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                str = str.Replace(caracteresEspeciais[i], "");
            }
            /** Troca os espaços no início por "" **/
            str = str.Replace("^\\s+", "");
            /** Troca os espaços no início por "" **/
            str = str.Replace("\\s+$", "");
            /** Troca os espaços duplicados, tabulações e etc por  " " **/
            str = str.Replace("\\s+", " ");
            return str;
        }

    }
}
