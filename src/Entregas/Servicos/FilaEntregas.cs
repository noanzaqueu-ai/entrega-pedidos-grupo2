using Entregas.Models;

namespace Entregas.Servicos;

/// <summary>
/// Fila (FIFO) de pedidos pendentes aguardando entrega.
/// </summary>
public class FilaEntregas
{
    private Queue<Pedido> _fila = new();

    public int Quantidade => _fila.Count;

    public IReadOnlyCollection<Pedido> Itens => _fila;

    public void Enfileirar(Pedido pedido) => _fila.Enqueue(pedido);

    public Pedido? Desenfileirar() => _fila.Count > 0 ? _fila.Dequeue() : null;

    public Pedido? Proximo() => _fila.Count > 0 ? _fila.Peek() : null;

    /// <summary>
    /// Reconstrói a fila com os pedidos na ordem informada.
    /// </summary>
    public void Reorganizar(IEnumerable<Pedido> novaOrdem)
    {
        _fila = new Queue<Pedido>(novaOrdem);
    }
}
