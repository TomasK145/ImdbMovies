using System;
using System.IO;

namespace ImdbMoviesConsoleApp
{
    public sealed class Logger
    {
        private static string LogFullPath { get; set; }
        private static Logger _instance = null;
        public static Logger Instance
        {
            get
            {
                // Check for valid instance
                if (_instance == null) _instance = new Logger();

                // Return instance
                return _instance;
            }
        }

        private Logger()
        {
            LogFullPath = ConfigReader.GetConfigValue("logPath") + "MoviesImdbLog_" + Guid.NewGuid().ToString() + ".txt";
        }

        private static System.Object lockThis = new System.Object();       

        public void WriteLog(string message)
        {
            lock (lockThis)
            {
                using (StreamWriter file = new StreamWriter(LogFullPath, true))
                {
                    file.WriteLine($"{DateTime.Now.ToString()} - {message}");
                }
            }
        }
    }
}
