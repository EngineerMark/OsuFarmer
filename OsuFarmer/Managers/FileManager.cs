using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
            //return System.Reflection.Assembly.GetEntryAssembly().Location;
            return AppDomain.CurrentDomain.BaseDirectory;
            // return string.Empty;
        }

        public static string? GetExecutableDirectory()
        {
            return Path.GetDirectoryName(GetExecutablePath());
        }

        public static async Task<bool> WriteFile(string? path, string? data)
        {
            if (path == null || data == null)
                return false;

            string? dir = Path.GetDirectoryName(path);

            if (dir == null)
                return false;

            Directory.CreateDirectory(dir);

            if (File.Exists(path))
                if (FileManager.IsFileLocked(path))
                    return false;
            await File.WriteAllTextAsync(path, String.Empty);
            await File.WriteAllTextAsync(path, data);
            return true;
        }

        public static bool IsFileLocked(string file)
        {
            try
            {
                using (FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
    }
}
