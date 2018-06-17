using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
            int defaultImdbId = 98000;  // 4154756; //tt0098000 - Nocturne indien (1989) - pociatok ziskavania filmov

            int imdbIdLastProcessed = DatabaseProcessor.GetMovieIdForNextProcessing();
            int imdbIdForProcessing = imdbIdLastProcessed == 0 ? defaultImdbId : (imdbIdLastProcessed + 1);
            int batchSize = 500;
            int countOfTasks = 10;

            movies = movieManager.GetMoviesFomImdb(countOfTasks, imdbIdForProcessing, batchSize);
            

            sw.Restart();
            DatabaseProcessor.SaveMoviesToDatabase(movies);
            sw.Stop();
            Logger.WriteLog($"Save movies to DB - duration: {sw.ElapsedMilliseconds} ms");

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
