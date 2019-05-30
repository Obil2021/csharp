using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SanJing.Hash
{
    public sealed class Encrypt
    {
        /// <summary>
        /// MD5加密（32位）
        /// </summary>
        /// <param name="text">待加密文本</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string MD5(string text, string encoding = "UTF-8")
        {
            using (var sha = new MD5CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
        /// <summary>
        /// SHA1加密（40位）
        /// </summary>
        /// <param name="text">待加密文本</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string SHA1(string text, string encoding = "UTF-8")
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="text">待加密文本</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string SHA256(string text, string encoding = "UTF-8")
        {
            using (var sha = new SHA256CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
        /// <summary>
        /// SHA348加密
        /// </summary>
        /// <param name="text">待加密文本</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string SHA384(string text, string encoding = "UTF-8")
        {
            using (var sha = new SHA384CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="text">待加密文本</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string SHA512(string text, string encoding = "UTF-8")
        {
            using (var sha = new SHA512CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
        /// <summary>
        /// 加密初始化向量
        /// </summary>
        private static readonly byte[] _iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">待加密文本</param>
        /// <param name="key">密钥(16位)</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string AES128(string text, string key = "1991060120210601", string encoding = "UTF-8")
        {
            byte[] plainText = Encoding.GetEncoding(encoding).GetBytes(text);
            return Convert.ToBase64String(AES128(plainText, key, encoding));
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="bytes">待加密二进制数据</param>
        /// <param name="key">密钥(16位)</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        private static byte[] AES128(byte[] bytes, string key = "csharp.37www.com", string encoding = "UTF-8")
        {
            using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
            {
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                rijndaelCipher.IV = _iv;
                byte[] ivBytes = Encoding.GetEncoding(encoding).GetBytes(key);
                rijndaelCipher.Key = ivBytes;
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = bytes;
                return transform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
        }
    }
}
