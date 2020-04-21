using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppNetConfiguration.Providers
{
    /// <summary>
    /// Base configuration provider
    /// </summary>
    /// <typeparam name="TAppConfig"></typeparam>
    public abstract class BaseConfigProvider<TAppConfig> : IConfigProvider where TAppConfig : AppNetConfig, new()
    {
        /// <summary>
        /// path to configuration store file
        /// </summary>
        protected string _path = AppDomain.CurrentDomain.BaseDirectory.Trim('\\');
        /// <summary>
        /// configuration store file name
        /// </summary>
        protected string _file = "setting";
        /// <summary>
        /// configuration store file extension
        /// </summary>
        protected string _extension = "config";
        /// <summary>
        /// Get the full path to the configuration repository file (with file name and extension)
        /// </summary>
        /// <returns></returns>
        protected string GetFilePath() => $"{_path}\\{_file}.{_extension}";
        /// <summary>
        /// Change configuration file name
        /// </summary>
        /// <param name="file">file name without extension</param>
        /// <returns></returns>
        public IConfigProvider SetFileName(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("Parameter cannot be null or wite space", "file");
            _file = file;
            return this;
        }
        /// <summary>
        /// Change the configuration file storage folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IConfigProvider SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                throw new ArgumentException("Parameter cannot be null and the folder must exist", "path");
            _path = path.Trim('\\');
            return this;
        }
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T Read<T>() where T : AppNetConfig, new();
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <param name="config">An instance of the class in which the settings will be read</param>
        /// <returns></returns>
        public abstract bool Read(AppNetConfig config);
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="raw"></param>
        /// <returns></returns>
        public abstract T Read<T>(string raw) where T : AppNetConfig, new();
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="config">An instance of the class in which the settings will be read</param>
        /// <returns></returns>
        public abstract bool Read(string raw, AppNetConfig config);
        /// <summary>
        /// Write configuration to storage
        /// </summary>
        /// <param name="config">Instance of a class with settings to be saved</param>
        /// <returns></returns>
        public abstract bool Write(AppNetConfig config);
        /// <summary>
        ///  Write configuration to string
        /// </summary>
        /// <param name="config">Instance of a class with settings to be saved</param>
        /// <returns></returns>
        public abstract string WriteAsString(AppNetConfig config);
    }
}
