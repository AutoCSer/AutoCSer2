using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Disable the object reference check example
    /// 禁用对象引用检查 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    class DisabledReference
    {
        /// <summary>
        /// Disable the object reference check test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            DisabledReference value = new DisabledReference();
            DisabledReference[] array = new DisabledReference[] { value, value };//Reference twice in the array

            byte[] data = AutoCSer.BinarySerializer.Serialize(array);
            var newArray = AutoCSer.BinaryDeserializer.Deserialize<DisabledReference[]>(data);

            if (newArray == null || newArray.Length != 2 || newArray[0] == null || newArray[1] == null || object.ReferenceEquals(newArray[0], newArray[1]))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
