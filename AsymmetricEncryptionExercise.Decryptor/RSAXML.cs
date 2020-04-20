using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricEncryptionExercise.Decryptor
{
    class RSAXML
    {
        //I miss my purple DEFINES
        const int BITLENGTH = 2096;

        //Save to 'different' locations
        const string pubpath = "/public/";
        const string pubname = "publickey.xml";
        const string pripath = "/private/";
        const string priname = "privatekey.xml";

        string baseDirectory;

        string[] filepaths;
        string[] dirpaths;


        RSAParameters _pubKey;
        RSAParameters _priKey;

        public byte[] Modulus { get { return _priKey.Modulus; } }
        public byte[] Exponent { get { return _priKey.Exponent; } }
        public byte[] P { get { return _priKey.P; } }
        public byte[] Q { get { return _priKey.Q; } }
        public byte[] D { get { return _priKey.D; } }
        public byte[] DP { get { return _priKey.DP; } }
        public byte[] DQ { get { return _priKey.DQ; } }
        public byte[] InverseQ { get { return _priKey.InverseQ; } }

        public RSAXML(string path = ".")
        {
            baseDirectory = BaseDirectory.GetBaseDirectoryWinFormat();

            dirpaths = new string[] {
                baseDirectory + path + pubpath,
                path + pubpath,
                path + pripath
            };

            filepaths = new string[] {
                baseDirectory + path + pubpath + pubname,
                path + pubpath + pubname,
                path + pripath + priname
            };

            GetKey(path);
        }

        public byte[] Decrypt(byte[] data)
        {
            //Since I'm writing this to read the keys from XML I'll be disabling key persist everywhere.
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(BITLENGTH))
            {
                rsa.PersistKeyInCsp = false;

                rsa.ImportParameters(_pubKey);
                rsa.ImportParameters(_priKey);

                return rsa.Decrypt(data, true);
            }
        }

        void GenerateKeyToXML(string path)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(BITLENGTH))
            {
                rsa.PersistKeyInCsp = false;

                Directory.CreateDirectory(path + pubpath);
                Directory.CreateDirectory(path + pripath);
                Directory.CreateDirectory(baseDirectory + path + pubpath);

                File.WriteAllText(path + pubpath + pubname, rsa.ToXmlString(false));
                File.WriteAllText(baseDirectory + path + pubpath + pubname, rsa.ToXmlString(false));
                File.WriteAllText(path + pripath + priname, rsa.ToXmlString(true));

                _pubKey = rsa.ExportParameters(false);
                _priKey = rsa.ExportParameters(true);
            }
        }

        /// <summary>
        /// Gets keys from xml file -- if it doesn't exist, it creates it.
        /// </summary>
        /// <param name="path"></param>
        void GetKey(string path)
        {
            //Check if key already exists
            for(int i = 0; i < dirpaths.Length; i++)
                if(!Directory.Exists(dirpaths[i]) || !File.Exists(filepaths[i]))
                { 
                    GenerateKeyToXML(path);

                    return;
                }

                ReadKeyFromXML(path);
        }

        void ReadKeyFromXML(string path)
        {
            //Read key from file
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(BITLENGTH))
            {
                rsa.PersistKeyInCsp = false;

                //Set program local parameters rather than constantly reading from file
                string xml = File.ReadAllText(path + pubpath + pubname);
                rsa.FromXmlString(xml);
                _pubKey = rsa.ExportParameters(false);

                xml = File.ReadAllText(path + pripath + priname);
                rsa.FromXmlString(xml);
                _priKey = rsa.ExportParameters(true);
            }
        }
    }
}
