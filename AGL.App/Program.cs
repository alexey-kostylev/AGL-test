using AGL.App.Controllers;
using AGL.App.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace AGL.App
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var container = new UnityContainer();
            try
            {
                container.WireupAglDependencies();
                MainAsync(container).Wait();
            }
            catch(Exception e)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("*** {0}", e);
                Console.ForegroundColor = color;
            }
            finally
            {
                container.Dispose();
            }            
            Console.Write("Finished. Press any key to quit: ");
            Console.ReadKey();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("*** Unhandled exception: {0}", e.ExceptionObject);
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }

        static async Task MainAsync(IUnityContainer container)
        {
            var controller = container.Resolve<PeopleControler>();

            var result = await controller.GetCatsWithOwnersGender();

            Console.WriteLine(result);
        }


    }
}
