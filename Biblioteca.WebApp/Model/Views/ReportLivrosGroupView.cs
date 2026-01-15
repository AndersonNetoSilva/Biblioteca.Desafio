namespace Biblioteca.WebApp.Model.Views
{
    public class ReportLivrosGroupView
    {
        public string Autor { get; set; }
        public IEnumerable<LivroGroupView> Livros { get; set; }
    }

    public class LivroGroupView
    {
        public string Livro { get; set; }
        public string Assunto { get; set; }
    }
}
