using Entregas.Models;

namespace Entregas.Servicos;

public static class DadosExemplo
{
    public static List<Pedido> Pedidos() => new()
    {
        new Pedido(105, "Mariana Souza", "Rua das Flores, 120 - Centro", Prioridade.Baixa),
        new Pedido(101, "Carlos Almeida", "Av. Brasil, 450 - Jardim Aurora", Prioridade.Alta),
        new Pedido(108, "Fernanda Lima", "Rua Rio Branco, 77 - Centro", Prioridade.Media),
        new Pedido(103, "João Pereira", "Rua das Palmeiras, 15 - Bela Vista", Prioridade.Baixa),
        new Pedido(107, "Ana Beatriz Costa", "Av. Brasil, 980 - Industrial", Prioridade.Alta),
        new Pedido(102, "Carlos Eduardo Nunes", "Rua Sete de Setembro, 33 - Centro", Prioridade.Media),
        new Pedido(104, "Patrícia Gomes", "Travessa do Sol, 8 - Bela Vista", Prioridade.Alta),
        new Pedido(106, "Roberto Dias", "Rua das Flores, 305 - Jardim Aurora", Prioridade.Media),
        new Pedido(109, "Luciana Prado", "Rua das Palmeiras, 240 - Industrial", Prioridade.Baixa)
    };
}
