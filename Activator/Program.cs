using System;
using CubesFramework.Security;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using CubesFramework.SystemManagement;
using System.Security.Cryptography;
using System.IO;
using System.Management;
using System.Collections.Generic;
using CubesFramework.Extensions;
using System.Text.Json;

namespace Activator
{
    class  Program
    {
        static void Main(string[] args)
        {
            var crypto = new Crypto(SHA256.Create());
            var regimanager = new RegistryDataManager();
            License license = new License(crypto, regimanager);
            license.GenerateLicense(HardwareInfo.GetDeviceDataAsSerial(), "cubes2021").Wait();
            var res = license.CheckLicense(license.GeneratedLicense, "cubes2021", HardwareInfo.GetDeviceDataAsSerial());
            license.SaveLicense(license.GeneratedLicense, HardwareInfo.GetDeviceDataAsSerial(), LicenseStorageMethod.ToRegistery).Wait();
            if (license.IsActivated("cubes2021").Result)
            {
                var encmodel = regimanager.LicenseModel;
                var model=JsonSerializer.Deserialize<LicenseModel>(crypto.DecryptAes(encmodel,HardwareInfo.GetDeviceDataAsSerial()).Result);
                Console.WriteLine(license.GeneratedLicense);
            }
            Console.Read();
        }
    }
}
