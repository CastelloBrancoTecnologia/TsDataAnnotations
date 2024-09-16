using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsModelGenerator;
public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Nao foram fornecidos os parametros AssemblyPathName e/ou TypeScriptOutputpath");

            return -1;
        }





        Console.WriteLine($"Assembly processado => {args[0]}");
        Console.WriteLine($"Output Path => {args[1]}");

        Console.WriteLine("Iniciando Geração de Modelos");

        if (!TsModelGenerator.GenerateModels(args[0], args[1]))
        {
            Console.WriteLine("Erro ao gerar Modelos");

            return -1;
        }
        else
        {
            Console.WriteLine("Geração de modelos concluida com sucesso");
        }

        return 0;
    }
}
