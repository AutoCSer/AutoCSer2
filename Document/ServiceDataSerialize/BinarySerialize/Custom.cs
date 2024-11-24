using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 自定义序列化 示例
    /// </summary>
    class Custom : AutoCSer.BinarySerialize.ICustomSerialize<Custom>
    {
        /// <summary>
        /// 字段数据
        /// </summary>
        int Value;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Custom>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.CustomSerialize(Value.ToString());
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Custom>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
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
            byte[] data = AutoCSer.BinarySerializer.Serialize(new Custom { Value = 1 });
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<Custom>(data);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            data = AutoCSer.BinarySerializer.Serialize(new CustomStruct { Value = 1 });
            var newStructValue = AutoCSer.BinaryDeserializer.Deserialize<CustomStruct>(data);
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
    struct CustomStruct : AutoCSer.BinarySerialize.ICustomSerialize<CustomStruct>
    {
        /// <summary>
        /// 字段数据
        /// </summary>
        public int Value;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<CustomStruct>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.CustomSerialize(Value.ToString());
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<CustomStruct>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            var stringValue = default(string);
            if (deserializer.CustomDeserialize(ref stringValue)) Value = int.Parse(stringValue.notNull());
        }
    }
}
