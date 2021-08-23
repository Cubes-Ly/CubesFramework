
/* Unmerged change from project 'CubesFramework (netcoreapp3.1)'
Before:
using System;
After:
using PrgramsSecurity;
using System;
*/

/* Unmerged change from project 'CubesFramework (net48)'
Before:
using System;
After:
using PrgramsSecurity;
using System;
*/
using PrgramsSecurity;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.Resolution;
namespace CubesFramework.Security
{
    /// <summary>
    /// Presents set of hashing and encryption methods based on the passed algorithm
    /// </summary>
    public class Crypto
    {
        private readonly HashAlgorithm algorithm;
        private byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private int BlockSize = 128;
        /// <summary>
        /// Objects constructor
        /// </summary>
        /// <param name="algorithm">the target algorithm</param>
        public Crypto(HashAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public string AesIV => Encoding.UTF8.GetString(IV);

        /// <summary>
        /// Generates new text based on the passed plaintext and salttex and hash it 
        /// </summary>
        /// <param name="text">target plaintext</param>
        /// <param name="salttext" defualtvalue="50666RR@#$WWWDDFF">target salttext</param>
        /// <returns>the hashed text</returns>
        public string HashText(string text, string salttext = "50666RR@#$WWWDDFF")
        {
            try
            {
                var hasher = (
                              IHashingService)ContainerManager.Resolve(typeof(IHashingService),
                              new DependencyOverride<HashAlgorithm>(algorithm)
                              );
                hasher.Message = text;
                hasher.SaltValue = salttext;
                return BitConverter.ToString(hasher.HashMessage());
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// Generates new text based on the passed plaintext and salttex and hash it (async)
        /// </summary>
        /// <param name="text">target plaintext</param>
        /// <param name="salttext" defualtvalue="50666RR@#$WWWDDFF">target salttext</param>
        /// <returns>the hashed text</returns>
        public async Task<string> HashTextAsync(string text, string salttext = "50666RR@#$WWWDDFF")
        {
            var hasher = (
                IHashingService)ContainerManager.Resolve(typeof(IHashingService),
                new DependencyOverride<HashAlgorithm>(algorithm)
                );
            hasher.Message = text;
            hasher.SaltValue = salttext;
            return await Task.Run(() => BitConverter.ToString(hasher.HashMessage()));
        }
        /// <summary>
        /// Set confidurations of the aes object
        /// </summary>
        /// <param name="password">target key</param>
        private byte[] GetKey(string password)
        {
            var keyBytes = Encoding.UTF8.GetBytes(password);
            return MD5.Create().ComputeHash(keyBytes);
        }

        /// <summary>
        /// Encrypting a text using the aes algorithm 
        /// </summary>
        /// <param name="text">target text</param>
        /// <param name="key">cipher key</param>
        /// <param name="paddingMode">cipher padding mode</param>
        /// <returns>encrypted text</returns>
        public Task<string> EncryptAes(string text, string key)
        {
            using (var cipher = Aes.Create())
            {
                cipher.BlockSize = BlockSize;
                cipher.IV = IV;
                cipher.Key = GetKey(key);
                ICryptoTransform Encryptor = cipher.CreateEncryptor();
                var textbytes = Encoding.UTF8.GetBytes(text);
                var ciphertex = Encryptor.TransformFinalBlock(textbytes, 0, textbytes.Length);
                return Task.Run(() => Convert.ToBase64String(ciphertex));
            }

        }
        /// <summary>
        /// Encrypting a text using the aes algorithm 
        /// </summary>
        /// <param name="text">target text</param>
        /// <param name="key">out cipher key</param>
        /// <param name="iv">out cipher iv</param>
        /// <returns>encrypted text</returns>
        public Task<string> EncryptAes(string text, out string key, out string iv)
        {
            using (var cipher = Aes.Create())
            {
                cipher.BlockSize = BlockSize;
                cipher.GenerateKey();
                cipher.GenerateIV();
                ICryptoTransform Encryptor = cipher.CreateEncryptor();
                var textbytes = Encoding.UTF8.GetBytes(text);
                var ciphertex = Encryptor.TransformFinalBlock(textbytes, 0, textbytes.Length);
                key = Encoding.UTF8.GetString(cipher.Key);
                iv = Encoding.UTF8.GetString(cipher.IV);
                return Task.Run(() => Convert.ToBase64String(ciphertex));
            }

        }
        /// <summary>
        /// Encrypting a text using custom symmetric encryption algorithm
        /// </summary>
        /// <param name="text">target text</param>
        /// <param name="key">target key</param>
        /// <returns>encrypted text</returns>
        public Task<string> EncryptCse(string text, string key) => Task.Run(() => AccountsSecurity.Encrypt(text, key));
        /// <summary>
        /// Decrypting an encrypted text using aes algorithm
        /// </summary>
        /// <param name="encryptedtext">target encryptedtext</param>
        /// <param name="key">password key</param>
        /// <returns>readable plain text</returns>
        public Task<string> DecryptAes(string encryptedtext, string key)
        {
            using (var cipher = Aes.Create())
            {
                cipher.BlockSize = BlockSize;
                cipher.IV = IV;
                cipher.Key = GetKey(key);
                ICryptoTransform Decryptor = cipher.CreateDecryptor();
                var ciphertext = Convert.FromBase64String(encryptedtext);
                var plaintext = Decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                return Task.Run(() => Encoding.UTF8.GetString(plaintext));
            }
        }
        /// <summary>
        /// Decrypting an encrypted text using custom symmetric encryption algorithm
        /// </summary>
        /// <param name="text">target encryptedtext</param>
        /// <param name="key">password key</param>
        /// <returns>readable plain text</returns>
        public Task<String> DecryptCse(string text, string key) => Task.Run(() => AccountsSecurity.Decrypt(text, key));
    }
}
