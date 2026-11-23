namespace Entregas.Models;

public enum Prioridade
{
    Alta = 1,
    Media = 2,
    Baixa = 3
}

public enum StatusPedido
{
    Pendente,
    Entregue
}

public class Pedido
{
    public int Codigo { get; }
    public string Cliente { get; }
    public string Endereco { get; }
    public Prioridade Prioridade { get; }
    public StatusPedido Status { get; set; } = StatusPedido.Pendente;
    public int Sequencia { get; set; }

    public Pedido(int codigo, string cliente, string endereco, Prioridade prioridade)
    {
        Codigo = codigo;
        Cliente = cliente;
        Endereco = endereco;
        Prioridade = prioridade;
    }
}
