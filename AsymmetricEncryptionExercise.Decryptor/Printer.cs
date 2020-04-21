using System.IO;
using BaseDirectory;

namespace AsymmetricEncryptionExercise.Decryptor
{
    class Printer
    {
        string baseDirectory = "";
        public Printer()
        {
            baseDirectory = Base.GetBaseDirectoryWinFormat();
        }
        public void SaveToFile(string data, string name, string path = "./")
        {
            CheckForFile(baseDirectory + path, name);
            File.WriteAllText(baseDirectory + path + name, data);
        }

        void CheckForFile(string path, string name)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
