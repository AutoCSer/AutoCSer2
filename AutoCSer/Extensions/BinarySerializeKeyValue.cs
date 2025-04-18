using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 键值对（用于二进制序列化屏蔽引用操作）
    /// </summary>
    /// <typeparam name="KT">键类型</typeparam>
    /// <typeparam name="VT">值类型</typeparam>
#if AOT
    [AutoCSer.CodeGenerator.XmlSerialize]
#endif
    [RemoteType]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct BinarySerializeKeyValue<KT, VT>
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
        public BinarySerializeKeyValue(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="keyValue">键值对</param>
        public BinarySerializeKeyValue(KeyValue<KT, VT> keyValue)
        {
            Key = keyValue.Key;
            Value = keyValue.Value;
        }
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="keyValue">键值对</param>
        public BinarySerializeKeyValue(KeyValuePair<KT, VT> keyValue)
        {
            Key = keyValue.Key;
            Value = keyValue.Value;
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
        /// <summary>
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
