using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 部分列表（用于主表与部分表 1:N 的扩展）
    /// </summary>
    /// <typeparam name="T">部分值模型定义</typeparam>
    [AutoCSer.JsonSerialize(CustomReferenceTypes = new Type[] { null })]
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[] { null })]
    public sealed class PartialList<T> : ListArray<T>
        where T : class
    {
        //XXX
    }
}
