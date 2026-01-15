using Biblioteca.WebApp.Infrastructure.Extensions;

namespace Biblioteca.Tests.Extension;

public class ExtensionTests
{
    [Theory]
    [InlineData("10,00")]
    [InlineData("1.000,00")]
    [InlineData("3,25")]
    [InlineData("3.250,75")]
    public void TryparseValorAlwaysReturnsTrue(string numeroAsString)
    {
        //Arrange
        //Act
        var parsed = numeroAsString.TryParseValor(out var numeroDecimal);

        //Asset
        Assert.True(parsed);
    }
}
