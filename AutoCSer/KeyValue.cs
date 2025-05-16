using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 键值对
    /// </summary>
    public static class KeyValue
    {
        /// <summary>
        /// 获取键值对
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static KeyValue<KT, VT> From<KT, VT>(KT key, VT value)
        {
            return new KeyValue<KT, VT>(key, value);
        }
    }
    /// <summary>
    /// 键值对
    /// </summary>
    /// <typeparam name="KT">键类型</typeparam>
    /// <typeparam name="VT">值类型</typeparam>
#if AOT
    [AutoCSer.CodeGenerator.JsonSerialize]
#endif
    [RemoteType]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct KeyValue<KT, VT>
    {
        /// <summary>
        /// 键
        /// </summary>
        public KT Key;
        /// <summary>
        /// 值
        /// </summary>
        public VT Value;
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
        ///// <summary>
        ///// 键值对
        ///// </summary>
        ///// <param name="key">键</param>
        ///// <param name="value">值</param>
        //public KeyValue(ref KT key, VT value)
        //{
        //    Key = key;
        //    Value = value;
        //}
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(ref KT key, ref VT value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNull()
        {
#pragma warning disable CS8601
            Key = default(KT);
            Value = default(VT);
#pragma warning restore CS8601
        }
        /// <summary>
        /// 重置键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Set(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
    }
}
