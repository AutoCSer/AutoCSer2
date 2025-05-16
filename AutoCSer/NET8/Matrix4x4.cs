using System;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 16)]
    public struct Matrix4x4
    {
        /// <summary>
        /// 
        /// </summary>
        public float M11;
        /// <summary>
        /// 
        /// </summary>
        public float M12;
        /// <summary>
        /// 
        /// </summary>
        public float M13;
        /// <summary>
        /// 
        /// </summary>
        public float M14;
        /// <summary>
        /// 
        /// </summary>
        public float M21;
        /// <summary>
        /// 
        /// </summary>
        public float M22;
        /// <summary>
        /// 
        /// </summary>
        public float M23;
        /// <summary>
        /// 
        /// </summary>
        public float M24;
        /// <summary>
        /// 
        /// </summary>
        public float M31;
        /// <summary>
        /// 
        /// </summary>
        public float M32;
        /// <summary>
        /// 
        /// </summary>
        public float M33;
        /// <summary>
        /// 
        /// </summary>
        public float M34;
        /// <summary>
        /// 
        /// </summary>
        public float M41;
        /// <summary>
        /// 
        /// </summary>
        public float M42;
        /// <summary>
        /// 
        /// </summary>
        public float M43;
        /// <summary>
        /// 
        /// </summary>
        public float M44;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m13"></param>
        /// <param name="m14"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="m23"></param>
        /// <param name="m24"></param>
        /// <param name="m31"></param>
        /// <param name="m32"></param>
        /// <param name="m33"></param>
        /// <param name="m34"></param>
        /// <param name="m41"></param>
        /// <param name="m42"></param>
        /// <param name="m43"></param>
        /// <param name="m44"></param>
        internal Matrix4x4(float m11, float m12, float m13, float m14
            , float m21, float m22, float m23, float m24
            , float m31, float m32, float m33, float m34
            , float m41, float m42, float m43, float m44
            )
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }
    }
}
