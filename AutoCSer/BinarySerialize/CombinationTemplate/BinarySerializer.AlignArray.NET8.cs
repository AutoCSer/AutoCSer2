using System;
/*UInt128;Int128;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

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
        private unsafe void primitiveSerializeOnly(UInt128[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (UInt128* arrayFixed = array)
                {
                    int dataSize = count * sizeof(UInt128);
                    byte* write = Stream.GetBeforeMove(sizeof(int) + dataSize);
                    if (write != null)
                    {
                        *(int*)write = count;
                        AutoCSer.Common.CopyTo(arrayFixed, write + sizeof(int), dataSize);
                    }
                }
            }
        }
    }
}
