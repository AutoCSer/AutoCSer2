using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// JSON 序列化对象（由于默认为二进制序列化，需要 JSON 序列化操作时可以使用此类型封装处理）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct JsonValue<T> : AutoCSer.BinarySerialize.ICustomSerialize<JsonValue<T>>
    {
        /// <summary>
        /// 数据对象
        /// </summary>
        public T Value;
        /// <summary>
        /// JSON 序列化对象（由于默认为二进制序列化，需要 JSON 序列化操作时可以使用此类型封装处理）
        /// </summary>
        /// <param name="value">数据对象</param>
        public JsonValue(T value)
        {
            Value = value;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(JsonValue<T> value) { return value.Value; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsonValue<T>(T value) { return new JsonValue<T>(value); }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<JsonValue<T>>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            int index = serializer.SerializeBufferStart();
            if (index >= 0) serializer.SerializeBufferEnd(index, serializer.GetJsonSerializer().SerializeCommandServerBuffer(ref Value, serializer.Stream));
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<JsonValue<T>>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            byte* end = deserializer.DeserializeBufferStart();
            if (end != null)
            {
                deserializer.DeserializeJson(out Value);
                deserializer.DeserializeBufferEnd(end);
            }
        }
    }
}
