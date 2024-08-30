using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 调用数据视图默认构造函数
        /// </summary>
        internal abstract AutoCSer.NetCoreWeb.View CallNetCoreWebViewDefaultConstructor { get; }
        /// <summary>
        /// 调用数据视图中间件默认构造函数
        /// </summary>
        internal abstract AutoCSer.NetCoreWeb.ViewMiddleware CallNetCoreWebViewMiddlewareDefaultConstructor { get; }

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
        /// 调用数据视图默认构造函数
        /// </summary>
        internal override AutoCSer.NetCoreWeb.View CallNetCoreWebViewDefaultConstructor { get { return AutoCSer.Metadata.DefaultConstructor<T>.Constructor() as AutoCSer.NetCoreWeb.View; } }
        /// <summary>
        /// 调用数据视图中间件默认构造函数
        /// </summary>
        internal override AutoCSer.NetCoreWeb.ViewMiddleware CallNetCoreWebViewMiddlewareDefaultConstructor { get { return AutoCSer.Metadata.DefaultConstructor<T>.Constructor() as AutoCSer.NetCoreWeb.ViewMiddleware; } }
    }
}
