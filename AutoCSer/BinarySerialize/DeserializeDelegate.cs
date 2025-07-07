using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 自定义反序列化委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeserializeDelegate
    {
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
        public Delegate Delegate;
        /// <summary>
        /// 成员自定义反序列化委托（需要判断是否为 null）
        /// </summary>
#if NetStandard21
        internal Delegate? MemberDelegate;
#else
        internal Delegate MemberDelegate;
#endif
        /// <summary>
        /// 是否内部类型
        /// </summary>
        internal bool IsPrimitive;
#if AOT
        ///// <summary>
        ///// 自定义二进制反序列化委托
        ///// </summary>
        ///// <param name="delegateValue">二进制反序列化委托，必须是静态方法，第一个参数类型为 AutoCSer.BinaryDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void</param>
        //internal DeserializeDelegate(KeyValue<Delegate, Delegate> delegateValue)
        //{
        //    Delegate = delegateValue.Key;
        //    MemberDelegate = delegateValue.Value;
        //    IsPrimitive = false;
        //}
        /// <summary>
        /// 自定义二进制反序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制反序列化委托</param>
        /// <param name="isPrimitive">是否内部类型</param>
        internal DeserializeDelegate(KeyValue<Delegate, Delegate> delegateValue, bool isPrimitive)
        {
            Delegate = delegateValue.Key;
            MemberDelegate = delegateValue.Value;
            IsPrimitive = isPrimitive;
        }
        /// <summary>
        /// 自定义二进制反序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制反序列化委托</param>
        /// <param name="memberDelegateValue">二进制反序列化委托</param>
        /// <param name="isPrimitive">是否内部类型</param>
        internal DeserializeDelegate(Delegate delegateValue, Delegate memberDelegateValue, bool isPrimitive)
        {
            Delegate = delegateValue;
            MemberDelegate = memberDelegateValue;
            IsPrimitive = isPrimitive;
        }
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="isPrimitive"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(KeyValue<Delegate, Delegate> delegateValue, bool isPrimitive = false)
        {
            Delegate = delegateValue.Key;
            MemberDelegate = delegateValue.Value;
            IsPrimitive = isPrimitive;
        }
#else
        /// <summary>
        /// 自定义二进制反序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制反序列化委托，必须是静态方法，第一个参数类型为 AutoCSer.BinaryDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void</param>
        public DeserializeDelegate(Delegate delegateValue)
        {
            Delegate = delegateValue;
            MemberDelegate = null;
            IsPrimitive = false;
        }
        /// <summary>
        /// 自定义二进制反序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制反序列化委托</param>
        /// <param name="isPrimitive">是否内部类型</param>
        internal DeserializeDelegate(Delegate delegateValue, bool isPrimitive)
        {
            Delegate = delegateValue;
            MemberDelegate = null;
            IsPrimitive = isPrimitive;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value">自定义反序列化委托</param>
        /// <returns>自定义反序列化委托</returns>
        public static implicit operator DeserializeDelegate(Delegate value) { return new DeserializeDelegate(value); }
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="isPrimitive"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(Delegate delegateValue, bool isPrimitive = false)
        {
            Delegate = delegateValue;
            MemberDelegate = null;
            IsPrimitive = isPrimitive;
        }
#endif
        /// <summary>
        /// 自定义二进制反序列化委托
        /// </summary>
        /// <param name="delegateValue">二进制序反列化委托</param>
        /// <param name="memberDelegateValue">二进制反序列化委托</param>
        internal DeserializeDelegate(Delegate delegateValue, Delegate memberDelegateValue)
        {
            Delegate = delegateValue;
            MemberDelegate = memberDelegateValue;
            IsPrimitive = true;
        }
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
        /// <param name="delegateValue"></param>
        /// <param name="memberDelegate"></param>
        /// <param name="isPrimitive"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Set(Delegate delegateValue, Delegate? memberDelegate, bool isPrimitive = false)
#else
        internal void Set(Delegate delegateValue, Delegate memberDelegate, bool isPrimitive = false)
#endif
        {
            Delegate = delegateValue;
            MemberDelegate = memberDelegate;
            IsPrimitive = isPrimitive;
        }
        /// <summary>
        /// 获取成员反序列化委托
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Delegate GetMemberDelegate()
        {
            return MemberDelegate ?? Delegate;
        }

        /// <summary>
        /// 判断自定义反序列化委托数据类型是否与委托匹配
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal bool Check(Type type)
        {
            if (Delegate != null)
            {
                var checkType = Check();
                if (type == checkType) return true;
                if (checkType != null) AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型反序列化函数数据类型不匹配 {type.fullName()} <> {checkType.fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            return false;
        }
        /// <summary>
        /// 获取自定义反序列化委托数据类型
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal Type? Check()
#else
        internal Type Check()
#endif
        {
            var type = AutoCSer.Common.CheckDeserializeType(typeof(BinaryDeserializer), Delegate);
            if (type != null)
            {
                if (!FieldSize.IsFixedSize(type)) return type;
                AutoCSer.LogHelper.ErrorIgnoreException($"固定字节数类型 {type.fullName()} 不允许自定义类型序列化", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            return null;
        }
    }
}
