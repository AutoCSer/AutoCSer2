using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 引用类型定义
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.CodeGenerator.FieldEquals]
    internal partial class MemberClass
    {
        public string String;
        public DateTime DateTime;
        public bool Bool;
    }
}
