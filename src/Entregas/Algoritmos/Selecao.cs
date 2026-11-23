using Entregas.Models;

namespace Entregas.Algoritmos;

public static class Selecao
{
    public static List<Pedido> Onde(IReadOnlyList<Pedido> pedidos, Predicate<Pedido> criterio)
    {
        var selecionados = new List<Pedido>();

        foreach (Pedido pedido in pedidos)
        {
            if (criterio(pedido))
            {
                selecionados.Add(pedido);
            }
        }

        return selecionados;
    }

    public static List<Pedido> PorStatus(IReadOnlyList<Pedido> pedidos, StatusPedido status) =>
        Onde(pedidos, pedido => pedido.Status == status);

    public static List<Pedido> PorPrioridade(IReadOnlyList<Pedido> pedidos, Prioridade prioridade) =>
        Onde(pedidos, pedido => pedido.Prioridade == prioridade);
}
