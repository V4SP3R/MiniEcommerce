namespace MiniEcommerce.Domain;

/// <summary>Comprovante de uma compra finalizada.</summary>
public sealed class Pedido
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset Data { get; } = DateTimeOffset.UtcNow;
    public IReadOnlyList<ItemPedido> Itens { get; }
    public decimal Total { get; }

    public Pedido(IEnumerable<ItemPedido> itens)
    {
        ArgumentNullException.ThrowIfNull(itens);
        var copia = itens.ToArray();
        if (copia.Length == 0)
            throw new ArgumentException("O pedido precisa de pelo menos um item.", nameof(itens));

        Itens = Array.AsReadOnly(copia);
        Total = copia.Sum(item => item.Subtotal);
    }
}
