using AppNetConfiguration.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AppNetConfiguration
{
    /// <summary>
    /// Base abstract class of settings container
    /// </summary>
    public abstract class AppNetConfig : SaveScheduler
    {
        /// <summary>
        /// Internal flag that checks to see if Initialize was called
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        protected bool _initialized = false;
        [XmlIgnore]
        [NonSerialized]
        private object init_locker = new object();
        /// <summary>
        /// An instance of a IConfigProvider that needs to be passed in via constructor or set explicitly to read and write from the configuration store.
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        private IConfigProvider _provider;
        public AppNetConfig() { }
        public AppNetConfig(IConfigProvider provider) => Initialize(provider);
        /// <summary>
        /// Enable or disable the configuration log
        /// </summary>
        /// <param name="value">true - enable write log</param>
        /// <returns></returns>
        protected void SetLoggerEnable(bool value, string path = null, string file_prefix = null) => _provider?.SetLoggerEnable(value, path, file_prefix);
        /// <summary>
        /// Save config with default delay
        /// </summary>
        public virtual void WriteAwait()
        {
            Initialize();
            ExecuteSave(() => _provider.Write(this));
        }
        /// <summary>
        /// Save config with delay
        /// </summary>
        /// <param name="interval">millisecond delay</param>
        public virtual void WriteAwait(int interval)
        {
            Initialize();
            ExecuteSave(() => _provider.Write(this), interval);
        }
        /// <summary>
        /// Save config
        /// </summary>
        /// <returns></returns>
        public virtual bool Write()
        {
            Initialize();
            return _provider.Write(this);
        }
        /// <summary>
        /// Save config as string
        /// </summary>
        /// <returns></returns>
        public virtual string WriteAsString()
        {
            Initialize();
            return _provider.WriteAsString(this);
        }
        /// <summary>
        /// Read config from storage
        /// </summary>
        /// <returns></returns>
        public virtual bool Read()
        {
            Initialize();
            return _provider.Read(this);
        }
        /// <summary>
        /// Read config from string
        /// </summary>
        /// <param name="raw">string of config</param>
        /// <returns></returns>
        public virtual bool Read(string raw)
        {
            Initialize();
            return _provider.Read(raw, this);
        }
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
        public void Initialize(IConfigProvider provider = null)
        {
            lock (init_locker)
            {
                // Initialization occurs only once
                if (_initialized) return;
                OnInitialize();
                _initialized = true;
            }
        }
        /// <summary>
        /// Override this method to handle custom initialization tasks.
        /// </summary>
        /// <param name="provider"></param>
        protected virtual void OnInitialize(IConfigProvider provider = null)
        {
            if (provider == null)
            {
                provider = OnCreateDefaultProvider();
                if (provider == null)
                    throw new NullReferenceException("You must pass an instance of IConfigProvider to initialize or override the OnCreateDefaultProvider method");
            }

            _provider = provider;
            if (!_provider.Read(this))
                _provider.Log ("Unable to read settings during initialization");
        }
        /// <summary>
        ///  Override this method to creat IConfigProvider
        /// </summary>
        /// <returns></returns>
        protected virtual IConfigProvider OnCreateDefaultProvider() => null;
    }
}
