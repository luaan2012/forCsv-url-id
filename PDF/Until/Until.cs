using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PDF.Until
{
    public static class Untils
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string toStringCnpj(this string cnpj) => long.TryParse(cnpj, out var n) ? n.ToString(@"00\.000\.000\/0000\-00") : cnpj;
        public static string toStringCelular(this string celular) => long.TryParse(celular, out var n) ? n.ToString(@"(00) 00000\-0000") : celular;
        public static string toStringTelefone(this string telefone) => long.TryParse(telefone, out var n) ? n.ToString(@"(00) 0000\-0000") : telefone;
        public static string toStringCep(this string telefone) => long.TryParse(telefone, out var n) ? n.ToString(@"00000\-000") : telefone;
        public static string toStringCpf(this string cpf) => long.TryParse(cpf, out var n) ? n.ToString(@"000\.000\.000\-00") : cpf;
        public static string toStringRG(this string rg) => long.TryParse(rg, out var n) ? n.ToString(@"00\.000\.000\-0") : rg;
        public static string toFirstCharUpper(this string text) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        public static string RemoveSpace(this string text) => Regex.Replace(text, @"\s", string.Empty);
        public static string RemoveCaracters(this string text) => Regex.Replace(text, "[^0-9a-zA-Z]+", string.Empty);
        public static string KeepCaracters(this string text, string carecter) => Regex.Replace(text, $"[^0-9a-zA-Z{carecter}]+", string.Empty);
        public static string Normalization(this string text) => text.RemoveSpace().RemoveCaracters().ToLower();
        public static string ReplaceWhitespace(string input, string replacement) => sWhitespace.Replace(input, replacement);
        public static string InvertDate(string input)
        {
            return input;
        }
        public static string FormatDate(this string input, string format)
        {
            DateTime date;
            string reconverse;
            if (DateTime.TryParseExact(input, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                reconverse = date.ToString(format);
            else
                reconverse = "not convert";
            return reconverse;
        }
    }
}
