using System;
using System.Reflection;
using System.Text;
using WebBuilder.Compress;
using WebBuilder.Doc;
using WebBuilder.Utils;


namespace WebBuilder
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
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
            args = args.Replace("\\", "\\\\");
            try
            {
                CmdParameter cmdParameter = CmdParameter.Create(args);
                if (string.IsNullOrEmpty(cmdParameter.inDir)) return;
                if (!string.IsNullOrEmpty(cmdParameter.docDir))
                {
                    DocBuilder docBuilder = new DocBuilder(cmdParameter);
                    docBuilder.Excute();
                }
                if (!string.IsNullOrEmpty(cmdParameter.outDir))
                {
                    CompressBuilder compressor = new CompressBuilder(cmdParameter);
                    compressor.Excute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("error:\"{0}\".", ex.Message));
            }
        }
    }
}
