using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 调用表达式委托
    /// </summary>
    internal abstract class CallDelegate
    {
        /// <summary>
        /// 创建表达式委托
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal abstract object Create(BinaryDeserializer deserializer);
    }
}
