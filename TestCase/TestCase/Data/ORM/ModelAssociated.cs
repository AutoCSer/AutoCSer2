using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 基本数据模型定义
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.AOT.Preserve(AllMembers = true)]
    [AutoCSer.BinarySerialize]
    public partial class ModelAssociated
    {
        public long CommonModelIdentity;
        public int Data;
    }
}
