using Biblioteca.WebApp.Data;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Infrastructure.Abstractions.Services;
using Biblioteca.WebApp.Infrastructure.Exceptions;
using Biblioteca.WebApp.Infrastructure.Extensions;
using Biblioteca.WebApp.Model;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.WebApp.Infrastructure.Services
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IAssuntoRepository _assuntoRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public LivroService(ILivroRepository livroRepository,
            IAssuntoRepository assuntoRepository,
            IAutorRepository autorRepository,
            ApplicationDbContext dbContext,
            IUnitOfWork unitOfWork)
        {
            _livroRepository = livroRepository;
            _assuntoRepository = assuntoRepository;
            _autorRepository = autorRepository;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(
            Livro livro,
            IEnumerable<int> autorIds,
            IEnumerable<int> assuntoIds,
            IEnumerable<PrecoDeVendaVM> precosDeVenda)
        {
            ValidarValor(livro);

            _livroRepository.Add(livro);

            await SyncAutoresAsync(livro, autorIds);
            await SyncAssuntosAsync(livro, assuntoIds);
            SyncPrecos(livro, precosDeVenda);

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(Livro Livro, IEnumerable<int> autorIds, IEnumerable<int> assuntoIds,
            IEnumerable<PrecoDeVendaVM> precosDeVenda)
        {
            var livro = await _livroRepository.GetForUpdateAsync(Livro.Id);

            if (livro != null)
            {
                ValidarValor(Livro);

                _dbContext.Entry(livro).CurrentValues.SetValues(Livro);

                await SyncAutoresAsync(livro, autorIds);
                await SyncAssuntosAsync(livro, assuntoIds);

                SyncPrecos(livro, precosDeVenda);

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _livroRepository.ExistsAsync(x => x.Id == Livro.Id))
                    {
                        throw new KeyNotFoundException();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private static void ValidarValor(Livro Livro)
        {
            if (!Livro.ValorString.TryParseValor(out string errorMessage, out var valorDecimal))
            {
                throw new ValidationListException(("Livro.ValorString", errorMessage));
            }

            Livro.Valor = valorDecimal;
        }

        private void SyncPrecos(Livro livro, IEnumerable<PrecoDeVendaVM> precosDeVenda)
        {
            var idsParaExclusao = precosDeVenda
                        .Where(x => x.Id.HasValue && x.MarcadoParaExclusao)
                        .Select(x => x.Id.Value)
                        .ToHashSet();

            livro.PrecosDeVenda.RemoveAll(p => idsParaExclusao.Contains(p.Id));

            var indice = 0;

            foreach (var precoDeVenda in precosDeVenda)
            {
                if (!precoDeVenda.MarcadoParaExclusao)
                {
                    if (!precoDeVenda.ValorString.TryParseValor(out string errorMessage, out var valor))
                    {
                        throw new ValidationListException(($"PrecosDeVenda[{indice}].ValorString", errorMessage));
                    }

                    if (precoDeVenda.Id.HasValue && precoDeVenda.Id.Value > 0)
                    {
                        var preco = livro.PrecosDeVenda.First(p => p.Id == precoDeVenda.Id);
                        preco.Tipo = precoDeVenda.Tipo;
                        preco.Valor = valor;
                    }
                    else
                    {
                        livro.PrecosDeVenda.Add(new PrecoDeVenda
                        {
                            Tipo = precoDeVenda.Tipo,
                            Valor = valor
                        });
                    }
                }

                indice++;
            }
        }

        private async Task SyncAssuntosAsync(Livro livro, IEnumerable<int> assuntosIds)
        {
            assuntosIds ??= new List<int>();

            // REMOVE assuntos desmarcados
            var remover = livro.Assuntos
                .Where(a => !assuntosIds.Contains(a.Id))
                .ToList();

            foreach (var assunto in remover)
                livro.Assuntos.Remove(assunto);

            // ADICIONA novos assuntos
            var existentes = livro.Assuntos.Select(a => a.Id).ToHashSet();

            foreach (var assuntoId in assuntosIds)
            {
                if (!existentes.Contains(assuntoId))
                {
                    livro.Assuntos.Add(await _assuntoRepository.GetByIdAsync(assuntoId));
                }
            }
        }

        private async Task SyncAutoresAsync(Livro livro, IEnumerable<int> autoresIds)
        {
            autoresIds ??= new List<int>();

            // REMOVE autores desmarcados
            var remover = livro.Autores
                .Where(a => !autoresIds.Contains(a.Id))
                .ToList();

            foreach (var autor in remover)
                livro.Autores.Remove(autor);

            // ADICIONA novos autores
            var existentes = livro.Autores.Select(a => a.Id).ToHashSet();

            foreach (var autorId in autoresIds)
            {
                if (!existentes.Contains(autorId))
                {
                    livro.Autores.Add(await _autorRepository.GetByIdAsync(autorId));
                }
            }
        }

    }
}
