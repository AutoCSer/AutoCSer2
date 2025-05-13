using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 业务模型定义
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
#endif
    public partial class BusinessModelAssociated : ModelAssociated
    {
        public int Other;
    }
}
