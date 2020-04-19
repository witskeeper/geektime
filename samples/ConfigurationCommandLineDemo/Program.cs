using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
namespace ConfigurationCommandLineDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();

            //builder.AddCommandLine(args);

            #region 命令替换
            var mapper = new Dictionary<string, string> { { "--k1", "CommandLineKey1" } };
            builder.AddCommandLine(args, mapper);
            #endregion

            var configurationRoot = builder.Build();

            Console.WriteLine($"CommandLineKey1:{configurationRoot["CommandLineKey1"]}");
            Console.WriteLine($"CommandLineKey2:{configurationRoot["CommandLineKey2"]}");
            Console.WriteLine($"CommandLineKey3:{configurationRoot["CommandLineKey3"]}");
            Console.ReadKey();
        }
    }
}
