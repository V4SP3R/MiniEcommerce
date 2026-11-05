using MiniEcommerce.Domain;

namespace MiniEcommerce.Services;

/// <summary>Centraliza cadastro e consulta de produtos nesta execucao.</summary>
public sealed class CatalogoProdutos
{
    private readonly Dictionary<int, Produto> _produtos = new();
    private int _proximoId = 1;

    public Produto CadastrarProduto(string nome, decimal preco, int quantidadeEstoque)
    {
        var produto = new Produto(_proximoId, nome, preco, quantidadeEstoque);
        _produtos.Add(produto.Id, produto);
        _proximoId++;
        return produto;
    }

    public IReadOnlyList<Produto> ListarProdutos() =>
        _produtos.Values.OrderBy(produto => produto.Id).ToArray();

    public Produto ObterPorId(int id) =>
        _produtos.TryGetValue(id, out var produto)
            ? produto
            : throw new KeyNotFoundException($"Produto com ID {id} nao encontrado.");
}
