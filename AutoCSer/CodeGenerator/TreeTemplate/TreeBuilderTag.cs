using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// CSharp 代码树节点标识
    /// </summary>
    internal sealed class TreeBuilderTag : IEquatable<TreeBuilderTag>
    {
        /// <summary>
        /// 树节点标识类型
        /// </summary>
        internal TreeBuilderTagTypeEnum TagType;
        /// <summary>
        /// 标识command
        /// </summary>
        internal string Command;
        /// <summary>
        /// 内容
        /// </summary>
        internal SubString Content;
        /// <summary>
        /// 判断树节点标识是否相等
        /// </summary>
        /// <param name="other">树节点标识</param>
        /// <returns>是否相等</returns>
        public bool Equals(TreeBuilderTag other)
        {
            return other != null && other.TagType == TagType && other.Command == Command && other.Content.Equals(ref Content);
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            switch (TagType)
            {
                case TreeBuilderTagTypeEnum.Region:
                    return "#region " + Command + " " + Content.ToString();
                case TreeBuilderTagTypeEnum.Note:
                    return "/*" + Command + ":" + Content.ToString() + "*/";
                case TreeBuilderTagTypeEnum.At:
                    return "@" + Content.ToString();
            }
            if (Content.String != null)
            {
                string code = Content.ToString().Replace('\r', ' ').Replace('\n', ' ');
                if (code.Length > 64) return code.Substring(0, 32) + " ... " + code.Substring(code.Length - 32);
                return code;
            }
            return null;
        }
    }
}
