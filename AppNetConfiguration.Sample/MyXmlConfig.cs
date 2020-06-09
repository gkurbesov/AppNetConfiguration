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
        private static object locker = new object();
        private static MyXmlConfig instance = null;

        public string Login { get; set; } = string.Empty;
        public SecureString Password { get; set; } = string.Empty;
        public int Age { get; set; } = 0;

        public MyXmlConfig() { }

        public static MyXmlConfig Instance()
        {
            lock (locker)
            {
                if (instance == null) instance = new MyXmlConfig();
                if (!instance._initialized)instance.Initialize();
                return instance;
            }
        }

        protected override IConfigProvider OnCreateDefaultProvider()
        {
            return new XmlConfigProvider<MyXmlConfig>()
                        .SetPath("D:\\")
                        .SetFileName("SampleConfig");
        }

        public override string ToString()
        {
            return $"Login: {Login}, pass: {Password}, age = {Age}";
        }
    }
}
