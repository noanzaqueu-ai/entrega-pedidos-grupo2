using Entregas.Models;

namespace Entregas.Algoritmos;

public enum AlgoritmoOrdenacao
{
    Bubble,
    Selection,
    Insertion
}

public static class Ordenacao
{
    public static List<Pedido> Ordenar(
        IReadOnlyList<Pedido> pedidos,
        AlgoritmoOrdenacao algoritmo,
        CriterioOrdenacao criterio)
    {
        Comparison<Pedido> comparar = Comparadores.Para(criterio);

        return algoritmo switch
        {
            AlgoritmoOrdenacao.Bubble => BubbleSort(pedidos, comparar),
            AlgoritmoOrdenacao.Selection => SelectionSort(pedidos, comparar),
            AlgoritmoOrdenacao.Insertion => InsertionSort(pedidos, comparar),
            _ => throw new ArgumentOutOfRangeException(nameof(algoritmo))
        };
    }

    public static List<Pedido> BubbleSort(IReadOnlyList<Pedido> pedidos, Comparison<Pedido> comparar)
    {
        var lista = new List<Pedido>(pedidos);

        for (int i = 0; i < lista.Count - 1; i++)
        {
            bool trocou = false;

            for (int j = 0; j < lista.Count - i - 1; j++)
            {
                if (comparar(lista[j], lista[j + 1]) > 0)
                {
                    Trocar(lista, j, j + 1);
                    trocou = true;
                }
            }

            if (!trocou)
            {
                break;
            }
        }

        return lista;
    }

    public static List<Pedido> SelectionSort(IReadOnlyList<Pedido> pedidos, Comparison<Pedido> comparar)
    {
        var lista = new List<Pedido>(pedidos);

        for (int i = 0; i < lista.Count - 1; i++)
        {
            int menor = i;

            for (int j = i + 1; j < lista.Count; j++)
            {
                if (comparar(lista[j], lista[menor]) < 0)
                {
                    menor = j;
                }
            }

            if (menor != i)
            {
                Trocar(lista, i, menor);
            }
        }

        return lista;
    }

    public static List<Pedido> InsertionSort(IReadOnlyList<Pedido> pedidos, Comparison<Pedido> comparar)
    {
        var lista = new List<Pedido>(pedidos);

        for (int i = 1; i < lista.Count; i++)
        {
            Pedido chave = lista[i];
            int j = i - 1;

            while (j >= 0 && comparar(lista[j], chave) > 0)
            {
                lista[j + 1] = lista[j];
                j--;
            }

            lista[j + 1] = chave;
        }

        return lista;
    }

    private static void Trocar(List<Pedido> lista, int a, int b)
    {
        (lista[a], lista[b]) = (lista[b], lista[a]);
    }
}
