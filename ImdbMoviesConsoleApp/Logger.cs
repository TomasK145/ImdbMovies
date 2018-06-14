using System;
using System.IO;

namespace ImdbMoviesConsoleApp
{
    public sealed class Logger
    {
        private const string LogPath = @"C:\Users\Public\MoviesImdbLog.txt";
        private static readonly Lazy<Logger> lazy = new Lazy<Logger>(() => new Logger());
        public static Logger Instance { get { return lazy.Value; } }       
        private Logger()
        {
        }

        public static void WriteLog(string message)
        {
            using (StreamWriter file = new StreamWriter(LogPath, true))
            {
                file.WriteLine($"{DateTime.Now.ToString()} - {message}");
            }
        }

        internal static void WriteLog(string v, int statusCode, string reasonPhrase)
        {
            throw new NotImplementedException();
        }
    }
}
