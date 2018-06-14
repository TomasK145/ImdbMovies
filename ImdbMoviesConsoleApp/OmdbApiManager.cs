using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ImdbMoviesConsoleApp
{
    public class OmdbApiManager
    {
        const string RequestUrl = "http://www.omdbapi.com/";
        const string SearchByIdParameter = "?apikey=3bc42aaa&type=movie&i=tt";
        const string SearchByQueryParameter = "?apikey=3bc42aaa&s=Lord&type=movie&y=1990&page=1";

        public Movie GetMovieDataByImdbId(string imdbIdNBumericPart)
        {
            HttpClient client = InicializeHttpClient();
            string urlParameters = SearchByIdParameter + imdbIdNBumericPart;

            // List data response.
            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync(urlParameters).Result;  // Blocking call!
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (response == null)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var moveResponse = response.Content.ReadAsAsync<MovieResponse>().Result;
                if (String.IsNullOrEmpty(moveResponse.imdbID) || moveResponse.imdbID.Equals("N/A"))
                {
                    return null;
                }
                var movie = new Movie(moveResponse);
                return movie;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return null;
        }

        public List<Movie> GetMovies()
        {
            HttpClient client = InicializeHttpClient();
            List<Movie> movies = new List<Movie>();

            // List data response.
            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync(SearchByQueryParameter).Result;  // Blocking call!
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (response == null)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var moveResponse = response.Content.ReadAsAsync<MovieResponse>().Result;

                movies.AddRange(movies);

                return movies;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return null;
        }

        private HttpClient InicializeHttpClient()
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 5);
            
            client.BaseAddress = new Uri(RequestUrl);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
