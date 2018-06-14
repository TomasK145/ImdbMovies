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
            OmdbApiManager omdbApiManager = new OmdbApiManager();
            MovieExporter movieExporter = new MovieExporter();

            List<Movie> movies = new List<Movie>();

            int imdbIdFrom = 4154756;
            int counter = 0;

            while (counter <= 10)
            {
                string imdbIdNBumericPart = imdbIdFrom.ToString();
                Movie movie = omdbApiManager.GetMovieDataByImdbId(imdbIdNBumericPart);
                if (movie != null)
                {
                    movies.Add(movie);
                    counter++;
                }
                imdbIdFrom--;
            }

            sw.Stop();
            Logger.WriteLog($"Get movies - duration: {sw.ElapsedMilliseconds} ms");

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
