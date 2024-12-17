using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// JSON 返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class JsonResponseParameter<T> : ResponseParameter
    {
        /// <summary>
        /// 返回对象
        /// </summary>
#if NetStandard21
        private T? value;
#else
        private T value;
#endif
        /// <summary>
        /// JSON 返回参数
        /// </summary>
        public JsonResponseParameter() : base(CallStateEnum.Unknown) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeJsonBuffer(ref value);
        }
        /// <summary>
        /// 获取返回参数结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
#if NetStandard21
        public ResponseResult<ValueResult<T?>> Get(ResponseResult<ResponseParameter> result)
#else
        public ResponseResult<ValueResult<T>> Get(ResponseResult<ResponseParameter> result)
#endif
        {
#if NetStandard21
            return result.CastValueResult<T?>(value);
#else
            return result.CastValueResult<T>(value);
#endif
        }
    }
}
