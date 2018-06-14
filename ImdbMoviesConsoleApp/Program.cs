using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            List<Movie> movies = new List<Movie>();

            //movies = omdbApiManager.GetMovies();

            int imdbIdFrom = 4154756;

            for (int i = imdbIdFrom; i >= (imdbIdFrom - 500); i--)
            {
                string imdbIdNBumericPart = i.ToString();
                Movie movie = omdbApiManager.GetMovieDataByImdbId(imdbIdNBumericPart);
                if (movie != null)
                {
                    movies.Add(movie);
                }
            }

            sw.Stop();
            Console.WriteLine($"Get movies - duration: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            PrintMovies(movies);
            sw.Stop();
            Console.WriteLine($"Print movies - duration: {sw.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }

        private static void PrintMovies(List<Movie> movies)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (var movie in movies)
            {
                sb.AppendLine(movie.ToString());
            }

            Console.WriteLine(sb);
        }

        
    }
}
