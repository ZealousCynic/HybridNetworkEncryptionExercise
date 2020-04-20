using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricEncryptionExercise.Encryptor
{
    class Printer
    {
        string baseDirectory = "";
        public Printer()
        {
            baseDirectory = BaseDirectory.GetBaseDirectoryWinFormat();
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
