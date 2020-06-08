using AppNetConfiguration.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppNetConfiguration
{
    /// <summary>
    /// Base abstract class of settings container
    /// </summary>
    public abstract class AppNetConfig : SaveScheduler
    {
        private IConfigProvider _provider;
        public AppNetConfig() { }
        public AppNetConfig(IConfigProvider provider) => _provider = provider;
        /// <summary>
        /// Enable or disable the configuration log
        /// </summary>
        /// <param name="value">true - enable write log</param>
        /// <returns></returns>
        protected void SetLoggerEnable(bool value, string path = null, string file_prefix = null) => _provider?.SetLoggerEnable(value, path, file_prefix);
        /// <summary>
        /// Save config with default delay
        /// </summary>
        public virtual void WriteAwait() => ExecuteSave(() => _provider.Write(this));
        /// <summary>
        /// Save config with delay
        /// </summary>
        /// <param name="interval">millisecond delay</param>
        public virtual void WriteAwait(int interval) => ExecuteSave(() => _provider.Write(this), interval);
        /// <summary>
        /// Save config
        /// </summary>
        /// <returns></returns>
        public virtual bool Write() => _provider.Write(this);
        /// <summary>
        /// Save config as string
        /// </summary>
        /// <returns></returns>
        public virtual string WriteAsString() => _provider.WriteAsString(this);
        /// <summary>
        /// Read config from storage
        /// </summary>
        /// <returns></returns>
        public virtual bool Read() => _provider.Read(this);
        /// <summary>
        /// Read config from string
        /// </summary>
        /// <param name="raw">string of config</param>
        /// <returns></returns>
        public virtual bool Read(string raw) => _provider.Read(raw, this);  
        public virtual bool CopyFrom(AppNetConfig other)
        {
            try
            {
                Utils.CopyObjectData(other, this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
