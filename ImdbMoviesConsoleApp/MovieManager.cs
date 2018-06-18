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
    }
}
