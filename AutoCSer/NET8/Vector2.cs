using System;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 2)]
    public struct Vector2
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
        /// <param name="x"></param>
        /// <param name="y"></param>
        internal Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
