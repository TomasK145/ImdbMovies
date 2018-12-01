using System;
using System.Collections.Generic;

namespace ImdbMoviesConsoleApp
{
    public class MovieManager
    {
        private IMovieObtainer MovieObtainer { get; set; }

        public MovieManager(IMovieObtainer movieObtainer)
        {
            MovieObtainer = movieObtainer;
        }

        public List<Movie> GetMoviesFomImdb(int imdbIdForProcessing, int batchSize)
        {
            List<Movie> movies = MovieObtainer.GetListOfMovies(imdbIdForProcessing, batchSize);
            return movies;
        }

        public List<Movie> GetMoviesFomImdb(List<int> failedMoviesIds)
        {
            List<Movie> movies = MovieObtainer.GetListOfMovies(failedMoviesIds);
            return movies;
        }

        public List<Movie> GetMoviesFromImdbFromIdList(List<int> movieIds)
        {
            List<Movie> movies = new List<Movie>();
            OmdbApiManager omdbApiManager = new OmdbApiManager();
            int imdbIdLength = 7;

            foreach (int movieId in movieIds)
            {
                string imdbIdNBumericPart = movieId.ToString();
                while (imdbIdNBumericPart.Length != imdbIdLength)
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
                movies.Add(movie);
            }

            return movies;
        }
    }
}
