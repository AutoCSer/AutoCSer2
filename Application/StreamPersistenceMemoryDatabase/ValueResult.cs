using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public struct ValueResult<T>
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Value;
        /// <summary>
        /// 是否存在返回数据，false 表示输入参数非法或者无返回值
        /// </summary>
        public bool IsValue;
        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="value">返回数据</param>
        public ValueResult(T value)
        {
            Value = value;
            IsValue = true;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ValueResult<T>(T value) { return new ValueResult<T>(value); }

        /// <summary>
        /// 空集合
        /// </summary>
        internal static readonly ValueResult<T>[] NullEnumerable = new ValueResult<T>[] { default(ValueResult<T>) };
    }
}
