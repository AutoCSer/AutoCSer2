using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Custom serialization example
    /// 自定义序列化 示例
    /// </summary>
    class Custom : AutoCSer.BinarySerialize.ICustomSerialize<Custom>
    {
        /// <summary>
        /// Field data
        /// </summary>
        int Value;
        /// <summary>
        /// Custom serialization
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Custom>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.CustomSerialize(Value.ToString());
        }
        /// <summary>
        /// Custom deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Custom>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            var stringValue = default(string);
            if (deserializer.CustomDeserialize(ref stringValue)) Value = int.Parse(stringValue.notNull());
        }

        /// <summary>
        /// Custom serialization test
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
    /// Custom serialization example
    /// </summary>
    struct CustomStruct : AutoCSer.BinarySerialize.ICustomSerialize<CustomStruct>
    {
        /// <summary>
        /// Field data
        /// </summary>
        public int Value;
        /// <summary>
        /// Custom serialization
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<CustomStruct>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.CustomSerialize(Value.ToString());
        }
        /// <summary>
        /// Custom deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<CustomStruct>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            var stringValue = default(string);
            if (deserializer.CustomDeserialize(ref stringValue)) Value = int.Parse(stringValue.notNull());
        }
    }
}
