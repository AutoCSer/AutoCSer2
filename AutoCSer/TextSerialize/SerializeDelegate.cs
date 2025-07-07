using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// Custom serialization委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SerializeDelegate
    {
        /// <summary>
        /// Custom serialization委托
        /// </summary>
#if NetStandard21
        public Delegate? Delegate;
#else
        public Delegate Delegate;
#endif
        /// <summary>
        /// 需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知
        /// </summary>
#if NetStandard21
        public Type?[]? ReferenceTypes;
#else
        public Type[] ReferenceTypes;
#endif
        /// <summary>
        /// Custom serialization委托
        /// </summary>
        /// <param name="delegateValue">序列化委托，必须是静态方法，第一个参数类型为 AutoCSer.JsonSerializer / AutoCSer.XmlSerializer，第二参数类型为具体数据类型，返回值类型为 void</param>
        /// <param name="referenceTypes">需要循环引用检查的类型，数组长度为 0 表示无需循环引用检查，null 表示未知</param>
#if NetStandard21
        public SerializeDelegate(Delegate delegateValue, Type?[]? referenceTypes = null)
#else
        public SerializeDelegate(Delegate delegateValue, Type[] referenceTypes = null)
#endif
        {
            Delegate = delegateValue;
            ReferenceTypes = referenceTypes;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value">自定义序列化委托，必须是静态方法，第一个参数类型为 AutoCSer.JsonSerializer / AutoCSer.XmlSerializer，第二参数类型为具体数据类型，返回值类型为 void</param>
        /// <returns>自定义序列化委托</returns>
        public static implicit operator SerializeDelegate(Delegate value) { return new SerializeDelegate(value); }
        /// <summary>
        /// 设置自定义序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="referenceTypes"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(Delegate delegateValue, Type[] referenceTypes)
        {
            Delegate = delegateValue;
            ReferenceTypes = referenceTypes;
        }
        /// <summary>
        /// 获取自定义序列化委托
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Delegate GetRemoveDelegate()
        {
            var delegateValue = Delegate.notNull();
            Delegate = null;
            return delegateValue;
        }
#if !AOT
        /// <summary>
        /// 检查自定义序列化委托
        /// </summary>
        /// <param name="serializerType"></param>
        /// <param name="type"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool Check(Type serializerType,[MaybeNullWhen(false)] out Type type, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#else
        internal bool Check(Type serializerType, out Type type, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#endif
        {
            serializeDelegateReference = default(AutoCSer.TextSerialize.DelegateReference);
            if (Delegate != null)
            {
                if (Check(serializerType, ref serializeDelegateReference, out type)) return true;
            }
            else AutoCSer.LogHelper.ErrorIgnoreException("自定义类型序列化函数缺少委托对象", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            type = null;
            return false;
        }
#endif
        /// <summary>
        /// 检查自定义序列化委托
        /// </summary>
        /// <param name="serializerType"></param>
        /// <param name="type"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Check(Type serializerType, Type type, ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            var checkType = default(Type);
            if (Delegate != null && Check(serializerType, ref serializeDelegateReference, out checkType))
            {
                if (type == checkType) return true;
                AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型序列化函数数据类型不匹配 {type.fullName()} <> {checkType.fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            return false;
        }
        /// <summary>
        /// 检查自定义序列化委托
        /// </summary>
        /// <param name="serializerType"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        private bool Check(Type serializerType, ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference,[MaybeNullWhen(false)] out Type type)
#else
        private bool Check(Type serializerType, ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, out Type type)
#endif
        {
            MethodInfo methodInfo = Delegate.notNull().Method;
            if (methodInfo.IsStatic)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length == 2)
                {
                    if (parameters[0].ParameterType == serializerType)
                    {
                        type = parameters[1].ParameterType;
                        if (ReferenceTypes == null) serializeDelegateReference.SetUnknown(type, Delegate.notNull());
                        else if (ReferenceTypes.Length == 0) serializeDelegateReference.SetNoLoop(Delegate.notNull());
                        else serializeDelegateReference.SetMember(ref this);
                        return true;
                    }
                    AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型序列化函数 {methodInfo.DeclaringType?.fullName()}.{methodInfo.Name} 第一个参数类型必须为 {serializerType.fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
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
