using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tcp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            int port = 8888;

            var server = new TcpServer(port);
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                server.Stop();
            };

            await server.StartAsync();
        }
    }
}
