using System;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    ///  JSON 混杂二进制序列化
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.CodeGenerator.FieldEquals]
#endif
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    internal partial class JsonField : Field
    {
    }
}
