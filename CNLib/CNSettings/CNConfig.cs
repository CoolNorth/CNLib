using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

namespace CNLib.CNSettings
{
    /// <summary>
    /// 节点值的状态 是获取还是设置
    /// </summary>
    enum ValueState
    {
        Get,
        Set
    };


    public class CNConfig
    {
        /// <summary>
        /// 获取AppSetting中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string Key(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="ConnName">连接名称</param>
        /// <returns>连接字符串</returns>
        public static string GetConn(string ConnName)
        {
            return ConfigurationManager.ConnectionStrings[ConnName].ConnectionString ?? string.Empty;
        }


        #region OldCode


        ///// <summary>
        ///// 外部访问路径
        ///// </summary>
        //public static string Path
        //{
        //    get
        //    {
        //        return CNConfigration.path;
        //    }
        //}

        ///// <summary>
        ///// 内部使用路径
        ///// </summary>
        //private static string path = string.Empty;

        ///// <summary>
        ///// Xml文档对象
        ///// </summary>
        //private static XmlDocument xmlDoc = new XmlDocument();

        ///// <summary>
        ///// 节点值
        ///// </summary>
        //private static object NodeValue = string.Empty;

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="Path"></param>
        //public CNConfigration(string Path)
        //{
        //    CNConfigration.path = Path;

        //    InitXmlDoc();
        //}

        ///// <summary>
        ///// 初始化Xml对象
        ///// </summary>
        //private void InitXmlDoc()
        //{
        //    xmlDoc = new XmlDocument();
        //    xmlDoc.Load(CNConfigration.path);
        //}

        ///// <summary>
        ///// JHS - 2022/01/11
        ///// 查询或设置节点值(由state来决定是查询还是修改)
        ///// </summary>
        ///// <param name="parentNode"></param>
        ///// <param name="nodeName"></param>
        ///// <param name="state"></param>
        //private static void SelectNodeValue(XmlNode parentNode, string nodeName, ValueState state)
        //{

        //    if (parentNode.ChildNodes.Count > 0)
        //    {
        //        foreach (XmlNode childNode in parentNode.ChildNodes)
        //        {
        //            if (childNode.Name == nodeName)
        //            {
        //                switch (state)
        //                {
        //                    case ValueState.Get:
        //                        NodeValue = childNode.InnerText;
        //                        return;
        //                    case ValueState.Set:
        //                        childNode.InnerText = NodeValue.ToString();
        //                        xmlDoc.Save(Path);
        //                        return;
        //                    default:
        //                        break;
        //                }
        //            }
        //            SelectNodeValue(childNode, nodeName, state);
        //        }
        //    }
        //}

        ///// <summary>
        ///// JHS - 2022/01/11 
        ///// 根据节点名称获取节点
        ///// </summary>
        ///// <param name="nodeName">节点名称</param>
        //public static object GetNodeValue(string nodeName)
        //{
        //    NodeValue = string.Empty;

        //    XmlNodeList xnl = xmlDoc.SelectNodes("root");

        //    SelectNodeValue(xnl[0], nodeName, ValueState.Get);

        //    return NodeValue;
        //}

        ///// <summary>
        ///// JHS - 2022/01/11
        ///// 设置节点属性
        ///// </summary>
        ///// <param name="NodeName"></param>
        ///// <param name="value"></param>
        //public static void SetNodeValue(string nodeName, object value)
        //{
        //    NodeValue = value;

        //    XmlNodeList xnl = xmlDoc.SelectNodes("root");

        //    SelectNodeValue(xnl[0], nodeName, ValueState.Set);

        //}

        #endregion
    }


}
