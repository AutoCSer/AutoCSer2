using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;DateTime;TimeSpan;char;float;double;decimal;Half;Int128;UInt128;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public unsafe abstract partial class UnmanagedStreamBase
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>是否写入成功</returns>
        public bool Write(ulong value)
        {
            if (PrepSize(sizeof(ulong)))
            {
                Data.Pointer.Write(value);
                return true;
            }
            return false;
        }
    }
}
