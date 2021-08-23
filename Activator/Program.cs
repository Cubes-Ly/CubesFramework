using System;
using CubesFramework.Security;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
namespace Activator
{
    class  Program
    {
        static async void Main(string[] args)
        {
            License license = new License();
            await license.GenerateLicense("cubes2021".ToUpper());
            Console.WriteLine(license.GeneratedLicense);
            Console.Read();
        }
    }
}
