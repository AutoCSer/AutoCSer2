using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 栈相关扩展
    /// </summary>
    public static class StackExtension
    {
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="value">弹出数据</param>
        /// <returns>返回 false 表示没有数据可以弹出</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            if (stack.Count != 0)
            {
                value = stack.Pop();
                return true;
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="value">下一个弹出数据</param>
        /// <returns>返回 false 表示没有数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryPeek<T>(this Stack<T> stack, out T value)
        {
            if (stack.Count != 0)
            {
                value = stack.Peek();
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
