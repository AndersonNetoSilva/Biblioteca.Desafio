using Biblioteca.WebApp.Infrastructure.Extensions;

namespace Biblioteca.Tests.Extension;

public class ExtensionTests
{
    [Theory]
    [InlineData("R$ 0,12")]
    [InlineData("R$ 0,1")]
    [InlineData("10")]
    [InlineData("R$ 10090")]
    [InlineData("10,00")]
    [InlineData("1.000,73")]
    [InlineData("1000,73")]
    [InlineData("3,25")]
    [InlineData("3.250,75")]
    [InlineData("185.003.250,75")]
    [InlineData("9.185.003.250,75")]
    public void TryparseValorAlwaysReturnsTrue(string numeroAsString)
    {
        //Arrange
        //Act
        var parsed = numeroAsString.TryParseValor(out var numeroDecimal);

        Console.Write(numeroDecimal);

        //Asset
        Assert.True(parsed);
    }

    [Theory]
    [InlineData("R$ 10.090")]
    [InlineData("R$ 10.27")]
    [InlineData(".10")]
    [InlineData("0.10")]
    [InlineData("0.22")]
    [InlineData("10.00")]
    [InlineData("1,000.23")]
    [InlineData("3.25")]
    [InlineData("3,250.75")]
    [InlineData("185,003,250.75")]
    [InlineData("9.185.003.250.75")]
    [InlineData("9185003250.00")]
    public void TryparseValorAlwaysReturnsFalse(string numeroAsString)
    {
        //Arrange
        //Act
        var parsed = numeroAsString.TryParseValor(out var numeroDecimal);

        Console.Write(numeroDecimal);

        //Asset
        Assert.False(parsed);
    }
}
