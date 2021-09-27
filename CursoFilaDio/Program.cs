using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace CursoFilaDio
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Iniciado Aplicacao");

            await Inicio();
        }
        public static async Task Inicio()
        {
            Console.WriteLine("Digite 1: para produzir mensagem para fila");
            Console.WriteLine("Digite 2: para ler mensagem ");
            Console.WriteLine("Digite 3: para produzir mensagem para o topico");
           
            string resposta = Console.ReadLine();

            if (resposta == "1")
            {
                await new ProdutorFila().ProduzirMensagem();
                await Inicio();
                
            }
            else if(resposta == "2")
            {
                CreateHostBuilder().Build().Run();
            }
            else if(resposta == "3")
            {
                await new ProdutorTopico().ProduzirMensagem();
                await Inicio();
            }
           
        }
        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsumidorTopico>();
               // services.AddHostedService<ConsumindoFila>();

            });
    }
}
