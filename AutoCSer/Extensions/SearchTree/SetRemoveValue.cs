using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 设置节点数据
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SetRemoveValue<KT, VT>
         where KT : IComparable<KT>
    {
        /// <summary>
        /// 设置关键字
        /// </summary>
        internal readonly KT Key;
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        internal readonly VT Value;
        /// <summary>
        /// Binary search tree dictionary node
        /// 二叉搜索树字典节点
        /// </summary>
        internal Dictionary<KT, VT>.Node Node
        {
            get { return new Dictionary<KT, VT>.Node(Key, Value); }
        }
        /// <summary>
        /// 被移除数据
        /// </summary>
        internal VT RemoveValue;
        /// <summary>
        /// 是否需要移除数据
        /// </summary>
        internal bool IsRemove;
        /// <summary>
        /// 设置节点数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        internal SetRemoveValue(KT key, VT value)
        {
            Key = key;
            Value = value;
            RemoveValue = value;
            IsRemove = false;
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="value">被移除数据</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal VT SetRemove(VT value)
        {
            RemoveValue = value;
            return Value;
        }
    }
}
