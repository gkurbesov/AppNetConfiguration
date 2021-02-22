using AppNetConfiguration.Providers;
using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppNetConfiguration
{
    /// <summary>
    /// Base abstract class of settings container
    /// </summary>
    public abstract class AppNetConfig
    {
        [XmlIgnore]
        [NonSerialized]
        private Task _taskSaveScheduler = null;
        [XmlIgnore]
        [NonSerialized]
        private int waitSaveInterval = 100;
        /// Internal flag that checks to see if Initialize was called
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        protected volatile bool initialized = false;
        [XmlIgnore]
        [NonSerialized]
        private object locker = new object();
        /// <summary>
        /// An instance of a IConfigProvider that needs to be passed in via constructor or set explicitly to read and write from the configuration store.
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        private IConfigProvider _provider;
        public AppNetConfig() { }
        public AppNetConfig(IConfigProvider provider) => Initialize(provider);
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
        /// Set wait interval
        /// </summary>
        /// <param name="ms"></param>
        public void SetWaitInterval(int ms) => waitSaveInterval = ms > 0 ? ms : 50;
        /// <summary>
        /// save with a delay
        /// </summary>
        /// <param name="action"></param>
        protected void ExecuteSave(Action action) => ExecuteSave(action, waitSaveInterval);
        /// <summary>
        /// save with a delay
        /// </summary>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        protected void ExecuteSave(Action action, int interval)
        {
            lock (locker)
            {
                if (_taskSaveScheduler == null)
                {
                    _taskSaveScheduler = Task.Factory.StartNew(async stateObject =>
                    {
                        await Task.Delay(waitSaveInterval > 0 ? waitSaveInterval : 100);
                        ((Action)stateObject).Invoke();
                    }, action).ContinueWith(x => _taskSaveScheduler = null);
                    _taskSaveScheduler?.ConfigureAwait(false);
                }
            }
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
            lock (locker)
            {
                // Initialization occurs only once
                if (initialized) return;
                OnInitialize(provider);
                initialized = true;
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
        }
        /// <summary>
        ///  Override this method to creat IConfigProvider
        /// </summary>
        /// <returns></returns>
        protected virtual IConfigProvider OnCreateDefaultProvider() => null;
    }
}
