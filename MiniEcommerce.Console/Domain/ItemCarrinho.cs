namespace MiniEcommerce.Domain;

/// <summary>Quantidade selecionada de um produto ainda nao comprado.</summary>
public sealed class ItemCarrinho
{
    public Produto Produto { get; }
    public int Quantidade { get; private set; }
    public decimal Subtotal => Produto.Preco * Quantidade;

    public ItemCarrinho(Produto produto, int quantidade)
    {
        Produto = produto ?? throw new ArgumentNullException(nameof(produto));
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade));

        Quantidade = quantidade;
    }

    internal void Acrescentar(int quantidade) => Quantidade += quantidade;
}
