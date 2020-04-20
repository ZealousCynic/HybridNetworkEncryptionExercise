using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsymmetricEncryptionExercise.Encryptor
{
    class AESenderViewModel : INotifyPropertyChanged
    {
        RSAXML rsaxml;

        ICommand encryptCommand;
        ICommand chooseFileCommand;
        string encryptButtonText = "Encrypt";
        string plainText = "";
        string exponent = "";
        string modulus = "";
        string cipherbytes = "";

        string fileChoice = "";
        string fileName = "Encrypted.txt";
        string baseDirectory;

        public event PropertyChangedEventHandler PropertyChanged;

        #region PROPERTIES
        public ICommand EncryptCommand { get { return encryptCommand; } }
        public ICommand ChooseFileCommand { get { return chooseFileCommand; } }

        public string FileChoice
        {
            get { return fileChoice; }
            set
            {
                fileChoice = value;
                OnPropertyChanged("FileChoice");
            }
        }

        public string EncryptButtonText
        {
            get { return encryptButtonText; }
            set
            {
                encryptButtonText = value;
                OnPropertyChanged("EncryptButtonText");
            }
        }

        public string PlainText
        {
            get { return plainText; }
            set
            {
                plainText = value;
                OnPropertyChanged("PlainText");
            }
        }

        public string Exponent
        {
            get { return exponent; }
            set
            {
                exponent = value;
                OnPropertyChanged("Exponent");
            }
        }

        public string Modulus
        {
            get { return modulus; }
            set
            {
                modulus = value;
                OnPropertyChanged("Modulus");
            }
        }


        public string CipherBytes
        {
            get { return cipherbytes; }
            set
            {
                cipherbytes = value;
                OnPropertyChanged("CipherBytes");
            }
        }

        #endregion

        public AESenderViewModel()
        {
            encryptCommand = new DelegateCommand(Encrypt);
            chooseFileCommand = new DelegateCommand(ChooseFile);

            baseDirectory = BaseDirectory.GetBaseDirectory();

            FileChoice = baseDirectory + fileName;
        }

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void Encrypt(object obj)
        {
            EncryptButtonText = "Working...";

            rsaxml = new RSAXML();
            Printer printer = new Printer();

            if (string.IsNullOrEmpty(PlainText))
                PlainText = "I BE TEXT";

            byte[] toEncrypt = Encoding.ASCII.GetBytes(PlainText);

            byte[] encrypted = rsaxml.Encrypt(toEncrypt);

            CipherBytes = Convert.ToBase64String(encrypted);

            Modulus = BuildHexString(rsaxml.Modulus);
            Exponent = BuildHexString(rsaxml.Exponent);            

            printer.SaveToFile(CipherBytes, fileName);

            EncryptButtonText = "Success!";
        }

        public void ChooseFile(object obj)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.InitialDirectory = baseDirectory;


            if (ofd.ShowDialog() == true)
            {
                FileChoice = ofd.FileName;
                fileName = ofd.SafeFileName;
            }
        }

        string BuildHexString(byte[] src) 
        {
            StringBuilder sb = new StringBuilder(src.Length * 2);
            for (int i = 0; i < src.Length; i++)
            {
                sb.AppendFormat("{0:X2}", src[i]);

                if (i != src.Length - 1)
                    sb.Append("-");
            }
            return sb.ToString();
        }
    }
}
