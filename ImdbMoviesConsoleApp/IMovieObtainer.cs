using System.Collections.Generic;

namespace ImdbMoviesConsoleApp
{
    public interface IMovieObtainer
    {
        List<Movie> GetListOfMovies(int countOfTask, int imdbIdForProcessing, int batchSize);
    }
}