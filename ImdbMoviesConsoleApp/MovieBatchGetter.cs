﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImdbMoviesConsoleApp
{
    public class MovieBatchGetter
    {
        public List<Movie> GetMovies(int imdbIdFrom, int movieCount)
        {
            Console.WriteLine($"imdbIdFrom: {imdbIdFrom} - movieCount: {movieCount}");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Movie> movies = new List<Movie>();

            OmdbApiManager omdbApiManager = new OmdbApiManager();
            int counter = 0;

            while (counter <= movieCount)
            {
                string imdbIdNBumericPart = imdbIdFrom.ToString();
                Movie movie = omdbApiManager.GetMovieDataByImdbId(imdbIdNBumericPart);
                if (movie != null)
                {
                    movies.Add(movie);
                }
                counter++;
                imdbIdFrom--;
            }

            sw.Stop();
            Logger.WriteLog($"Get movies - duration: {sw.ElapsedMilliseconds} ms");

            return movies;
        }
    }
}
