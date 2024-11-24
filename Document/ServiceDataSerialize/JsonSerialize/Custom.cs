using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServiceDataSerialize.JsonSerialize
{
    /// <summary>
    /// 自定义序列化 示例
    /// </summary>
    class Custom : AutoCSer.Json.ICustomSerialize<Custom>
    {
        /// <summary>
        /// 字段数据
        /// </summary>
        int Value;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.Json.ICustomSerialize<Custom>.Serialize(AutoCSer.JsonSerializer serializer)
        {
            serializer.CustomSerialize(Value.ToString());
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.Json.ICustomSerialize<Custom>.Deserialize(AutoCSer.JsonDeserializer deserializer)
        {
            var stringValue = default(string);
            if (deserializer.CustomDeserialize(ref stringValue)) Value = int.Parse(stringValue.notNull());
        }

        /// <summary>
        /// 自定义序列化 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            string json = AutoCSer.JsonSerializer.Serialize(new Custom { Value = 1 });
            var newValue = AutoCSer.JsonDeserializer.Deserialize<Custom>(json);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            json = AutoCSer.JsonSerializer.Serialize(new CustomStruct { Value = 1 });
            var newStructValue = AutoCSer.JsonDeserializer.Deserialize<CustomStruct>(json);
            if (newStructValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
    /// <summary>
    /// 自定义序列化 示例
    /// </summary>
    struct CustomStruct : AutoCSer.Json.ICustomSerialize<CustomStruct>
    {
        /// <summary>
        /// 字段数据
        /// </summary>
        public int Value;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.Json.ICustomSerialize<CustomStruct>.Serialize(AutoCSer.JsonSerializer serializer)
        {
            serializer.CustomSerialize(Value.ToString());
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.Json.ICustomSerialize<CustomStruct>.Deserialize(AutoCSer.JsonDeserializer deserializer)
        {
            var stringValue = default(string);
            if (deserializer.CustomDeserialize(ref stringValue)) Value = int.Parse(stringValue.notNull());
        }
    }
}
