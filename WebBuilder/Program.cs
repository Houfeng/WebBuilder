using System;
using WebBuilder.Utils;


namespace WebBuilder
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length < 0)
            {
                Console.WriteLine("²ÎÊý´íÎó");
                return;
            }
            Parameter parameter = Parameter.Create(args[0]);
            Builder compressor = new Builder(parameter);
            compressor.Excute();
        }
    }
}
