using System;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 4)]
    public struct Quaternion
    {
        /// <summary>
        /// 
        /// </summary>
        public float X;
        /// <summary>
        /// 
        /// </summary>
        public float Y;
        /// <summary>
        /// 
        /// </summary>
        public float Z;
        /// <summary>
        /// 
        /// </summary>
        public float W;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector3"></param>
        /// <param name="w"></param>
        internal Quaternion(Vector3 vector3, float w)
        {
            X = vector3.X;
            Y = vector3.Y;
            Z = vector3.Z;
            W = w;
        }
    }
}
