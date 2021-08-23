using System.Security.Cryptography;
using System
/* Unmerged change from project 'CubesFramework (netcoreapp3.1)'
Before:
using System.Text;
After:
using System.Security.Cryptography;
*/

/* Unmerged change from project 'CubesFramework (net48)'
Before:
using System.Text;
After:
using System.Security.Cryptography;
*/
.Text;
namespace CubesFramework.Security
{
    /// <summary>
    /// Represnets the base properties for any hashing provider
    /// </summary>
    internal interface IHashingService
    {
        string Message { get; set; }

        string SaltValue { get; set; }

        HashAlgorithm Algorithm { get; }

        byte[] HashMessage();
        byte[] HashMessage(string msg);
        string FilterHashed(params char[] values);

    }
}
