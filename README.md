# AppNetConfiguration

A container of settings or configurations for .Net application

## Description

The library allows you to create a settings repository that is stored as an XML/JSON file.
This library provides:
- Read/Write to string
- Read/Write to file
- Creating task  a save to a file (it is not called many times and does not block the file)
- Copy configuration objects
- Save secure string (*using AppNetConfiguration.SecureString class*)

## Example

```C#
using System;
using System.IO;
using AppNetConfiguration;
using AppNetConfiguration.Providers;

namespace ExampleConfig
{
    class AppXmlConfig : AppNetConfig
    {
        public string Login { get; set; }
        public SecureString Password { get; set; }

        public AppXmlConfig() : base(new XmlConfigProvider<AppXmlConfig>()
            .SetPath(Directory.GetCurrentDirectory())
            .SetFileName("appconfig")) { }

        public override string ToString() => 
		$"login: {Login}, password: {Password.ToString()}";        
    }
    class Program
    {
        static void Main(string[] args)
        {
            var config = new AppXmlConfig();
            config.Login = "admin";
            config.Password = "123456";
            config.Write();
            Console.WriteLine(config.ToString());
            Console.ReadLine();
        }
    }
}
```
