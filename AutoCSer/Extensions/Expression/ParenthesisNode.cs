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
        internal ParenthesisNode(ValueNode parenthesis, ValueNode next = null) : base(NodeTypeEnum.Parenthesis, ValueTypeEnum.Parenthesis, next)
        {
            Parenthesis = parenthesis;
        }
    }
}
