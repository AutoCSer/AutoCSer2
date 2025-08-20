using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data.ORM
{
    /// <summary>
    /// ORM 基本数据模型定义
    /// </summary>
    [AutoCSer.CodeGenerator.FieldEquals]
    [AutoCSer.CodeGenerator.RandomObject]
    public partial class CommonModel
    {
        public long CommonModelIdentity;
        public int Data;
    }
}
