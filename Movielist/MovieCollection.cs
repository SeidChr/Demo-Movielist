using Movielist.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XEO.Core.Text;

namespace Movielist
{
    public class MovieCollection : IEnumerable<Movie>
    {
        private Dictionary<string, Movie> movieStore = new Dictionary<string, Movie>();

        public void Add(FileInfo file, string location)
        {
            var movie = new Movie();
            movie.Location = location;
            var match = Regex.Match(file.Name, @"^(?<name>.*?)\s*\[(?<tag>[^,]*)(,\s?(?<tag>[^,]*))*\]\.\w+$");
            movie.Name = match.Groups["name"].Value;
            movie.Tags = match.Groups["tag"].Captures.Cast<Capture>().Select(c => c.Value).ToList();
            string key = location + file.FullName.Substring(1);

            movieStore.Add(key, movie);
        }

        public IEnumerator<Movie> GetEnumerator()
        {
            return movieStore.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return movieStore.Values.GetEnumerator();
        }
    }
}
