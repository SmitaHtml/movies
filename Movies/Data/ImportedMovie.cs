using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data
{
    public class ImportedMovie
    {
        //represents data in importedmovie.json file
        public string Title { get; set; }
        public int Year { get; set; }
        public List<String> Cast { get; set; }
        public List<String> Genres { get; set; }
    }
}