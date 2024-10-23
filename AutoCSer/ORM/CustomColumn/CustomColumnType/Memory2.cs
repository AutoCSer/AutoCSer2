using System;

namespace AutoCSer.ORM.CustomColumn
{
    /// <summary>
    /// 两位小数金额
    /// </summary>
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[0])]
    [AutoCSer.JsonSerialize(CustomReferenceTypes = new Type[0], DocumentType = typeof(long))]
    [AutoCSer.ORM.CustomColumn(NameConcatType = AutoCSer.ORM.CustomColumnNameConcatTypeEnum.Parent)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct Memory2 : AutoCSer.BinarySerialize.ICustomSerialize<Memory2>, AutoCSer.Json.ICustomSerialize<Memory2>
    {
        /// <summary>
        /// 金额整数
        /// </summary>
        public long Value;
        /// <summary>
        /// 两位小数金额
        /// </summary>
        public decimal Decimal
        {
            get { return ((decimal)Value) / 100; }
        }
        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Memory2>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            AutoCSer.BinarySerialize.TypeSerializer<long>.DefaultSerializer(serializer, Value);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Memory2>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            AutoCSer.BinarySerialize.TypeDeserializer<long>.DefaultDeserializer(deserializer, ref Value);
        }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.Json.ICustomSerialize<Memory2>.Serialize(JsonSerializer serializer)
        {
            AutoCSer.Json.TypeSerializer<long>.DefaultSerializer(serializer, Value);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.Json.ICustomSerialize<Memory2>.Deserialize(JsonDeserializer deserializer)
        {
            AutoCSer.Json.TypeDeserializer<long>.DefaultDeserializer(deserializer, ref Value);
        }
    }
}
