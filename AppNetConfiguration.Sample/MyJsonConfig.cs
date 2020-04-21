using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNetConfiguration.Sample
{
    public class MyJsonConfig : AppNetConfig
    {
        private bool is_init = false;
        private static MyJsonConfig instance = null;

        public string Login { get; set; } = string.Empty;
        public SecureString Password { get; set; } = string.Empty;
        public int Age { get; set; } = 0;

        public MyJsonConfig()
            : base(new JsonProvider.JsonConfigProvider<MyJsonConfig>()
                  .SetPath("D:\\").SetFileName("SampleConfig_2"))
        { }

        public static MyJsonConfig Instance()
        {
            if (instance == null) instance = new MyJsonConfig();
            if (!instance.is_init) instance.Read();
            return instance;
        }

        public override string ToString()
        {
            return $"Login: {Login}, pass: {Password}, age = {Age}";
        }
    }
}
