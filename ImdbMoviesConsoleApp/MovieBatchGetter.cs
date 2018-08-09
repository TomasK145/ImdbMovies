using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImdbMoviesConsoleApp
{
    public class MovieBatchGetter
    {
        private const int ImdbIdLength = 7;
        public List<Movie> GetMovies(int imdbIdFrom, int movieCount)
        {
            Logger.Instance.WriteLog($"MovieBatchGetter - imdbIdFrom: {imdbIdFrom} - movieCount: {movieCount}");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Movie> movies = new List<Movie>();

            OmdbApiManager omdbApiManager = new OmdbApiManager();
            int counter = 0;

            while (counter < movieCount)
            {
                string imdbIdNBumericPart = imdbIdFrom.ToString();
                while (imdbIdNBumericPart.Length != ImdbIdLength)
                {
                    imdbIdNBumericPart = "0" + imdbIdNBumericPart;
                }

                Movie movie = null;
                try
                {
                    movie = omdbApiManager.GetMovieDataByImdbId(imdbIdNBumericPart);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteLog($"imdbIdNBumericPart: {imdbIdNBumericPart} - Exception: {ex.ToString()}");
                    movie = new Movie(imdbIdNBumericPart, ex.ToString());
                }
                
                if (movie != null)
                {
                    movies.Add(movie);
                }
                counter++;
                imdbIdFrom++;
            }

            sw.Stop();
            Logger.Instance.WriteLog($"Get movies - duration: {sw.ElapsedMilliseconds} ms");

            return movies;
        }

        public List<Movie> GetMovies(List<int> failedIdsLists)
        {
            Logger.Instance.WriteLog($"MovieBatchGetter - GetMoviesForIdList start");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Movie> movies = new List<Movie>();

            OmdbApiManager omdbApiManager = new OmdbApiManager();

            foreach (var movieId in failedIdsLists)
            {
                string imdbIdNBumericPart = movieId.ToString();
                while (imdbIdNBumericPart.Length != ImdbIdLength)
                {
                    imdbIdNBumericPart = "0" + imdbIdNBumericPart;
                }

                Movie movie = null;
                try
                {
                    movie = omdbApiManager.GetMovieDataByImdbId(imdbIdNBumericPart);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteLog($"imdbIdNBumericPart: {imdbIdNBumericPart} - Exception: {ex.ToString()}");
                    movie = new Movie(imdbIdNBumericPart, ex.ToString());
                }

                if (movie != null)
                {
                    movies.Add(movie);
                }
            }

            sw.Stop();
            Logger.Instance.WriteLog($"MovieBatchGetter - GetMoviesForIdList end - duration: {sw.ElapsedMilliseconds} ms");

            return movies;
        }
    }
}
