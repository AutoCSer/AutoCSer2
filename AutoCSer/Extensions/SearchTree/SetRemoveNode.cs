using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 设置节点数据
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SetRemoveNode<KT, VT>
         where KT : IComparable<KT>
        where VT : Node<VT, KT>
    {
        /// <summary>
        /// 设置数据
        /// </summary>
        internal readonly VT Value;
        /// <summary>
        /// 被移除数据
        /// </summary>
        internal VT RemoveValue;
        /// <summary>
        /// 是否需要移除数据
        /// </summary>
        internal bool IsRemove;
        /// <summary>
        /// 是否添加了新数据
        /// </summary>
        internal bool IsNewValue;
        /// <summary>
        /// 设置节点数据
        /// </summary>
        /// <param name="value">数据</param>
        internal SetRemoveNode(VT value)
        {
            Value = value;
            RemoveValue = value;
            IsRemove = IsNewValue = false;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value">被移除数据</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal VT SetRemove(VT value)
        {
            IsRemove = false;
            RemoveValue = value;
            Value.SetRemove(value);
            return Value;
        }
    }
}
