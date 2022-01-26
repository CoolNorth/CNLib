using System;
using System.Collections.Generic;
using System.Text;

namespace CNLib.CNConfig
{
    /// <summary>
    /// JHS - 2022/01/11
    /// 配置工具类
    /// </summary>
    public class CNSettings
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Path">配置文件路径</param>
        public CNSettings(string Path)
        {
            this.Path = Path;
        }



        
    }
}
