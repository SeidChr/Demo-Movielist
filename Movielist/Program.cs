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
            do
            {
                Console.Write("Path: ");
                var path = Console.ReadLine();
                directory = new DirectoryInfo(path);
                if (!directory.Exists)
                {
                    Console.WriteLine("Given Path does not exist.");
                }
            } 
            while (!directory.Exists);

            ReadMovies(directory);
        }

        /// <summary>
        /// Read all movies from a given directory and its subdirectories.
        /// </summary>
        /// <param name="directory">The source directory.</param>
        public static void ReadMovies(params DirectoryInfo[] directories)
        {
            foreach (var directory in directories) 
            {
                ReadMovies(directory.EnumerateDirectories().ToArray());
                var files = directory.EnumerateFiles().Where(f=>IsMovieFile(f));
                foreach (var file in files)
                {
                    Console.WriteLine(file.FullName);
                }
            }
        }

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
