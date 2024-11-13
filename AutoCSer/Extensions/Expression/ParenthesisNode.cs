using System;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 小括号表达式节点
    /// </summary>
    internal sealed class ParenthesisNode : ValueNode
    {
        /// <summary>
        /// 括号内节点
        /// </summary>
        internal ValueNode Parenthesis;
        /// <summary>
        /// 小括号表达式节点
        /// </summary>
        /// <param name="parenthesis">括号内节点</param>
        /// <param name="next">下一个节点</param>
#if NetStandard21
        internal ParenthesisNode(ValueNode parenthesis, ValueNode? next = null)
#else
        internal ParenthesisNode(ValueNode parenthesis, ValueNode next = null) 
#endif
            : base(NodeTypeEnum.Parenthesis, ValueTypeEnum.Parenthesis, next)
        {
            Parenthesis = parenthesis;
        }
    }
}
