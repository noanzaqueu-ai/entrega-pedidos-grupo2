using Entregas.Models;
using Entregas.Servicos;

namespace Entregas.Interface;

public class TelaPedidos
{
    private readonly ListaPedidos _lista;
    private readonly ServicoEntregas _servico;

    public TelaPedidos(ListaPedidos lista, ServicoEntregas servico)
    {
        _lista = lista;
        _servico = servico;
    }

    public void Cadastrar()
    {
        Console.WriteLine();
        int codigo = Entrada.LerInteiro("Código do pedido: ", 1);

        if (_lista.Existe(codigo))
        {
            Console.WriteLine("Esse código já existe!");
            return;
        }

        string cliente = Entrada.LerTexto("Nome do cliente: ");
        string endereco = Entrada.LerTexto("Endereço de entrega: ");
        int prioridade = Entrada.LerInteiro("Prioridade (1 - Alta, 2 - Média, 3 - Baixa): ", 1, 3);

        _servico.Cadastrar(new Pedido(codigo, cliente, endereco, (Prioridade)prioridade));
        Console.WriteLine("\nPedido cadastrado e incluído na fila de entregas!");
    }

    public void Listar()
    {
        Apresentacao.ExibirTabela("LISTA DE PEDIDOS", _lista.Pedidos);
    }

    public void CarregarExemplos()
    {
        int cadastrados = 0;

        foreach (Pedido pedido in DadosExemplo.Pedidos())
        {
            if (_servico.Cadastrar(pedido) == StatusCadastro.Cadastrado)
            {
                cadastrados++;
            }
        }

        Console.WriteLine($"\n{cadastrados} pedido(s) de exemplo carregado(s)!");
    }
}
