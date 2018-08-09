using System.Collections.Generic;

namespace ImdbMoviesConsoleApp
{
    public interface IMovieRepository
    {
        void SaveMoviesToDatabase(List<Movie> movies);
        void DeleteMoviesFromDatabase(List<int> moviesIdsForDeletion);
        int GetMovieIdForNextProcessing();
        List<Movie> ReadMoviesFromDatabase();
        List<int> GetFailedMovieIds(int selectTopCount);
    }
}
