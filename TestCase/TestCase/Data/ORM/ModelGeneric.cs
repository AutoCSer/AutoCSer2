using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 关联模型定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [AutoCSer.AOT.Preserve(AllMembers = true)]
    [AutoCSer.BinarySerialize]
    public class ModelGeneric<T> : CommonModel
        where T : ModelAssociated
    {
        public T ModelAssociated;
        public ListArray<T> ModelAssociatedList;
    }
    /// <summary>
    /// ORM 关联模型定义
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize]
    public partial class ModelGeneric : ModelGeneric<ModelAssociated>
    {
    }
}
