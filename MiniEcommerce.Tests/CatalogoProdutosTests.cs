using MiniEcommerce.Services;

namespace MiniEcommerce.Tests;

public class CatalogoProdutosTests
{
    [Fact]
    public void CadastrarProduto_DeveGerarIdsDistintosEListarProdutos()
    {
        var catalogo = new CatalogoProdutos();

        var mouse = catalogo.CadastrarProduto("Mouse", 89.90m, 4);
        var teclado = catalogo.CadastrarProduto("Teclado", 120.00m, 3);

        Assert.NotEqual(mouse.Id, teclado.Id);
        Assert.Equal(2, catalogo.ListarProdutos().Count);
        Assert.Same(mouse, catalogo.ObterPorId(mouse.Id));
        Assert.Same(teclado, catalogo.ObterPorId(teclado.Id));
    }

    [Fact]
    public void CadastrarProduto_NaoDeveAdicionarProdutoInvalidoAoCatalogo()
    {
        var catalogo = new CatalogoProdutos();

        Assert.Throws<ArgumentOutOfRangeException>(() => catalogo.CadastrarProduto("Invalido", -5m, 1));
        Assert.Empty(catalogo.ListarProdutos());
    }

    [Fact]
    public void ObterPorId_DeveRejeitarProdutoInexistente()
    {
        var catalogo = new CatalogoProdutos();

        Assert.Throws<KeyNotFoundException>(() => catalogo.ObterPorId(999));
    }
}
