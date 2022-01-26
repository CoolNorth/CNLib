using System;
using System.Collections.Generic;
using System.Text;

namespace CNLib.CNCode
{
    /// <summary>
    /// JHS - 2022/01/19
    /// 编码接口
    /// </summary>
    public interface ICNCoding
    {
        /// <summary>
        /// JHS - 2022/01/19
        /// 数据编码
        /// </summary>
        /// <param name="strMsg">编码前信息</param>
        /// <returns>编码后数据</returns>
        byte[] Encode(string strMsg);

        /// <summary>
        /// JHS - 2022/01/19
        /// 数据解码
        /// </summary>
        /// <param name="strMSg">解码前数据</param>
        /// <returns>解码后信息</returns>
        string Decode(byte[] buffer);

        /// <summary>
        /// JHS - 2022/01/20
        /// 存储图标文件
        /// </summary>
        /// <param name="buffer">图标文件数据</param>
        /// <param name="FilePath">文件路径</param>
        void SaveMarkerFile(byte[] buffer, string FilePath);
    }
}
