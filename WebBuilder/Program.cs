using System;
using System.Reflection;
using WebBuilder.Utils;


namespace WebBuilder
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var title = "WebBuilder";
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.Title = title;
            Console.WriteLine(string.Format("{0} v{1}", title, version));
            Console.WriteLine("Powered By Houfeng.net");
            //--
            if (args != null && args.Length > 0)
            {
                Excute(args[0]);
                return;
            }
            while (true)
            {
                Console.Write("Input:");
                var input = Console.ReadLine();
                if (input == "exit")
                    Environment.Exit(0);
                else if (input == "clear")
                    Console.Clear();
                else if (!string.IsNullOrEmpty(input))
                    Excute(input);
            }
        }
        private static void Excute(string args)
        {
            try
            {
                Parameter parameter = Parameter.Create(args);
                Builder compressor = new Builder(parameter);
                compressor.Excute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("error:\"{0}\".", ex.Message));
            }
        }
    }
}
