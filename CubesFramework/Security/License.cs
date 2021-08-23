
/* Unmerged change from project 'CubesFramework (netcoreapp3.1)'
Before:
using System;
After:
using CubesFramework.SystemManagement;
using System;
*/

/* Unmerged change from project 'CubesFramework (net48)'
Before:
using System;
After:
using CubesFramework.SystemManagement;
using System;
*/
using CubesFramework.SystemManagement;
using System.IO;
using System.Threading.Tasks;
namespace CubesFramework.Security
{
    public class License
    {
        private string _license = string.Empty;
        public License()
        {

        }
        public string GeneratedLicense => _license;
        public async void SaveLicenseToFile(string filepath, string serial, string password)
        {
            switch (!string.IsNullOrEmpty(_license))
            {
                case true:
                    File.WriteAllText(filepath, _license);
                    return;
                default:
                    await GenerateLicense(serial,password);
                    File.WriteAllText(filepath, _license);
                    break;
            }
        }
        public async Task<string> GenerateLicense(string serial,string password)
        {
            serial = serial.Replace("-", string.Empty);
            var targettext = serial.Trim();
            Crypto crypto = new Crypto(System.Security.Cryptography.SHA256.Create());
            _license = await crypto.EncryptCse(targettext, password);
            return _license;
        }
        public async Task<bool> CheckLicense(string license, string password, string serial)
        {
            Crypto crypto = new Crypto(System.Security.Cryptography.SHA256.Create());
            var plaintxt = await crypto.DecryptCse(license, password);
            serial=serial.Replace("-", string.Empty);
            return string.Compare(plaintxt, serial, true) == 0;
        }
    }
}
