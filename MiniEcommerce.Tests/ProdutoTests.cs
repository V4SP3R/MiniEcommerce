using MiniEcommerce.Domain;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void Construtor_DeveRejeitarPrecoNegativo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Produto(1, "Mouse", -0.01m, 2));
    }

    [Fact]
    public void Construtor_DeveRejeitarEstoqueNegativo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Produto(1, "Mouse", 50m, -1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_DeveRejeitarNomeVazio(string nome)
    {
        Assert.Throws<ArgumentException>(() => new Produto(1, nome, 50m, 1));
    }

    [Fact]
    public void Construtor_DevePermitirPrecoEEstoqueZero()
    {
        var produto = new Produto(1, "Brinde", 0m, 0);

        Assert.Equal(0m, produto.Preco);
        Assert.Equal(0, produto.QuantidadeEstoque);
    }

    [Fact]
    public void BaixarEstoque_DeveDescontarQuantidadeDisponivel()
    {
        var produto = new Produto(1, "Mouse", 50m, 7);

        produto.BaixarEstoque(3);

        Assert.Equal(4, produto.QuantidadeEstoque);
    }

    [Fact]
    public void BaixarEstoque_NaoDevePermitirQuantidadeSuperiorAoSaldo()
    {
        var produto = new Produto(1, "Mouse", 50m, 2);

        Assert.Throws<InvalidOperationException>(() => produto.BaixarEstoque(3));
        Assert.Equal(2, produto.QuantidadeEstoque);
    }

    [Fact]
    public void BaixarEstoque_DeveRejeitarQuantidadeZero()
    {
        var produto = new Produto(1, "Mouse", 50m, 2);

        Assert.Throws<ArgumentOutOfRangeException>(() => produto.BaixarEstoque(0));
    }
}
