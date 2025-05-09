using System;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 操作数据
    /// </summary>
    /// <typeparam name="T">关键字数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct OperationData<T>
    {
        /// <summary>
        /// 关键字数据
        /// </summary>
        public readonly T Key;
        /// <summary>
        /// 操作数据类型
        /// </summary>
        public readonly OperationDataTypeEnum DataType;
        /// <summary>
        /// 操作类型
        /// </summary>
        public readonly OperationTypeEnum OperationType;
    }
}
