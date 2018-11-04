

using Newtonsoft.Json;
using Movies.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Movies
{
    public class MovieDatabase
    {
        private readonly string _importedmoviesFilePath;
        private readonly string _moviesFilePath;
        private readonly string _castsFilePath;
        private readonly string _genresFilePath;

        public MovieDatabase()
        {
            //initialize file paths
            var folderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _importedmoviesFilePath = Path.Combine(folderPath, "importedmovies.json");
            _moviesFilePath = Path.Combine(folderPath, "movies.json");
            _castsFilePath = Path.Combine(folderPath, "casts.json");
            _genresFilePath = Path.Combine(folderPath, "genres.json");
            Load();
        }
        //initialize to empty
        public List<ImportedMovie> ImportedMovies { get; set; } = new List<ImportedMovie>();
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public List<Cast> Casts { get; set; } = new List<Cast>();
        public List<Genre> Genres { get; set; } = new List<Genre>();

        private void Load()
        {
            if (File.Exists(_importedmoviesFilePath))
            {
                using (var reader = new StreamReader(_importedmoviesFilePath))
                {
                    //Import movies from file importedmovies.json and cleanup to upload fresh data
                    //jsonBinaryFormatMadhe = StreamReader("C:\movies.json").ReadToEnd();
                    //JsonConvert.DeserializeObject<List<Movie>>(jsonBinaryFormatMadhe);
                    var importedmoviesJson = reader.ReadToEnd();
                    ImportedMovies = JsonConvert.DeserializeObject<List<ImportedMovie>>(importedmoviesJson);
                    File.Delete(_moviesFilePath);
                    File.Delete(_castsFilePath);
                    File.Delete(_genresFilePath);                    
                }
                File.Delete(_importedmoviesFilePath);
            }

            if (File.Exists(_moviesFilePath))
            {
                using (var reader = new StreamReader(_moviesFilePath))
                {
                    var moviesJson = reader.ReadToEnd(); 
                    Movies = JsonConvert.DeserializeObject<List<Movie>>(moviesJson);
                }
            }

            if (File.Exists(_castsFilePath))
            {
                using (var reader = new StreamReader(_castsFilePath))
                {
                    var castsJson = reader.ReadToEnd();
                    Casts = JsonConvert.DeserializeObject<List<Cast>>(castsJson);
                }
            }
            if (File.Exists(_genresFilePath))
            {
                using (var reader = new StreamReader(_genresFilePath))
                {
                    var genresJson = reader.ReadToEnd();
                    Genres = JsonConvert.DeserializeObject<List<Genre>>(genresJson);
                }
            }

        }

        public void SaveChanges()
        {
            var moviesJson = JsonConvert.SerializeObject(Movies);
            var moviesFile = File.Create(_moviesFilePath);
            using (var writer = new StreamWriter(moviesFile))
            {
                writer.Write(moviesJson);
            }

            var castsJson = JsonConvert.SerializeObject(Casts);
            var castsFile = File.Create(_castsFilePath);
            using (var writer = new StreamWriter(castsFile))
            {
                writer.Write(castsJson);
            }

            var genresJson = JsonConvert.SerializeObject(Genres);
            var genresFile = File.Create(_genresFilePath);
            using (var writer = new StreamWriter(genresFile))
            {
                writer.Write(genresJson);
            }

        }
    }
}
