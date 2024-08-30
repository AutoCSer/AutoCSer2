using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 部分值（用于分表，主键与主表一致）
    /// </summary>
    /// <typeparam name="T">部分值分表模型定义</typeparam>
    [AutoCSer.JsonSerialize(CustomReferenceTypes = new Type[] { null })]
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[] { null })]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct PartialValue<T> where T : class
    {
        /// <summary>
        /// 部分值
        /// </summary>
        public T Value;
        //XXX
    }
}
