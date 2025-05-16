using System;
/*ulong;long;uint;int;float;double;decimal;DateTime;TimeSpan;Guid*/

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public static byte* Deserialize(byte* data, ref ulong? value)
        {
            if (*(int*)data == 0)
            {
                value = *(ulong*)(data + sizeof(int));
                return data + (sizeof(int) + sizeof(ulong));
            }
            if (*(int*)data == BinarySerializer.NullValue)
            {
                value = null;
                return data + sizeof(int);
            }
            return null;
        }
    }
}
