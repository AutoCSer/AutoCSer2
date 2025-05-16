using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 公共配置，该类型不允许增加依赖成员
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 根配置
        /// </summary>
        internal static IRoot Root = AutoCSer.Configuration.Root.Null;
        /// <summary>
        /// 配置缓存是否已经加载
        /// </summary>
        internal static bool IsConfigLoaded;
        /// <summary>
        /// 设置根配置，用于如果不是 Assembly.GetEntryAssembly() 或者不希望扫描程序集的场景
        /// </summary>
        /// <param name="root">根配置</param>
        /// <returns>false 表示配置缓存已经加载，设置无效</returns>
        public static bool SetRoot(IRoot root)
        {
            if (IsConfigLoaded || root == null) return false;
            Root = root;
            return true;
        }

        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置名称，默认为 null 表示默认名称</param>
        /// <returns>配置项数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ConfigObject? Get(Type type, string name = "")
#else
        public static ConfigObject Get(Type type, string name = "")
#endif
        {
            return Cache.Get(type, name);
        }
        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置名称，默认为 null 表示默认名称</param>
        /// <returns>配置项数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ConfigObject?> GetAsync(Type type, string name = "")
#else
        public static Task<ConfigObject> GetAsync(Type type, string name = "")
#endif
        {
            var creator = Cache.GetCreator(type, name);
            return creator != null ? creator.CreateAsync() : CompletedTask<ConfigObject>.Default;
        }
        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">配置名称，默认为 null 表示默认名称</param>
        /// <returns>配置项数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ConfigObject<T>? Get<T>(string name = "") where T : class
#else
        public static ConfigObject<T> Get<T>(string name = "") where T : class
#endif
        {
#if NetStandard21
            return (ConfigObject<T>?)Cache.Get(typeof(T), name);
#else
            return (ConfigObject<T>)Cache.Get(typeof(T), name);
#endif
        }
        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">配置名称，默认为 null 表示默认名称</param>
        /// <returns>配置项数据</returns>
#if NetStandard21
        public static async ValueTask<ConfigObject<T>?> GetAsync<T>(string name = "") where T : class
#else
        public static async Task<ConfigObject<T>> GetAsync<T>(string name = "") where T : class
#endif
        {
            var creator = Cache.GetCreator(typeof(T), name);
#if NetStandard21
            return creator != null ? (ConfigObject<T>?)await creator.CreateAsync() : null;
#else
            return creator != null ? (ConfigObject<T>)await creator.CreateAsync() : null;
#endif
        }
    }
}
