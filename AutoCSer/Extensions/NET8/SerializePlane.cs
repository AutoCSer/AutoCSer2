using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.XmlSerialize]
#endif
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 4)]
    internal partial struct SerializePlane
    {
        /// <summary>
        /// 
        /// </summary>
        public SerializeVector3 Normal;
        /// <summary>
        /// 
        /// </summary>
        public float D;
    }
}
