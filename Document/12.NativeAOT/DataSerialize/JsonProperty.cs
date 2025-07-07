using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCSer.Document.NativeAOT.DataSerialize
{
    /// <summary>
    /// Example of JSON serialization
    /// JSON 序列化 示例
    /// </summary>
    [AutoCSer.CodeGenerator.JsonSerialize]
    partial class JsonProperty
    {
        /// <summary>
        /// Automatically implementing property
        /// 自动属性
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// JSON serialization test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            string data = AutoCSer.JsonSerializer.Serialize(new JsonProperty { Value = 1 });
            var newValue = AutoCSer.JsonDeserializer.Deserialize<JsonProperty>(data);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
