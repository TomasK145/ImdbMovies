using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbMoviesConsoleApp
{
    public static class DatabaseProcessor
    {
        public static void SaveMoviesToDatabase(List<Movie> movies)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ImdbMovieConnection"].ConnectionString; ;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string cmdString = "INSERT INTO IMDB_MOVIE (IMDB_ID,TITLE,YEAR,RELEASED,RUNTIME,GENRE,COUNTRY,POSTER,METASCORE,IMDB_RATING,BOX_OFFICE,PRODUCTION,WEBSITE,INFO_MESSAGE,IMDB_ID_NUM) VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9, @val10, @val11, @val12, @val13, @val14, @val15)";

                foreach (var movie in movies)
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = cmdString;
                        sqlCommand.Parameters.AddWithValue("@val1", movie.imdbID);
                        sqlCommand.Parameters.AddWithValue("@val2", movie.Title ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val3", movie.Year ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val4", movie.Released ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val5", movie.Runtime ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val6", movie.Genre ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val7", movie.Country ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val8", movie.Poster ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val9", movie.Metascore ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val10", movie.imdbRating ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val11", movie.BoxOffice ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val12", movie.Production ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val13", movie.Website ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val14", movie.InfoMessage ?? "N/A");
                        sqlCommand.Parameters.AddWithValue("@val15", Convert.ToInt32(movie.imdbID.Replace("tt", "")));

                        try
                        {
                            sqlCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ex: " + ex);
                        }
                    }
                }
            }
        }

        public static int GetMovieIdForNextProcessing()
        {
            int lastMovieId = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["ImdbMovieConnection"].ConnectionString; ;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {  
                string cmdString = "SELECT MAX(IMDB_ID_NUM) FROM IMDB_MOVIE";

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = cmdString;

                    sqlConnection.Open();
                    try
                    {
                        lastMovieId = (int)sqlCommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return lastMovieId;
        }
    }
}
