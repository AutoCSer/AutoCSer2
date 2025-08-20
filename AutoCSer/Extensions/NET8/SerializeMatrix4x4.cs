using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [AutoCSer.CodeGenerator.XmlSerialize]
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 16)]
    internal partial struct SerializeMatrix4x4
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
    }
}
