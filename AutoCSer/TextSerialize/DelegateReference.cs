using AutoCSer.Metadata;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// 序列化委托循环引用信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct DelegateReference
    {
        /// <summary>
        /// 引用循环执行类型
        /// </summary>
        internal AutoCSer.TextSerialize.PushTypeEnum PushType;
        /// <summary>
        /// 是否存在未知子节点
        /// </summary>
        internal bool IsUnknownMember;
        /// <summary>
        /// 循环引用是否需要检查成员类型
        /// </summary>
        internal bool IsCheckMember;
        /// <summary>
        /// 状态是否计算完成
        /// </summary>
        internal bool IsCompleted;
        /// <summary>
        /// 序列化委托
        /// </summary>
        internal SerializeDelegate Delegate;
#if AOT
        /// <summary>
        /// 需要循环引用检查的类型
        /// </summary>
        internal Type[]? ReferenceTypes;
#else
        /// <summary>
        /// 需要循环引用检查的类型
        /// </summary>
#if NetStandard21
        internal GenericType[]? ReferenceTypes;
#else
        internal GenericType[] ReferenceTypes;
#endif
#endif
        /// <summary>
        /// 序列化委托循环引用信息
        /// </summary>
        /// <param name="delegateValue">序列化委托</param>
        internal DelegateReference(Delegate delegateValue)
        {
            PushType = default(AutoCSer.TextSerialize.PushTypeEnum);
            IsUnknownMember = IsCheckMember = false;
            ReferenceTypes = null;
            Delegate = new SerializeDelegate(delegateValue, EmptyArray<Type>.Array);
            IsCompleted = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetNoLoop(Delegate delegateValue)
        {
            Delegate.Delegate = delegateValue;
            IsCompleted = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="delegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetUnknown(Type type, Delegate delegateValue)
        {
            Delegate.Delegate = delegateValue;
            PushType = type.IsValueType ? AutoCSer.TextSerialize.PushTypeEnum.UnknownDepthCount : AutoCSer.TextSerialize.PushTypeEnum.Push;
            IsCompleted = IsUnknownMember = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetMember(ref SerializeDelegate delegateValue)
        {
            Delegate = delegateValue;
            IsCheckMember = true;
            //PushType = SerializePushType.Push;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="referenceTypes"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetMember(Delegate delegateValue, Type[] referenceTypes)
        {
            Delegate.Set(delegateValue, referenceTypes);
            IsCheckMember = true;
            //PushType = SerializePushType.Push;
        }
        ///// <summary>
        ///// 设置自定义序列化委托
        ///// </summary>
        ///// <param name="delegateValue"></param>
        ///// <param name="pushLoopType"></param>
        ///// <param name="isUnknownMember"></param>
        ///// <param name="checkLoopType"></param>
        ///// <param name="isCompleted"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void Set(Delegate delegateValue, SerializeLoopType pushLoopType, bool isUnknownMember, SerializeLoopType checkLoopType, bool isCompleted)
        //{
        //    Delegate.Delegate = delegateValue;
        //    PushLoopType = pushLoopType;
        //    IsUnknownMember = isUnknownMember;
        //    CheckLoopType = checkLoopType;
        //    IsCompleted = isCompleted;
        //}
    }
}
