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

            int imdbIdFrom = 4154756;
            int movieCount = 500;

            Task<List<Movie>> taskA = Task<List<Movie>>.Run(() => GetMovies(imdbIdFrom, movieCount));
            Task<List<Movie>> taskB = Task<List<Movie>>.Run(() => GetMovies((imdbIdFrom - movieCount), movieCount));
            Task<List<Movie>> taskC = Task<List<Movie>>.Run(() => GetMovies((imdbIdFrom - movieCount), movieCount));
            Task<List<Movie>> taskD = Task<List<Movie>>.Run(() => GetMovies((imdbIdFrom - movieCount), movieCount));
            Task<List<Movie>> taskE = Task<List<Movie>>.Run(() => GetMovies((imdbIdFrom - movieCount), movieCount));


            // movies.AddRange(GetMovies(imdbIdFrom, movieCount));

            taskA.Wait();
            taskB.Wait();
            taskC.Wait();
            taskD.Wait();
            taskE.Wait();

            Logger.WriteLog("taskA.Result count: "  + taskA.Result.Count);
            movies.AddRange(taskA.Result);

            Logger.WriteLog("taskB.Result count: " + taskB.Result.Count);
            movies.AddRange(taskB.Result);

            Logger.WriteLog("taskC.Result count: " + taskC.Result.Count);
            movies.AddRange(taskC.Result);

            Logger.WriteLog("taskD.Result count: " + taskD.Result.Count);
            movies.AddRange(taskD.Result);

            Logger.WriteLog("taskE.Result count: " + taskE.Result.Count);
            movies.AddRange(taskE.Result);



            sw.Restart();
            string moviesInfo = PrintMovies(movies);
            sw.Stop();
            Logger.WriteLog($"Process movies info - duration: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            movieExporter.ExportMoviesToCsv(moviesInfo);
            sw.Stop();
            Logger.WriteLog($"Export movies to CSV - duration: {sw.ElapsedMilliseconds} ms");
        }       
        
        private static List<Movie> GetMovies(int imdbIdFrom, int movieCount)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Movie> movies = new List<Movie>();

            OmdbApiManager omdbApiManager = new OmdbApiManager();       
            int counter = 0;

            while (counter <= movieCount) //TODO: prerobit na paralelne volanie
            {
                string imdbIdNBumericPart = imdbIdFrom.ToString();
                Movie movie = omdbApiManager.GetMovieDataByImdbId(imdbIdNBumericPart);
                if (movie != null)
                {
                    movies.Add(movie);
                }
                counter++;
                imdbIdFrom--;
            }

            sw.Stop();
            Logger.WriteLog($"Get movies - duration: {sw.ElapsedMilliseconds} ms");

            return movies;
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
