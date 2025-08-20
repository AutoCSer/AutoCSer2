using System;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 空壳类型定义
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.CodeGenerator.FieldEquals]
    internal partial class NoMemberClass
    {
    }
}
