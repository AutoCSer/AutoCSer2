using System;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 0 长度空数组已完成任务
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public static class EmptyLeftArrayCompletedTask<T>
    {
        /// <summary>
        /// 0 长度空数组已完成任务
        /// </summary>
        public static readonly Task<LeftArray<T>> EmptyArray = Task.FromResult(new LeftArray<T>(EmptyArray<T>.Array));
    }
}
