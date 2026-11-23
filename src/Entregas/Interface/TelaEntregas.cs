using Entregas.Algoritmos;
using Entregas.Models;
using Entregas.Servicos;

namespace Entregas.Interface;

public class TelaEntregas
{
    private readonly ServicoEntregas _servico;
    private readonly FilaEntregas _fila;

    public TelaEntregas(ServicoEntregas servico, FilaEntregas fila)
    {
        _servico = servico;
        _fila = fila;
    }

    public void MostrarFila()
    {
        Apresentacao.ExibirTabela("FILA DE ENTREGAS", _fila.Itens);

        if (_fila.Quantidade == 0)
        {
            Console.WriteLine("Não existem entregas pendentes!");
        }
    }

    public void OrganizarPorPrioridade()
    {
        if (_fila.Quantidade == 0)
        {
            Console.WriteLine("\nNão existem entregas pendentes!");
            return;
        }

        AlgoritmoOrdenacao algoritmo = TelaConsultas.EscolherAlgoritmo();
        List<Pedido> organizados = _servico.OrganizarFilaPorPrioridade(algoritmo);

        Apresentacao.ExibirTabela($"FILA ORGANIZADA POR PRIORIDADE ({algoritmo} Sort)", organizados);
    }

    public void RealizarEntrega()
    {
        ResultadoEntrega resultado = _servico.RealizarEntrega();

        if (resultado.Status == StatusEntrega.FilaVazia)
        {
            Console.WriteLine("\nNão existem pedidos para entregar!");
            return;
        }

        Console.WriteLine("\nEntrega realizada!");
        Apresentacao.ExibirPedido(resultado.Pedido!);
    }
}
