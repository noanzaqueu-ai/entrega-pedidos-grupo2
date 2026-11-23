using Entregas.Algoritmos;
using Entregas.Models;
using Entregas.Servicos;
using static Entregas.Tests.Executor;

namespace Entregas.Tests;

public static class TestesServicos
{
    private static readonly int[] OrdemPorPrioridade = { 101, 107, 104, 108, 102, 106, 105, 103, 109 };
    private static readonly int[] OrdemDeChegada = { 105, 101, 108, 103, 107, 102, 104, 106, 109 };

    public static void Executar()
    {
        TestarLista();
        TestarCadastro();
        TestarOrganizacaoDaFila();
        TestarEntregas();
    }

    private static void TestarLista()
    {
        Teste("Lista mantém pedidos ordenados por código", () =>
        {
            ListaPedidos lista = TestesAlgoritmos.MontarLista();

            Sequencia(new[] { 101, 102, 103, 104, 105, 106, 107, 108, 109 },
                lista.Pedidos.Select(p => p.Codigo));
        });

        Teste("Lista rejeita código duplicado", () =>
        {
            ListaPedidos lista = TestesAlgoritmos.MontarLista();

            Verdadeiro(!lista.Adicionar(new Pedido(101, "Outro", "Rua X, 1", Prioridade.Baixa)));
            Igual(9, lista.Pedidos.Count);
        });
    }

    private static void TestarCadastro()
    {
        Teste("Cadastro enfileira os pedidos na ordem de chegada (FIFO)", () =>
        {
            var (_, fila, servico) = Montar();

            foreach (Pedido pedido in DadosExemplo.Pedidos())
            {
                Igual(StatusCadastro.Cadastrado, servico.Cadastrar(pedido));
            }

            Sequencia(OrdemDeChegada, fila.Itens.Select(p => p.Codigo));
        });

        Teste("Cadastro com código repetido não entra na fila", () =>
        {
            var (_, fila, servico) = Montar();

            servico.Cadastrar(new Pedido(1, "Ana", "Rua A, 1", Prioridade.Alta));
            StatusCadastro status = servico.Cadastrar(new Pedido(1, "Bia", "Rua B, 2", Prioridade.Baixa));

            Igual(StatusCadastro.CodigoDuplicado, status);
            Igual(1, fila.Quantidade);
        });
    }

    private static void TestarOrganizacaoDaFila()
    {
        foreach (AlgoritmoOrdenacao algoritmo in Enum.GetValues<AlgoritmoOrdenacao>())
        {
            Teste($"Fila organizada por prioridade com {algoritmo} Sort", () =>
            {
                var (_, fila, servico) = MontarComExemplos();

                servico.OrganizarFilaPorPrioridade(algoritmo);

                Sequencia(OrdemPorPrioridade, fila.Itens.Select(p => p.Codigo));
            });
        }

        Teste("Pedido cadastrado depois de organizar vai para o fim da fila", () =>
        {
            var (_, fila, servico) = MontarComExemplos();

            servico.OrganizarFilaPorPrioridade(AlgoritmoOrdenacao.Insertion);
            servico.Cadastrar(new Pedido(200, "Novo Cliente", "Rua Nova, 9", Prioridade.Alta));

            Igual(200, fila.Itens.Last().Codigo);
            Igual(10, fila.Quantidade);
        });
    }

    private static void TestarEntregas()
    {
        Teste("Entrega retira o primeiro da fila e marca o pedido como entregue", () =>
        {
            var (lista, fila, servico) = MontarComExemplos();

            ResultadoEntrega resultado = servico.RealizarEntrega();

            Igual(StatusEntrega.Entregue, resultado.Status);
            Igual(105, resultado.Pedido?.Codigo);
            Igual(StatusPedido.Entregue, lista.Buscar(105)!.Status);
            Igual(8, fila.Quantidade);
            Igual(9, lista.Pedidos.Count);
        });

        Teste("Entregas seguem a ordem de prioridade após organizar a fila", () =>
        {
            var (_, _, servico) = MontarComExemplos();

            servico.OrganizarFilaPorPrioridade(AlgoritmoOrdenacao.Selection);

            var entregues = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                entregues.Add(servico.RealizarEntrega().Pedido!.Codigo);
            }

            Sequencia(new[] { 101, 107, 104, 108 }, entregues);
        });

        Teste("Entrega com fila vazia", () =>
        {
            var (_, _, servico) = Montar();

            ResultadoEntrega resultado = servico.RealizarEntrega();

            Igual(StatusEntrega.FilaVazia, resultado.Status);
            Verdadeiro(resultado.Pedido is null);
        });

        Teste("Fila esvazia depois de entregar todos os pedidos", () =>
        {
            var (lista, fila, servico) = MontarComExemplos();

            for (int i = 0; i < 9; i++)
            {
                servico.RealizarEntrega();
            }

            Igual(0, fila.Quantidade);
            Igual(StatusEntrega.FilaVazia, servico.RealizarEntrega().Status);
            Igual(9, Selecao.PorStatus(lista.Pedidos, StatusPedido.Entregue).Count);
        });
    }

    private static (ListaPedidos, FilaEntregas, ServicoEntregas) Montar()
    {
        var lista = new ListaPedidos();
        var fila = new FilaEntregas();

        return (lista, fila, new ServicoEntregas(lista, fila));
    }

    private static (ListaPedidos, FilaEntregas, ServicoEntregas) MontarComExemplos()
    {
        var conjunto = Montar();

        foreach (Pedido pedido in DadosExemplo.Pedidos())
        {
            conjunto.Item3.Cadastrar(pedido);
        }

        return conjunto;
    }
}
