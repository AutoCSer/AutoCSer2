using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 反序列化配置参数
    /// </summary>
    public sealed class DeserializeConfig
    {
        /// <summary>
        /// 最大数组长度
        /// </summary>
        public int MaxArraySize = int.MaxValue;
    }
}
