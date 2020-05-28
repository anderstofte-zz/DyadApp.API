using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DyadApp.API.Helpers
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string clearText, string encryptionKey)
        {
            byte[] clearBytes;
            var initVector = new byte[16];

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = initVector;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using var streamWriter = new StreamWriter((Stream)cryptoStream);
                    streamWriter.Write(clearText);

                }
                clearBytes = memoryStream.ToArray();

            }

            return Convert.ToBase64String(clearBytes);
        }

        public static string Decrypt(string cipherText, string encryptionKey)
        {
            var initVector = new byte[16];
            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = initVector;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader((Stream)cryptoStream);
            return streamReader.ReadToEnd();
        }

        public static EncryptWithSaltModel EncryptWithSalt(string clearText)
        {
            var salt = new byte[16];
            using (var provider = new RNGCryptoServiceProvider())
                provider.GetBytes(salt);

            var hashedPasswordAndSalt = new Rfc2898DeriveBytes(clearText, salt, 10000);

            var hash = hashedPasswordAndSalt.GetBytes(20);

            var hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return new EncryptWithSaltModel
            {
                Text = Convert.ToBase64String(hashBytes),
                Salt = Convert.ToBase64String(salt)
            };
        }

    }

    public class EncryptWithSaltModel
    {
        public string Text { get; set; }
        public string Salt { get; set; }
    }
}
