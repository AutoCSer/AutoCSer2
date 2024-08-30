using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 表单值位置
    /// </summary>
    internal struct FormValueIndex
    {
        /// <summary>
        /// 名称起始位置
        /// </summary>
        private int nameIndex;
        /// <summary>
        /// 名称字节长度
        /// </summary>
        private int nameSize;
        /// <summary>
        /// 值起始位置
        /// </summary>
        private int valueIndex;
        /// <summary>
        /// 值字节长度
        /// </summary>
        private int valueSize;
        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="nameIndex"></param>
        /// <param name="nameSize"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetName(int nameIndex, int nameSize)
        {
            this.nameIndex = nameIndex;
            this.nameSize = nameSize;
        }
        /// <summary>
        /// 设置表单值
        /// </summary>
        /// <param name="nameIndex"></param>
        /// <param name="nameSize"></param>
        /// <param name="valueIndex"></param>
        /// <param name="valueSize"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int nameIndex, int nameSize, int valueIndex, int valueSize)
        {
            this.nameIndex = nameIndex;
            this.nameSize = nameSize;
            this.valueIndex = valueIndex;
            this.valueSize = valueSize;
        }
    }
}
