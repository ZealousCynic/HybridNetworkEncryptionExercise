using System.IO;

namespace CommonClasses
{
    public class BaseDirectory
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
