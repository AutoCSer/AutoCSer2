using AutoCSer.Extensions;
using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;DateTime;TimeSpan*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonDeserialize(ref ulong[]? array)
#else
        public void JsonDeserialize(ref ulong[] array)
#endif
        {
            if (IsBinaryMix)
            {
                if (searchBinaryMixArraySize(ref array))
                {
                    if (array.Length != 0)
                    {
                        int index = 0;
                        do
                        {
                            binaryMix(ref array[index]);
                            if (State == AutoCSer.Json.DeserializeStateEnum.Success) ++index;
                            else return;
                        }
                        while (index != array.Length);
                        return;
                    }
                }
                else return;
            }
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    JsonDeserialize(ref array[index]);
                    if (State == AutoCSer.Json.DeserializeStateEnum.Success)
                    {
                        ++index;
                        if (IsNextArrayValue())
                        {
                            if (index == array.Length) break;
                        }
                        else
                        {
                            if (index == array.Length) return;
                            break;
                        }
                    }
                    else return;
                }
                while (true);
                State = AutoCSer.Json.DeserializeStateEnum.ArraySizeError;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref ulong[]? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref ulong[] value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}
