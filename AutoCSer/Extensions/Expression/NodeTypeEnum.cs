using System;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 表达式节点类型
    /// </summary>
    internal enum NodeTypeEnum : byte
    {
        /// <summary>
        /// 取值
        /// </summary>
        Value,
        /// <summary>
        /// 内容节点，包括 成员/数字/字符串
        /// </summary>
        Content,
        /// <summary>
        /// 方法调用
        /// </summary>
        Call,
        /// <summary>
        /// 小括号 ()
        /// </summary>
        Parenthesis,
        /// <summary>
        /// 三元表达式 ?:
        /// </summary>
        IfElse,
    }
}
