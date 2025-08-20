using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// Key-value pairs
    /// 键值对
    /// </summary>
    public static class KeyValue
    {
        /// <summary>
        /// Get the key-value pair
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
    /// Key-value pairs
    /// 键值对
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// </typeparam>
    /// <typeparam name="VT">Data value type
    /// 数据值类型</typeparam>
    [AutoCSer.CodeGenerator.JsonSerialize]
    [RemoteType]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct KeyValue<KT, VT>
    {
        /// <summary>
        /// Keyword
        /// </summary>
        public KT Key;
        /// <summary>
        /// Data value
        /// </summary>
        public VT Value;
        /// <summary>
        /// Key-value pairs
        /// 键值对
        /// </summary>
        /// <param name="key">Keyword</param>
        /// <param name="value">Data value</param>
        public KeyValue(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
        ///// <summary>
        ///// 键值对
        ///// </summary>
        ///// <param name="key">Keyword</param>
        ///// <param name="value">Data value</param>
        //public KeyValue(ref KT key, VT value)
        //{
        //    Key = key;
        //    Value = value;
        //}
        /// <summary>
        /// Key-value pairs
        /// 键值对
        /// </summary>
        /// <param name="key">Keyword</param>
        /// <param name="value">Data value</param>
        public KeyValue(ref KT key, ref VT value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// Clear the data
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
        /// Reset the key-value pair
        /// 重置键值对
        /// </summary>
        /// <param name="key">Keyword</param>
        /// <param name="value">Data value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Set(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
    }
}
