using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movielist.Contracts
{
    public interface IMovie
    {
        string Name { get; }
        IEnumerable<string> Tags { get; }
    }
}
