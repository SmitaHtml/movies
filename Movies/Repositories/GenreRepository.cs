

using Movies.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Repositories
{
    public class GenreRepository
    {
        private readonly MovieDatabase _movieDatabase;

        public GenreRepository(MovieDatabase movieDatabase)
        {
            _movieDatabase = movieDatabase;
        }

        public List<Genre> Get()
        {
            return _movieDatabase.Genres;
        }

        public Genre Get(int id)
        {
            return _movieDatabase.Genres.Single(t => t.Id == id);
        }

        public int Add(Genre genre)
        {
            if (_movieDatabase.Genres.Any())
            {
                var searchGenre = _movieDatabase.Genres.SingleOrDefault(searchg => searchg.Category == genre.Category);
                if (searchGenre != null)
                {
                    genre.Id = searchGenre.Id;
                }
                else
                {
                    genre.Id = _movieDatabase.Genres.Max(maxg => maxg.Id) + 1;
                    _movieDatabase.Genres.Add(genre);
                    _movieDatabase.SaveChanges();
                }
            }
            else
            {
                genre.Id = 1;
                _movieDatabase.Genres.Add(genre);
                _movieDatabase.SaveChanges();
            }
            return (genre.Id);
        }

        public void Edit(Genre genre)
        {
            var dbGenre = _movieDatabase.Genres.Single(t => t.Id == genre.Id);
            dbGenre.Category = genre.Category;
          //  dbCas.FirstName = cast.FirstName;
            _movieDatabase.SaveChanges();
        }

        public void Remove(int id)
        {
            var dbGenre = _movieDatabase.Genres.Single(t => t.Id == id);
            _movieDatabase.Genres.Remove(dbGenre);
            _movieDatabase.SaveChanges();
        }
    }
}