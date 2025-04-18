using System;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 空壳类型定义
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.DefaultConstructor]
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.CodeGenerator.FieldEquals]
#endif
    internal partial class NoMemberClass
    {
    }
}
