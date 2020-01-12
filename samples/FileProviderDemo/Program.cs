using System;
using Microsoft.Extensions.FileProviders;
namespace FileProviderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileProvider provider1 = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);

            //var contents = provider1.GetDirectoryContents("/");


            //foreach (var item in contents)
            //{
            //    Console.WriteLine(item.Name);
            //}

            IFileProvider provider2 = new EmbeddedFileProvider(typeof(Program).Assembly);


            var html = provider2.GetFileInfo("emb.html");

            IFileProvider provider = new CompositeFileProvider(provider1, provider2);



            var contents = provider.GetDirectoryContents("/");


            foreach (var item in contents)
            {
                Console.WriteLine(item.Name);
            }

            //Console.ReadKey();
        }
    }
}
