using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
namespace ConfigurationDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "key1","value1" },
                { "key2","value2" },
                { "section1:key4","value4" },
                { "section2:key5","value5" },
                { "section2:key6","value6" },
                { "section2:section3:key7","value7" }
            });




            IConfigurationRoot configurationRoot = builder.Build();


            ///IConfiguration config = configurationRoot;

            Console.WriteLine(configurationRoot["key1"]);
            Console.WriteLine(configurationRoot["key2"]);

            IConfigurationSection section = configurationRoot.GetSection("section1");
            Console.WriteLine($"key4:{section["key4"]}");
            Console.WriteLine($"key5:{section["key5"]}");


            IConfigurationSection section2 = configurationRoot.GetSection("section2");
            Console.WriteLine($"key5_v2:{section2["key5"]}");
            var section3 = section2.GetSection("section3");
            Console.WriteLine($"key7:{section3["key7"]}");



        }
    }
}
