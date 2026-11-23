using Entregas.Models;

namespace Entregas.Algoritmos;

public enum CampoBusca
{
    Cliente,
    Endereco
}

public static class Busca
{
    public static List<Pedido> LinearPorTexto(IReadOnlyList<Pedido> pedidos, CampoBusca campo, string termo)
    {
        var encontrados = new List<Pedido>();

        if (string.IsNullOrWhiteSpace(termo))
        {
            return encontrados;
        }

        foreach (Pedido pedido in pedidos)
        {
            string texto = campo == CampoBusca.Cliente ? pedido.Cliente : pedido.Endereco;

            if (texto.Contains(termo, StringComparison.OrdinalIgnoreCase))
            {
                encontrados.Add(pedido);
            }
        }

        return encontrados;
    }

    public static Pedido? BinariaPorCodigo(IReadOnlyList<Pedido> pedidosOrdenadosPorCodigo, int codigo)
    {
        int inicio = 0;
        int fim = pedidosOrdenadosPorCodigo.Count - 1;

        while (inicio <= fim)
        {
            int meio = inicio + (fim - inicio) / 2;
            int comparacao = pedidosOrdenadosPorCodigo[meio].Codigo.CompareTo(codigo);

            if (comparacao == 0)
            {
                return pedidosOrdenadosPorCodigo[meio];
            }

            if (comparacao < 0)
            {
                inicio = meio + 1;
            }
            else
            {
                fim = meio - 1;
            }
        }

        return null;
    }
}
