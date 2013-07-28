using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movielist
{
    class Program
    {
        /// <summary>
        /// Main Applicatin Entry Point
        /// </summary>
        /// <param name="args">Commandline Arguements.</param>
        static void Main(string[] args)
        {
            Menue();
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
                        MenuReadMoviesRecursive();
                        break;

                    default:
                        Console.WriteLine("Eingabe nicht erkannt. Bitte erneut versuchen.");
                        break;
                }
            }
            while (pressedKeyInfo.Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Get parameters for reading the movie files and execute it.
        /// </summary>
        public static void MenuReadMoviesRecursive()
        {
            DirectoryInfo directory;
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

            ReadMovies(directory);
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
                    Console.WriteLine("Error reading directory " + subdir.FullName + ": " + ex.Message);
                }
            }

            var files = directory.EnumerateFiles().Where(f => IsMovieFile(f));
            foreach (var file in files)
            {
                Console.WriteLine(file.FullName);
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
