using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Return data
    /// 返回数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public struct ValueResult<T>
    {
        /// <summary>
        /// Return data
        /// 返回数据
        /// </summary>
        public T Value;
        /// <summary>
        /// Returning false indicates that the input parameter is illegal or has no return value
        /// 返回 false 表示输入参数非法或者无返回值
        /// </summary>
        public bool IsValue;
        /// <summary>
        /// Return data
        /// 返回数据
        /// </summary>
        /// <param name="value">Return data
        /// 返回数据</param>
        public ValueResult(T value)
        {
            Value = value;
            IsValue = true;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ValueResult<T>(T value) { return new ValueResult<T>(value); }

        /// <summary>
        /// Empty collection
        /// 空集合
        /// </summary>
        internal static readonly ValueResult<T>[] NullEnumerable = new ValueResult<T>[] { default(ValueResult<T>) };
    }
}
