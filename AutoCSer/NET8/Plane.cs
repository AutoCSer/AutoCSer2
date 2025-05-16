using System;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 4)]
    public struct Plane
    {
        /// <summary>
        /// 
        /// </summary>
        public Vector3 Normal;
        /// <summary>
        /// 
        /// </summary>
        public float D;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="d"></param>
        internal Plane(Vector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }
    }
}
