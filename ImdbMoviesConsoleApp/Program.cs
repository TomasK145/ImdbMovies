using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImdbMoviesConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<Movie> movies = new List<Movie>();
            MovieExporter movieExporter = new MovieExporter();
            MovieManager movieManager = new MovieManager(new TaskManager());

            int imdbIdForProcessing = 4154756;
            int batchSize = 1000;
            int countOfTasks = 10;

            movies = movieManager.GetMoviesFomImdb(countOfTasks, imdbIdForProcessing, batchSize);

            sw.Restart();
            string moviesInfo = PrintMovies(movies);
            sw.Stop();
            Logger.WriteLog($"Process movies info - duration: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            movieExporter.ExportMoviesToCsv(moviesInfo);
            sw.Stop();
            Logger.WriteLog($"Export movies to CSV - duration: {sw.ElapsedMilliseconds} ms");
        }

        private static string PrintMovies(List<Movie> movies)
        {
            StringBuilder sb = new StringBuilder();            
            foreach (var movie in movies)
            {
                sb.AppendLine(movie.ToString());
            }
            return sb.ToString();
        }

        
    }
}
