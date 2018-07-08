using System.Collections.Generic;

namespace ImdbMoviesConsoleApp
{
    public interface IMovieRepository
    {
        void SaveMoviesToDatabase(List<Movie> movies);
        int GetMovieIdForNextProcessing();
        List<Movie> ReadMoviesFromDatabase();
        List<int> GetFailedMovieIds();
    }
}
