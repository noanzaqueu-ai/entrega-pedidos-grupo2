using Entregas.Models;

namespace Entregas.Interface;

public static class Apresentacao
{
    private const int LarguraMenu = 36;

    public static void ExibirMenu(string titulo, params string[] opcoes)
    {
        string borda = "+" + new string('-', LarguraMenu + 2) + "+";

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine(borda);
        Console.WriteLine(Linha(Centralizar(titulo)));
        Console.WriteLine(borda);

        foreach (string opcao in opcoes)
        {
            Console.WriteLine(Linha(opcao));
        }

        Console.WriteLine(borda);
        Console.ResetColor();
    }

    public static void ExibirTabela(string titulo, IReadOnlyCollection<Pedido> pedidos)
    {
        Console.WriteLine($"\n===== {titulo} =====");

        if (pedidos.Count == 0)
        {
            Console.WriteLine("Nenhum pedido encontrado!");
            return;
        }

        Console.WriteLine(FormatarLinha("Cód", "Cliente", "Endereço", "Prioridade", "Situação"));
        Console.WriteLine(new string('-', 100));

        foreach (Pedido pedido in pedidos)
        {
            Console.WriteLine(FormatarLinha(
                pedido.Codigo.ToString(),
                pedido.Cliente,
                pedido.Endereco,
                NomePrioridade(pedido.Prioridade),
                pedido.Status == StatusPedido.Pendente ? "Pendente" : "Entregue"));
        }

        Console.WriteLine($"\nTotal: {pedidos.Count} pedido(s)");
    }

    public static void ExibirPedido(Pedido pedido)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine($"Código: {pedido.Codigo}");
        Console.WriteLine($"Cliente: {pedido.Cliente}");
        Console.WriteLine($"Endereço: {pedido.Endereco}");
        Console.WriteLine($"Prioridade: {NomePrioridade(pedido.Prioridade)}");
        Console.WriteLine($"Situação: {(pedido.Status == StatusPedido.Pendente ? "Pendente" : "Entregue")}");
    }

    public static string NomePrioridade(Prioridade prioridade) => prioridade switch
    {
        Prioridade.Alta => "Alta",
        Prioridade.Media => "Média",
        _ => "Baixa"
    };

    private static string FormatarLinha(
        string codigo, string cliente, string endereco, string prioridade, string situacao) =>
        $"{codigo,-6}{Truncar(cliente, 24),-26}{Truncar(endereco, 40),-42}{prioridade,-12}{situacao}";

    private static string Truncar(string texto, int tamanho) =>
        texto.Length <= tamanho ? texto : texto[..(tamanho - 1)] + "…";

    private static string Linha(string texto) => "| " + texto.PadRight(LarguraMenu) + " |";

    private static string Centralizar(string texto) =>
        new string(' ', Math.Max(0, (LarguraMenu - texto.Length) / 2)) + texto;
}
