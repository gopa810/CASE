using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CASE
{
    public class AppServant
    {
        public static string MainDirectory
        {
            get
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                folder = Path.Combine(folder, "CASE");
                Directory.CreateDirectory(folder);                
                return folder;
            }
        }

        public static string ProjectsDirectory
        {
            get
            {
                string temp = Path.Combine(MainDirectory, "Projects");
                Directory.CreateDirectory(temp);
                return temp;
            }
        }

        /// <summary>
        /// Gets unique file name in the given directory. Returns name of file which 
        /// consists from (prefix)(number)(extension)
        /// </summary>
        /// <param name="directory">full path of directory</param>
        /// <param name="filePrefix">prefix for file</param>
        /// <param name="extension">suffix for file, can contain extension. If extension is given, then dot must be at the beginning of this string</param>
        /// <returns>unique name for new file</returns>
        public static string GetUniqueFileName(string directory, string filePrefix, string extension)
        {
            for (int i = 0; i < 1000; i++)
            {
                string fileName = string.Format("{0}{1}{2}", filePrefix, i, extension);
                if (!File.Exists(Path.Combine(directory, fileName)))
                    return Path.Combine(directory, fileName);
            }
            return string.Empty;
        }
    }
}
