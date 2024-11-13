using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.RandomObject.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateMemberDelegate { get; }
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal abstract Delegate CreateArrayDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static GenericType? lastGenericType;
#else
        protected static GenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 创建随机对象
        /// </summary>
#if NetStandard21
        internal override Delegate CreateDelegate { get { return (Func<Config, bool, T?>)Creator.Create<T>; } }
#else
        internal override Delegate CreateDelegate { get { return (Func<Config, bool, T>)Creator.Create<T>; } }
#endif
        /// <summary>
        /// 创建随机对象
        /// </summary>
        internal override Delegate CreateMemberDelegate { get { return (Creator<T>.MemberCreator)Creator.CreateMember<T>; } }
        /// <summary>
        /// 创建随机对象
        /// </summary>
#if NetStandard21
        internal override Delegate CreateArrayDelegate { get { return (Func<Config, T?[]?>)Creator.CreateArray<T>; } }
#else
        internal override Delegate CreateArrayDelegate { get { return (Func<Config, T[]>)Creator.CreateArray<T>; } }
#endif
    }
}
