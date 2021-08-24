
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
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CubesFramework.Security
{
    public enum LicenseStorageMethod
    {
        ToFile=1,
        ToRegistery,
        None
    }
    public class License
    {
        private string _license = string.Empty;
        private readonly Crypto crypto;
        private readonly RegistryDataManager registryDataManager;

        public License(Crypto crypto,
            RegistryDataManager registryDataManager)
        {
            this.crypto = crypto;
            this.registryDataManager = registryDataManager;
        }
        public string GeneratedLicense => _license;
        /// <summary>
        /// Runs generate license method if it wasn't and save the generated license to a file
        /// </summary>
        /// <param name="filepath">the target file path</param>
        /// <param name="serial">device serial</param>
        /// <param name="password">license key</param>
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
        /// <summary>
        /// Generates a license based on the passed serail and password 
        /// </summary>
        /// <param name="serial">the target device serial</param>
        /// <param name="password">encryption key</param>
        /// <returns>the generated license</returns>
        public async Task<string> GenerateLicense(string serial,string password)
        {
            serial = serial.Replace("-", string.Empty);
            var targettext = serial.Trim();
            _license = await crypto.EncryptCse(targettext, password);
            return _license;
        }
        /// <summary>
        /// Check whether a license was generated from the passed serial and password or not
        /// </summary>
        /// <param name="license">license to be checked</param>
        /// <param name="password">license key</param>
        /// <param name="serial">device serial</param>
        /// <returns>True if the license matches the password and the serial,otherwise flase</returns>
        public async Task<bool> CheckLicense(string license, string password, string serial)
        {
            var plaintxt = await crypto.DecryptCse(license, password);
            serial=serial.Replace("-", string.Empty);
            return string.Compare(plaintxt, serial, true) == 0;
        }
        /// <summary>
        /// Saves the generated license to a place
        /// </summary>
        /// <param name="license">generated license</param>
        /// <param name="serail">device serial</param>
        /// <param name="storageMethod">storage method</param>
        /// <param name="filepath">file path for file storage method</param>
        /// <returns>The stored license model</returns>
        public async Task<LicenseModel> SaveLicense(string license,string serail, LicenseStorageMethod storageMethod,string filepath="license.enc")
        {
            var licensemodel = new LicenseModel()
            {
                DeviceSerial=serail,
                ActivationSerial=license,
                StorageMethod=storageMethod
            };
            var json = JsonConvert.SerializeObject(licensemodel);
            var encjson = await crypto.EncryptCse(json, serail);
            switch (storageMethod)
            {
                case LicenseStorageMethod.ToFile:

                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }
                    File.WriteAllText(filepath, encjson);
                    registryDataManager.StorageMethod = LicenseStorageMethod.ToFile;
                    break;
                case LicenseStorageMethod.ToRegistery:
                    registryDataManager.LicenseModel = encjson;
                    registryDataManager.StorageMethod = LicenseStorageMethod.ToRegistery;
                    break;
            }
            return licensemodel;
        }
        /// <summary>
        /// Gives the client a quick results about the license status
        /// </summary>
        /// <param name="password">key password</param>
        /// <param name="filepath">file path for file storage method</param>
        /// <returns>True when the current version of client software was activated ,otherwise false</returns>
        public async Task<bool> IsActivated(string password,string filepath = "license.enc")
        {
            bool activated= false;
            switch (registryDataManager.StorageMethod)
            {
                case LicenseStorageMethod.ToFile:
                    if (File.Exists(filepath))
                    {
                        var enclicense=File.ReadAllText(filepath);
                        try
                        {
                            var filemodel = JsonConvert.DeserializeObject<LicenseModel>(await crypto.DecryptCse(enclicense, HardwareInfo.GetDeviceDataAsSerial(new HardwareDeviceData())));
                            activated = await CheckLicense(filemodel.ActivationSerial, password, filemodel.DeviceSerial);
                        }
                        catch 
                        {
                            activated = false;
                        }
                    }
                    break;
                case LicenseStorageMethod.ToRegistery:
                    var encmodel = registryDataManager.LicenseModel;
                    try
                    {
                        var registrymodel = JsonConvert.DeserializeObject<LicenseModel>(await crypto.DecryptCse(encmodel, HardwareInfo.GetDeviceDataAsSerial(new HardwareDeviceData())));
                        activated = await CheckLicense(registrymodel.ActivationSerial, password, registrymodel.DeviceSerial);
                    }
                    catch
                    {
                        activated = false;
                    }
                    break;
                case LicenseStorageMethod.None:
                    activated = false;
                    break;
            }
            return activated;
        }
    }
    public class LicenseModel
    {
        public DateTime ActivationDate { get; set; }=DateTime.Now;
        public string DeviceSerial { get; set; }
        public string ActivationSerial { get; set; }
        public LicenseStorageMethod StorageMethod { get; set; }
    }
}
