using System;

namespace AutoCSer.ORM.CustomColumn
{

    /// <summary>
    /// 日期
    /// </summary>
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[0])]
    [AutoCSer.JsonSerialize(CustomReferenceTypes = new Type[0], DocumentType = typeof(int))]
    [AutoCSer.ORM.CustomColumn(NameConcatType = AutoCSer.ORM.CustomColumnNameConcatTypeEnum.Parent)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct Date : AutoCSer.BinarySerialize.ICustomSerialize<Date>, AutoCSer.Json.ICustomSerialize<Date>, AutoCSer.ORM.IVerify<Date>
    {
        /// <summary>
        /// 日期值
        /// </summary>
        public int Value;
        /// <summary>
        /// 年份
        /// </summary>
        public int Year
        {
            get { return Value >> 9; }
        }
        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get { return (Value >> 5) & 15; }
        }
        /// <summary>
        /// 日期天数
        /// </summary>
        public int Day
        {
            get { return Value & 31; }
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Date(DateTime value) { return new Date { Value = value != default(DateTime) ? (value.Year << 9) + (value.Month << 5) + value.Day : 0 }; }
        /// <summary>
        /// 自定义验证，验证失败需要抛出异常
        /// </summary>
        /// <returns></returns>
        public Date Verify()
        {
            if (Value >= 0)
            {
                if (Value != 0) new DateTime(Value >> 9, (Value >> 5) & 15, Value & 31);
                return this;
            }
            throw new ArgumentOutOfRangeException($"{Value} 不能小于 0");
        }

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Date>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            AutoCSer.BinarySerialize.TypeSerializer<int>.DefaultSerializer(serializer, Value);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<Date>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            AutoCSer.BinarySerialize.TypeDeserializer<int>.DefaultDeserializer(deserializer, ref Value);
        }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.Json.ICustomSerialize<Date>.Serialize(JsonSerializer serializer)
        {
            AutoCSer.Json.TypeSerializer<int>.DefaultSerializer(serializer, Value);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.Json.ICustomSerialize<Date>.Deserialize(JsonDeserializer deserializer)
        {
            AutoCSer.Json.TypeDeserializer<int>.DefaultDeserializer(deserializer, ref Value);
        }
    }
}
