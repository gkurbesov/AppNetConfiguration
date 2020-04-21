using AppNetConfiguration.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNetConfiguration.Sample
{
    public class MyXmlConfig : AppNetConfig
    {
        private bool is_init = false;
        private static MyXmlConfig instance = null;

        public string Login { get; set; } = string.Empty;
        public SecureString Password { get; set; } = string.Empty;
        public int Age { get; set; } = 0;

        public MyXmlConfig()
            : base(new XmlConfigProvider<MyXmlConfig>()
                  .SetPath("D:\\").SetFileName("SampleConfig"))
        { }

        public static MyXmlConfig Instance()
        {
            if (instance == null) instance = new MyXmlConfig();
            if (!instance.is_init) instance.Read();
            return instance;
        }

        public override string ToString()
        {
            return $"Login: {Login}, pass: {Password}, age = {Age}";
        }
    }
}
