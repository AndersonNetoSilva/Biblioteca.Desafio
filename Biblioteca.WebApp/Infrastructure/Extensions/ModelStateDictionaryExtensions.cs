using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Biblioteca.WebApp.Infrastructure.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Valida e converte uma string de valor monetário em decimal (formato pt-BR).
        /// Se inválido, adiciona erro ao ModelState.
        /// </summary>
        /// <param name="modelState">ModelState da página ou controller</param>
        /// <param name="campo">Nome do campo para exibir erro</param>
        /// <param name="valorString">Valor em string (ex.: "10,50" ou "1.020,25")</param>
        /// <param name="valorDecimal">Valor decimal convertido, se válido</param>
        /// <returns>True se válido, false caso contrário</returns>
        public static bool TryParseValor(
            this ModelStateDictionary modelState,
            string campo,
            string valorString,
            out decimal valorDecimal)
        {
            valorDecimal = 0m;

            if (string.IsNullOrWhiteSpace(valorString))
            {
                modelState.AddModelError(campo, "O valor é obrigatório.");
                return false;
            }

            if (!valorString.TryParseValor(out valorDecimal))
            {
                modelState.AddModelError(campo, "Formato inválido. Use 10,50 ou 1.020,25");
                return false;
            }

            return true;
        }
    }
}
