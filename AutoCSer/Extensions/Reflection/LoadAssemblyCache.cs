using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 动态加载程序集缓存
    /// </summary>
    public static class LoadAssemblyCache
    {
        /// <summary>
        /// 动态加载程序集缓存
        /// </summary>
        private static readonly Dictionary<HashBytes, KeyValue<Assembly, byte[]>> cache = DictionaryCreator<HashBytes>.Create<KeyValue<Assembly, byte[]>>();
        /// <summary>
        /// 动态加载程序集缓存访问锁
        /// </summary>
        private static readonly object cacheLock = new object();
        /// <summary>
        /// 动态加载程序集
        /// </summary>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <returns></returns>
        public static Assembly Load(ref byte[] rawAssembly)
        {
            HashBytes hashKey = rawAssembly;
            Monitor.Enter(cacheLock);
            KeyValue<Assembly, byte[]> assemblyBuffer;
            if (cache.TryGetValue(hashKey, out assemblyBuffer))
            {
                Monitor.Exit(cacheLock);
                rawAssembly = assemblyBuffer.Value;
                return assemblyBuffer.Key;
            }
            try
            {
                Assembly assembly = Assembly.Load(rawAssembly);
                cache.Add(hashKey, new KeyValue<Assembly, byte[]>(assembly, rawAssembly));
                return assembly;
            }
            finally { Monitor.Exit(cacheLock); }
        }
    }
}
