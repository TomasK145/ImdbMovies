using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ImdbMoviesConsoleApp
{
    public class DapperMovieRepository : IMovieRepository
    {
        private string ImdbDbConnectionString { get; }
        public DapperMovieRepository()
        {
            ImdbDbConnectionString = ConfigurationManager.ConnectionStrings["ImdbMovieConnection"].ConnectionString;
        }

        public List<Movie> ReadMoviesFromDatabase()
        {
            List<Movie> movies;

            using (IDbConnection connection = new SqlConnection(ImdbDbConnectionString))
            {
                string selectQuery = "SELECT IMDB_ID as imdbId,TITLE,YEAR,RELEASED,RUNTIME,GENRE,COUNTRY,POSTER,METASCORE,IMDB_RATING as imdbRating,BOX_OFFICE as BoxOffice,PRODUCTION,WEBSITE FROM IMDB_MOVIE WHERE INFO_MESSAGE = 'N/A' ORDER BY IMDB_ID_NUM ASC";

                movies = connection.Query<Movie>(selectQuery).AsList();
            }
            return movies;
        }

        public void SaveMoviesToDatabase(List<Movie> movies)
        {
            string insertQuery = "INSERT INTO IMDB_MOVIE (IMDB_ID,TITLE,YEAR,RELEASED,RUNTIME,GENRE,COUNTRY,POSTER,METASCORE,IMDB_RATING,BOX_OFFICE,PRODUCTION,WEBSITE,INFO_MESSAGE,IMDB_ID_NUM) VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9, @val10, @val11, @val12, @val13, @val14, @val15)";

            using (IDbConnection connection = new SqlConnection(ImdbDbConnectionString))
            {
                foreach (var movie in movies)
                {
                    try
                    {
                        connection.Execute(insertQuery, new { IMDB_ID = movie.imdbID,
                                                                TITLE = movie.Title ?? "N/A",
                                                                YEAR = movie.Year ?? "N/A",
                                                                RELEASED = movie.Released ?? "N/A",
                                                                RUNTIME = movie.Runtime ?? "N/A",
                                                                GENRE = movie.Genre ?? "N/A",
                                                                COUNTRY = movie.Country ?? "N/A",
                                                                POSTER = movie.Poster ?? "N/A",
                                                                METASCORE = movie.Metascore ?? "N/A",
                                                                IMDB_RATING = movie.imdbRating ?? "N/A",
                                                                BOX_OFFICE = movie.BoxOffice ?? "N/A",
                                                                PRODUCTION = movie.Production ?? "N/A",
                                                                WEBSITE = movie.Website ?? "N/A",
                                                                INFO_MESSAGE = movie.InfoMessage ?? "N/A",
                                                                IMDB_ID_NUM = Convert.ToInt32(movie.imdbID.Replace("tt", ""))
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteLog($"ImdbId: {movie.imdbID} - movieString: {movie.ToString()} - Ex: {ex}");
                    }
                }
            }
        }

        public int GetMovieIdForNextProcessing()
        {
            int lastMovieId = 0;

            using (IDbConnection connection = new SqlConnection(ImdbDbConnectionString))
            {
                string selectQuery = "SELECT MAX(IMDB_ID_NUM) FROM IMDB_MOVIE";
                lastMovieId = connection.QueryFirst<int>(selectQuery);
            }
            return lastMovieId;
        }

        public List<int> GetFailedMovieIds()
        {
            List<int> movieIdList = new List<int>();

            //TODO: logika pre ziskanie ID filmov u ktorych zlyhalo ziskanie dat z IMDB pre opatovny pokus

            return movieIdList;
        }
    }
}
