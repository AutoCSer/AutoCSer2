using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCSer.Document.NativeAOT.DataSerialize
{
    /// <summary>
    /// Example of XML serialization
    /// XML 序列化 示例
    /// </summary>
    [AutoCSer.CodeGenerator.XmlSerialize]
    partial class XmlProperty
    {
        /// <summary>
        /// Automatically implementing property
        /// 自动属性
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// XML serialization test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            string data = AutoCSer.XmlSerializer.Serialize(new XmlProperty { Value = 1 });
            var newValue = AutoCSer.XmlDeserializer.Deserialize<XmlProperty>(data);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
