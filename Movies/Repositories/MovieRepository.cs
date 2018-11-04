
using Movies.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Repositories
{
    public class MovieRepository
    {
        private readonly MovieDatabase _movieDatabase;

        public MovieRepository(MovieDatabase movieDatabase)
        {
            _movieDatabase = movieDatabase;
        }

        public List<ImportedMovie> GetImportedMovies()
        {
            return _movieDatabase.ImportedMovies;
        }

        public List<Movie> Get()
        {
            return _movieDatabase.Movies;
        }

        public Movie Get(int id)
        {
            return _movieDatabase.Movies.Single(c => c.Id == id);
        }

        public int Add(Movie Movie)
        {
            if (_movieDatabase.Movies.Any())
            {
                var searchMovie = _movieDatabase.Movies.SingleOrDefault(searchm => searchm.Title == Movie.Title);
                if (searchMovie != null)
                {
                    Movie.Id = searchMovie.Id;
                }
                else
                {
                    Movie.Id = _movieDatabase.Movies.Max(maxm => maxm.Id) + 1;
                    _movieDatabase.Movies.Add(Movie);
                    _movieDatabase.SaveChanges();
                }
            }
            else
            {
                Movie.Id = 1;
                _movieDatabase.Movies.Add(Movie);
                _movieDatabase.SaveChanges();
            }
            return (Movie.Id);
        }

        public void Update(Movie Movie)
        {
            var dbMovie = _movieDatabase.Movies.Single(c => c.Id == Movie.Id);
            dbMovie.Title = Movie.Title;
            dbMovie.Year = Movie.Year;
            //var dbCast = _movieDatabase.Casts.Single(t => t.Id == Movie.Cast.Id);
            //dbMovie.Cast = dbCast;

            _movieDatabase.SaveChanges();
        }

        public void Remove(int id)
        {
            var dbMovie = _movieDatabase.Movies.Single(c => c.Id == id);
            _movieDatabase.Movies.Remove(dbMovie);
            _movieDatabase.SaveChanges();
        }
    }
}
