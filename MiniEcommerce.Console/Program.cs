using System.Globalization;
using System.Text;
using MiniEcommerce.Domain;
using MiniEcommerce.Services;

Console.OutputEncoding = Encoding.UTF8;
var catalogo = new CatalogoProdutos();
var carrinho = new CarrinhoDeCompras();
var checkout = new ServicoCheckout();
var moeda = CultureInfo.GetCultureInfo("pt-BR");

while (true)
{
    Console.WriteLine("\n====== MINI E-COMMERCE ======");
    Console.WriteLine("1 - Cadastrar produto");
    Console.WriteLine("2 - Listar catalogo");
    Console.WriteLine("3 - Adicionar ao carrinho");
    Console.WriteLine("4 - Consultar carrinho");
    Console.WriteLine("5 - Remover item do carrinho");
    Console.WriteLine("6 - Finalizar pedido");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opcao: ");
    var opcao = Console.ReadLine();
    if (opcao is null || opcao.Trim() == "0")
        break;

    try
    {
        switch (opcao.Trim())
        {
            case "1":
                Console.Write("Nome: ");
                var nome = Console.ReadLine() ?? throw new EndOfStreamException();
                var preco = LerDecimal("Preco (ex.: 29,90): ");
                var estoque = LerInteiro("Quantidade em estoque: ");
                var novo = catalogo.CadastrarProduto(nome, preco, estoque);
                Console.WriteLine($"Produto cadastrado com ID {novo.Id}.");
                break;

            case "2":
                MostrarCatalogo();
                break;

            case "3":
                MostrarCatalogo();
                var produtoId = LerInteiro("ID do produto: ");
                var quantidade = LerInteiro("Quantidade desejada: ");
                carrinho.AdicionarProduto(catalogo.ObterPorId(produtoId), quantidade);
                Console.WriteLine("Produto adicionado ao carrinho.");
                break;

            case "4":
                MostrarCarrinho();
                break;

            case "5":
                MostrarCarrinho();
                var idRemover = LerInteiro("ID a remover: ");
                Console.WriteLine(carrinho.RemoverProduto(idRemover)
                    ? "Item removido do carrinho."
                    : "Esse produto nao esta no carrinho.");
                break;

            case "6":
                var pedido = checkout.FinalizarPedido(carrinho);
                Console.WriteLine($"\nPedido confirmado: {pedido.Id}");
                foreach (var item in pedido.Itens)
                    Console.WriteLine($"- {item.NomeProduto} | {item.Quantidade} x {Moeda(item.PrecoUnitario)} = {Moeda(item.Subtotal)}");
                Console.WriteLine($"TOTAL: {Moeda(pedido.Total)}");
                Console.WriteLine("Estoque atualizado e carrinho esvaziado.");
                break;

            default:
                Console.WriteLine("Opcao invalida. Tente novamente.");
                break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException or FormatException)
    {
        Console.WriteLine($"Atencao: {ex.Message}");
    }
    catch (EndOfStreamException)
    {
        break;
    }
}

Console.WriteLine("Programa encerrado.");

void MostrarCatalogo()
{
    var produtos = catalogo.ListarProdutos();
    Console.WriteLine("\n--- CATALOGO ---");
    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }

    foreach (Produto produto in produtos)
        Console.WriteLine($"{produto.Id} - {produto.Nome} | {Moeda(produto.Preco)} | Estoque: {produto.QuantidadeEstoque}");
}

void MostrarCarrinho()
{
    Console.WriteLine("\n--- CARRINHO ---");
    if (carrinho.Itens.Count == 0)
    {
        Console.WriteLine("Carrinho vazio.");
        return;
    }

    foreach (var item in carrinho.Itens)
        Console.WriteLine($"{item.Produto.Id} - {item.Produto.Nome} | Qtde: {item.Quantidade} | Subtotal: {Moeda(item.Subtotal)}");

    Console.WriteLine($"Subtotal geral: {Moeda(carrinho.Subtotal)}");
}

static int LerInteiro(string mensagem)
{
    Console.Write(mensagem);
    var entrada = Console.ReadLine() ?? throw new EndOfStreamException();
    if (!int.TryParse(entrada, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numero))
        throw new FormatException("Informe um numero inteiro valido.");
    return numero;
}

static decimal LerDecimal(string mensagem)
{
    Console.Write(mensagem);
    var entrada = Console.ReadLine() ?? throw new EndOfStreamException();
    var estilo = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
    if (decimal.TryParse(entrada, estilo, CultureInfo.GetCultureInfo("pt-BR"), out var numero)
        || decimal.TryParse(entrada, estilo, CultureInfo.InvariantCulture, out numero))
        return numero;
    throw new FormatException("Informe um valor monetario valido (ex.: 29,90).");
}

string Moeda(decimal valor) => valor.ToString("C2", moeda);
