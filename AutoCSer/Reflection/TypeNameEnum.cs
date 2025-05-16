using System;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 类型名称输出类型
    /// </summary>
    public enum TypeNameBuildEnum : byte
    {
        /// <summary>
        /// CSharp 代码
        /// </summary>
        Code,
        /// <summary>
        /// 读取 XML 文档
        /// </summary>
        XmlDocument,
        /// <summary>
        /// XML 文档输出注释
        /// </summary>
        OutputXml,
    }
}
