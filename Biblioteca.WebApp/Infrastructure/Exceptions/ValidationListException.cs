namespace Biblioteca.WebApp.Infrastructure.Exceptions
{
    public class ValidationListException : Exception
    {
        public IReadOnlyDictionary<string, string> Errors { get; }

        public ValidationListException(params (string Field, string Message)[] errors)
            : base("Erro(s) de validação")
        {
            Errors = errors
                .GroupBy(e => e.Field)
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(". ", g.Select(x => x.Message))
                );
        }
    }
}
