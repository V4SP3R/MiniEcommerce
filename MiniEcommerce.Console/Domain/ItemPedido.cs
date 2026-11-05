namespace MiniEcommerce.Domain;

/// <summary>Registro imutavel de um item confirmado no pedido.</summary>
public sealed record ItemPedido(int ProdutoId, string NomeProduto, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}
