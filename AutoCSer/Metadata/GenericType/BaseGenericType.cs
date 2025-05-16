using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class BaseGenericType : GenericTypeCache<BaseGenericType>
    {
#if !AOT
        /// <summary>
        /// 获取 JSON 基类序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetJsonSerializeBaseDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference);
        /// <summary>
        /// JSON 基类反序列化委托
        /// </summary>
        internal abstract Delegate JsonDeserializeBaseDelegate { get; }

        /// <summary>
        /// 获取基类二进制序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetBinarySerializeBaseDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference);
        /// <summary>
        /// 获取基类二进制序列化委托
        /// </summary>
        internal abstract Delegate BinarySerializeBaseDelegate { get; }
        /// <summary>
        /// 获取基类二进制反序列化委托
        /// </summary>
        internal abstract Delegate BinaryDeserializeBaseDelegate { get; }
#endif

        /// <summary>
        /// 获取控制器创建器
        /// </summary>
        /// <param name="controllerCreator"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controllerCreator, string? controllerName);
#else
        internal abstract AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controllerCreator, string controllerName);
#endif
        /// <summary>
        /// 获取控制器创建器
        /// </summary>
        /// <param name="controllerCreator"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreatorWithCommandListener(object controllerCreator, string? controllerName);
#else
        internal abstract AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreatorWithCommandListener(object controllerCreator, string controllerName);
#endif

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static BaseGenericType create<T, BT>()
            where T : class, BT
        {
            return new BaseGenericType<T, BT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static BaseGenericType? lastGenericType;
#else
        protected static BaseGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static BaseGenericType Get(Type type, Type baseType)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;

            try
            {
                if (cache.TryGetValue(type, out value) && value.CurrentType == type)
                {
                    lastGenericType = value;
                    return value;
                }
            }
            catch { }

            cacheLock.EnterYield();
            try
            {
                if (!cache.TryGetValue(type, out value))
                {
                    value = createMethod.MakeGenericMethod(type, baseType).Invoke(null, null).notNullCastType<BaseGenericType>();
                    cache.Add(type, value);
                }
            }
            finally { cacheLock.Exit(); }
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="BT"></typeparam>
    internal sealed class BaseGenericType<T, BT> : BaseGenericType
        where T : class, BT
    {
        /// <summary>
        /// 引用类型数组
        /// </summary>
        internal static readonly Type[] ReferenceTypes = new Type[] { typeof(BT) };
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
#if !AOT
        /// <summary>
        /// 获取 JSON 基类序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetJsonSerializeBaseDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<JsonSerializer, T>)JsonSerializer.Base<T, BT>, ReferenceTypes);
        }
        /// <summary>
        /// JSON 基类反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeBaseDelegate { get { return (JsonDeserializer.DeserializeDelegate<T?>)JsonDeserializer.Base<T, BT>; } }
#else
        internal override Delegate JsonDeserializeBaseDelegate { get { return (JsonDeserializer.DeserializeDelegate<T>)JsonDeserializer.Base<T, BT>; } }
#endif
        /// <summary>
        /// 获取基类二进制序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetBinarySerializeBaseDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<BinarySerializer, T>)BinarySerializer.Base<T, BT>, ReferenceTypes, BinarySerialize.SerializePushTypeEnum.Primitive);
        }
        /// <summary>
        /// 获取基类二进制序列化委托
        /// </summary>
        internal override Delegate BinarySerializeBaseDelegate { get { return (Action<BinarySerializer, T>)BinarySerializer.Base<T, BT>; } }
        /// <summary>
        /// 获取基类二进制反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate BinaryDeserializeBaseDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T?>)BinaryDeserializer.Base<T, BT>; } }
#else
        internal override Delegate BinaryDeserializeBaseDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T>)BinaryDeserializer.Base<T, BT>; } }
#endif
#endif
        /// <summary>
        /// 获取控制器创建器
        /// </summary>
        /// <param name="controllerCreator"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        internal override AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controllerCreator, string? controllerName)
#else
        internal override AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controllerCreator, string controllerName)
#endif
        {
            return AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator<BT>((Func<T>)controllerCreator, controllerName);
        }
        /// <summary>
        /// 获取控制器创建器
        /// </summary>
        /// <param name="controllerCreator"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        internal override AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreatorWithCommandListener(object controllerCreator, string? controllerName)
#else
        internal override AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreatorWithCommandListener(object controllerCreator, string controllerName)
#endif
        {
            return AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator<BT>((Func<AutoCSer.Net.CommandListener, T>)controllerCreator, controllerName);
        }
    }
}