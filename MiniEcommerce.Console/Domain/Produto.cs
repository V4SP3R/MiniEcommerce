namespace MiniEcommerce.Domain;

/// <summary>Produto com preco fixo e estoque controlado.</summary>
public sealed class Produto
{
    public int Id { get; }
    public string Nome { get; }
    public decimal Preco { get; }
    public int QuantidadeEstoque { get; private set; }

    public Produto(int id, string nome, decimal preco, int quantidadeEstoque)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "O ID deve ser positivo.");
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome e obrigatorio.", nameof(nome));
        if (preco < 0)
            throw new ArgumentOutOfRangeException(nameof(preco), "O preco nao pode ser negativo.");
        if (quantidadeEstoque < 0)
            throw new ArgumentOutOfRangeException(nameof(quantidadeEstoque), "O estoque nao pode ser negativo.");

        Id = id;
        Nome = nome.Trim();
        Preco = preco;
        QuantidadeEstoque = quantidadeEstoque;
    }

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser positiva.");
        if (quantidade > QuantidadeEstoque)
            throw new InvalidOperationException($"Estoque insuficiente para {Nome}.");

        QuantidadeEstoque -= quantidade;
    }
}
