using System;
using System.IO;
using System.Text;

namespace ImdbMoviesConsoleApp
{
    public class MovieExporter
    {
        private string CsvHeader { get; set; }
        private string CsvFileLocation
        {
            get
            {
                return CsvExportPath + "MoviesImdb_" + Guid.NewGuid().ToString() + ".csv";
            }
        }
        private string CsvExportPath { get; set; }

        public MovieExporter()
        {
            CsvHeader = ConfigReader.GetConfigValue("csvHeader");
            CsvExportPath = ConfigReader.GetConfigValue("csvExportPath");
        }

        public void ExportMoviesToCsv(string resultText, int currentBatch)
        {
            string csvFileLocation = CsvExportPath + "MoviesImdb_" + currentBatch.ToString() + ".csv";
            StringBuilder sb = new StringBuilder();        
            sb.AppendLine(CsvHeader);
            sb.AppendLine(resultText);
            using (StreamWriter file = new StreamWriter(csvFileLocation, true))
            {
                file.WriteLine(sb.ToString());
            }
        }
    }
}
