using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Autofac;

namespace ImdbMoviesConsoleApp
{
    class Program
    {
        private static IMovieRepository dbProcessor;
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Zadaj: 1 - ulozi filmy z IMDB do databazy, 2 - exportuje filmy z databazy do CSV suboru");
            string selectedOption = Console.ReadLine();
            Container = DependencyConfiguration.InitializeContainer();
            dbProcessor = Container.Resolve<IMovieRepository>();



            //TODO: zvacsit timeout pre pracu s API --> ziskat z DB vsetky ID, ktore skoncili v chybe a opatovne spracovat s vacsim timeoutom
            //TODO: logika pre ziskanie zaznamov z DB s errorom
            //TODO: prirobit requestovanie podla konkretneho ID 

            stopwatch.Start();
            if (selectedOption.Equals("1"))
            {
                Console.WriteLine("Prebieha ziskavanie filmov z IMDB a ukladanie do databazy...");
                GetMoviesFromImdbToDatabase();
            }
            else if (selectedOption.Equals("2"))
            {
                Console.WriteLine("Prebieha export filmov z databazy do CSV suboru...");
                ExportMoviesFromDatabaseToCsv();
            }
            else
            {
                Console.WriteLine("Nespravna volba! Program bude ukonceny.");
                Console.ReadLine();
            }
            stopwatch.Stop();
            Logger.Instance.WriteLog($"Total duration: {stopwatch.ElapsedMilliseconds} ms");
        }

        private static void GetMoviesFromImdbToDatabase()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<Movie> movies = new List<Movie>();

            MovieManager movieManager = new MovieManager(new TaskManager());
            int defaultImdbId = Int32.Parse(ConfigReader.GetConfigValue("defaultImdbId")); //tt0098000 - Nocturne indien (1989) - pociatok ziskavania filmov?

            int imdbIdLastProcessed = dbProcessor.GetMovieIdForNextProcessing();
            int imdbIdForProcessing = imdbIdLastProcessed == 0 ? defaultImdbId : (imdbIdLastProcessed + 1);
            int batchSize = Int32.Parse(ConfigReader.GetConfigValue("batchSize"));

            Console.WriteLine($"Data budu ziskavane pre IMDB filmy od ID = {imdbIdForProcessing}");
            movies = movieManager.GetMoviesFomImdb(imdbIdForProcessing, batchSize);
            
            sw.Restart();
            dbProcessor.SaveMoviesToDatabase(movies);
            sw.Stop();
            Logger.Instance.WriteLog($"Save movies to DB - duration: {sw.ElapsedMilliseconds} ms");
        }

        private static void ExportMoviesFromDatabaseToCsv()
        {
            List<Movie> movies = new List<Movie>();
            MovieExporter movieExporter = new MovieExporter();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            movies = dbProcessor.ReadMoviesFromDatabase();
            sw.Stop();
            Logger.Instance.WriteLog($"Read movies from DB - duration: {sw.ElapsedMilliseconds} ms");
            //TODO : https://stackoverflow.com/questions/15414347/how-to-loop-through-ienumerable-in-batches
            //TODO: optimalizovat zapisovanie do CSV
            sw.Restart();
            string moviesInfo = PrintMovies(movies);
            sw.Stop();
            Logger.Instance.WriteLog($"Process movies info - duration: {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            movieExporter.ExportMoviesToCsv(moviesInfo);
            sw.Stop();
            Logger.Instance.WriteLog($"Export movies to CSV - duration: {sw.ElapsedMilliseconds} ms");
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
