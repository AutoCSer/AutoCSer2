using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程表达式常量参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ConstantParameter : IEquatable<ConstantParameter>
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// 常量值
        /// </summary>
#if NetStandard21
        internal readonly object? Value;
#else
        internal readonly object Value;
#endif
        /// <summary>
        /// 远程表达式常量参数
        /// </summary>
        /// <param name="type"></param>
        internal ConstantParameter(Type type)
        {
            this.Type = type;
            Value = null;
        }
        /// <summary>
        /// 远程表达式常量参数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
#if NetStandard21
        internal ConstantParameter(Type type, object? value)
#else
        internal ConstantParameter(Type type, object value)
#endif
        {
            this.Type = type;
            this.Value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ConstantParameter other)
        {
            return object.ReferenceEquals(Value, other.Value) && Type == other.Type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<ConstantParameter>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Value.notNull().GetHashCode();
        }
    }
}
