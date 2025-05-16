using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 序列化委托引用检查信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeDelegateReference
    {
        /// <summary>
        /// 引用执行类型
        /// </summary>
        internal SerializePushTypeEnum PushType;
        /// <summary>
        /// 作为根节点时是否需要添加屏蔽引用
        /// </summary>
        internal bool NotReferenceCount;
        /// <summary>
        /// 子节点存在引用检查需求
        /// </summary>
        private bool isReferenceMember;
        /// <summary>
        /// 状态是否计算完成
        /// </summary>
        internal bool IsCompleted;
        /// <summary>
        /// 自定义序列化委托
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
        /// <param name="memberDelegateValue">序列化委托</param>
#if NetStandard21
        internal SerializeDelegateReference(Delegate delegateValue, Delegate? memberDelegateValue = null)
#else
        internal SerializeDelegateReference(Delegate delegateValue, Delegate memberDelegateValue = null)
#endif
        {
            PushType = SerializePushTypeEnum.Primitive;
            isReferenceMember = false;
            ReferenceTypes = null;
            Delegate = new SerializeDelegate(delegateValue, memberDelegateValue, EmptyArray<Type>.Array);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="memberDelegateValue"></param>
#if NetStandard21
        internal void SetPrimitive(Delegate delegateValue, Delegate? memberDelegateValue = null)
#else
        internal void SetPrimitive(Delegate delegateValue, Delegate memberDelegateValue = null)
#endif
        {
            PushType = SerializePushTypeEnum.Primitive;
            Delegate.Set(delegateValue, memberDelegateValue, EmptyArray<Type>.Array, false);
            IsCompleted = NotReferenceCount = true;
        }
#if AOT
        /// <summary>
        /// 序列化委托循环引用信息
        /// </summary>
        /// <param name="delegateValue">JSON 序列化委托</param>
        /// <param name="referenceType">需要循环引用检查的类型</param>
        internal SerializeDelegateReference(Delegate delegateValue, Type referenceType)
        {
            PushType = SerializePushTypeEnum.Primitive;
            isReferenceMember = false;
            ReferenceTypes = new Type[] { referenceType };
            Delegate = new SerializeDelegate(delegateValue, null, EmptyArray<Type>.Array);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="referenceType"></param>
        internal void SetPrimitive(Delegate delegateValue, Type referenceType)
        {
            PushType = SerializePushTypeEnum.Primitive;
            ReferenceTypes = new Type[] { referenceType };
            Delegate.Set(delegateValue, null, EmptyArray<Type>.Array, false);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 序列化委托循环引用信息
        /// </summary>
        /// <param name="delegateValue">序列化委托</param>
        /// <param name="memberDelegateValue"></param>
        /// <param name="referenceType">需要循环引用检查的类型</param>
        internal SerializeDelegateReference(Delegate delegateValue, Delegate memberDelegateValue, Type referenceType)
        {
            PushType = SerializePushTypeEnum.Primitive;
            isReferenceMember = false;
            ReferenceTypes = new Type[] { referenceType };
            Delegate = new SerializeDelegate(delegateValue, memberDelegateValue, EmptyArray<Type>.Array);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="referenceType"></param>
        internal void SetPrimitive(KeyValue<Delegate, Delegate> delegateValue, Type referenceType)
        {
            PushType = SerializePushTypeEnum.Primitive;
            ReferenceTypes = new Type[] { referenceType };
            Delegate.Set(delegateValue.Key, delegateValue.Value, EmptyArray<Type>.Array, false);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="referenceTypes"></param>
        /// <param name="pushType"></param>
        /// <param name="isCollection"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetMember(KeyValue<Delegate, Delegate> delegateValue, Type[] referenceTypes, SerializePushTypeEnum pushType = SerializePushTypeEnum.DepthCount, bool isCollection = false)
        {
            PushType = pushType;
            Delegate.Set(delegateValue.Key, delegateValue.Value, referenceTypes, isCollection);
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="memberDelegateValue"></param>
        /// <param name="referenceTypes"></param>
        /// <param name="pushType"></param>
        /// <param name="isCollection"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetMember(Delegate delegateValue, Delegate memberDelegateValue, Type[] referenceTypes, SerializePushTypeEnum pushType = SerializePushTypeEnum.DepthCount, bool isCollection = false)
        {
            PushType = pushType;
            Delegate.Set(delegateValue, memberDelegateValue, referenceTypes, isCollection);
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="delegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetUnknown(Type type, KeyValue<Delegate, Delegate> delegateValue)
        {
            PushType = type.IsValueType ? SerializePushTypeEnum.DepthCount : SerializePushTypeEnum.TryReference;
            Delegate.Set(delegateValue.Key, delegateValue.Value, EmptyArray<Type>.Array, false);
            IsCompleted = isReferenceMember = true;
        }
        /// <summary>
        /// 获取序列化委托循环引用信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
        private static bool getSerializeDelegateReference(Type type, out SerializeDelegateReference serializeDelegateReference)
        {
            var serializeType = typeof(TypeSerializer<>).MakeGenericType(type);
            if (serializeType != null)
            {
                var field = serializeType.GetField("SerializeDelegateReference", BindingFlags.Static | BindingFlags.NonPublic);
                if (field != null)
                {
                    serializeDelegateReference = field.GetValue(null).castValue<SerializeDelegateReference>();
                    return true;
                }
                else AutoCSer.LogHelper.ExceptionIgnoreException(new MissingMemberException(serializeType.fullName(), "SerializeDelegateReference"));
            }
            else AutoCSer.LogHelper.ExceptionIgnoreException(new MissingMemberException(typeof(TypeSerializer<>).fullName(), type.fullName()));
            serializeDelegateReference = default(SerializeDelegateReference);
            return false;
        }
#else
        /// <summary>
        /// 序列化委托循环引用信息
        /// </summary>
        /// <param name="delegateValue">JSON 序列化委托</param>
        /// <param name="referenceType">需要循环引用检查的类型</param>
        internal SerializeDelegateReference(Delegate delegateValue, GenericType referenceType)
        {
            PushType = SerializePushTypeEnum.Primitive;
            isReferenceMember = false;
            ReferenceTypes = new GenericType[] { referenceType };
            Delegate = new SerializeDelegate(delegateValue, null, EmptyArray<Type>.Array);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="referenceType"></param>
        internal void SetPrimitive(Delegate delegateValue, GenericType referenceType)
        {
            PushType = SerializePushTypeEnum.Primitive;
            ReferenceTypes = new GenericType[] { referenceType };
            Delegate.Set(delegateValue, null, EmptyArray<Type>.Array, false);
            IsCompleted = NotReferenceCount = true;
        }
#endif
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="delegateValue"></param>
        /// <param name="memberDelegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void SetUnknown(Type type, Delegate delegateValue, Delegate? memberDelegateValue = null)
#else
        internal void SetUnknown(Type type, Delegate delegateValue, Delegate memberDelegateValue = null)
#endif
        {
            PushType = type.IsValueType ? SerializePushTypeEnum.DepthCount : SerializePushTypeEnum.TryReference;
            Delegate.Set(delegateValue, memberDelegateValue, EmptyArray<Type>.Array, false);
            IsCompleted = isReferenceMember = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="referenceTypes"></param>
        /// <param name="pushType"></param>
        /// <param name="isCollection"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetMember(Delegate delegateValue, Type[] referenceTypes, SerializePushTypeEnum pushType = SerializePushTypeEnum.DepthCount, bool isCollection = false)
        {
            PushType = pushType;
            Delegate.Set(delegateValue, null, referenceTypes, isCollection);
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="memberDelegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void SetNotReference(Delegate delegateValue, Delegate? memberDelegateValue = null)
#else
        internal void SetNotReference(Delegate delegateValue, Delegate memberDelegateValue = null)
#endif
        {
            PushType = SerializePushTypeEnum.NotReferenceCount;
            Delegate.Set(delegateValue, memberDelegateValue, EmptyArray<Type>.Array, false);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetTryReference(Delegate delegateValue)
        {
            PushType = SerializePushTypeEnum.TryReference;
            Delegate.Set(delegateValue, null, EmptyArray<Type>.Array, false);
            IsCompleted = NotReferenceCount = true;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetMember(ref SerializeDelegate delegateValue)
        {
            PushType = SerializePushTypeEnum.DepthCount;
            Delegate = delegateValue;
        }
        /// <summary>
        /// 子节点存在引用检查需求
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setReferenceMember(Type type)
        {
            if (PushType != SerializePushTypeEnum.Primitive) PushType = type.IsValueType ? SerializePushTypeEnum.DepthCount : SerializePushTypeEnum.TryReference;
            isReferenceMember = IsCompleted = true;
            Delegate.ReferenceTypes = null;
        }
        /// <summary>
        /// 计算状态完成检查
        /// </summary>
        /// <param name="genericType"></param>
#if AOT
        internal void CheckCompleted(Type genericType)
#else
        internal void CheckCompleted(GenericType genericType)
#endif
        {
            if (!IsCompleted)
            {
                int memberCount = 0;
#if AOT
                Type type = genericType;
                var lastGenericType = default(Type);
#else
                Type type = genericType.CurrentType;
                var lastGenericType = default(GenericType);
#endif
                bool notReferenceCount = true;
                foreach (var memberType in Delegate.ReferenceTypes.notNull())
                {
#if AOT
                    Type checkGenericType = memberType.notNull();
                    SerializeDelegateReference serializeDelegateReference;
                    if (getSerializeDelegateReference(checkGenericType, out serializeDelegateReference))
                    {
                        if (serializeDelegateReference.isReferenceMember || !serializeDelegateReference.IsCompleted)
                        {
                            setReferenceMember(genericType);
                            return;
                        }
                        if (serializeDelegateReference.ReferenceTypes != null)
                        {
                            if (Delegate.IsCollection)
                            {
                                setReferenceMember(genericType);
                                return;
                            }
                            lastGenericType = checkGenericType;
                            switch (memberCount++)
                            {
                                case 0:
                                    if (!genericType.IsValueType)
                                    {
                                        checkCompleted(genericType, notReferenceCount);
                                        return;
                                    }
                                    break;
                                case 1:
                                    checkCompleted(genericType, notReferenceCount);
                                    return;
                            }
                        }
                        notReferenceCount &= serializeDelegateReference.NotReferenceCount;
                    }
                    else
                    {
                        setReferenceMember(genericType);
                        return;
                    }
#else
                    GenericType checkGenericType = GenericType.Get(memberType.notNull());
                    SerializeDelegateReference serializeDelegateReference = checkGenericType.BinarySerializeDelegateReference;
                    if (serializeDelegateReference.isReferenceMember || !serializeDelegateReference.IsCompleted)
                    {
                        setReferenceMember(genericType.CurrentType);
                        return;
                    }
                    if (serializeDelegateReference.ReferenceTypes != null)
                    {
                        if (Delegate.IsCollection)
                        {
                            setReferenceMember(genericType.CurrentType);
                            return;
                        }
                        lastGenericType = checkGenericType;
                        switch (memberCount++)
                        {
                            case 0:
                                if (!genericType.CurrentType.IsValueType)
                                {
                                    checkCompleted(genericType, notReferenceCount);
                                    return;
                                }
                                break;
                            case 1:
                                checkCompleted(genericType, notReferenceCount);
                                return;
                        }
                    }
                    notReferenceCount &= serializeDelegateReference.NotReferenceCount;
#endif
                }
                if (memberCount == 0)
                {
                    if (!type.IsValueType)
                    {
                        if (PushType != SerializePushTypeEnum.Primitive) PushType = SerializePushTypeEnum.TryReference;
#if AOT
                        ReferenceTypes = new Type[] { genericType };
#else
                        ReferenceTypes = new GenericType[] { genericType };
#endif
                        completed(notReferenceCount);
                        return;
                    }
                }
                else
                {
#if AOT
                    SerializeDelegateReference serializeDelegateReference;
                    if (getSerializeDelegateReference(lastGenericType.notNull(), out serializeDelegateReference)) ReferenceTypes = serializeDelegateReference.ReferenceTypes;
                    else
                    {
                        setReferenceMember(genericType);
                        return;
                    }
#else
                    ReferenceTypes = lastGenericType.notNull().BinarySerializeDelegateReference.ReferenceTypes;
#endif
                }
                if (PushType != SerializePushTypeEnum.Primitive) PushType = SerializePushTypeEnum.DepthCount;
                completed(notReferenceCount);
            }
        }
        /// <summary>
        /// 计算状态完成检查
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="notReferenceCount"></param>
#if AOT
        private void checkCompleted(Type genericType, bool notReferenceCount)
#else
        private void checkCompleted(GenericType genericType, bool notReferenceCount)
#endif
        {
#if AOT
            Type type = genericType;
            Dictionary<HashObject<System.Type>, Type> types = DictionaryCreator.CreateHashObject<System.Type, Type>();
#else
            Type type = genericType.CurrentType;
            Dictionary<HashObject<System.Type>, GenericType> types = DictionaryCreator.CreateHashObject<System.Type, GenericType>();
#endif
            if (!type.IsValueType) types.Add(type, genericType);
            foreach (var memberType in Delegate.ReferenceTypes.notNull())
            {
#if AOT
                SerializeDelegateReference serializeDelegateReference;
                if (getSerializeDelegateReference(memberType.notNull(), out serializeDelegateReference))
                {
                    if (serializeDelegateReference.ReferenceTypes != null)
                    {
                        foreach (var checkGenericType in serializeDelegateReference.ReferenceTypes)
                        {
                            if (!types.TryAdd(checkGenericType, checkGenericType))
                            {
                                setReferenceMember(type);
                                return;
                            }
                        }
                    }
                    notReferenceCount &= serializeDelegateReference.NotReferenceCount;
                }
                else
                {
                    setReferenceMember(type);
                    return;
                }
#else
                SerializeDelegateReference serializeDelegateReference = GenericType.Get(memberType.notNull()).BinarySerializeDelegateReference;
                if (serializeDelegateReference.ReferenceTypes != null)
                {
                    foreach (var checkGenericType in serializeDelegateReference.ReferenceTypes)
                    {
                        if (!types.TryAdd(checkGenericType.CurrentType, checkGenericType))
                        {
                            setReferenceMember(type);
                            return;
                        }
                    }
                }
                notReferenceCount &= serializeDelegateReference.NotReferenceCount;
#endif
            }
            //SerializeReferenceTypeArray serializeReferenceTypeArray = new SerializeReferenceTypeArray(genericType);
            //foreach (Type memberType in Delegate.ReferenceTypes)
            //{
            //    SerializeDelegateReference serializeDelegateReference = GenericType.Get(memberType).BinarySerializeDelegateReference;
            //    if (serializeDelegateReference.ReferenceGenericTypes != null)
            //    {
            //        foreach (GenericType checkGenericType in serializeDelegateReference.ReferenceGenericTypes)
            //        {
            //            if (!serializeReferenceTypeArray.Append(checkGenericType))
            //            {
            //                setReferenceMember(type);
            //                return;
            //            }
            //        }
            //    }
            //}
            //ReferenceGenericTypes = serializeReferenceTypeArray.TypeArray.ToArray();
            if (PushType != SerializePushTypeEnum.Primitive) PushType = type.IsValueType ? SerializePushTypeEnum.DepthCount : SerializePushTypeEnum.TryReference;
            completed(notReferenceCount);
        }
        /// <summary>
        /// 计算状态完成
        /// </summary>
        /// <param name="notReferenceCount"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void completed(bool notReferenceCount)
        {
            NotReferenceCount = notReferenceCount;
            IsCompleted = true;
            Delegate.ReferenceTypes = null;
        }
    }
}
