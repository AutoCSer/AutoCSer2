using System;

namespace AutoCSer.Document.RemoteExpression
{
    /// <summary>
    /// The operation object of the Action test
    /// Action 测试操作对象
    /// </summary>
    internal sealed class ActionTarget
    {
        /// <summary>
        /// Operation data
        /// 操作数据
        /// </summary>
        internal int Value;
        /// <summary>
        /// Set operation data
        /// 设置操作数据
        /// </summary>
        /// <param name="value"></param>
        internal void Set(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Default test operation object
        /// 默认测试操作对象
        /// </summary>
        internal static readonly ActionTarget Default = new ActionTarget();
    }
}
