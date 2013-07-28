using Movielist.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movielist
{
    public class MovieCollection : IEnumerable<Movie>
    {
        private Dictionary<string, Movie> movieStore = new Dictionary<string, Movie>();

        public void Add(FileInfo file)
        {
            
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
