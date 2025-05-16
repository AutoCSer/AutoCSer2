using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 自定义序列化委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SerializeDelegate
    {
        /// <summary>
        /// 自定义序列化委托
        /// </summary>
        public Delegate Delegate;
        /// <summary>
        /// 成员自定义序列化委托（需要判断是否为 null）
        /// </summary>
#if NetStandard21
        internal Delegate? MemberDelegate;
#else
        internal Delegate MemberDelegate;
#endif
        /// <summary>
        /// 需要检查引用的类型（包含当前类型），数组长度为 0 表示无需检查引用，null 表示未知
        /// </summary>
#if NetStandard21
        public Type?[]? ReferenceTypes;
#else
        public Type[] ReferenceTypes;
#endif
        /// <summary>
        /// 是否集合
        /// </summary>
        internal bool IsCollection;
        /// <summary>
        /// 自定义二进制序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制序列化委托必须是静态方法，第一个参数类型为 AutoCSer.BinarySerializer，第二参数类型为具体数据类型，返回值类型为 void</param>
        /// <param name="referenceTypes">需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知</param>
#if NetStandard21
        public SerializeDelegate(Delegate delegateValue, Type?[]? referenceTypes = null)
#else
        public SerializeDelegate(Delegate delegateValue, Type[] referenceTypes = null)
#endif
        {
            Delegate = delegateValue;
            MemberDelegate = null;
            ReferenceTypes = referenceTypes;
            IsCollection = false;
        }
        /// <summary>
        /// 自定义二进制序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制序列化委托</param>
        /// <param name="referenceTypes">需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知</param>
        /// <param name="isCollection">是否集合</param>
        internal SerializeDelegate(Delegate delegateValue, Type[] referenceTypes, bool isCollection = false)
        {
            Delegate = delegateValue;
            MemberDelegate = null;
            ReferenceTypes = referenceTypes;
            IsCollection = isCollection;
        }
#if AOT
        /// <summary>
        /// 自定义二进制序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制序列化委托必须是静态方法，第一个参数类型为 AutoCSer.BinarySerializer，第二参数类型为具体数据类型，返回值类型为 void</param>
        /// <param name="memberDelegateValue">二进制序列化委托</param>
        /// <param name="referenceTypes">需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知</param>
        public SerializeDelegate(Delegate delegateValue, Delegate? memberDelegateValue, Type?[]? referenceTypes = null)
        {
            Delegate = delegateValue;
            MemberDelegate = memberDelegateValue;
            ReferenceTypes = referenceTypes;
            IsCollection = false;
        }
        ///// <summary>
        ///// 自定义二进制序列化委托
        ///// </summary>
        ///// <param name="delegateValue">二进制序列化委托必须是静态方法，第一个参数类型为 AutoCSer.BinarySerializer，第二参数类型为具体数据类型，返回值类型为 void</param>
        ///// <param name="referenceTypes">需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知</param>
        //internal SerializeDelegate(KeyValue<Delegate, Delegate> delegateValue, Type?[]? referenceTypes = null)
        //{
        //    Delegate = delegateValue.Key;
        //    MemberDelegate = delegateValue.Value;
        //    ReferenceTypes = referenceTypes;
        //    IsCollection = false;
        //}
        ///// <summary>
        ///// 自定义二进制序列化委托
        ///// </summary>
        ///// <param name="delegateValue">二进制序列化委托</param>
        ///// <param name="referenceTypes">需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知</param>
        ///// <param name="isCollection">是否集合</param>
        //internal SerializeDelegate(KeyValue<Delegate, Delegate> delegateValue, Type[] referenceTypes, bool isCollection = false)
        //{
        //    Delegate = delegateValue.Key;
        //    MemberDelegate = delegateValue.Value;
        //    ReferenceTypes = referenceTypes;
        //    IsCollection = isCollection;
        //}
#else
        /// <summary>
        /// 自定义二进制序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制序列化委托</param>
        /// <param name="memberDelegateValue">二进制序列化委托</param>
        /// <param name="referenceTypes">需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知</param>
        /// <param name="isCollection">是否集合</param>
#if NetStandard21
        internal SerializeDelegate(Delegate delegateValue, Delegate? memberDelegateValue, Type[] referenceTypes, bool isCollection = false)
#else
        internal SerializeDelegate(Delegate delegateValue, Delegate memberDelegateValue, Type[] referenceTypes, bool isCollection = false)
#endif
        {
            Delegate = delegateValue;
            MemberDelegate = memberDelegateValue;
            ReferenceTypes = referenceTypes;
            IsCollection = isCollection;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">自定义序列化委托，第一个参数类型为 AutoCSer.BinarySerializer，第二参数类型为具体数据类型，返回值类型为 void</param>
        /// <returns>自定义序列化委托</returns>
        public static implicit operator SerializeDelegate(Delegate value) { return new SerializeDelegate(value); }
#endif
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="memberDelegateValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(Delegate delegateValue, Delegate memberDelegateValue)
        {
            Delegate = delegateValue;
            MemberDelegate = memberDelegateValue;
        }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="memberDelegateValue"></param>
        /// <param name="referenceTypes"></param>
        /// <param name="isCollection"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Set(Delegate delegateValue, Delegate? memberDelegateValue, Type[] referenceTypes, bool isCollection)
#else
        internal void Set(Delegate delegateValue, Delegate memberDelegateValue, Type[] referenceTypes, bool isCollection)
#endif
        {
            Delegate = delegateValue;
            MemberDelegate = memberDelegateValue;
            ReferenceTypes = referenceTypes;
            IsCollection = isCollection;
        }
        /// <summary>
        /// 获取成员自定义序列化委托（需要判断是否为 null）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Delegate GetMemberDelegate()
        {
            return MemberDelegate ?? Delegate;
        }

        /// <summary>
        /// 检查自定义二进制序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Check(Type type, ref SerializeDelegateReference serializeDelegateReference)
        {
            var checkType = default(Type);
            if (Delegate != null && Check(ref serializeDelegateReference, out checkType))
            {
                if (type == checkType) return true;
                AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型序列化函数数据类型不匹配 {type.fullName()} <> {checkType.fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            return false;
        }
#if !AOT
        /// <summary>
        /// 检查自定义序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool Check([MaybeNullWhen(false)] out Type type, out SerializeDelegateReference serializeDelegateReference)
#else
        internal bool Check(out Type type, out SerializeDelegateReference serializeDelegateReference)
#endif
        {
            serializeDelegateReference = default(SerializeDelegateReference);
            if (Delegate != null)
            {
                if (Check(ref serializeDelegateReference, out type)) return true;
            }
            else AutoCSer.LogHelper.ErrorIgnoreException("自定义类型序列化函数缺少委托对象", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            type = null;
            return false;
        }
#endif
        /// <summary>
        /// 检查自定义序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        private bool Check(ref SerializeDelegateReference serializeDelegateReference, [MaybeNullWhen(false)] out Type type)
#else
        private bool Check(ref SerializeDelegateReference serializeDelegateReference, out Type type)
#endif
        {
            MethodInfo methodInfo = Delegate.Method;
            if (methodInfo.IsStatic)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length == 2)
                {
                    if (parameters[0].ParameterType == typeof(BinarySerializer))
                    {
                        if (!FieldSize.IsFixedSize(type = parameters[1].ParameterType))
                        {
                            if (ReferenceTypes == null) serializeDelegateReference.SetUnknown(type, Delegate, MemberDelegate);
                            else if (ReferenceTypes.Length == 0) serializeDelegateReference.SetNotReference(Delegate, MemberDelegate);
                            else serializeDelegateReference.SetMember(ref this);
                            return true;
                        }
                        AutoCSer.LogHelper.ErrorIgnoreException($"固定字节数类型 {type.fullName()} 不允许自定义类型序列化", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
                    }
                    else AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 第一个参数类型必须为 {typeof(AutoCSer.BinarySerializer).fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
                }
                else AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 参数数量 {parameters.Length.toString()} 不匹配", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
                type = null;
            }
            else
            {
                type = null;
                AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 必须为非静态函数", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            return false;
        }
    }
}
