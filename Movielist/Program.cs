using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Movielist
{
    static class Program
    {
        static List<string> movieList = new List<string>();

        /// <summary>
        /// Main Applicatin Entry Point
        /// </summary>
        /// <param name="args">Commandline Arguements.</param>
        static void Main(string[] args)
        {
            Menue();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void BuildContainer()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);
        }

        /// <summary>
        /// The main application loop with menue.
        /// </summary>
        public static void Menue()
        {
            ConsoleKeyInfo pressedKeyInfo;
            do
            {
                Console.WriteLine("Please Chose:");
                Console.WriteLine(" 1.: Read Movies Recursive.");
                Console.WriteLine(" 2.: Print all Movies to Screen.");
                Console.WriteLine("ESC: Exit.");
                pressedKeyInfo = Console.ReadKey();
                switch (pressedKeyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        Console.WriteLine("Exiting.");
                        break;

                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        Console.WriteLine("Read Movies Recursive:");
                        ReadMoviesSequence();
                        break;

                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                        Console.WriteLine("Print all Movies to Screen:");
                        PrintMoviesToScreen();
                        break;

                    default:
                        Console.WriteLine("Option not found. Please try again.");
                        break;
                }
            }
            while (pressedKeyInfo.Key != ConsoleKey.Escape);
        }

        private static void PrintMoviesToScreen()
        {
            var movies = movieList.Select(moviePath => new FileInfo(moviePath)).OrderBy(movieFile => movieFile.Name);

            var listFile = new FileInfo(@"movielist.txt");
            using (var writer = listFile.CreateText())
            {
                foreach (var movie in movies)
                {
                    writer.WriteLine(movie.Name);
                    Console.WriteLine(movie.Name);
                }
            }
        }

        /// <summary>
        /// Get parameters for reading the movie files and execute it.
        /// </summary>
        public static void ReadMoviesSequence()
        {
            var directory = GetPathInteractive();
            var drive = directory.Root;
            var driveId = IdentifyDirectory(drive);

            ReadMovies(directory);
        }

        private static string IdentifyDirectoryInteractive(DirectoryInfo directory)
        {
            string result = null;

            do
            {
                Console.Write("Please Enter a name, as identifier for this directory: ");
                var name = Console.ReadLine();

                if (name != string.Empty)
                {
                    result = name;
                }
                else
                {
                    Console.WriteLine("An empty identifier is invalid");
                }

            }
            while (result == null);

            return result;
        }

        private static string IdentifyDirectory(DirectoryInfo directory)
        {
            var result = directory.Name;

            var configFilePath = Path.Combine(directory.FullName, "MovieListConfiguration.xml");
            var configFile = new FileInfo(configFilePath);

            if (!configFile.Exists)
            {
                var identifier = IdentifyDirectoryInteractive(directory);
                var document =
                    new XElement(
                        "MovieList",
                        new XElement(
                            "DirectoryConfiguration",
                            new XElement(
                                "Id",
                                identifier
                            )
                        )
                    );

                using (var writer = configFile.CreateText())
                {
                    document.WriteTo(XmlWriter.Create(writer));
                }

                result = identifier;
            }
            else
            {
                var configDocument = XDocument.Load(configFile.OpenRead());
                result = configDocument.Element("MovieList").Element("DirectoryConfiguration").Element("Id").Value;
            }

            return result;
        }

        public static DirectoryInfo GetPathInteractive()
        {
            DirectoryInfo directory = default(DirectoryInfo);
            while (true)
            {
                Console.Write("Path: ");
                var path = Console.ReadLine();
                if (path == string.Empty)
                {
                    Console.WriteLine("Empty Path is invalid.");
                    continue;
                }

                try
                {
                    directory = new DirectoryInfo(path);
                }
                catch
                {
                    Console.WriteLine("Invalid Path specified.");
                    continue;
                }

                if (!directory.Exists)
                {
                    Console.WriteLine("Given Path does not exist.");
                    continue;
                }

                break;
            }

            return directory;
        }

        /// <summary>
        /// Read all movies from a given directory and its subdirectories.
        /// </summary>
        /// <param name="directory">The source directory.</param>
        public static void ReadMovies(DirectoryInfo directory)
        {

            foreach (var subdir in directory.EnumerateDirectories())
            {
                try
                {
                    ReadMovies(subdir);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error reading directory " + subdir.FullName + ": " + ex.Message);
                }
            }

            var files = directory.EnumerateFiles().Where(f => IsMovieFile(f));
            foreach (var file in files)
            {
                if (!movieList.Contains(file.FullName))
                {
                    if (file.Name.Contains('[') && file.Name.Contains(']'))
                    {
                        movieList.Add(file.FullName);
                    }

                    //Console.WriteLine("Added: " + file.FullName);
                }
                else
                {
                    //Console.WriteLine("Existing: " + file.FullName);
                }
            }
        }

        /// <summary>
        /// Checks whether a given file is a movie file or not.
        /// </summary>
        /// <param name="file">File to validate.</param>
        /// <returns>True if the file is an movie file. false otherwise.</returns>
        private static bool IsMovieFile(FileInfo file)
        {
            var result = false;
            var extension = file.Extension;
            if (extension.StartsWith("."))
            {
                extension = extension.Substring(1);
            }

            if (Config.Endings.Contains(extension))
            {
                result = true;
            }

            return result;
        }
    }
}
