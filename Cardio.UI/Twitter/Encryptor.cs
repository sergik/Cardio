using System;
using System.Security.Cryptography;
using System.Text;

namespace Cardio.UI.Twitter
{
	public class Encryptor
	{
	    private readonly string _cryptingKey;

        public Encryptor(string cryptingKey)
        {
            _cryptingKey = cryptingKey;
        }

        public string Encrypt(string source)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(source);
            var hashmd5 = new MD5CryptoServiceProvider();
            byte[] pwdhash = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(_cryptingKey));
            var tdesProvider = new TripleDESCryptoServiceProvider {Key = pwdhash, Mode = CipherMode.ECB};

            String encrypted = Convert.ToBase64String(
               tdesProvider.CreateEncryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            return encrypted;
        }

        public string Decrypt(string encryptedString)
        {
            byte[] inputBytes = Convert.FromBase64String(encryptedString);
            var hashmd5 = new MD5CryptoServiceProvider();
            byte[] pwdhash = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(_cryptingKey));
            var tdesProvider = new TripleDESCryptoServiceProvider
                    {
                        Key = pwdhash,
                        Mode = CipherMode.ECB
                    };

            String decyprted = Encoding.ASCII.GetString(
                tdesProvider.CreateDecryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
            return decyprted;
        }
    }
}
