using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 引用类型定义
    /// </summary>
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.AOT.Preserve(AllMembers = true)]
    internal partial class MemberClass
    {
        public string String;
        public DateTime DateTime;
        public bool Bool;
    }
}
