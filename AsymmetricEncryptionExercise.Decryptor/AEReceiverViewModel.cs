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

namespace AsymmetricEncryptionExercise.Decryptor
{
    class AEReceiverViewModel : INotifyPropertyChanged
    {
        string fileChoice = "";
        string fileName = "Encrypted.txt";

        string decryptButtonText = "Decrypt";
        string plainText = "";
        string exponent = "";
        string modulus = "";
        string cipherbytes = "";

        string d, dp, dq, p, q, inverseq;

        string baseDirectory;

        public event PropertyChangedEventHandler PropertyChanged;
        ICommand decryptCommand;
        ICommand chooseFileCommand;

        public ICommand DecryptCommand { get { return decryptCommand; } }
        public ICommand ChooseFileCommand { get { return chooseFileCommand; } }

        #region PROPERTIES

        public string FileChoice
        {
            get { return fileChoice; }
            set
            {
                fileChoice = value;
                OnPropertyChanged("FileChoice");
            }
        }

        public string DencryptButtonText
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

            baseDirectory = BaseDirectory.GetBaseDirectory();

            FileChoice = baseDirectory + fileName;
        }

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void Decrypt(object obj)
        {
            RSAXML rsaxml = new RSAXML();

            CipherBytes = "Done";

            string temp = File.ReadAllText(FileChoice);

            byte[] toDecrypt = Convert.FromBase64String(temp);
            byte[] decrypted = rsaxml.Decrypt(toDecrypt);

            PlainText = Encoding.ASCII.GetString(decrypted);
            Modulus = BuildHexString(rsaxml.Modulus);
            Exponent = BuildHexString(rsaxml.Exponent);

            P = BuildHexString(rsaxml.P);
            Q = BuildHexString(rsaxml.Q);
            D = BuildHexString(rsaxml.D);
            DP = BuildHexString(rsaxml.DP);
            DQ = BuildHexString(rsaxml.DQ);
            InverseQ = BuildHexString(rsaxml.InverseQ);
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
