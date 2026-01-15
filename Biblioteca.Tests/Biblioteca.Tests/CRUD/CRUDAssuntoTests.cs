using Biblioteca.Tests.Configuration;
using Biblioteca.WebApp.Infrastructure.Abstractions.Repositories;
using Biblioteca.WebApp.Model;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Biblioteca.Tests.CRUD
{
    public class CRUDAssuntoTests: TestServiceBase
    {
        public CRUDAssuntoTests(TestServiceFixture fixture, ITestOutputHelper output)
            :base(fixture, output) 
        {
            
        }

        [Fact]
        public async Task AddAssunto_ComSucesso_Async()
        {
            //Arrange
            var assuntoRepository = _fixture.ServiceProvider.GetRequiredService<IAssuntoRepository>();
            var unitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var assunto = new Assunto()
            {
                Descricao = "Assunto do Teste"
            };

            //Act
            assuntoRepository.Add(assunto);
            await unitOfWork.CommitAsync();

            //Assert
            Assert.True(assunto.Id > 0);
        }
    }
}