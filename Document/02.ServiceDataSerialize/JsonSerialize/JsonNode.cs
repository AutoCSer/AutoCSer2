using System;

namespace AutoCSer.Document.ServiceDataSerialize.JsonSerialize
{
    /// <summary>
    /// Example of deserializing JSON nodes
    /// </summary>
    class JsonNode
    {
        /// <summary>
        /// Deserialization JSON node test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            var value = new { Number = 1.1, Bool = true, DateTime = new DateTime(2000, 1, 2, 3, 4, 5), String = @"大
小" };
            string json = AutoCSer.JsonSerializer.Serialize(value);
            AutoCSer.JsonNode node = AutoCSer.JsonDeserializer.Deserialize<AutoCSer.JsonNode>(json);
            if (node.Type != JsonNodeTypeEnum.Dictionary || node[nameof(value.Number)].Number != value.Number || node[nameof(value.Bool)].Bool != value.Bool
                 || node[nameof(value.DateTime)].DateTime != value.DateTime || node[nameof(value.String)].String != value.String)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
