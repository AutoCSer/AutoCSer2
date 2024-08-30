using System;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 方法调用表达式节点
    /// </summary>
    internal sealed class CallNode : ValueNode
    {
        /// <summary>
        /// 参数节点集合
        /// </summary>
        internal LeftArray<ValueNode> Parameters;
        /// <summary>
        /// 方法调用表达式节点
        /// </summary>
        /// <param name="valueType">取值类型</param>
        /// <param name="parameters">参数节点集合</param>
        internal CallNode(ValueTypeEnum valueType, ref LeftArray<ValueNode> parameters) : base(NodeTypeEnum.Call, valueType)
        {
            Parameters = parameters;
        }
    }
}
