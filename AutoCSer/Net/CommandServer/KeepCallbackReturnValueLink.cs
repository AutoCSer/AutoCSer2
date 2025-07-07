using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// The keep callback linked list of the return value
    /// 保持回调返回值链表
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public abstract class KeepCallbackReturnValueLink<T> where T : KeepCallbackReturnValueLink<T>
    {
        /// <summary>
        /// The next node
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsIgnoreCurrent = true)]
        [AutoCSer.JsonSerializeMember(IsIgnoreCurrent = true)]
#if NetStandard21
        internal T? LinkNext;
#else
        internal T LinkNext;
#endif

        /// <summary>
        /// Gets the specified number of end nodes
        /// 获取指定数量的结束节点
        /// </summary>
        /// <param name="head">Head node
        /// 头节点</param>
        /// <param name="getCount">Get the quantity
        /// 获取数量</param>
        /// <param name="endCount">Actual end number
        /// 实际结束数量</param>
        /// <returns>End node
        /// 结束节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? GetEndCount(T? head, int getCount, out int endCount)
#else
        public static T GetEndCount(T head, int getCount, out int endCount)
#endif
        {
            if (head != null && getCount > 0) return GetEnd(head, getCount, out endCount);
            endCount = 0;
            return head;
        }
        /// <summary>
        /// Gets the specified number of end nodes
        /// 获取指定数量的结束节点
        /// </summary>
        /// <param name="head">Head node
        /// 头节点</param>
        /// <param name="getCount">Get the quantity
        /// 获取数量</param>
        /// <param name="endCount">Actual end number
        /// 实际结束数量</param>
        /// <returns>End node
        /// 结束节点</returns>
#if NetStandard21
        internal static T? GetEnd(T head, int getCount, out int endCount)
#else
        internal static T GetEnd(T head, int getCount, out int endCount)
#endif
        {
            int index = 0;
            var node = head;
            do
            {
                node = node.LinkNext;
                ++index;
            }
            while (node != null && --getCount > 0);
            endCount = index;
            return node;
        }
    }
}
