using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppNetConfiguration
{
    public class FileLogger : IDisposable
    {
        public string TimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
        protected string FileExtension = ".log";
        protected string FilePath { get; set; } = string.Empty;
        protected string FilePrefix { get; set; } = "appnetconfig";

        private FileStream stream = null;
        private BufferedStream buffer = null;
        private string CurrentFile = string.Empty;

        private object locker = new object();
        private object locker2 = new object();

        private ConcurrentQueue<string> LogItems = new ConcurrentQueue<string>();
        private Task TaskWriter;

        public FileLogger()
        {
            BuildStream();
        }
        public FileLogger(string path, string file_prefix)
        {
            FilePath = path;
            FilePrefix = file_prefix;
            BuildStream();
        }

        #region file
        protected void BuildStream()
        {
            CurrentFile = GetFileName();
            stream = new FileStream(CurrentFile, FileMode.Append);
            buffer = new BufferedStream(stream);
        }
        protected virtual string GetFileName()
        {
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);
            StringBuilder sb = new StringBuilder();
            sb.Append(FilePath);
            sb.Append("\\");
            sb.Append(FilePrefix);
            sb.Append("_");
            sb.Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
            sb.Append(FileExtension);
            return sb.ToString();
        }
        private void WriteToFile(string value)
        {
            lock (locker)
            {
                byte[] tmp = Encoding.Default.GetBytes(value + Environment.NewLine);
                buffer?.Write(tmp, 0, tmp.Length);
            }
        }
        #endregion

        #region async write
        private void StartWritingTask()
        {
            lock (locker2)
            {
                if (TaskWriter == null && buffer != null && buffer.CanWrite)
                {
                    TaskWriter = Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(100);
                        DataProcessing();
                        TaskWriter = null;
                    });
                }
            }
        }

        private void DataProcessing()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    if (LogItems.TryDequeue(out var log))
                        WriteToFile(log);
                    else
                        break;
                }
                buffer.Flush();
                if (LogItems.Count > 0) StartWritingTask();
            }
            catch { }
        }

        private void DataProcessingFlush()
        {
            try
            {
                while (LogItems.Count > 0)
                {
                    if (LogItems.TryDequeue(out var log))
                        WriteToFile(log);
                    else
                        break;
                }
                buffer?.Flush();
            }
            catch { }
        }
        #endregion

        #region write voids 
        public void Write(string message) => Write(string.Empty, message);

        public void Write(string tag, string message)
        {
            Task.Run(() =>
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(DateTime.Now.ToString(TimeFormat));
                builder.Append("\t");
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    builder.Append(tag);
                    builder.Append(":\t");
                }
                builder.Append(message);
                LogItems.Enqueue(builder.ToString());
                StartWritingTask();
            });
        }

        public void Dispose()
        {            
            lock (locker2)
            {
                DataProcessingFlush();
                buffer?.Flush();
                stream?.Flush();
                buffer?.Close();
                stream?.Close();
                buffer?.Dispose();
                stream?.Dispose();
                buffer = null;
                stream = null;
            }
        }

        #endregion
    }
}
