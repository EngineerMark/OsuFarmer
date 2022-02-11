using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Managers
{
    public class FileManager : Manager<FileManager>
    {
        public FileManager()
        {
            Register(this);
        }

        public static string GetExecutablePath()
        {
            return System.Reflection.Assembly.GetEntryAssembly().Location;
        }

        public static string GetExecutableDirectory()
        {
            return Path.GetDirectoryName(GetExecutablePath());
        }
    }
}
