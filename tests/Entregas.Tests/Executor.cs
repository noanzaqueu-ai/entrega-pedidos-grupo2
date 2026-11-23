namespace Entregas.Tests;

public static class Executor
{
    private static int _aprovados;
    private static int _falhas;

    public static void Teste(string nome, Action acao)
    {
        try
        {
            acao();
            _aprovados++;
            Console.WriteLine($"[OK]     {nome}");
        }
        catch (Exception erro)
        {
            _falhas++;
            Console.WriteLine($"[FALHOU] {nome}: {erro.Message}");
        }
    }

    public static void Igual<T>(T esperado, T atual)
    {
        if (!EqualityComparer<T>.Default.Equals(esperado, atual))
        {
            throw new Exception($"esperado <{esperado}>, obtido <{atual}>");
        }
    }

    public static void Sequencia<T>(IEnumerable<T> esperado, IEnumerable<T> atual)
    {
        string a = string.Join(", ", esperado);
        string b = string.Join(", ", atual);

        if (a != b)
        {
            throw new Exception($"esperado [{a}], obtido [{b}]");
        }
    }

    public static void Verdadeiro(bool condicao, string mensagem = "condição falsa")
    {
        if (!condicao)
        {
            throw new Exception(mensagem);
        }
    }

    public static int Resumo()
    {
        Console.WriteLine($"\n{_aprovados} aprovado(s), {_falhas} falha(s).");
        return _falhas == 0 ? 0 : 1;
    }
}
