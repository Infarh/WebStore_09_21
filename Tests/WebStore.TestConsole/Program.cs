using System;
using System.Net.Http;
using System.Threading.Tasks;
using Clients;

namespace WebStore.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient()
            {
                //BaseAddress = new Uri("http://localhost:5001")
            };

            //var api = new WebAPIClient("http://localhost:5001", client);

            //var employee = await api.Employees4Async(2);
        }
    }
}
