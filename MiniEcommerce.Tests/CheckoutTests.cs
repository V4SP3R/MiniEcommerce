using MiniEcommerce.Domain;
using MiniEcommerce.Services;

namespace MiniEcommerce.Tests;

public class CheckoutTests
{
    [Fact]
    public void FinalizarPedido_DeveCalcularTotalBaixarEstoqueELimparCarrinho()
    {
        var mouse = new Produto(1, "Mouse", 49.90m, 8);
        var teclado = new Produto(2, "Teclado", 100m, 2);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarProduto(mouse, 3);
        carrinho.AdicionarProduto(teclado, 1);

        var pedido = new ServicoCheckout().FinalizarPedido(carrinho);

        Assert.Equal(249.70m, pedido.Total);
        Assert.Equal(2, pedido.Itens.Count);
        Assert.Equal(5, mouse.QuantidadeEstoque);
        Assert.Equal(1, teclado.QuantidadeEstoque);
        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.Subtotal);
        Assert.NotEqual(Guid.Empty, pedido.Id);
    }

    [Fact]
    public void FinalizarPedido_DeveFalharParaCarrinhoVazio()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => new ServicoCheckout().FinalizarPedido(carrinho));
    }

    [Fact]
    public void FinalizarPedido_DeveFalharSemAlterarEstoqueSeSaldoMudou()
    {
        var mouse = new Produto(1, "Mouse", 25m, 3);
        var teclado = new Produto(2, "Teclado", 40m, 2);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarProduto(mouse, 2);
        carrinho.AdicionarProduto(teclado, 2);
        teclado.BaixarEstoque(1); // outro pedido consumiu uma unidade

        Assert.Throws<InvalidOperationException>(() => new ServicoCheckout().FinalizarPedido(carrinho));
        Assert.Equal(3, mouse.QuantidadeEstoque); // sem baixa parcial
        Assert.Equal(1, teclado.QuantidadeEstoque);
        Assert.Equal(2, carrinho.Itens.Count); // carrinho preservado
    }

    [Fact]
    public void FinalizarPedido_DevePreservarValoresDoPedidoAposLimparCarrinho()
    {
        var produto = new Produto(1, "Caneca", 12.50m, 4);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarProduto(produto, 2);

        var pedido = new ServicoCheckout().FinalizarPedido(carrinho);

        Assert.Equal("Caneca", pedido.Itens[0].NomeProduto);
        Assert.Equal(25.00m, pedido.Itens[0].Subtotal);
        Assert.Equal(25.00m, pedido.Total);
    }
}
