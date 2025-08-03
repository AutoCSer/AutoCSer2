using System;

namespace AutoCSer
{
    /// <summary>
    /// An empty array of 0 length
    /// 0 长度空数组
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public static class EmptyArray<T>
    {
        /// <summary>
        /// 0-element Array (Serious warning: Do not perform Array.resize operations on this object. Do not use in scenarios where it cannot be guaranteed)
        /// 0 元素数组（严重警告，禁止对该对象进行 Array.Resize 操作，在无法保证的场景禁止使用）
        /// </summary>
        public static readonly T[] Array = new T[0];
    }
}
