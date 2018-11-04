
using Movies.Data;
using Movies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Movies
{ 
    class Program
    {
        private static CastRepository _castRepository;
        private static MovieRepository _movieRepository;
        private static GenreRepository _genreRepository;

        static void Main(string[] args)
        {
            //Prepare for importing movies
            var movieDatabase = new MovieDatabase();
            _castRepository = new CastRepository(movieDatabase);
            _movieRepository = new MovieRepository(movieDatabase);
            _genreRepository = new GenreRepository(movieDatabase);

            //import movies from json file: importedmovies.json in folder: User\Documents\Project\Movies\Movies\bin\Debug
            GetImportedMovies();

            //Menu options to run methods
            var menuOption = MainMenu();
            while (menuOption < 7)
            {
                switch (menuOption)
                {
                    case 1:
                        GetAllMovies();
                        break;                       
                    case 2:
                        GetAllCasts();
                        break;
                    case 3:
                        GetAllGenres();
                        break;
                    case 4:
                        AddMovie();
                        break;
                    case 5:
                        AddCast();
                        break;
                    case 6:
                        AddGenre();
                        break;
                    case 7:
                        break;
                    default:
                        Console.WriteLine("You chose an invalid option.");
                        Clear();
                        break;
                }

                if (menuOption != 7)
                {
                    menuOption = MainMenu();
                }
            }
        }

        static int MainMenu()
        {
            //Display menu on console
            Console.WriteLine("Main Menu");
            Console.WriteLine("1. Get all movies");
            Console.WriteLine("2. Get all casts");
            Console.WriteLine("3. Get all genres");
            Console.WriteLine("4. Add a movie");
            Console.WriteLine("5. Add a cast");
            Console.WriteLine("6. Add a genre");
            Console.WriteLine("7. Exit");
            
            Console.WriteLine();
            Console.Write("Enter an option: ");

            var option = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(option))
            {
                return 7;
            }

            return int.Parse(option);
        }

        static void GetImportedMovies()
        {
            //import movies from json file: importedmovies.json
            var importedmovies = _movieRepository.GetImportedMovies();
            var movies = _movieRepository.Get();
            //importedmovies.json file / movies.json file is needed to show movies
            if (!importedmovies.Any() && !movies.Any())
            {
                Console.WriteLine("The file: importedmovies.json is needed for import. Please add it to folder: User\\Documents\\Project\\Movies\\Movies\\bin\\Debug");
            }
            else
            {
                if (!movies.Any())
                {
                    Console.WriteLine($"Importing movies from external file...");
                    foreach (var importedmovie in importedmovies)
                    {
                        //----for each movie in importedmovie.json add list of cast and genre -------
                        //------------- Cast - start -------------------------------                    
                        var newListCast = new List<Cast>();
                        var cast = importedmovie.Cast;
                        //--------- loop thru cast in string array format from importedmovies.json
                        //--------- to create List of Cast to be inserted into Movies.json -------
                        foreach (String c in cast)
                        {
                            //----- create new cast for one movie -------
                            var newCast = new Cast
                            {
                                Name = c
                            };
                            int newCastId = _castRepository.Add(newCast);
                            newCast = _castRepository.Get(newCastId); // Get new cast along with id
                            newListCast.Add(newCast); //Add to Cast List collection
                            //Console.WriteLine($"New Cast Id: {newCastId} {c}");
                            //----- create new cast for one movie -------
                        }
                        //------------- Cast - End ---------------------------------

                        //------------- Genres - start -------------------------------
                        var newListGenres = new List<Genre>();

                        var genre = importedmovie.Genres;
                        foreach (String g in genre)
                        {
                            //----- create new cast for one movie -------
                            var newGenre = new Genre
                            {
                                Category = g
                            };
                            int newGenreId = _genreRepository.Add(newGenre);
                            newGenre = _genreRepository.Get(newGenreId); // Get new cast along with id
                            newListGenres.Add(newGenre); //Add to Cast List collection
                            //Console.WriteLine($"New Genre Id: {newGenreId} {g}");
                            //----- create new cast for one movie -------
                        }
                        //------------- Genres - End -------------------------------

                        //------------- Add Movie start ----------------------
                        var newMovie = new Movie()
                        {
                            Title = importedmovie.Title,
                            Year = importedmovie.Year,
                            Cast = newListCast, //includes List of casts
                            Genre = newListGenres //includes List of genres
                        };
                        int newMovieId = _movieRepository.Add(newMovie);
                        //------------- Add Movie end-------------------------
                        Console.WriteLine($"Movie: {newMovieId} {importedmovie.Title}, {importedmovie.Year}");
                        newMovie = _movieRepository.Get(newMovieId);
                    }
                }
            }       

            Clear();
        }

        static void GetAllMovies()
        {
            var movies = _movieRepository.Get();
            if (!movies.Any())
            {
                Console.WriteLine("No movies have been added.");
            }
            else
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine($"{movie.Id}: {movie.Title} {movie.Year}");
                    foreach (var cast in movie.Cast)
                    {
                        Console.WriteLine($"                              Cast: {cast.Name}");
                    }

                    foreach (var genre in movie.Genre)
                    {
                        Console.WriteLine($"                                                   Genre: {genre.Category}");
                    }
                }
            }

            Clear();
        }

        static void GetAllCasts()
        {
            var casts = _castRepository.Get();
            if (!casts.Any())
            {
                Console.WriteLine("No casts have been added.");
            }
            else
            {
                foreach (var cast in casts)
                {
                    Console.WriteLine($"{cast.Id}: {cast.Name}");
                }
            }
            Clear();
        }

        static void GetAllGenres()
        {
            var genres = _genreRepository.Get();

            if (!genres.Any())
            {
                Console.WriteLine("No genres have been added.");
            }
            else
            {
                foreach (var genre in genres)
                {
                    Console.WriteLine($"{genre.Id}: {genre.Category} ");
                }
            }
            Clear();
        }

        static void AddMovie()
        {
            //Input movie
            Console.Write("Movie title: ");
            var movieTitle = Console.ReadLine();
            Console.Write("Movie year: ");
            var movieYear = int.Parse(Console.ReadLine());

            //add cast to new movie
            var addMovieCastInput = "Y";
            var searchListCast = new List<Cast>();
            while (addMovieCastInput == "Y")
            {
                Console.Write("Do you need to add casts to the movie? (Enter Y/N): ");
                addMovieCastInput = Console.ReadLine();
                if (addMovieCastInput == "Y")
                {
                    Console.Write("Enter the name of the cast: ");
                    var newCastName = Console.ReadLine();
                    var newCast = new Cast()
                    {
                        Name = newCastName
                    };
                    int newCastId = _castRepository.Add(newCast);
                    newCast = _castRepository.Get(newCastId);
                    searchListCast.Add(newCast);
                    //do not write new movie till we have all the casts
                }
            }

            //add genre to new movie
            var addMovieGenreInput = "Y";
            var searchListGenre = new List<Genre>();
            while (addMovieGenreInput == "Y")
            {
                Console.Write("Do you need to add genre to the movie? (Enter Y/N): ");
                addMovieGenreInput = Console.ReadLine();
                if (addMovieGenreInput == "Y")
                {
                    Console.Write("Enter the the genre: ");
                    var newGenreName = Console.ReadLine();
                    var newGenre = new Genre()
                    {
                        Category = newGenreName
                    };
                    int newGenreId = _genreRepository.Add(newGenre);
                    newGenre = _genreRepository.Get(newGenreId);
                    searchListGenre.Add(newGenre);
                    //do not write new movie till we have all the casts
                }
            }

            //add movie after adding cast and genre, to movie
            var newMovie = new Movie
            {
                Title = movieTitle,
                Year = movieYear,
                Cast = searchListCast,
                Genre = searchListGenre
            };
            _movieRepository.Add(newMovie);
            Console.WriteLine($"Movie {movieTitle} was added!");

            Clear();
        }

        static void AddCast()
        {
            Console.Write("Name: ");
            var name= Console.ReadLine();            
            var newCast = new Cast
            {
                Name = name
            };
            _castRepository.Add(newCast);
            Console.WriteLine($"Cast {name} was added!");
            Clear();
        }
               
        static void AddGenre()
        {
            Console.Write("Category: ");
            var category = Console.ReadLine();
            var newGenre = new Genre
            {
                Category = category,

            };
            _genreRepository.Add(newGenre);
            Console.WriteLine($"Genre {category} was added!");
            Clear();
        }

        static void Clear()
        {
            Console.Write("Press <Enter> to continue...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
