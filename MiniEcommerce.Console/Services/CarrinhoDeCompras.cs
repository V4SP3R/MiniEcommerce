using MiniEcommerce.Domain;

namespace MiniEcommerce.Services;

/// <summary>Gerencia itens, respeitando o saldo atual do estoque.</summary>
public sealed class CarrinhoDeCompras
{
    private readonly Dictionary<int, ItemCarrinho> _itens = new();

    public IReadOnlyList<ItemCarrinho> Itens =>
        _itens.Values.OrderBy(item => item.Produto.Id).ToArray();

    public decimal Subtotal => _itens.Values.Sum(item => item.Subtotal);

    public void AdicionarProduto(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser positiva.");

        _itens.TryGetValue(produto.Id, out var itemExistente);
        int quantidadeTotal = (itemExistente?.Quantidade ?? 0) + quantidade;

        if (quantidadeTotal > produto.QuantidadeEstoque)
            throw new InvalidOperationException("Quantidade solicitada maior que o estoque disponivel.");

        if (itemExistente is null)
            _itens.Add(produto.Id, new ItemCarrinho(produto, quantidade));
        else
            itemExistente.Acrescentar(quantidade);
    }

    public bool RemoverProduto(int produtoId) => _itens.Remove(produtoId);

    internal void Esvaziar() => _itens.Clear();
}
