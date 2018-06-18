using System;
using System.IO;

namespace ImdbMoviesConsoleApp
{
    public sealed class Logger
    {
        private static System.Object lockThis = new System.Object();
        private static string LogPath = ConfigReader.GetConfigValue("logPath") + "MoviesImdbLog.txt";
        private static readonly Lazy<Logger> lazy = new Lazy<Logger>(() => new Logger());
        public static Logger Instance { get { return lazy.Value; } }       
        private Logger()
        {
        }

        public static void WriteLog(string message)
        {
            lock (lockThis)
            {
                using (StreamWriter file = new StreamWriter(LogPath, true))
                {
                    file.WriteLine($"{DateTime.Now.ToString()} - {message}");
                }
            }
        }

        internal static void WriteLog(string v, int statusCode, string reasonPhrase)
        {
            throw new NotImplementedException();
        }
    }
}
