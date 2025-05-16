using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 程序集缓存
    /// </summary>
    internal sealed class AssemblyCache
    {
        /// <summary>
        /// 程序集全名称
        /// </summary>
        private readonly string fullName;
        /// <summary>
        /// 程序集名称
        /// </summary>
        private readonly string name;
        /// <summary>
        /// 程序集
        /// </summary>
        private readonly Assembly assembly;
        /// <summary>
        /// 程序集缓存
        /// </summary>
        /// <param name="assembly">程序集</param>
        private AssemblyCache(Assembly assembly)
        {
            var fullName = assembly.FullName;
            if (fullName == null) throw new ArgumentNullException($"{nameof(assembly)}.{nameof(assembly.FullName)}");
            this.assembly = assembly;
            this.fullName = fullName;
            int nameIndex = fullName.IndexOf(',');
            name = nameIndex > 0 ? fullName.Substring(0, nameIndex) : string.Empty;
        }

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="fullName">程序集名称</param>
        /// <returns>程序集,失败返回null</returns>
#if NetStandard21
        public static Assembly? Get(string fullName)
#else
        public static Assembly Get(string fullName)
#endif
        {
            var assemblyCache = lastAssembly;
            if (assemblyCache?.fullName == fullName) return assemblyCache.assembly;
            while (cache.TryGetValue(fullName, out assemblyCache))
            {
                if (assemblyCache.fullName == fullName)
                {
                    lastAssembly = assemblyCache;
                    return assemblyCache.assembly;
                }
            }
            int nameIndex = fullName.IndexOf(',');
            if (nameIndex > 0)
            {
                string name = fullName.Substring(0, nameIndex);
                assemblyCache = lastNameAssembly;
                if (assemblyCache?.name == name) return assemblyCache.assembly;
                while (nameCache.TryGetValue(name, out assemblyCache))
                {
                    if (assemblyCache.name == name)
                    {
                        lastNameAssembly = assemblyCache;
                        return assemblyCache.assembly;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 程序集缓存
        /// </summary>
        private static readonly Dictionary<string, AssemblyCache> cache = DictionaryCreator<string>.Create<AssemblyCache>();
        /// <summary>
        /// 程序集缓存
        /// </summary>
        private static readonly Dictionary<string, AssemblyCache> nameCache = DictionaryCreator<string>.Create<AssemblyCache>();
        /// <summary>
        /// 最后一次访问的程序集
        /// </summary>
#if NetStandard21
        private static AssemblyCache? lastAssembly;
#else
        private static AssemblyCache lastAssembly;
#endif
        /// <summary>
        /// 最后一次访问的程序集
        /// </summary>
#if NetStandard21
        private static AssemblyCache? lastNameAssembly;
#else
        private static AssemblyCache lastNameAssembly;
#endif
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
#if NetStandard21
        private static void loadAssembly(object? sender, AssemblyLoadEventArgs args)
#else
        private static void loadAssembly(object sender, AssemblyLoadEventArgs args)
#endif
        {
            AssemblyCache assemblyCache = new AssemblyCache(args.LoadedAssembly);
            string key = assemblyCache.fullName, name = assemblyCache.name;
            Monitor.Enter(cache);
            try
            {
                cache[key] = assemblyCache;
                if (name != null) nameCache[name] = assemblyCache;
            }
            finally { Monitor.Exit(cache); }
        }

        static AssemblyCache()
        {
            AppDomain.CurrentDomain.AssemblyLoad += loadAssembly;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyCache assemblyCache = new AssemblyCache(assembly);
                cache[assemblyCache.fullName] = assemblyCache;
                if(assemblyCache.name != null) nameCache[assemblyCache.name] = assemblyCache;
            }
        }
    }
}
