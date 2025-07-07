using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCSer.Document.NativeAOT.DataSerialize
{
    /// <summary>
    /// Example of automatically implement property binary serialization
    /// 自动属性二进制序列化 示例
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize]
    partial class BinaryProperty
    {
        /// <summary>
        /// Automatically implementing property
        /// 自动属性
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Automatically implement property binary serialization testing
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(new BinaryProperty { Value = 1 });
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<BinaryProperty>(data);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
