using System;
using System.Configuration;

namespace ImdbMoviesConsoleApp
{
    public static class ConfigReader
    {
        public static string GetConfigValue(string key)
        {
            return Convert.ToString(ConfigurationManager.AppSettings[key]);
        }
    }
}
