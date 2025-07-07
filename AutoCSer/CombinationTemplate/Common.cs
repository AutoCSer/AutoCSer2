using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;float;double;decimal;char;DateTime;TimeSpan;Guid;UInt128;Int128;Half;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public unsafe static partial class Common
    {
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="source">原串起始地址，长度必须大于0</param>
        /// <param name="destination">目标数据</param>
        public static void CopyTo(void* source, ulong[] destination)
        {
            fixed (ulong* destinationFixed = destination) CopyTo(source, destinationFixed, (long)destination.Length * sizeof(ulong));
        }
    }
}
