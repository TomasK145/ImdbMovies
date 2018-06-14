using System;
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

            HttpResponseMessage response = null;
            try
            {
                response = client.GetAsync(urlParameters).Result;  // Blocking call!
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"imdbIdNBumericPart: {imdbIdNBumericPart} - Exception: {ex.ToString()}");
            }

            if (response == null)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var moveResponse = response.Content.ReadAsAsync<MovieResponse>().Result;
                if (String.IsNullOrEmpty(moveResponse.imdbID) || moveResponse.imdbID.Equals("N/A") || moveResponse.Country.Equals("N/A"))
                {
                    return null;
                }
                var movie = new Movie(moveResponse);
                return movie;
            }
            else
            {
                Logger.WriteLog($"{(int)response.StatusCode} ({response.ReasonPhrase})");
            }
            return null;
        }

        private HttpClient InicializeHttpClient()
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 5);            
            client.BaseAddress = new Uri(RequestUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
