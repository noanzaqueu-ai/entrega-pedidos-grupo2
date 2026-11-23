using Entregas.Algoritmos;
using Entregas.Models;
using Entregas.Servicos;
using static Entregas.Tests.Executor;

namespace Entregas.Tests;

public static class TestesAlgoritmos
{
    private static readonly int[] OrdemPorPrioridade = { 101, 107, 104, 108, 102, 106, 105, 103, 109 };

    public static void Executar()
    {
        TestarOrdenacao();
        TestarBuscaBinaria();
        TestarBuscaLinear();
        TestarSelecao();
    }

    public static ListaPedidos MontarLista()
    {
        var lista = new ListaPedidos();

        foreach (Pedido pedido in DadosExemplo.Pedidos())
        {
            lista.Adicionar(pedido);
        }

        return lista;
    }

    private static IEnumerable<int> Referencia(IEnumerable<Pedido> pedidos, CriterioOrdenacao criterio)
    {
        IEnumerable<Pedido> ordenados = criterio switch
        {
            CriterioOrdenacao.Prioridade => pedidos.OrderBy(p => p.Prioridade).ThenBy(p => p.Sequencia),
            CriterioOrdenacao.Codigo => pedidos.OrderBy(p => p.Codigo),
            CriterioOrdenacao.Cliente => pedidos.OrderBy(p => p.Cliente, StringComparer.OrdinalIgnoreCase)
                .ThenBy(p => p.Sequencia),
            _ => pedidos.OrderBy(p => p.Endereco, StringComparer.OrdinalIgnoreCase)
                .ThenBy(p => p.Sequencia)
        };

        return ordenados.Select(p => p.Codigo);
    }

    private static void TestarOrdenacao()
    {
        foreach (AlgoritmoOrdenacao algoritmo in Enum.GetValues<AlgoritmoOrdenacao>())
        {
            foreach (CriterioOrdenacao criterio in Enum.GetValues<CriterioOrdenacao>())
            {
                Teste($"Ordenação {algoritmo} por {criterio}", () =>
                {
                    ListaPedidos lista = MontarLista();
                    List<Pedido> resultado = Ordenacao.Ordenar(lista.Pedidos, algoritmo, criterio);

                    Sequencia(Referencia(lista.Pedidos, criterio), resultado.Select(p => p.Codigo));
                });
            }
        }

        Teste("Ordenação por prioridade: alta primeiro, empate pela ordem de cadastro", () =>
        {
            ListaPedidos lista = MontarLista();

            foreach (AlgoritmoOrdenacao algoritmo in Enum.GetValues<AlgoritmoOrdenacao>())
            {
                Sequencia(OrdemPorPrioridade,
                    Ordenacao.Ordenar(lista.Pedidos, algoritmo, CriterioOrdenacao.Prioridade)
                        .Select(p => p.Codigo));
            }
        });

        Teste("Ordenação não altera a lista original", () =>
        {
            ListaPedidos lista = MontarLista();
            int[] antes = lista.Pedidos.Select(p => p.Codigo).ToArray();

            Ordenacao.Ordenar(lista.Pedidos, AlgoritmoOrdenacao.Bubble, CriterioOrdenacao.Prioridade);

            Sequencia(antes, lista.Pedidos.Select(p => p.Codigo));
        });

        Teste("Ordenação de lista vazia e de um elemento", () =>
        {
            foreach (AlgoritmoOrdenacao algoritmo in Enum.GetValues<AlgoritmoOrdenacao>())
            {
                Igual(0, Ordenacao.Ordenar(new List<Pedido>(), algoritmo, CriterioOrdenacao.Prioridade).Count);

                var unico = new List<Pedido> { DadosExemplo.Pedidos()[0] };
                Igual(1, Ordenacao.Ordenar(unico, algoritmo, CriterioOrdenacao.Cliente).Count);
            }
        });
    }

    private static void TestarBuscaBinaria()
    {
        Teste("Busca binária encontra todos os códigos da lista", () =>
        {
            ListaPedidos lista = MontarLista();

            foreach (Pedido pedido in DadosExemplo.Pedidos())
            {
                Igual(pedido.Cliente, Busca.BinariaPorCodigo(lista.Pedidos, pedido.Codigo)?.Cliente);
            }
        });

        Teste("Busca binária retorna nulo para código inexistente", () =>
        {
            ListaPedidos lista = MontarLista();

            Verdadeiro(Busca.BinariaPorCodigo(lista.Pedidos, 0) is null);
            Verdadeiro(Busca.BinariaPorCodigo(lista.Pedidos, 110) is null);
            Verdadeiro(Busca.BinariaPorCodigo(lista.Pedidos, 999) is null);
        });

        Teste("Busca binária em lista vazia", () =>
        {
            Verdadeiro(Busca.BinariaPorCodigo(new List<Pedido>(), 1) is null);
        });
    }

    private static void TestarBuscaLinear()
    {
        Teste("Busca linear por cliente (parcial e sem diferenciar maiúsculas)", () =>
        {
            ListaPedidos lista = MontarLista();

            Sequencia(new[] { 101, 102 },
                Busca.LinearPorTexto(lista.Pedidos, CampoBusca.Cliente, "CARLOS").Select(p => p.Codigo));
            Sequencia(new[] { 104 },
                Busca.LinearPorTexto(lista.Pedidos, CampoBusca.Cliente, "patrícia").Select(p => p.Codigo));
        });

        Teste("Busca linear por endereço retorna todas as ocorrências", () =>
        {
            ListaPedidos lista = MontarLista();

            Sequencia(new[] { 101, 107 },
                Busca.LinearPorTexto(lista.Pedidos, CampoBusca.Endereco, "av. brasil").Select(p => p.Codigo));
            Sequencia(new[] { 102, 105, 108 },
                Busca.LinearPorTexto(lista.Pedidos, CampoBusca.Endereco, "centro").Select(p => p.Codigo));
        });

        Teste("Busca linear sem resultado ou com termo vazio", () =>
        {
            ListaPedidos lista = MontarLista();

            Igual(0, Busca.LinearPorTexto(lista.Pedidos, CampoBusca.Cliente, "xyz").Count);
            Igual(0, Busca.LinearPorTexto(lista.Pedidos, CampoBusca.Endereco, "   ").Count);
        });
    }

    private static void TestarSelecao()
    {
        Teste("Seleção por situação (pendentes e entregues)", () =>
        {
            ListaPedidos lista = MontarLista();
            lista.Buscar(104)!.Status = StatusPedido.Entregue;

            Igual(8, Selecao.PorStatus(lista.Pedidos, StatusPedido.Pendente).Count);
            Sequencia(new[] { 104 },
                Selecao.PorStatus(lista.Pedidos, StatusPedido.Entregue).Select(p => p.Codigo));
        });

        Teste("Seleção por prioridade", () =>
        {
            ListaPedidos lista = MontarLista();

            Sequencia(new[] { 101, 104, 107 },
                Selecao.PorPrioridade(lista.Pedidos, Prioridade.Alta).Select(p => p.Codigo));
            Igual(3, Selecao.PorPrioridade(lista.Pedidos, Prioridade.Media).Count);
            Igual(3, Selecao.PorPrioridade(lista.Pedidos, Prioridade.Baixa).Count);
        });
    }
}
