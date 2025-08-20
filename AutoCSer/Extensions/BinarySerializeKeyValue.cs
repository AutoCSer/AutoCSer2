using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// Key-value pairs (used for binary serialization masking reference operations)
    /// 键值对（用于二进制序列化屏蔽引用操作）
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data value type
    /// 数据值类型</typeparam>
    [AutoCSer.CodeGenerator.XmlSerialize]
    [RemoteType]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct BinarySerializeKeyValue<KT, VT>
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
        public BinarySerializeKeyValue(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// Key-value pairs
        /// 键值对
        /// </summary>
        /// <param name="keyValue">Key-value pair
        /// 键值对</param>
        public BinarySerializeKeyValue(KeyValue<KT, VT> keyValue)
        {
            Key = keyValue.Key;
            Value = keyValue.Value;
        }
        /// <summary>
        /// Key-value pairs
        /// 键值对
        /// </summary>
        /// <param name="keyValue">Key-value pair
        /// 键值对</param>
        public BinarySerializeKeyValue(KeyValuePair<KT, VT> keyValue)
        {
            Key = keyValue.Key;
            Value = keyValue.Value;
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
        /// <summary>
        /// Get the key-value pair
        /// 获取键值对
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public KeyValue<KT, VT> GetKeyValue()
        {
            return new KeyValue<KT, VT>(Key, Value);
        }
    }
}
