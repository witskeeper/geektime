//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Primitives;
//namespace ConfigurationFileDemo
//{
//    static class Class1
//    {

//        public static void CheckChangeToken(this IConfigurationRoot configurationRoot)
//        {
//            var token1 = configurationRoot.GetReloadToken();

//            var token2 = configurationRoot.GetReloadToken();

//            Console.WriteLine($"token1==token2: {token1 == token2}");

//            token1.RegisterChangeCallback(data =>
//            {
//                var token3 = configurationRoot.GetReloadToken();
//                Console.WriteLine("配置发生了变化");
//                Console.WriteLine($"token1==token3: {token1 == token3}");

//            }, null);
//        }


//        public static void Change(this IConfigurationRoot configurationRoot)
//        {
//            ChangeToken.OnChange(() => configurationRoot.GetReloadToken(), () =>
//            {
//                Console.WriteLine("配置发生了改变，新的配置值为：");

//                Console.WriteLine($"Key1:{configurationRoot["Key1"]}");
//                Console.WriteLine($"Key2:{configurationRoot["Key2"]}");
//                Console.WriteLine($"Key3:{configurationRoot["Key3"]}");
//                Console.WriteLine($"Key4:{configurationRoot["Key4"]}");
//            });
//        }


//        public static void BindDemo(this IConfigurationRoot configurationRoot)
//        {
//            var myconfig = new MyConfig();

//            configurationRoot.Bind(myconfig, option => { option.BindNonPublicProperties = false; });

//            Console.WriteLine($"MyConfig.Key1:{myconfig.Key1}");
//            Console.WriteLine($"MyConfig.Key5:{myconfig.Key5}");
//            Console.WriteLine($"MyConfig.Key6:{myconfig.Key6}");
//        }


//        class MyConfig
//        {
//            public string Key1 { get; set; } = "default";
//            public bool Key5 { get; set; } = true;
//            public int Key6 { get; private set; } = 20;
//        }
//    }
//}
