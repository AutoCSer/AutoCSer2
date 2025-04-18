using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 关联模型定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if AOT
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    [AutoCSer.BinarySerialize]
    public partial class ModelGeneric<T> : CommonModel
        where T : ModelAssociated
    {
        public T ModelAssociated;
        public ListArray<T> ModelAssociatedList;
    }
    /// <summary>
    /// ORM 关联模型定义
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.DefaultConstructor]
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    public partial class ModelGeneric : ModelGeneric<ModelAssociated>
    {
    }
}
