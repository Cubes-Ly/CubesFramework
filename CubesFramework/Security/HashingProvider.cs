using System;
using System.Security.Cryptography;
using System.Text;
using Unity;
namespace CubesFramework.Security
{
    internal sealed class HashingProvider : IHashingService
    {
        [InjectionConstructor]
        public HashingProvider(HashAlgorithm algorithm)
        {
            Algorithm = algorithm;
        }
        public HashingProvider(string message, HashAlgorithm algorithm)
        {
            Message = message;
            Algorithm = algorithm;
        }
        public string Message { get; set; }
        public HashAlgorithm Algorithm { get; }
        public string SaltValue { get; set; }

        /// <summary>
        /// Hashing the message's value after concatinating it with salt's value
        /// </summary>
        /// <returns>the hashed bytes</returns>
        public byte[] HashMessage() =>
             //converting the plaintext to bytes and compute the bytes arraies values
             Algorithm.ComputeHash(
                Encoding.UTF8.GetBytes(
                    string.Concat(
                        SaltValue.ToLower(),
                        Message,
                        SaltValue.ToUpper()
                        )));
        /// <summary>
        /// Clean the hashed text from the given characters
        /// </summary>
        /// <param name="values">Target characters to be removed from the hashed text</param>
        /// <returns>Cleaned hash text</returns>
        public string FilterHashed(params char[] values) =>
            BitConverter.ToString(HashMessage()).Trim(values);
        /// <summary>
        /// Set new message and hash it
        /// </summary>
        /// <param name="msg">target message</param>
        /// <returns>the hashed bytes</returns>
        public byte[] HashMessage(string msg)
        {
            Message = msg;
            return HashMessage();
        }
    }
}
