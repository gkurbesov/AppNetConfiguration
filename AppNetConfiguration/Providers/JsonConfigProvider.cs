using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppNetConfiguration.Providers
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
                    semaphore.Wait();
                    using (TextReader reader = new StreamReader(GetFilePath()))
                    {
                        var tmp = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            return JsonConvert.DeserializeObject<T>(tmp,
                                new Newtonsoft.Json.Converters.StringEnumConverter());
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    semaphore.Release();
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
                    semaphore.Wait();
                    using (TextReader reader = new StreamReader(GetFilePath()))
                    {
                        var tmp = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            TAppConfig new_config = JsonConvert.DeserializeObject<TAppConfig>(tmp,
                             new Newtonsoft.Json.Converters.StringEnumConverter());
                            Utils.CopyObjectData(new_config, config);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    semaphore.Release();
                    return Write(config);
                }
                finally
                {
                    if (semaphore.CurrentCount == 0)
                        semaphore.Release();
                }
            }
            else
            {
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
                return false;
            }
        }
        public override bool Write(AppNetConfig config)
        {
            if (config != null)
            {
                try
                {
                    semaphore.Wait();
                    using (TextWriter writer = new StreamWriter(GetFilePath()))
                    {
                        writer.Write(JsonConvert.SerializeObject(config, Formatting.Indented,
                            new Newtonsoft.Json.Converters.StringEnumConverter()));
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    semaphore.Release();
                }
            }
            else
            {
                return false;
            }
        }
        public override string WriteAsString(AppNetConfig config)
        {
            if (config != null)
            {
                return JsonConvert.SerializeObject(config, Formatting.Indented,
                        new Newtonsoft.Json.Converters.StringEnumConverter());
            }
            else
            {
                return null;
            }
        }
    }
}
