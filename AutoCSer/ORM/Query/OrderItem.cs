using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 排序项
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct OrderItem
    {
        /// <summary>
        /// 排序成员字符串
        /// </summary>
        public string Member;
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending;
        /// <summary>
        /// 设置排序项
        /// </summary>
        /// <param name="member"></param>
        /// <param name="isAscending"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(string member, bool isAscending)
        {
            Member = member;
            IsAscending = isAscending;
        }
    }
}
