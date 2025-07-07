using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">Array</param>
        public void JsonSerialize(ulong[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (ulong value in array) JsonSerialize(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length << 1);
                    foreach (ulong value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, ulong[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
    }
}
