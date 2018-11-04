using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Data
{
    public class Movie
    {
        //movie contains List of casts and genres
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public List<Cast> Cast { get; set; }
        public List<Genre> Genre { get; set; }
    }
}
