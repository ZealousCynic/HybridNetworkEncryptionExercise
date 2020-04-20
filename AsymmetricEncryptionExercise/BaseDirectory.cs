using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricEncryptionExercise
{
    public static class BaseDirectory
    {
        static string baseDirectory = System.IO.Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
        static string baseDirectoryWinFormat = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\");

        public static string GetBaseDirectory()
        {
            return baseDirectory;
        }

        public static string GetBaseDirectoryWinFormat()
        {
            return baseDirectoryWinFormat;
        }
    }
}
