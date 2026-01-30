using System.Globalization;
using System.Text.RegularExpressions;

namespace Biblioteca.WebApp.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Valida e converte uma string de valor monetário em decimal (formato pt-BR).
        /// Se inválido, retorna false
        /// </summary>
        /// <param name="owner">Valor em string (ex.: "10,50" ou "1.020,25")</param>
        /// <param name="valorDecimal">Valor decimal convertido, se válido</param>
        /// <returns>True se válido, false caso contrário</returns>
        public static bool TryParseValor(this string owner, out decimal valorDecimal)
        {
            valorDecimal = 0m;

            if (string.IsNullOrWhiteSpace(owner))
            {
                return false;
            }

            // Remove símbolo de moeda e espaços
            owner = owner.Replace("R$", string.Empty).Trim();

            // Regex pt-BR: aceita pontos como milhar e vírgula como decimal
            var regex = new Regex(@"^\d{1,3}(\.\d{3})*(,\d{2})$|^\d+(,\d{1,2})?$");

            if (!regex.IsMatch(owner))
            {
                return false;
            }

            // Parse seguro
            if (!decimal.TryParse(
                    owner,
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    new CultureInfo("pt-BR"),
                    out valorDecimal))
            {
                return false;
            }

            return true;
        }
    }
}
