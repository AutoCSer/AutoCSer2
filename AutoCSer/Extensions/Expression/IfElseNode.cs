using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 三元表达式节点
    /// </summary>
    internal sealed class IfElseNode : ValueNode
    {
        /// <summary>
        /// false 节点
        /// </summary>
        internal ValueNode Else;
        /// <summary>
        /// 三元表达式节点
        /// </summary>
        /// <param name="ifValue">true 节点</param>
        /// <param name="elseValue">false 节点</param>
        internal IfElseNode(ValueNode ifValue, ValueNode elseValue) : base(NodeTypeEnum.IfElse, ValueTypeEnum.IfElse, ifValue)
        {
            Else = elseValue;
        }
    }
}
