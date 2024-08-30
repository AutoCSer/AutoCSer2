using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 参数鉴权
        /// </summary>
        internal abstract Delegate CheckAccessTokenParameterDelegate { get; }

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
        protected static GenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
        {
            GenericType value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 参数鉴权
        /// </summary>
        internal override Delegate CheckAccessTokenParameterDelegate { get { return (Func<ViewMiddleware, T, Task<ResponseResult>>)ViewMiddleware.CheckAccessTokenParameter<T>; } }
    }
}
