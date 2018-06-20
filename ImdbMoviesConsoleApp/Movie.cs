namespace ImdbMoviesConsoleApp
{
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
        public string InfoMessage { get; set; }

        public Movie()
        {

        }

        public Movie(string imdbIdIValue, string infoMessage)
        {
            imdbID = "tt" + imdbIdIValue;
            InfoMessage = infoMessage;
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
            InfoMessage = "N/A";
        }

        public override string ToString()
        {
            var separator = ConfigReader.GetConfigValue("csvSeparator");
            return $"{Title}{separator}{Year}{separator}{Released}{separator}{Runtime}{separator}{Genre}{separator}{Country}{separator}{Poster}{separator}{Metascore}{separator}{imdbRating}{separator}{BoxOffice}{separator}{Production}{separator}{Website}{separator}{ImdbUrl}";
        }
    }
}
