using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Session callback identifier
    /// 会话回调标识
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = sizeof(uint) * 2)]
    internal struct CallbackIdentity : IEquatable<CallbackIdentity>
    {
        /// <summary>
        /// 会话索引有效位
        /// </summary>
        internal const int CallbackIndexBits = 29;
        /// <summary>
        /// 会话索引最大值
        /// </summary>
        internal const uint CallbackIndexAnd = ((1U << CallbackIndexBits) - 1);

        /// <summary>
        /// 会话序号 + 输出标识
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal uint Index;
        /// <summary>
        /// 会话序号
        /// </summary>
        internal int CallbackIndex { get { return (int)(Index & CallbackIndexAnd); } }
        /// <summary>
        /// 会话标识
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(uint))]
        internal uint Identity;
        /// <summary>
        /// Session callback identifier
        /// 会话回调标识
        /// </summary>
        /// <param name="index">会话序号</param>
        internal CallbackIdentity(uint index)
        {
            Index = index;
            Identity = 0;
        }
        /// <summary>
        /// Session callback identifier
        /// 会话回调标识
        /// </summary>
        /// <param name="index">会话序号</param>
        /// <param name="identity"></param>
        internal CallbackIdentity(uint index, uint identity)
        {
            Index = index;
            Identity = identity;
        }
        /// <summary>
        /// 设置未知回调标识
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNull()
        {
            Index = uint.MaxValue;
            Identity = uint.MaxValue;
        }
        /// <summary>
        /// 设置会话回调标识
        /// </summary>
        /// <param name="index"></param>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int index, uint identity)
        {
            Index = (uint)index;
            Identity = identity;
        }
        /// <summary>
        /// 客户端保持回调比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool CallbackEquals(CallbackIdentity other)
        {
            return (((Index ^ other.Index) & CallbackIndexAnd) | (Identity ^ other.Identity)) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(CallbackIdentity other)
        {
            return ((Index ^ other.Index) | (Identity ^ other.Identity)) == 0;
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
            return Equals(obj.castValue<CallbackIdentity>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(Index ^ Identity);
        }
    }
}
