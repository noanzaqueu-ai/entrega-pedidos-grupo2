using Entregas.Models;

namespace Entregas.Algoritmos;

public enum CriterioOrdenacao
{
    Prioridade,
    Codigo,
    Cliente,
    Endereco
}

public static class Comparadores
{
    /// <summary>
    /// Cada comparador desempata pela ordem de cadastro (Sequencia), formando
    /// uma ordem total: qualquer algoritmo produz exatamente o mesmo resultado.
    /// </summary>
    public static Comparison<Pedido> Para(CriterioOrdenacao criterio) => criterio switch
    {
        CriterioOrdenacao.Prioridade => (a, b) =>
            Desempatar(a.Prioridade.CompareTo(b.Prioridade), a, b),
        CriterioOrdenacao.Codigo => (a, b) =>
            Desempatar(a.Codigo.CompareTo(b.Codigo), a, b),
        CriterioOrdenacao.Cliente => (a, b) =>
            Desempatar(CompararTexto(a.Cliente, b.Cliente), a, b),
        CriterioOrdenacao.Endereco => (a, b) =>
            Desempatar(CompararTexto(a.Endereco, b.Endereco), a, b),
        _ => throw new ArgumentOutOfRangeException(nameof(criterio))
    };

    private static int Desempatar(int resultado, Pedido a, Pedido b) =>
        resultado != 0 ? resultado : a.Sequencia.CompareTo(b.Sequencia);

    private static int CompararTexto(string a, string b) =>
        string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
}
