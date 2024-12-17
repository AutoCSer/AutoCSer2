using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回字符串参数
    /// </summary>
    public sealed class StringResponseParameter : ResponseParameter
    {
        /// <summary>
        /// 字符串
        /// </summary>
#if NetStandard21
        private string? value;
#else
        private string value;
#endif
        /// <summary>
        /// 返回字符串参数
        /// </summary>
        public StringResponseParameter() : base(CallStateEnum.Unknown) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref value);
        }
        /// <summary>
        /// 获取返回参数结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
#if NetStandard21
        public ResponseResult<ValueResult<string?>> Get(ResponseResult<ResponseParameter> result)
#else
        public ResponseResult<ValueResult<string>> Get(ResponseResult<ResponseParameter> result)
#endif
        {
#if NetStandard21
            return result.CastValueResult<string?>(value);
#else
            return result.CastValueResult<string>(value);
#endif
        }
    }
}
