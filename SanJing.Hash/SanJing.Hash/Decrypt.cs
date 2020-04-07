using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SanJing.Hash
{
    public sealed class Decrypt
    {
        /// <summary>
        /// 加密初始化向量
        /// </summary>
        private static readonly byte[] _iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text">待解密文本</param>
        /// <param name="key">密钥(16位)</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string AES128(string text, string key = "csharp.37www.com", string encoding = "UTF-8")
        {
            byte[] encryptedData = Convert.FromBase64String(text);
            return Encoding.GetEncoding(encoding).GetString(AES128(encryptedData, key, encoding));
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="bytes">待解密二进制数据</param>
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
                byte[] encryptedData = bytes;
                rijndaelCipher.IV = _iv;
                byte[] ivBytes = Encoding.GetEncoding(encoding).GetBytes(key);
                rijndaelCipher.Key = ivBytes;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                return transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }
        }

        /// <summary>
        /// Rsa验签 1024bit
        /// </summary>
        /// <param name="sign">签名</param>
        /// <param name="content">内容</param>
        /// <param name="xmlPubKey">公钥|xml格式</param>
        /// <param name="hash">类型|MD5|SHA1</param>
        /// <param name="keySize">长度|1024|2048</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static bool RsaSignVerify(string sign, string content, string xmlPubKey, string hash = "MD5", int keySize = 1024, string encoding = "utf-8")
        {
            byte[] btContent = Encoding.GetEncoding(encoding).GetBytes(content);
            byte[] btSign = Encoding.GetEncoding(encoding).GetBytes(sign);
            RSACryptoServiceProvider rsp = new RSACryptoServiceProvider(keySize);
            rsp.FromXmlString(xmlPubKey);
            return rsp.VerifyData(btContent, hash, btSign);
        }
    }
}
