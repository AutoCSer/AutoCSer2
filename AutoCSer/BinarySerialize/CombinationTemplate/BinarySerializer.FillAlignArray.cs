using System;
/*ushort;short;sbyte;byte;char*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(ushort[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (ushort* arrayFixed = array) Stream.Serialize(arrayFixed, count, count * sizeof(ushort));
            }
        }
        /// <summary>
        /// 整数数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(ushort?[] array)
        {
            if (array.Length != 0)
            {
                AutoCSer.BinarySerialize.SerializeArrayMap arrayMap = new AutoCSer.BinarySerialize.SerializeArrayMap(Stream, array.Length, (array.Length * sizeof(ushort) + 3) & (int.MaxValue - 3));
                if (arrayMap.WriteIndex != -1)
                {
                    ushort* write = (ushort*)Stream.Data.Pointer.Current;
                    foreach (ushort? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            *write++ = (ushort)value;
                        }
                        else arrayMap.NextFalse();
                    }
                    arrayMap.End();
                    Stream.Data.Pointer.SerializeFillByteSize(write);
                }
            }
            else Stream.Write(0);
        }
    }
}
