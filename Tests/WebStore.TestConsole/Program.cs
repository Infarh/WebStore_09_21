using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebStore.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HubConnectionBuilder();
            var connection = builder
               .WithUrl("http://localhost:5000/chat")
               .Build();

            using var registration = connection.On<string>("MessageFromClient", OnMessageFromClient);

            Console.WriteLine("Готов к подключению.");
            Console.ReadLine();

            await connection.StartAsync();

            Console.WriteLine("Соединение установлено.");

            while (true)
            {
                var message = Console.ReadLine();
                await connection.InvokeAsync("SendMessage", message);
            }
        }

        private static void OnMessageFromClient(string Message)
        {
            Console.WriteLine("Message from server: {0}", Message);
        }
    }
}
