
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
        public async void SaveLicenseToFile(string filepath)
        {
            switch (!string.IsNullOrEmpty(_license))
            {
                case true:
                    File.WriteAllText(filepath, _license);
                    return;
                default:
                    await GenerateLicense("20Cubes87234909SoftwareCompany");
                    File.WriteAllText(filepath, _license);
                    break;
            }
        }
        public async Task<string> GenerateLicense(string password)
        {
            var Processor = HardwareInfo.GetProcessorId();
            var hardserialnumber = HardwareInfo.GetHDDSerialNo();
            var BoardProductId = HardwareInfo.GetBoardProductId();
            var targettext = $"{Processor}{hardserialnumber}{BoardProductId}".Trim();
            Crypto crypto = new Crypto(System.Security.Cryptography.MD5.Create());
            _license = await crypto.EncryptCse(targettext, password);
            return _license;
        }


    }
}
