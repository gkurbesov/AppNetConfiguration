using AppNetConfiguration.Providers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace AppNetConfiguration.JsonProvider
{
    public class JsonConfigProvider<TAppConfig> : BaseConfigProvider<TAppConfig> where TAppConfig : AppNetConfig, new()
    {
        public JsonConfigProvider() => _extension = "json";
        public override T Read<T>()
        {
            if (File.Exists(GetFilePath()))
            {
                try
                {
                    using (TextReader reader = new StreamReader(GetFilePath()))
                    {
                        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(),
                            new Newtonsoft.Json.Converters.StringEnumConverter());
                    }
                }
                catch (Exception ex)
                {
                    Log("T Read<T> ERROR", ex.Message);
                    Log("T Read<T> TRACE", ex.StackTrace);
                    return null;
                }
            }
            else
            {
                Log("T Read<T>", "File not exist: " + GetFilePath());
                return null;
            }
        }
        public override bool Read(AppNetConfig config)
        {
            if (File.Exists(GetFilePath()))
            {
                try
                {
                    using (TextReader reader = new StreamReader(GetFilePath()))
                    {
                        TAppConfig new_config = JsonConvert.DeserializeObject<TAppConfig>(reader.ReadToEnd(),
                            new Newtonsoft.Json.Converters.StringEnumConverter());
                        Utils.CopyObjectData(new_config, config);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Log("bool Read(obj) ERROR", ex.Message);
                    Log("bool Read(obj) TRACE", ex.StackTrace);
                    return Write(config);
                }
            }
            else
            {
                Log("bool Read(obj)", "File not exist: " + GetFilePath());
                return Write(config);
            }
        }
        public override T Read<T>(string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw,
                            new Newtonsoft.Json.Converters.StringEnumConverter());
        }
        public override bool Read(string raw, AppNetConfig config)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(raw)))
                {
                    TAppConfig new_config = JsonConvert.DeserializeObject<TAppConfig>(raw,
                            new Newtonsoft.Json.Converters.StringEnumConverter());
                    Utils.CopyObjectData(new_config, config);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log("bool Read(string, obj) ERROR", ex.Message);
                Log("bool Read(string, obj) TRACE", ex.StackTrace);
                return false;
            }
        }
        public override bool Write(AppNetConfig config)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(GetFilePath()))
                {
                    writer.Write(JsonConvert.SerializeObject(config, Formatting.Indented,
                        new Newtonsoft.Json.Converters.StringEnumConverter()));
                }
                return true;
            }
            catch (Exception ex)
            {
                Log("bool Write(obj) ERROR", ex.Message);
                Log("bool Write(obj) TRACE", ex.StackTrace);
                return false;
            }
        }
        public override string WriteAsString(AppNetConfig config)
        {
            return JsonConvert.SerializeObject(config, Formatting.Indented,
                        new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}
