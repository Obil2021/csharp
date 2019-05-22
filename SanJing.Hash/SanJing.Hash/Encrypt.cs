using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SanJing.Hash
{
    public class Encrypt
    {
        /// <summary>
        /// MD5加密（32位）
        /// </summary>
        /// <param name="text">待加密文本</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string MD5(string text, string encoding = "UTF-8")
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
        public string SHA1(string text, string encoding = "UTF-8")
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
        public string SHA256(string text, string encoding = "UTF-8")
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
        public string SHA384(string text, string encoding = "UTF-8")
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
        public string SHA512(string text, string encoding = "UTF-8")
        {
            using (var sha = new SHA512CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }
        }
    }
}
