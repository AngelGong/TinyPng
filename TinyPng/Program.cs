using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace TinyPng
{
    class Program
    {
        static void Main()
        {
            FileContext context = new FileContext("D:\\workspace\\res\\IOS\\320x480\\","D:\\workspace\\TiniedPng\\IOS\\320x480\\");
            context.tinyPngFiles();
            Console.Read();
        }

        public static void test()
        {
            
        }
    }
}
