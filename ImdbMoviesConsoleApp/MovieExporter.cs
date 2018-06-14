using System;
using System.IO;
using System.Text;

namespace ImdbMoviesConsoleApp
{
    public class MovieExporter
    {
        private const string CsvHeader = "Title;Year;Released;Runtime;Genre;Country;Poster;Metascore;imdbRating;BoxOffice;Production;Website;ImdbUrl";
        private string CsvFileLocation
        {
            get
            {
                return @"C:\Users\Public\MoviesImdb_" + Guid.NewGuid().ToString() + ".csv";
            }
        }

        public void ExportMoviesToCsv(string resultText)
        {
            StringBuilder sb = new StringBuilder();        
            sb.AppendLine(CsvHeader);
            sb.AppendLine(resultText);
            using (StreamWriter file = new StreamWriter(CsvFileLocation, true))
            {
                file.WriteLine(sb.ToString());
            }
        }
    }
}
