using Encryption.RSA;
using CommonClasses;
using CommonClasses.Wpf;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace AsymmetricEncryptionExercise.Decryptor
{
    class AEReceiverViewModel : INotifyPropertyChanged
    {
        RSAXML rsaxml;

        bool manual = false;

        public event PropertyChangedEventHandler PropertyChanged;

        #region STRINGS

        string fileChoice = "";
        string fileName = "Encrypted.txt";

        string decryptButtonText = "Decrypt";
        string plainText = "";
        string exponent = "";
        string modulus = "";
        string cipherbytes = "";
        string newKeys = "Generate New Keypair";

        string d, dp, dq, p, q, inverseq;

        string baseDirectory;

        #endregion

        #region COMMANDS

        ICommand decryptCommand;
        ICommand showKeyCommand;
        ICommand chooseFileCommand;
        ICommand generateKeysCommand;

        public ICommand DecryptCommand { get { return decryptCommand; } }
        public ICommand ShowKeyCommand { get { return showKeyCommand; } }
        public ICommand ChooseFileCommand { get { return chooseFileCommand; } }
        public ICommand GenerateKeysCommand { get { return generateKeysCommand; } }

        #endregion

        #region PROPERTIES

        public bool Manual
        {
            get { return manual; }
            set
            {
                manual = value;
                OnPropertyChanged("Manual");
            }
        }

        public string NewKeys
        {
            get { return newKeys; }
            set
            {
                newKeys = value;
                OnPropertyChanged("NewKeys");
            }
        }

        public string FileChoice
        {
            get { return fileChoice; }
            set
            {
                fileChoice = value;
                OnPropertyChanged("FileChoice");
            }
        }

        public string DecryptButtonText
        {
            get { return decryptButtonText; }
            set
            {
                decryptButtonText = value;
                OnPropertyChanged("DecryptButtonText");
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

        public string D
        {
            get { return d; }
            set
            {
                d = value;
                OnPropertyChanged("D");
            }
        }

        public string Q
        {
            get { return q; }
            set
            {
                q = value;
                OnPropertyChanged("Q");
            }
        }

        public string DP
        {
            get { return dp; }
            set
            {
                dp = value;
                OnPropertyChanged("DP");
            }
        }

        public string DQ
        {
            get { return dq; }
            set
            {
                dq = value;
                OnPropertyChanged("DQ");
            }
        }

        public string P
        {
            get { return p; }
            set
            {
                p = value;
                OnPropertyChanged("P");
            }
        }

        public string InverseQ
        {
            get { return inverseq; }
            set
            {
                inverseq = value;
                OnPropertyChanged("InverseQ");
            }
        }

        #endregion

        public AEReceiverViewModel()
        {
            decryptCommand = new DelegateCommand(Decrypt);
            chooseFileCommand = new DelegateCommand(ChooseFile);
            generateKeysCommand = new DelegateCommand(GenerateKeys);
            showKeyCommand = new DelegateCommand(ShowKey);

            baseDirectory = BaseDirectory.GetBaseDirectory();

            FileChoice = baseDirectory + fileName;
        }

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        void Decrypt(object obj)
        {
            if (rsaxml is null)
                rsaxml = new RSAXML();

            ShowKey(obj);

            CipherBytes = "Done";

            string temp = File.ReadAllText(FileChoice);

            byte[] toDecrypt = Convert.FromBase64String(temp);
            byte[] decrypted = rsaxml.Decrypt(toDecrypt);

            PlainText = Encoding.ASCII.GetString(decrypted);
        }

        void ChooseFile(object obj)
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

        void GenerateKeys(object obj)
        {
            NewKeys = "Working...";

            if (rsaxml is null)
                rsaxml = new RSAXML();

            rsaxml.GenerateKeyToXML("./Keys");

            NewKeys = "Success";
        }

        void ShowKey(object obj)
        {
            if (rsaxml is null)
                rsaxml = new RSAXML();

            if (Manual)
            {
                Modulus = Convert.ToBase64String(rsaxml.Modulus);
                Exponent = Convert.ToBase64String(rsaxml.Exponent);
            }
            else
            {
                Modulus = BuildHexString(rsaxml.Modulus);
                Exponent = BuildHexString(rsaxml.Exponent);
            }

            P = BuildHexString(rsaxml.P);
            Q = BuildHexString(rsaxml.Q);
            D = BuildHexString(rsaxml.D);
            DP = BuildHexString(rsaxml.DP);
            DQ = BuildHexString(rsaxml.DQ);
            InverseQ = BuildHexString(rsaxml.InverseQ);
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
