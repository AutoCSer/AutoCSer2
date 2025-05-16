using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 历史对象指针位置
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ObjectReferenceType
    {
        /// <summary>
        /// 对象类型
        /// </summary>
        private Type type;
        /// <summary>
        /// 对象类型数组
        /// </summary>
        internal LeftArray<Type> TypeArray;
        /// <summary>
        /// 历史对象指针位置
        /// </summary>
        internal int Point
        {
            get { return TypeArray.Reserve; }
        }
        /// <summary>
        /// 历史对象指针位置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="point"></param>
        internal ObjectReferenceType(Type type, int point)
        {
            this.type = type;
            TypeArray = new LeftArray<Type>(0);
            TypeArray.Reserve = point;
        }
        /// <summary>
        /// 添加对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal bool Append(Type type)
        {
            if (this.type == type || TypeArray.Contains(type)) return false;
            TypeArray.Add(type);
            return true;
        }
    }
}
