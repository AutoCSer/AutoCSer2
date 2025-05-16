using System;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 3)]
    public struct Vector3
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
        /// <param name="vector2"></param>
        /// <param name="z"></param>
        internal Vector3(Vector2 vector2, float z)
        {
            X = vector2.X;
            Y = vector2.Y;
            Z = z;
        }
    }
}
