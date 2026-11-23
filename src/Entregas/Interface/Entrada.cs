namespace Entregas.Interface;

public static class Entrada
{
    public static string LerLinha(string mensagem)
    {
        Console.Write(mensagem);
        return Console.ReadLine() ?? throw new EndOfStreamException();
    }

    public static string LerTexto(string mensagem)
    {
        while (true)
        {
            string texto = LerLinha(mensagem).Trim();

            if (texto.Length > 0)
            {
                return texto;
            }

            Console.WriteLine("O campo não pode ficar vazio!");
        }
    }

    public static int LerInteiro(string mensagem, int minimo = int.MinValue, int maximo = int.MaxValue)
    {
        while (true)
        {
            if (int.TryParse(LerLinha(mensagem), out int valor) && valor >= minimo && valor <= maximo)
            {
                return valor;
            }

            Console.WriteLine("Valor inválido! Tente novamente.");
        }
    }
}
