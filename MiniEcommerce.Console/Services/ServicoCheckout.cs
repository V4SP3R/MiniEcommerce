using MiniEcommerce.Domain;

namespace MiniEcommerce.Services;

/// <summary>Confere todos os itens antes de alterar o estoque.</summary>
public sealed class ServicoCheckout
{
    public Pedido FinalizarPedido(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);
        var itens = carrinho.Itens;

        if (itens.Count == 0)
            throw new InvalidOperationException("O carrinho esta vazio.");

        // Validacao completa antes da primeira baixa evita compras parcialmente processadas.
        foreach (var item in itens)
        {
            if (item.Quantidade > item.Produto.QuantidadeEstoque)
                throw new InvalidOperationException($"Estoque insuficiente para {item.Produto.Nome}.");
        }

        var resumo = itens.Select(item => new ItemPedido(
            item.Produto.Id, item.Produto.Nome, item.Produto.Preco, item.Quantidade)).ToArray();
        var pedido = new Pedido(resumo);

        foreach (var item in itens)
            item.Produto.BaixarEstoque(item.Quantidade);

        carrinho.Esvaziar();
        return pedido;
    }
}
