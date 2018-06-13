using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImdbMoviesConsoleApp
{
    class Program
    {
        const string apiKey = "3bc42aaa";
        const string requestUrl = "http://www.omdbapi.com/";
        const string urlParameters = "?apikey=3bc42aaa&i=tt4154756";

        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(requestUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var moveResponse = response.Content.ReadAsAsync<MovieResponse>().Result;
                var movie = new Movie(moveResponse);
                Console.WriteLine(movie.ToString());
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }

    public class Rating
    {
        public string Source { get; set; }
        public string Value { get; set; }
    }

    public class MovieResponse
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public List<Rating> Ratings { get; set; }
        public string Metascore { get; set; }
        public string imdbRating { get; set; }
        public string imdbVotes { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string DVD { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public string Response { get; set; }
    }

    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public string Poster { get; set; }
        public string Metascore { get; set; }
        public string imdbID { get; set; }
        public string imdbRating { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public string ImdbUrl {
            get
            {
                return "https://www.imdb.com/title/" + imdbID;
            }
        }

        public Movie(MovieResponse movieResponse)
        {
            Title = movieResponse.Title;
            Year = movieResponse.Year;
            Released = movieResponse.Released;
            Runtime = movieResponse.Runtime;
            Genre = movieResponse.Genre;
            Country = movieResponse.Country;
            Poster = movieResponse.Poster;
            Metascore = movieResponse.Metascore;
            imdbID = movieResponse.imdbID;
            imdbRating = movieResponse.imdbRating;
            BoxOffice = movieResponse.BoxOffice;
            Production = movieResponse.Production;
            Website = movieResponse.Website;
        }

        public override string ToString()
        {
            return $"{Title};{Year};{Released};{Runtime};{Genre};{Country};{Poster};{Metascore};{imdbRating};{BoxOffice};{Production};{Website};{ImdbUrl}";
        }
    }
}
