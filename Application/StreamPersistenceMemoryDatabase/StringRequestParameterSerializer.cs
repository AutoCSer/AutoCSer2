using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字符串请求参数序列化
    /// </summary>
    internal sealed class StringRequestParameterSerializer : RequestParameterSerializer
    {
        /// <summary>
        /// 字符串
        /// </summary>
#if NetStandard21
        private readonly string? value;
#else
        private readonly string value;
#endif
        /// <summary>
        /// 字符串请求参数序列化
        /// </summary>
        /// <param name="value">字符串</param>
#if NetStandard21
        private StringRequestParameterSerializer(string? value)
#else
        private StringRequestParameterSerializer(string value)
#endif
        {
            this.value = value;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator StringRequestParameterSerializer(string? value)
#else
        public static implicit operator StringRequestParameterSerializer(string value)
#endif
        {
            if (value != null)
            {
                return value.Length != 0 ? new StringRequestParameterSerializer(value) : empty;
            }
            return nullString;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SerializeBuffer(value);
        }

        /// <summary>
        /// null
        /// </summary>
        private static readonly StringRequestParameterSerializer nullString = new StringRequestParameterSerializer(null);
        /// <summary>
        /// 空字符串
        /// </summary>
        private static readonly StringRequestParameterSerializer empty = new StringRequestParameterSerializer(string.Empty);
    }
}
