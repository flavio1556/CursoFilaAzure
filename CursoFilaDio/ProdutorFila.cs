using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CursoFilaDio
{
    class ProdutorFila
    {
        private readonly QueueClient _client;
        public ProdutorFila()
        {
            _client = new QueueClient("Endpoint=sb://cursodiofila.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XdbQVovmcVXVNkzi3Mdcq68EWdNqivSJhe7PjQFLcKo=",
                "fila1", ReceiveMode.PeekLock);
            Console.WriteLine("Inicando a Criador de fila da fila do ServiceBus");
        }


        public async Task ProduzirMensagem()
        {
            try
            {
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine(
                        $"Enviado mensagem: {i}");
                    await _client.SendAsync(new Message
                        (Encoding.UTF8.GetBytes($"Numero: {i}")));
                }
                Console.WriteLine("Concluido o envio das mensagens");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            }
            finally
            {
                if(_client != null)
                {
                    await _client.CloseAsync();
                    Console.WriteLine(
                        $"Finalizando Conexão com ServicesBus");
                }
            }
        }
    }
}
