using AppNetConfiguration.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNetConfiguration.Sample
{
    public class MyJsonConfig : AppNetConfig
    {
        private static object locker = new object();
        private static MyJsonConfig instance = null;

        public string Login { get; set; } = string.Empty;
        public SecureString Password { get; set; } = string.Empty;
        public int Age { get; set; } = 0;

        public MyJsonConfig() { }

        public static MyJsonConfig Instance()
        {
            lock (locker)
            {
                if (instance == null) instance = new MyJsonConfig();
                if (!instance.initialized) instance.Initialize();
                return instance;
            }
        }

        protected override IConfigProvider OnCreateDefaultProvider()
        {
            return new JsonConfigProvider<MyJsonConfig>()
                        .SetPath("D:\\")
                        .SetFileName("SampleConfig_2");
        }

        public override string ToString()
        {
            return $"Login: {Login}, pass: {Password}, age = {Age}";
        }
    }
}
