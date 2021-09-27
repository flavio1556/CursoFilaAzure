using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CursoFilaDio
{
    public class ConsumindoFila : BackgroundService
    {
        private readonly QueueClient _client;
        public ConsumindoFila()
        {
            _client = new QueueClient("Endpoint=sb://cursodiofila.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XdbQVovmcVXVNkzi3Mdcq68EWdNqivSJhe7PjQFLcKo=",
                "fila1", ReceiveMode.PeekLock);
            Console.WriteLine("Inicando a leitura da fila do ServiceBus");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await Task.Run(() =>
                {
                    _client.RegisterMessageHandler(ProcessarMensagem,
                        new MessageHandlerOptions(ProcessarErro)
                        {
                            MaxConcurrentCalls = 5,
                            AutoComplete = false
                        }
                     );
                });
            } while (!stoppingToken.IsCancellationRequested);
           
        }


        private async Task ProcessarMensagem(Message message, CancellationToken token)
        {
            var corpo = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("[Nova Mensagem recebida na fila]" + corpo);
            //await _client.DeadLetterAsync(message.SystemProperties.LockToken, "Motivo", "Descricao");
            await _client.CompleteAsync(message.SystemProperties.LockToken);
        }
        private Task ProcessarErro(ExceptionReceivedEventArgs e)
        {
            Console.WriteLine("[Erro]" +
                e.Exception.GetType().FullName + " " +
                e.Exception.Message
                );
           
            return Task.CompletedTask;
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _client.CloseAsync();

            Console.WriteLine("Finalizado conexão com o azure Service");
        }
    }
}
