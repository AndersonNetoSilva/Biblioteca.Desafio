using Biblioteca.WebApp.Model;

namespace Biblioteca.WebApp.Infrastructure.Abstractions.Services
{
    public interface ILivroService
    {
        Task UpdateAsync(Livro Livro, IEnumerable<int> autorIds, IEnumerable<int> assuntoIds,
                IEnumerable<PrecoDeVendaVM> precosDeVenda, ArquivoVM? arquivoImagem, ArquivoVM? arquivoDownload,
                IEnumerable<AnexoDoLivroVM> anexosDoLivro);

        Task AddAsync(Livro livro, IEnumerable<int> autorIds, IEnumerable<int> assuntoIds,
            IEnumerable<PrecoDeVendaVM> precosDeVenda, ArquivoVM? arquivoImagem, ArquivoVM? arquivoDownload,
            IEnumerable<AnexoDoLivroVM> anexosDoLivro);
    }
}
