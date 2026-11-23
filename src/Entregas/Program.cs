using System.Text;
using Entregas.Interface;
using Entregas.Servicos;

Console.OutputEncoding = new UTF8Encoding(false);
Console.InputEncoding = new UTF8Encoding(false);

var lista = new ListaPedidos();
var fila = new FilaEntregas();
var servico = new ServicoEntregas(lista, fila);

var menu = new MenuPrincipal(
    new TelaPedidos(lista, servico),
    new TelaConsultas(lista),
    new TelaEntregas(servico, fila));

try
{
    menu.Executar();
}
catch (EndOfStreamException)
{
    Console.WriteLine("\nEntrada encerrada. Sistema finalizado!");
}
