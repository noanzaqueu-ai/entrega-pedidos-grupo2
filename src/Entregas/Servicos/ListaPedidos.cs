using Entregas.Algoritmos;
using Entregas.Models;

namespace Entregas.Servicos;

/// <summary>
/// Lista linear de pedidos mantida sempre ordenada por código,
/// o que permite usar busca binária para localizar um pedido.
/// </summary>
public class ListaPedidos
{
    private readonly List<Pedido> _pedidos = new();
    private int _proximaSequencia = 1;

    public IReadOnlyList<Pedido> Pedidos => _pedidos;

    public bool Adicionar(Pedido pedido)
    {
        if (Existe(pedido.Codigo))
        {
            return false;
        }

        pedido.Sequencia = _proximaSequencia++;

        int posicao = _pedidos.FindIndex(p => p.Codigo > pedido.Codigo);

        if (posicao < 0)
        {
            _pedidos.Add(pedido);
        }
        else
        {
            _pedidos.Insert(posicao, pedido);
        }

        return true;
    }

    public Pedido? Buscar(int codigo) => Busca.BinariaPorCodigo(_pedidos, codigo);

    public bool Existe(int codigo) => Buscar(codigo) is not null;
}
