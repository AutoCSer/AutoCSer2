using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 基本数据模型定义
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.FieldEquals]
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    public partial class CommonModel
    {
        public long CommonModelIdentity;
        public int Data;
    }
}
