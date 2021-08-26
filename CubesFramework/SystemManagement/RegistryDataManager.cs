using CubesFramework.Security;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CubesFramework.SystemManagement
{
    public class RegistryDataManager
    {
        public RegistryDataManager()
        {
            Initialize();
        }
        public void Initialize()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\CubesFoundation"))
            {
                //Installation data
                if (key.OpenSubKey("Installation", true)==null)
                {
                    var installationkey = key.CreateSubKey("Installation", true);
                    installationkey.SetValue("InstallationDate", DateTime.Now);
                    installationkey.SetValue("FrameworkBuild", Assembly.GetExecutingAssembly().ImageRuntimeVersion);
                    installationkey.SetValue("LastUpdate", DateTime.MinValue);
                }
                //Activation data

                if (key.OpenSubKey("Activation", true) == null)
                {
                    var activationkey = key.CreateSubKey("Activation", true);
                    activationkey.SetValue("StorageMethod", LicenseStorageMethod.None);
                    activationkey.SetValue("License", "");
                }
            }
        }
        private InstallationData installationData;
        public InstallationData InstallationData
        {
            get
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CubesFoundation\Installation",true))
                {
                    if (key != null)
                    {
                        installationData = new InstallationData
                        {
                            BuildVersion=(string) key.GetValue("FrameworkBuild"),
                            InstalationDate=(DateTime) key.GetValue("InstallationDate"),
                            LastUpdate= (DateTime)key.GetValue("LastUpdate")
                        };
                    }
                    else installationData = null;
                }
                return installationData;
            }
        }
        private LicenseStorageMethod storageMethod;
        public LicenseStorageMethod StorageMethod
        {
            get
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CubesFoundation\Activation", true))
                {
                    return key != null? storageMethod = (LicenseStorageMethod)key.GetValue("StorageMethod")
                        : storageMethod = LicenseStorageMethod.None;
                }
            }
            set
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CubesFoundation\Activation", true))
                {
                    if (key != null)
                    {
                        key.SetValue("StorageMethod",(int) value);
                    }
                }
            }
        }
        private string licenseModel;
        public string LicenseModel
        {
            
            get
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CubesFoundation\Activation", true))
                {
                    return key != null ? licenseModel = (string)key.GetValue("License")
                        : licenseModel = string.Empty;
                }
            }
            set
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CubesFoundation\Activation", true))
                {
                    if (key != null)
                    {
                        key.SetValue("License", value);
                    }
                }
            }
        }

    }
    public class InstallationData
    {
        public DateTime InstalationDate { get; set; }
        public string BuildVersion { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
