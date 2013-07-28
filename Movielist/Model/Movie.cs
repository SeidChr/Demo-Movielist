using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movielist.Model
{
    public class Movie
    {
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
