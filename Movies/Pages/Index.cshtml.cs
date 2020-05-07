using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {

        /// <summary>
        /// The movies to display on the index page 
        /// </summary>
        public IEnumerable<Movie> Movies { get; protected set; }

        /// <summary>
        /// Gets and sets the search terms
        /// </summary>
        public string SearchTerms { get; set; }

        /// <summary>
        /// Gets and sets the MPAA rating filters
        /// </summary>
        public string[] MPAARatings { get; set; }

        /// <summary>
        /// Gets and sets the MPAA rating filters
        /// </summary>
        [BindProperty]
        public string[] Genres { get; set; }

        /// <summary>
        /// Gets and sets the IMDB minimium rating
        /// </summary>
        public double? IMDBMin { get; set; }

        /// <summary>
        /// Gets and sets the IMDB maximum rating
        /// </summary>
        public double? IMDBMax { get; set; }

        /// <summary>
        /// Gets and sets the IMDB maximum rating
        /// </summary>
        public double? MaxTomato { get; set; }

        /// <summary>
        /// Gets and sets the IMDB minimium rating
        /// </summary>
        public double? MinTomato { get; set; }

        /// <summary>
        /// Does the response initialization for incoming GET requests
        /// </summary>
        public void OnGet(double? IMDBMin, double? IMDBMax, double? MaxTomato, double? MinTomato)
        {
            // Nullable conversion workaround
            this.IMDBMin = IMDBMin;
            this.IMDBMax = IMDBMax;
            this.MinTomato = MinTomato;
            this.MaxTomato = MaxTomato;
            SearchTerms = Request.Query["SearchTerms"];
            MPAARatings = Request.Query["MPAARatings"];
            Genres = Request.Query["Genres"];
            Movies = MovieDatabase.All;
            // Search movie titles for the SearchTerms
            if (SearchTerms != null)
            {
                Movies = Movies.Where(movie => movie.Title != null && movie.Title.Contains(SearchTerms, StringComparison.InvariantCultureIgnoreCase));
            }
            // Filter by MPAA Rating 
            if (MPAARatings != null && MPAARatings.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MPAARating != null &&
                    MPAARatings.Contains(movie.MPAARating)
                    );
            }
            // Filter by Genre Rating 
            if (Genres != null && Genres.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MajorGenre != null &&
                    Genres.Contains(movie.MajorGenre)
                    );
            }
            // Filter by IMDB Rating 
            if (IMDBMin == null && IMDBMax != null)
            {
                Movies = Movies.Where(movie =>
                    movie.IMDBRating <= IMDBMax);
            }
            if (IMDBMin != null && IMDBMax == null)
            {
                Movies = Movies.Where(movie =>
                    movie.IMDBRating >= IMDBMin);
            }
            if (IMDBMin != null && IMDBMax != null)
            {
                Movies = Movies.Where(movie =>
                    movie.IMDBRating >= IMDBMin && movie.IMDBRating <= IMDBMax);
            }
            // Filter by Roten Tomatoes Rating 
            if (MinTomato == null && MaxTomato != null)
            {
                Movies = Movies.Where(movie =>
                    movie.RottenTomatoesRating <= MaxTomato);
            }
            if (MinTomato != null && MaxTomato == null)
            {
                Movies = Movies.Where(movie =>
                    movie.RottenTomatoesRating >= MinTomato);
            }
            if (MinTomato != null && MaxTomato != null)
            {
                Movies = Movies.Where(movie =>
                    movie.RottenTomatoesRating >= MinTomato && movie.RottenTomatoesRating <= MaxTomato);
            }
            /*Movies = MovieDatabase.Search(SearchTerms);
            Movies = MovieDatabase.FilterByMPAARating(Movies, MPAARatings);
            Movies = MovieDatabase.FilterByGenre(Movies, Genres);
            Movies = MovieDatabase.FilterByIMDBRating(Movies, IMDBMin, IMDBMax);
            Movies = MovieDatabase.FilterByTomatoesRating(Movies, MinTomato, MaxTomato);*/
        }
    }
}
