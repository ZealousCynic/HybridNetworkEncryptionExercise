using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricEncryptionExercise.Encryptor
{
    class RSAXML
    {
        //I miss my purple DEFINES
        const int BITLENGTH = 2096;

        //Save to 'different' locations
        const string pubpath = "/public/";
        const string pubname = "publickey.xml";

        string baseDirectory;

        string[] filepaths;
        string[] dirpaths;


        RSAParameters _pubKey;

        public byte[] Modulus { get { return _pubKey.Modulus; } }
        public byte[] Exponent { get { return _pubKey.Exponent; } }

        public RSAXML(string path = ".")
        {
            baseDirectory = BaseDirectory.GetBaseDirectoryWinFormat();

            dirpaths = new string[] {
                baseDirectory + path + pubpath,
                path + pubpath,
            };

            filepaths = new string[] {
                baseDirectory + path + pubpath + pubname,
                path + pubpath + pubname,
            };

            if (Modulus is null) //Dumb check because I added the option to manually insert public key data later
                GetKey(baseDirectory + path);
        }

        public RSAXML(byte[] modulus, byte[] exponent, string path = ".") : this(path)
        {
            _pubKey.Modulus = modulus;
            _pubKey.Exponent = exponent;
        }


        public byte[] Encrypt(byte[] data)
        {
            //Since I'm writing this to read the keys from XML I'll be disabling key persist everywhere.
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(BITLENGTH))
            {
                rsa.PersistKeyInCsp = false;

                rsa.ImportParameters(_pubKey);

                return rsa.Encrypt(data, true);
            }
        }

        /// <summary>
        /// Gets keys from xml file -- if it doesn't exist, it creates it.
        /// </summary>
        /// <param name="path"></param>
        void GetKey(string path)
        {
            //Check if key exists
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
            }
        }
    }
}
