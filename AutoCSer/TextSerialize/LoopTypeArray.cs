using AutoCSer.Metadata;
using System;

namespace AutoCSer.TextSerialize
{

    /// <summary>
    /// 序列化委托循环引用检查数组
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct LoopTypeArray
    {
        /// <summary>
        /// 需要循环引用检查的类型
        /// </summary>
#if NetStandard21
        internal Type?[]? ReferenceTypes;
#else
        internal Type[] ReferenceTypes;
#endif
#if AOT
        /// <summary>
        /// 需要循环引用检查的类型
        /// </summary>
        internal Type[]? ReferenceGenericTypes;
#else
        /// <summary>
        /// 需要循环引用检查的类型
        /// </summary>
#if NetStandard21
        internal GenericType[]? ReferenceGenericTypes;
#else
        internal GenericType[] ReferenceGenericTypes;
#endif
#endif
        /// <summary>
        /// 当前检查的类型位置
        /// </summary>
        internal int Index;
        /// <summary>
        /// 设置检查数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal void Set(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            ReferenceTypes = serializeDelegateReference.Delegate.ReferenceTypes;
            ReferenceGenericTypes = serializeDelegateReference.ReferenceTypes;
            Index = 0;
        }
        /// <summary>
        /// 设置检查数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal void Set(AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            Set(ref serializeDelegateReference);
        }
    }
}
