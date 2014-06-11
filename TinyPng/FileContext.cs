using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace TinyPng
{
    public class FileContext
    {
        public FileContext() { }
        public FileContext(string input, string output)
        {
            Input = input;
            Output = output;
        }
        private string _input;
        /// <summary>
        /// 
        /// </summary>
        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                _input = value;
            }
        }

        private string _output;
        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
            }
        }

        public string[] getSubDirs(string path)
        {
            return Directory.GetDirectories(path);
        }

        public string[] getSubFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public string getMD5ofFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            // Convert the input string to a byte array and compute the hash.
            byte[] data = MD5.Create().ComputeHash(bytes);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(string hashOfInput, string hash)
        {
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void clearTiniedPngs()
        {
            string[] files = getSubFiles(Output);
            if (files.Length <= 0)
                return;
            foreach (var file in files)
            {
                string iPath = Input+getFileName(file);
                if (File.Exists(iPath))
                {
                    string omd5 = getMD5ofFile(file);
                    string imd5 = getMD5ofFile(Input + getFileName(file));
                    if (VerifyMd5Hash(omd5, imd5))
                    {
                        Console.WriteLine("Tinied:{0}",iPath);
                        File.Delete(iPath);
                    }
                }
            }
        }

        public void tinyPngFiles()
        {
            clearTiniedPngs();
            string[] files = getSubFiles(Input);
            foreach(var file in files)
            {
                if (file.EndsWith(".png") || file.EndsWith(".PNG"))
                {
                    CompressPng.getInstance().tinyPng(file,Output+getFileName(file));
                }
            }
            tinyPngFilesInSubDir();
        }

        public void tinyPngFilesInSubDir()
        {
            string subOutput = "";
            string subInput = "";
            string[] subDirs = getSubDirs(Input);
            if (subDirs.Length <= 0)
                return;
            foreach (string dir in subDirs)
            {
                subOutput = getOutputofInput(dir);
                subInput = dir;
                if (!File.Exists(subOutput))
                    Directory.CreateDirectory(subOutput);
                if (!dir.EndsWith("\\")) subInput = subInput + "\\";
                new FileContext(subInput, subOutput).tinyPngFiles();
            }
        }

        private string getOutputofInput(string input)
        {
            string name = getFileName(input);
            return Output + name + "\\";
        }

        private string getFileName(string path)
        {
            string[] paths = path.Split('\\');
            return paths[paths.Length - 1];
        }
    }
}
