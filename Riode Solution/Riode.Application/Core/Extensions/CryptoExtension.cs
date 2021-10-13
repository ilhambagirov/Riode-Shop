using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Riode.Application.Core.Extensions
{
    static public partial class Extension
    {

        public static string Encrypt(this string value, string key)
        {

            try
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"#{key}!"));
                    var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"@{key}$"));

                    var transform = tdes.CreateEncryptor(keyBuffer, ivBuffer);

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        byte[] valueBuffer = Encoding.UTF8.GetBytes(value);
                        cs.Write(valueBuffer, 0, valueBuffer.Length);
                        cs.FlushFinalBlock();

                        ms.Position = 0;

                        byte[] result = new byte[ms.Length];

                        ms.Read(result, 0, result.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
            catch (Exception)
            {

                return "";
            }
        }

        public static string Decrypt(this string value, string key)
        {
            try
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"#{key}!"));
                    var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"@{key}$"));

                    var transform = tdes.CreateDecryptor(keyBuffer, ivBuffer);

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        byte[] valueBuffer = Convert.FromBase64String(value);
                        cs.Write(valueBuffer, 0, valueBuffer.Length);
                        cs.FlushFinalBlock();
                        ms.Position = 0;

                        byte[] result = new byte[ms.Length];

                        ms.Read(result, 0, result.Length);

                        return Encoding.UTF8.GetString(result);
                    }
                }
            }
            catch (Exception)
            {

                return "";
            }
        }
    }
}
