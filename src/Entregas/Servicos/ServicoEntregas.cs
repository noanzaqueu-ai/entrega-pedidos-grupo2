using Entregas.Algoritmos;
using Entregas.Models;

namespace Entregas.Servicos;

public enum StatusCadastro
{
    Cadastrado,
    CodigoDuplicado
}

public enum StatusEntrega
{
    Entregue,
    FilaVazia
}

public record ResultadoEntrega(StatusEntrega Status, Pedido? Pedido);

public class ServicoEntregas
{
    private readonly ListaPedidos _lista;
    private readonly FilaEntregas _fila;

    public ServicoEntregas(ListaPedidos lista, FilaEntregas fila)
    {
        _lista = lista;
        _fila = fila;
    }

    public StatusCadastro Cadastrar(Pedido pedido)
    {
        if (!_lista.Adicionar(pedido))
        {
            return StatusCadastro.CodigoDuplicado;
        }

        _fila.Enfileirar(pedido);
        return StatusCadastro.Cadastrado;
    }

    /// <summary>
    /// Reordena a fila por prioridade (alta primeiro). Pedidos de mesma
    /// prioridade mantêm a ordem de chegada.
    /// </summary>
    public List<Pedido> OrganizarFilaPorPrioridade(AlgoritmoOrdenacao algoritmo)
    {
        List<Pedido> ordenados = Ordenacao.Ordenar(
            _fila.Itens.ToList(), algoritmo, CriterioOrdenacao.Prioridade);

        _fila.Reorganizar(ordenados);
        return ordenados;
    }

    public ResultadoEntrega RealizarEntrega()
    {
        Pedido? pedido = _fila.Desenfileirar();

        if (pedido is null)
        {
            return new ResultadoEntrega(StatusEntrega.FilaVazia, null);
        }

        pedido.Status = StatusPedido.Entregue;
        return new ResultadoEntrega(StatusEntrega.Entregue, pedido);
    }
}
