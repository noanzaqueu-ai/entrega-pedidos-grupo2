using Entregas.Algoritmos;
using Entregas.Models;
using Entregas.Servicos;

namespace Entregas.Interface;

public class TelaConsultas
{
    private readonly ListaPedidos _lista;

    public TelaConsultas(ListaPedidos lista)
    {
        _lista = lista;
    }

    public void Buscar()
    {
        Apresentacao.ExibirMenu(
            "BUSCAR PEDIDO",
            "1 - Por código (binária)",
            "2 - Por cliente (linear)",
            "3 - Por endereço (linear)",
            "0 - Voltar");

        switch (Entrada.LerInteiro("Escolha: ", 0, 3))
        {
            case 1:
                BuscarPorCodigo();
                break;
            case 2:
                BuscarPorTexto(CampoBusca.Cliente, "nome do cliente");
                break;
            case 3:
                BuscarPorTexto(CampoBusca.Endereco, "endereço");
                break;
        }
    }

    public void Ordenar()
    {
        Apresentacao.ExibirMenu(
            "ORDENAR PEDIDOS",
            "1 - Por prioridade",
            "2 - Por código",
            "3 - Por cliente",
            "4 - Por endereço",
            "0 - Voltar");

        int opcaoCriterio = Entrada.LerInteiro("Escolha o critério: ", 0, 4);

        if (opcaoCriterio == 0)
        {
            return;
        }

        CriterioOrdenacao criterio = opcaoCriterio switch
        {
            1 => CriterioOrdenacao.Prioridade,
            2 => CriterioOrdenacao.Codigo,
            3 => CriterioOrdenacao.Cliente,
            _ => CriterioOrdenacao.Endereco
        };

        AlgoritmoOrdenacao algoritmo = EscolherAlgoritmo();

        List<Pedido> ordenados = Ordenacao.Ordenar(_lista.Pedidos, algoritmo, criterio);
        Apresentacao.ExibirTabela(
            $"PEDIDOS ORDENADOS ({algoritmo} Sort / {NomeCriterio(criterio)})", ordenados);
    }

    public void Selecionar()
    {
        Apresentacao.ExibirMenu(
            "SELECIONAR PEDIDOS",
            "1 - Pendentes",
            "2 - Entregues",
            "3 - Por prioridade",
            "0 - Voltar");

        switch (Entrada.LerInteiro("Escolha: ", 0, 3))
        {
            case 1:
                Apresentacao.ExibirTabela(
                    "PEDIDOS PENDENTES", Selecao.PorStatus(_lista.Pedidos, StatusPedido.Pendente));
                break;
            case 2:
                Apresentacao.ExibirTabela(
                    "PEDIDOS ENTREGUES", Selecao.PorStatus(_lista.Pedidos, StatusPedido.Entregue));
                break;
            case 3:
                SelecionarPorPrioridade();
                break;
        }
    }

    public static AlgoritmoOrdenacao EscolherAlgoritmo()
    {
        Apresentacao.ExibirMenu(
            "ALGORITMO",
            "1 - Bubble Sort",
            "2 - Selection Sort",
            "3 - Insertion Sort");

        return Entrada.LerInteiro("Escolha o algoritmo: ", 1, 3) switch
        {
            1 => AlgoritmoOrdenacao.Bubble,
            2 => AlgoritmoOrdenacao.Selection,
            _ => AlgoritmoOrdenacao.Insertion
        };
    }

    private void SelecionarPorPrioridade()
    {
        int opcao = Entrada.LerInteiro("\nPrioridade (1 - Alta, 2 - Média, 3 - Baixa): ", 1, 3);
        var prioridade = (Prioridade)opcao;

        Apresentacao.ExibirTabela(
            $"PEDIDOS DE PRIORIDADE {Apresentacao.NomePrioridade(prioridade).ToUpper()}",
            Selecao.PorPrioridade(_lista.Pedidos, prioridade));
    }

    private void BuscarPorCodigo()
    {
        int codigo = Entrada.LerInteiro("\nDigite o código: ", 1);
        Pedido? encontrado = _lista.Buscar(codigo);

        if (encontrado is null)
        {
            Console.WriteLine("Pedido não encontrado!");
            return;
        }

        Console.WriteLine("\nPedido encontrado:");
        Apresentacao.ExibirPedido(encontrado);
    }

    private void BuscarPorTexto(CampoBusca campo, string nomeCampo)
    {
        string termo = Entrada.LerTexto($"\nDigite o {nomeCampo}: ");
        List<Pedido> encontrados = Busca.LinearPorTexto(_lista.Pedidos, campo, termo);

        Apresentacao.ExibirTabela("RESULTADO DA BUSCA", encontrados);
    }

    private static string NomeCriterio(CriterioOrdenacao criterio) => criterio switch
    {
        CriterioOrdenacao.Prioridade => "Prioridade",
        CriterioOrdenacao.Codigo => "Código",
        CriterioOrdenacao.Cliente => "Cliente",
        _ => "Endereço"
    };
}
