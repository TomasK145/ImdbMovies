using System.Collections.Generic;

namespace ImdbMoviesConsoleApp
{
    public interface IMovieObtainer
    {
        List<Movie> GetListOfMovies(int imdbIdForProcessing, int batchSize);
    }
}