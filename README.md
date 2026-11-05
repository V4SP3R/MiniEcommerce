# MiniEcommerce — Console .NET + testes xUnit

Projeto acadêmico da disciplina **Gestão e Qualidade de Software / Garantia da Qualidade de Software**.
O objetivo é desenvolver um mini e-commerce em C# com **TDD básico**, **testes unitários xUnit** e histórico Git em **Conventional Commits**.

## Requisitos atendidos

- Cadastro de produtos com **nome, preço e estoque**, impedindo preços e estoques negativos.
- Catálogo com consulta de preços e quantidades em estoque.
- Carrinho com adição de produtos, soma de quantidades, verificação de estoque e remoção de itens.
- Cálculo do subtotal dos itens antes da compra.
- Checkout com resumo do pedido, total em reais e baixa de estoque após validação de **todos** os itens.
- Limpeza do carrinho depois do pedido confirmado e preservação do carrinho quando o checkout falha.
- Menu de console para operar todo o fluxo.

> **Escopo:** aplicação didática em memória, sem banco de dados, autenticação ou pagamento real. Os dados são reiniciados quando o programa termina.

## Tecnologias

- .NET SDK **8.0** (C#)
- **xUnit 2.9.2** para testes unitários
- **Git** com mensagens em Conventional Commits
- GitHub Actions para validação automatizada dos testes em push / pull request

## Estrutura de pastas

```text
MiniEcommerce/
├── MiniEcommerce.sln
├── MiniEcommerce.Console/
│   ├── Domain/
│   │   ├── Produto.cs
│   │   ├── ItemCarrinho.cs
│   │   ├── ItemPedido.cs
│   │   └── Pedido.cs
│   ├── Services/
│   │   ├── CatalogoProdutos.cs
│   │   ├── CarrinhoDeCompras.cs
│   │   └── ServicoCheckout.cs
│   ├── Program.cs
│   └── MiniEcommerce.Console.csproj
├── MiniEcommerce.Tests/
│   ├── ProdutoTests.cs
│   ├── CatalogoProdutosTests.cs
│   ├── CarrinhoDeComprasTests.cs
│   ├── CheckoutTests.cs
│   └── MiniEcommerce.Tests.csproj
├── .github/workflows/dotnet-tests.yml
└── README.md
```

## Como executar

1. Instale o [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0).
2. Na pasta raiz do projeto, restaure as dependências e execute:

```bash
dotnet restore MiniEcommerce.sln
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

No menu, use as opções: `1` cadastrar, `2` listar, `3` adicionar ao carrinho, `4` consultar carrinho, `5` remover e `6` finalizar a compra.

### Exemplo de fluxo

1. Cadastrar `Mouse`, preço `49,90`, estoque `5`.
2. Adicionar `2` unidades ao carrinho.
3. Consultar o carrinho: subtotal **R$ 99,80**.
4. Finalizar pedido: total **R$ 99,80**.
5. Consultar o catálogo: estoque atualizado para **3**.

## Executar os testes

```bash
dotnet test MiniEcommerce.sln
```

A suíte testa as regras de produto e catálogo, as validações de quantidade no carrinho, remoção, subtotal, checkout, total, limpeza e proteção contra baixa parcial do estoque.

## TDD e histórico Git

Cada etapa de domínio foi dividida em commit de **testes primeiro (red)** e commit de **implementação depois (green)**. O histórico conserva esses commits separados para auditoria:

```bash
git log --oneline
```

Exemplos de mensagens usadas:

- `test(produto): add unit tests for product validation rules`
- `feat(produto): implement product entity and validation logic`
- `test(carrinho): add unit tests for cart addition and stock limits`
- `feat(carrinho): implement shopping cart logic and stock verification`
- `test(pedido): add unit tests for checkout and stock deduction`
- `feat(pedido): implement order processing and inventory update`
- `feat(console): implement interactive menu for e-commerce store`
- `refactor(core): improve code readability and organize formatting`
- `docs: add comprehensive instructions in README.md`

> O uso de TDD aqui registra a ordem de criação dos testes e do código. A execução real da suíte deve ser confirmada em um ambiente com o SDK instalado (local ou pelo GitHub Actions).

## Publicar no GitHub

Crie um repositório **público**, vazio, com o nome `MiniEcommerce`. Dentro da pasta do projeto, execute:

```bash
git remote add origin https://github.com/SEU_USUARIO/MiniEcommerce.git
git branch -M main
git push -u origin main
```

Para projetos extraídos do ZIP com `.git`, **não use `git init` novamente**; o histórico completo já está presente.

## Observações técnicas

- Usa `decimal` para evitar imprecisão típica de `float`/`double` no cálculo financeiro.
- O estoque é apenas consultado ao adicionar ao carrinho e é reduzido somente na confirmação do pedido.
- Antes de atualizar qualquer produto, o checkout valida **todos** os saldos: caso algum esteja insuficiente, nenhuma baixa é realizada.
- Não há bloqueio de concorrência entre usuários: a solução é um programa de console de instância única, sem persistência.
