using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 业务模型定义
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize]
    public partial class BusinessModelAssociated : ModelAssociated
    {
        public int Other;
    }
}
