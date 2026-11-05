using MiniEcommerce.Domain;
using MiniEcommerce.Services;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    [Fact]
    public void AdicionarProduto_DeveInserirItemECalcularSubtotal()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto(1, "Mouse", 39.90m, 5);

        carrinho.AdicionarProduto(produto, 2);

        Assert.Single(carrinho.Itens);
        Assert.Equal(2, carrinho.Itens[0].Quantidade);
        Assert.Equal(79.80m, carrinho.Subtotal);
        Assert.Equal(5, produto.QuantidadeEstoque); // so baixa no checkout
    }

    [Fact]
    public void AdicionarProduto_DeveSomarItensIguais()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto(1, "Mouse", 10m, 5);

        carrinho.AdicionarProduto(produto, 2);
        carrinho.AdicionarProduto(produto, 3);

        Assert.Single(carrinho.Itens);
        Assert.Equal(5, carrinho.Itens[0].Quantidade);
        Assert.Equal(50m, carrinho.Subtotal);
    }

    [Fact]
    public void AdicionarProduto_DeveRejeitarQuantidadeMaiorQueEstoque()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto(1, "Mouse", 10m, 2);

        Assert.Throws<InvalidOperationException>(() => carrinho.AdicionarProduto(produto, 3));
        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void AdicionarProduto_DeveConsiderarQuantidadeJaNoCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto(1, "Mouse", 10m, 3);
        carrinho.AdicionarProduto(produto, 2);

        Assert.Throws<InvalidOperationException>(() => carrinho.AdicionarProduto(produto, 2));
        Assert.Equal(2, carrinho.Itens[0].Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void AdicionarProduto_DeveRejeitarQuantidadeNaoPositiva(int quantidade)
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = new Produto(1, "Mouse", 10m, 5);

        Assert.Throws<ArgumentOutOfRangeException>(() => carrinho.AdicionarProduto(produto, quantidade));
    }

    [Fact]
    public void RemoverProduto_DeveRetirarItemDoCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        var mouse = new Produto(1, "Mouse", 10m, 3);
        var teclado = new Produto(2, "Teclado", 20m, 3);
        carrinho.AdicionarProduto(mouse, 2);
        carrinho.AdicionarProduto(teclado, 1);

        var removeu = carrinho.RemoverProduto(mouse.Id);

        Assert.True(removeu);
        Assert.Single(carrinho.Itens);
        Assert.Equal(20m, carrinho.Subtotal);
        Assert.False(carrinho.RemoverProduto(mouse.Id));
    }
}
