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

        public List<Movie> GetMoviesFomImdb(int countOfTasks, int imdbIdForProcessing, int batchSize)
        {
            List<Movie> movies = MovieObtainer.GetListOfMovies(countOfTasks, imdbIdForProcessing, batchSize);
            return movies;
        }
    }
}
