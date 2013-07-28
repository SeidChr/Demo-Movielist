using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Movielist
{
    static class Config
    {
        private static IEnumerable<string> endings;

        static Config()
        {
            var configDocument = XDocument.Load("Config.xml");
            // configDocument.Root.Element("Config").Element("Extensions").Elements()
            endings = configDocument
                .Root
                .Element("Config")
                .Element("Extensions")
                .Elements("Extension")
                .Select(e => e.Value);
        }

        public static IEnumerable<string> Endings
        {
            get 
            {
                return endings;
            }
        }
    }
}
