using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// XML 序列化配置参数
    /// </summary>
    public class XmlSerializeConfig : AutoCSer.TextSerialize.SerializeConfig
    {
        /// <summary>
        /// 默认集合子节点名称 item
        /// </summary>
        public const string DefaultItemName = "item";

        /// <summary>
        /// XML头部
        /// </summary>
        public string Header = @"<?xml version=""1.0"" encoding=""utf-8""?>";
        /// <summary>
        /// 根节点名称默认为 xml
        /// </summary>
        public string BootNodeName = "xml";
        /// <summary>
        /// 集合子节点名称默认为 item
        /// </summary>
        public string ItemName = DefaultItemName;
        /// <summary>
        /// 是否输出空对象
        /// </summary>
        public bool IsOutputNull;
        /// <summary>
        /// 是否输出长度为 0 的字符串，默认为 true
        /// </summary>
        public bool IsOutputEmptyString = true;
    }
}
