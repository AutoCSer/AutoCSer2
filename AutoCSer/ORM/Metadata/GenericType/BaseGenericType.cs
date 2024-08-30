using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class BaseGenericType : AutoCSer.Metadata.GenericTypeCache<BaseGenericType>
    {
        /// <summary>
        /// 持久化表格模型类型构造函数
        /// </summary>
        internal abstract Delegate BusinessConstructorDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static BaseGenericType create<T, BT>()
            where T : class, BT
            where BT : class
        {
            return new BaseGenericType<T, BT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static BaseGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static BaseGenericType Get(Type type, Type baseType)
        {
            BaseGenericType value = lastGenericType;
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
                    value = (BaseGenericType)createMethod.MakeGenericMethod(type, baseType).Invoke(null, null);
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
        where BT : class
    {
        /// <summary>
        /// 引用类型数组
        /// </summary>
        private static readonly Type[] referenceTypes = new Type[] { typeof(BT) };
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 持久化表格模型类型构造函数
        /// </summary>
        internal override Delegate BusinessConstructorDelegate { get { return (Func<BT>)BusinessPersistence.BusinessConstructor<T, BT>; } }
    }
}
