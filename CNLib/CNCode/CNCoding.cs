using CNLib.CNMessage;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CNLib.CNCode
{
    /// <summary>
    /// 编码
    /// </summary>
    public class CNCoding : ICNCoding
    {
        private static byte[] key = new byte[]{ 0xAA, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7 };
        private static byte[] iv = new byte[] { 0xCA, 0xC1, 0xC2, 0xC3, 0xC4, 0xC5, 0xA6, 0xA7 };

        /// <summary>
        /// JHS - 2022/01/19
        /// 数据编码
        /// </summary>
        /// <param name="strMsg">编码前信息</param>
        /// <returns>编码后数据</returns>
        public byte[] Encode(string strMsg)
        {
            // 加密字节数组
            byte[] newBuffer = new byte[] { };
            MemoryStream memoryStream = new MemoryStream();       // 内存流
            CryptoStream? cryptoStream = null;

            try
            {
                // 将信息转换为字节数组
                byte[] oldBuffer = Encoding.UTF8.GetBytes(strMsg);

                // 创建数据流并进行加密写入
                memoryStream = new MemoryStream();
                DESCryptoServiceProvider DESalg = new DESCryptoServiceProvider();

                cryptoStream = new CryptoStream(memoryStream, new DESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cryptoStream.Write(oldBuffer, 0, oldBuffer.Length);
                cryptoStream.FlushFinalBlock();

                newBuffer = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw CNLog.NewError("编码失败", ex);
            }
            finally
            {
                try
                {
                    // 关闭流
                    cryptoStream.Close();
                    memoryStream.Close();
                }
                catch (Exception)
                {

                }
            }

            return newBuffer;
        }

        /// <summary>
        /// JHS - 2022/01/19
        /// 数据解码
        /// </summary>
        /// <param name="strMSg">解码前数据</param>
        /// <returns>解码后信息</returns>
        public string Decode(byte[] buffer)
        {
            // 解码字节数组
            byte[] newBuffer = new byte[] { };
            MemoryStream memoryStream = new MemoryStream(); ;
            CryptoStream? cryptoStream = null;

            try
            {
                DESCryptoServiceProvider DESalg = new DESCryptoServiceProvider();
                memoryStream = new MemoryStream(buffer);
                cryptoStream = new CryptoStream(memoryStream, new DESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read);
                newBuffer = new byte[buffer.Length];
                cryptoStream.Read(newBuffer, 0, newBuffer.Length);
                return Encoding.UTF8.GetString(newBuffer);
            }
            catch (Exception ex)
            {
                throw CNLog.NewError("解码失败", ex);
            }
            finally
            {
                try
                {
                    memoryStream.Close();
                    cryptoStream.Close();
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// JHS - 2022/01/20
        /// 存储图标文件
        /// </summary>
        /// <param name="buffer">图标文件数据</param>
        /// <param name="FilePath">文件路径</param>
        public void Save(byte[] buffer, string FilePath)
        {
            FileStream stream = File.Create(FilePath);
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
        }

        /// <summary>
        /// JHS - 2022/01/20
        /// 存储图标文件
        /// </summary>
        /// <param name="buffer">文档对象</param>
        /// <param name="FilePath">文件路径</param>
        public void Save(XmlDocument doc, string FilePath)
        {
            string strContext = doc.OuterXml;
            byte[] buffer = Encode(strContext);
            FileStream stream = File.Create(FilePath);
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
        }


        /*****************/

        /// <summary>
        /// JHS - 2022/05/11
        /// MD5加密
        /// </summary>
        /// <param name="message">加密前的内容</param>
        /// <returns>加密后的内容</returns>
        public static string MD5Coding(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(buffer);
            Array.Reverse(output);
            return BitConverter.ToString(output).Replace("-", string.Empty);
        }
    }
}
