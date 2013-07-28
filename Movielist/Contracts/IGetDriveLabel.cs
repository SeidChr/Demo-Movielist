using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movielist.Contracts
{
    public interface IGetDriveLabel
    {
        string Process(FileInfo file);
    }
}
