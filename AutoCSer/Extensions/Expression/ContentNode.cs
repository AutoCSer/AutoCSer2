using System;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 包含内容的取值表达式节点
    /// </summary>
    internal sealed class ContentNode : ValueNode
    {
        /// <summary>
        /// 节点内容字符串
        /// </summary>
        internal SubString Content;
        /// <summary>
        /// 包含内容的取值表达式节点
        /// </summary>
        /// <param name="valueType">取值类型</param>
        /// <param name="content">节点内容字符串</param>
        /// <param name="next">下一个节点</param>
        /// <param name="memberDepth">成员回溯深度</param>
#if NetStandard21
        internal ContentNode(ValueTypeEnum valueType, SubString content, ValueNode? next = null, byte memberDepth = 0)
#else
        internal ContentNode(ValueTypeEnum valueType, SubString content, ValueNode next = null, byte memberDepth = 0) 
#endif
            : base(NodeTypeEnum.Content, valueType, next, memberDepth)
        {
            Content = content;
        }
    }
}
