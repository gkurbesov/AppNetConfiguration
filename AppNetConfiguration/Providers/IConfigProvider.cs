namespace AppNetConfiguration.Providers
{
    /// <summary>
    /// Interface for working with the configuration storage provider
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Change the configuration file storage folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IConfigProvider SetPath(string path);
        /// <summary>
        /// Change configuration file name
        /// </summary>
        /// <param name="file">file name without extension</param>
        /// <returns></returns>
        IConfigProvider SetFileName(string file);       
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Read<T>() where T : AppNetConfig, new();
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <param name="config">An instance of the class in which the settings will be read</param>
        /// <returns></returns>
        bool Read(AppNetConfig config);
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="raw"></param>
        /// <returns></returns>
        T Read<T>(string raw) where T : AppNetConfig, new();
        /// <summary>
        /// Read configuration
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="config">An instance of the class in which the settings will be read</param>
        /// <returns></returns>
        bool Read(string raw, AppNetConfig config);
        /// <summary>
        /// Write configuration to storage
        /// </summary>
        /// <param name="config">Instance of a class with settings to be saved</param>
        /// <returns></returns>
        bool Write(AppNetConfig config);
        /// <summary>
        ///  Write configuration to string
        /// </summary>
        /// <param name="config">Instance of a class with settings to be saved</param>
        /// <returns></returns>
        string WriteAsString(AppNetConfig config);
    }
}
