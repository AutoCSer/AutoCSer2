using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 根配置接口
    /// </summary>
    public interface IRoot
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        IEnumerable<Type> MainTypes { get; }
        /// <summary>
        /// 公共配置类型集合
        /// </summary>
        IEnumerable<Type> PublicTypes { get; }
        /// <summary>
        /// 缓存类型加载异常（注意不要同步阻塞配置线程）
        /// </summary>
        /// <param name="exceptionTypes">缓存类型加载异常</param>
        Task OnLoadException(LeftArray<KeyValue<Type, Exception>> exceptionTypes);
    }
    /// <summary>
    /// 根配置
    /// </summary>
    public class Root : IRoot
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public virtual IEnumerable<Type> MainTypes { get { return EmptyArray<Type>.Array; } }
        /// <summary>
        /// 公共配置类型集合
        /// </summary>
        public virtual IEnumerable<Type> PublicTypes { get { return EmptyArray<Type>.Array; } }
        /// <summary>
        /// 缓存类型加载异常
        /// </summary>
        /// <param name="exceptionTypes">缓存类型加载异常</param>
        public virtual async Task OnLoadException(LeftArray<KeyValue<Type, Exception>> exceptionTypes)
        {
            foreach (KeyValue<Type, Exception> exception in exceptionTypes)
            {
                await AutoCSer.LogHelper.Exception(exception.Value, exception.Key.fullName(), LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
        }
        /// <summary>
        /// 缓存类型加载异常
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
        /// 默认空配置
        /// </summary>
        internal static readonly Root Null = new Root();
    }
}
