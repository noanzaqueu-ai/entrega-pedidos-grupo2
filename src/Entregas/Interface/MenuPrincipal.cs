namespace Entregas.Interface;

public class MenuPrincipal
{
    private readonly TelaPedidos _pedidos;
    private readonly TelaConsultas _consultas;
    private readonly TelaEntregas _entregas;

    public MenuPrincipal(TelaPedidos pedidos, TelaConsultas consultas, TelaEntregas entregas)
    {
        _pedidos = pedidos;
        _consultas = consultas;
        _entregas = entregas;
    }

    public void Executar()
    {
        int opcao;

        do
        {
            Apresentacao.ExibirMenu(
                "SISTEMA DE PEDIDOS",
                "1 - Cadastrar pedido",
                "2 - Listar pedidos",
                "3 - Buscar pedido",
                "4 - Ordenar pedidos",
                "5 - Selecionar pedidos",
                "6 - Mostrar fila de entregas",
                "7 - Organizar fila por prioridade",
                "8 - Realizar entrega",
                "9 - Carregar pedidos de exemplo",
                "0 - Sair");

            opcao = Entrada.LerInteiro("Escolha: ", 0, 9);
            Executar(opcao);
        }
        while (opcao != 0);
    }

    private void Executar(int opcao)
    {
        switch (opcao)
        {
            case 1:
                _pedidos.Cadastrar();
                break;
            case 2:
                _pedidos.Listar();
                break;
            case 3:
                _consultas.Buscar();
                break;
            case 4:
                _consultas.Ordenar();
                break;
            case 5:
                _consultas.Selecionar();
                break;
            case 6:
                _entregas.MostrarFila();
                break;
            case 7:
                _entregas.OrganizarPorPrioridade();
                break;
            case 8:
                _entregas.RealizarEntrega();
                break;
            case 9:
                _pedidos.CarregarExemplos();
                break;
            case 0:
                Console.WriteLine("Sistema encerrado!");
                break;
        }
    }
}
