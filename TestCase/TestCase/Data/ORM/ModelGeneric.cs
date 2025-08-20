using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 关联模型定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [AutoCSer.CodeGenerator.RandomObject]
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
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.RandomObject]
    public partial class ModelGeneric : ModelGeneric<ModelAssociated>
    {
    }
}
