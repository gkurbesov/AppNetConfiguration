using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppNetConfiguration.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = MyXmlConfig.Instance();

            Console.WriteLine("First read setting: ");
            Console.WriteLine(config.ToString() + Environment.NewLine);

            Console.ReadLine();

            config.Login = "admin";
            config.Password = "123456";
            config.Age = 18;
            config.Write();

            Console.WriteLine("Momental write: ");
            Console.WriteLine(config.ToString() + Environment.NewLine);
            Console.ReadLine();

            config.Login = "User";
            config.Age = 100;
            config.WriteAwait(1000);

            var some_config = new MyXmlConfig();
            some_config.Read();
            Console.WriteLine("Reading during pre-write config: ");
            Console.WriteLine(some_config.ToString() + Environment.NewLine);

            Console.WriteLine("Wait 1 sec and read again");

            Console.ReadLine();

            Console.WriteLine("Read again...");
            some_config.Read();

            Console.WriteLine("Read file after waiting: ");
            Console.WriteLine(some_config.ToString() + Environment.NewLine);


            Console.WriteLine();

            Console.ReadLine();
            Console.WriteLine("Copy XML to JSON class instance");

            var json_config = MyJsonConfig.Instance();
            json_config.CopyFrom(some_config);
            Console.WriteLine("JSON config before copy: ");
            Console.WriteLine(json_config.ToString() + Environment.NewLine);


            Console.ReadLine();
            Console.WriteLine("Write json config to file");
            json_config.Write();
            Console.WriteLine("Press any key to exit...");

            Console.ReadLine();
        }
    }
}
