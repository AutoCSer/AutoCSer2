using System;
/*ulong;long;uint;int;float;double;decimal;DateTime;TimeSpan;Guid*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(ulong[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (ulong* arrayFixed = array)
                {
                    int dataSize = count * sizeof(ulong);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(ulong?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, array.Length * sizeof(ulong));
                if (arrayMap.WriteIndex != -1)
                {
                    ulong* write = (ulong*)Stream.Data.Pointer.Current;
                    foreach (ulong? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (ulong)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SetCurrent(write);
                }
            }
            else Stream.Write(0);
        }
    }
}
