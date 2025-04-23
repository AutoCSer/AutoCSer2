using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// System.Numerics.Matrix3x2
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.XmlSerialize]
#endif
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 6)]
    internal partial struct SerializeMatrix3x2
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
        public float M21;
        /// <summary>
        /// 
        /// </summary>
        public float M22;
        /// <summary>
        /// 
        /// </summary>
        public float M31;
        /// <summary>
        /// 
        /// </summary>
        public float M32;
    }
}
