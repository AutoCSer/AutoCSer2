using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照字典节点
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SnapshotDictionarySnapshotNode<KT, VT>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        private KT key;
        /// <summary>
        /// 数据
        /// </summary>
        internal VT Value;
        /// <summary>
        /// Key-value pairs
        /// 键值对
        /// </summary>
        internal BinarySerializeKeyValue<KT, VT> BinarySerializeKeyValue
        {
            get { return new BinarySerializeKeyValue<KT, VT>(key, Value); }
        }
        /// <summary>
        /// Key-value pairs
        /// 键值对
        /// </summary>
        internal KeyValue<KT, VT> KeyValue
        {
            get { return new KeyValue<KT, VT>(key, Value); }
        }
        /// <summary>
        /// 是否存在快照数据
        /// </summary>
        internal bool IsSnapshot;
        /// <summary>
        /// 设置快照数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TrySet(KT key, VT value)
        {
            if (!IsSnapshot)
            {
                this.key = key;
                Value = value;
                IsSnapshot = true;
            }
        }
        /// <summary>
        /// 设置快照数据
        /// </summary>
        /// <param name="keyValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TrySet(ref BinarySerializeKeyValue<KT, VT> keyValue)
        {
            if (!IsSnapshot)
            {
                this.key = keyValue.Key;
                Value = keyValue.Value;
                IsSnapshot = true;
            }
        }
    }
}
