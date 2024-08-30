using System;

namespace AutoCSer
{
    /// <summary>
    /// XML 解析配置参数
    /// </summary>
    public class XmlDeserializeConfig : TextDeserializeConfig
    {
        /// <summary>
        /// 根节点名称默认为 xml (不能包含非法字符)
        /// </summary>
        public string BootNodeName = "xml"; 
        /// <summary>
        /// 集合子节点名称默认为 item (不能包含非法字符)
        /// </summary>
        public string ItemName = XmlSerializeConfig.DefaultItemName;
        /// <summary>
        /// 是否保存属性索引
        /// </summary>
        public bool IsAttribute;
    }
}
