using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Movielist
{
    public class GetVolumeLabel
    {

        private Dictionary<string, string> labelStore = new Dictionary<string, string>();
        private bool initialized;

        public string Process(FileInfo file)
        {
            string result = string.Empty;

            if (!initialized)
            {
                Initialize();
            }

            return result;
        }

        private void Initialize()
        {
            ManagementObjectSearcher searcher = new
            ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach(ManagementObject wmi_HD in searcher.Get())
            {
                labelStore["x"] = "y";
            }
         
        }
    }
}
