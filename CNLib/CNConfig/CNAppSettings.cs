using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CNLib.CNConfig
{
    /// <summary>
    /// 节点值的状态 是获取还是设置
    /// </summary>
    enum ValueState
    {
        Get,
        Set
    };


    public class CNAppSettings
    {

        /// <summary>
        /// 外部访问路径
        /// </summary>
        public static string Path
        {
            get
            {
                return CNAppSettings.path;
            }
        }

        /// <summary>
        /// 内部使用路径
        /// </summary>
        private static string path = string.Empty;

        /// <summary>
        /// Xml文档对象
        /// </summary>
        private static XmlDocument xmlDoc = new XmlDocument();

        /// <summary>
        /// 节点值
        /// </summary>
        private static object NodeValue = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Path"></param>
        public CNAppSettings(string Path)
        {
            CNAppSettings.path = Path;

            InitXmlDoc();
        }

        /// <summary>
        /// 初始化Xml对象
        /// </summary>
        private void InitXmlDoc()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(CNAppSettings.path);
        }

        /// <summary>
        /// JHS - 2022/01/11
        /// 查询或设置节点值(由state来决定是查询还是修改)
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="nodeName"></param>
        /// <param name="state"></param>
        private static void SelectNodeValue(XmlNode parentNode, string nodeName, ValueState state)
        {

            if (parentNode.ChildNodes.Count > 0)
            {
                foreach (XmlNode childNode in parentNode.ChildNodes)
                {
                    if (childNode.Name == nodeName)
                    {
                        switch (state)
                        {
                            case ValueState.Get:
                                NodeValue = childNode.InnerText;
                                return;
                            case ValueState.Set:
                                childNode.InnerText = NodeValue.ToString();
                                xmlDoc.Save(Path);
                                return;
                            default:
                                break;
                        }
                    }
                    SelectNodeValue(childNode, nodeName, state);
                }
            }
        }

        /// <summary>
        /// JHS - 2022/01/11 
        /// 根据节点名称获取节点
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        public static object GetNodeValue(string nodeName)
        {
            NodeValue = string.Empty;

            XmlNodeList xnl = xmlDoc.SelectNodes("root");

            SelectNodeValue(xnl[0], nodeName, ValueState.Get);

            return NodeValue;
        }

        /// <summary>
        /// JHS - 2022/01/11
        /// 设置节点属性
        /// </summary>
        /// <param name="NodeName"></param>
        /// <param name="value"></param>
        public static void SetNodeValue(string nodeName, object value)
        {
            NodeValue = value;

            XmlNodeList xnl = xmlDoc.SelectNodes("root");

            SelectNodeValue(xnl[0], nodeName, ValueState.Set);

        }
        
        
    }
}
