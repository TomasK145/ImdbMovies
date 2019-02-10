using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            Console.WriteLine("Zadaj: 1 - ulozi filmy z IMDB do databazy, 2 - exportuje filmy z databazy do CSV suboru, 3 - opatovny pokus o ziskanie filmov s exception, 4 - ziskaj chybajuce filmy, 5 - ziskaj podla ID");
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
            else if (selectedOption.Equals("3"))
            {
                Console.WriteLine("Prebieha...");
                int taskCount = Int32.Parse(ConfigReader.GetConfigValue("taskCount"));
                int batchSize = Int32.Parse(ConfigReader.GetConfigValue("batchSize"));
                int selectCount = 5000; // taskCount * batchSize;

                List<int> failedMoviesIds = dbProcessor.GetFailedMovieIds(selectCount);
                GetFailedMoviesFromImdbToDatabase(failedMoviesIds);
            }
            else if (selectedOption.Equals("4"))
            {
                Console.WriteLine("Prebieha...");
                //posledne spracovane: takeCount:  // skipCount:
                int takeCount = 100000;
                int skipCount = 4000000; //zvysok o tie kt boli checknute

                List<int> failedMoviesIds = dbProcessor.GetNotExistingMovieIds(takeCount, skipCount);
                GetFailedMoviesFromImdbToDatabase(failedMoviesIds);
                //TODO: spracovanie
            }
            else if (selectedOption.Equals("5"))
            {
                Console.WriteLine("Prebieha...");
                int takeCount = 100000;
                int skipCount = 0;

                List<int> missingMoviesIds = dbProcessor.GetNotExistingMovieIds(takeCount, skipCount);


                //List<int> movieIdList = missingMoviesIds;
                //GetMoviesByIds(movieIdList);
                GetFailedMoviesFromImdbToDatabase(missingMoviesIds);
                //TODO: spracovanie
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

        private static void GetFailedMoviesFromImdbToDatabase(List<int> failedMoviesIds)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            MovieManager movieManager = new MovieManager(new TaskManager());
            List<Movie> movies = movieManager.GetMoviesFomImdb(failedMoviesIds);

            sw.Restart();
            List<int> movieIdForDeletion = movies.Select(m => Convert.ToInt32(m.imdbID.Replace("tt", ""))).ToList();
            dbProcessor.DeleteMoviesFromDatabase(movieIdForDeletion);
            dbProcessor.SaveMoviesToDatabase(movies);
            sw.Stop();
            Logger.Instance.WriteLog($"Save movies to DB - duration: {sw.ElapsedMilliseconds} ms");
        }

        private static void GetMoviesByIds(List<int> movieIdList)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            MovieManager movieManager = new MovieManager(new TaskManager());
            List<Movie> movies = movieManager.GetMoviesFromImdbFromIdList(movieIdList);

            List<int> movieIdListToDelete = movies.Select(m => Convert.ToInt32(m.imdbID.Replace("tt", ""))).ToList();

            sw.Restart();
            dbProcessor.DeleteMoviesFromDatabase(movieIdListToDelete);
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
            int movieCount = movies.Count;
            int batchSize = 500000;
            int currentBatch = 0;
            int totalBatchCount = (int)Math.Ceiling((double)movieCount / (double)batchSize);

            sw.Stop();
            Logger.Instance.WriteLog($"Read movies from DB - duration: {sw.ElapsedMilliseconds} ms");
            //TODO : https://stackoverflow.com/questions/15414347/how-to-loop-through-ienumerable-in-batches
            //TODO: optimalizovat zapisovanie do CSV

            while (currentBatch < totalBatchCount)
            {
                List<Movie> moviesForProcessing = movies.Skip(currentBatch * batchSize).Take(batchSize).ToList();

                sw.Restart();
                string moviesInfo = PrintMovies(moviesForProcessing);
                sw.Stop();
                Logger.Instance.WriteLog($"Process movies info - duration: {sw.ElapsedMilliseconds} ms");

                sw.Restart();
                movieExporter.ExportMoviesToCsv(moviesInfo, currentBatch);
                sw.Stop();
                Logger.Instance.WriteLog($"Export movies to CSV - duration: {sw.ElapsedMilliseconds} ms");

                currentBatch++;
            }
            
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
