﻿using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.XmlSerialize]
#endif
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 2)]
    internal partial struct SerializeVector2
    {
        /// <summary>
        /// 
        /// </summary>
        public float X;
        /// <summary>
        /// 
        /// </summary>
        public float Y;
    }
}
