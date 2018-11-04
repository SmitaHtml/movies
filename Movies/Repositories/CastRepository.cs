
using Movies.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Repositories
{
    public class CastRepository
    {
        private readonly MovieDatabase _movieDatabase;

        public CastRepository(MovieDatabase movieDatabase)
        {
            _movieDatabase = movieDatabase;
        }

        public List<Cast> Get()
        {
            return _movieDatabase.Casts;
        }

        public Cast Get(int id)
        {
            return _movieDatabase.Casts.Single(t => t.Id == id);
        }

        public int Add(Cast cast)
        {
            if (_movieDatabase.Casts.Any())
            {
                var searchCast = _movieDatabase.Casts.SingleOrDefault(searchc => searchc.Name == cast.Name);
                if (searchCast != null)
                {
                    cast.Id = searchCast.Id;
                }
                else
                {
                    cast.Id = _movieDatabase.Casts.Max(maxc => maxc.Id) + 1;
                    _movieDatabase.Casts.Add(cast);
                    _movieDatabase.SaveChanges();
                }                
            }
            else
            {
                cast.Id = 1;
                _movieDatabase.Casts.Add(cast);
                _movieDatabase.SaveChanges();
            }             
            return (cast.Id);
        }

        public void Edit(Cast cast)
        {
            var dbCast = _movieDatabase.Casts.Single(t => t.Id == cast.Id);
            dbCast.Name = cast.Name;
            _movieDatabase.SaveChanges();
        }

        public void Remove(int id)
        {
            var dbCast = _movieDatabase.Casts.Single(t => t.Id == id);
            _movieDatabase.Casts.Remove(dbCast);
            _movieDatabase.SaveChanges();
        }
    }
}
