using System;
using CubesFramework.Security;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
namespace Activator
{
    class  Program
    {
        static void Main(string[] args)
        {
            //BFEBFB - FF0003 - 06C384 - 83D983 - 7697A0 - E7VIQY - 0Y1
            char[] charsToTrim = { '*', ' ', '\'' };
            Crypto crypto = new Crypto(System.Security.Cryptography.MD5.Create());
            var encryptedtxt = crypto.EncryptAes("Amir loves Cubes", "@m!r506080").Result;
            Console.WriteLine($"Encrypted Text : {encryptedtxt}");
            var plaintxt = crypto.DecryptAes(encryptedtxt, "@m!r506080");
            Console.WriteLine($"Plain Text : {plaintxt.Result}");
            var bb = "BFEBFB-FF0003-06C384-83D983-7697A0-E7VIQY-0Y1".Replace("-", string.Empty);
            var tt = bb.Trim(charsToTrim);
            License license = new License();
            //license.SaveLicenseToFile("Cubes.txt");
            //Console.WriteLine(crypto.HashText(tt));
            license.GenerateLicense("cubes2021".ToUpper()).Wait();
            Console.WriteLine(license.GeneratedLicense);
            Console.Read();
        }
    }
}
