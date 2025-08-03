using System;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式委托类型
    /// </summary>
    internal enum DelegateTypeEnum : byte
    {
        /// <summary>
        /// Func{T}
        /// </summary>
        Func,
        /// <summary>
        /// Func{T1, T}
        /// </summary>
        Func1,
        /// <summary>
        /// Func{T1, T2, T}
        /// </summary>
        Func2,
        /// <summary>
        /// Func{T1, T2, T3, T}
        /// </summary>
        Func3,
        /// <summary>
        /// Action
        /// </summary>
        Action,
        /// <summary>
        /// Action{T1}
        /// </summary>
        Action1,
        /// <summary>
        /// Action{T1, T2}
        /// </summary>
        Action2,
        /// <summary>
        /// Action{T1, T2, T3}
        /// </summary>
        Action3,

        /// <summary>
        /// 保留，用于计数
        /// </summary>
        Count,
    }
}
