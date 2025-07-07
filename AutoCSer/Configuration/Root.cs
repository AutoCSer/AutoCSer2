using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// Root configuration interface
    /// 根配置接口
    /// </summary>
    public interface IRoot
    {
        /// <summary>
        /// Main configuration type collection
        /// 主配置类型集合
        /// </summary>
        IEnumerable<Type> MainTypes { get; }
        /// <summary>
        /// Collection of public configuration types
        /// 公共配置类型集合
        /// </summary>
        IEnumerable<Type> PublicTypes { get; }
        /// <summary>
        /// Configuration cache type loading exception (Note: Do not synchronously block the configuration thread)
        /// 配置缓存类型加载异常（注意不要同步阻塞配置线程）
        /// </summary>
        /// <param name="exceptionTypes">Configuration cache type loading exception
        /// 配置缓存类型加载异常</param>
        Task OnLoadException(LeftArray<KeyValue<Type, Exception>> exceptionTypes);
    }
    /// <summary>
    /// Root configuration
    /// 根配置
    /// </summary>
    public class Root : IRoot
    {
        /// <summary>
        /// Main configuration type collection
        /// 主配置类型集合
        /// </summary>
        public virtual IEnumerable<Type> MainTypes { get { return EmptyArray<Type>.Array; } }
        /// <summary>
        /// Collection of public configuration types
        /// 公共配置类型集合
        /// </summary>
        public virtual IEnumerable<Type> PublicTypes { get { return EmptyArray<Type>.Array; } }
        /// <summary>
        /// Configuration cache type loading exception (Note: Do not synchronously block the configuration thread)
        /// 配置缓存类型加载异常（注意不要同步阻塞配置线程）
        /// </summary>
        /// <param name="exceptionTypes">Configuration cache type loading exception
        /// 配置缓存类型加载异常</param>
        public virtual async Task OnLoadException(LeftArray<KeyValue<Type, Exception>> exceptionTypes)
        {
            foreach (KeyValue<Type, Exception> exception in exceptionTypes)
            {
                await AutoCSer.LogHelper.Exception(exception.Value, exception.Key.fullName(), LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
        }
        /// <summary>
        /// Configuration cache type loading exception
        /// 配置缓存类型加载异常
        /// </summary>
        /// <param name="root"></param>
        /// <param name="exceptionTypes"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static async Task OnLoadException(IRoot root, LeftArray<KeyValue<Type, Exception>> exceptionTypes)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await root.OnLoadException(exceptionTypes);
        }
        /// <summary>
        /// Default empty configuration
        /// </summary>
        internal static readonly Root Null = new Root();
    }
}
