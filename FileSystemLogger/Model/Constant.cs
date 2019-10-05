using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemLogger.Model
{
    public static class Constant
    {
        public static string SAVE_FILE_NAME { get; } = "setting.json";
        public static string SAVE_FILE_PATH { get; } = Path.Combine(Environment.CurrentDirectory, SAVE_FILE_NAME);
    }
}
