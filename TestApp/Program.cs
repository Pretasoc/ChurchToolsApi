using ChurchToolsApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            StringBuilder password = new StringBuilder();
            string email;
            Console.WriteLine("email:");
            email = Console.ReadLine();
            Console.WriteLine("password:");
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                password.Append(key.KeyChar);
            }


            using (var session = new ChurchToolsSession(new Uri("https://roderbruch.church.tools/"), "api test app"))
            {
                var (id, token) = await session.GetUserLoginToken(email, password.ToString());
                await session.LoginAsync(id, token);
                var homeModule = new HomeModule(session);

                var settings = await homeModule.GetSettingsAsync();
                var tables = await homeModule.GetMasterdataTablenames();
            }
        }
    }
}
