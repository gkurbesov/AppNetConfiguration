using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AppNetConfiguration.Providers
{
    public class XmlConfigProvider<TAppConfig> : BaseConfigProvider<TAppConfig> where TAppConfig : AppNetConfig, new()
    {
        public XmlConfigProvider() => _extension = "xml";
        public override T Read<T>()
        {
            if (File.Exists(GetFilePath()))
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    using (TextReader reader = new StreamReader(GetFilePath()))
                    {
                        var result = ser.Deserialize(reader) as T;
                        return result;
                    }
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public override bool Read(AppNetConfig config)
        {
            if (File.Exists(GetFilePath()))
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(TAppConfig));
                    using (TextReader reader = new StreamReader(GetFilePath()))
                    {
                        if (ser.Deserialize(reader) is TAppConfig new_config)
                        {
                            Utils.CopyObjectData(new_config, config);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch
                {
                    return Write(config);
                }
            }
            else
            {
                return Write(config);
            }
        }
        public override T Read<T>(string raw)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(raw)))
            {
                var result = ser.Deserialize(ms) as T;
                return result;
            }
        }
        public override bool Read(string raw, AppNetConfig config)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(TAppConfig));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(raw)))
                {
                    if (ser.Deserialize(ms) is TAppConfig new_config)
                    {
                        Utils.CopyObjectData(new_config, config);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public override bool Write(AppNetConfig config)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TAppConfig));
                using (TextWriter writer = new StreamWriter(GetFilePath()))
                {
                    serializer.Serialize(writer, config);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override string WriteAsString(AppNetConfig config)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TAppConfig));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, config);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
