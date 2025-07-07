using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据 基类
    /// </summary>
    /// <typeparam name="T">泛型类型元数据类型</typeparam>
    internal abstract class GenericTypeCache<T> where T : GenericTypeCache<T>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal abstract Type CurrentType { get; }

        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        protected static readonly MethodInfo createMethod = typeof(T).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic).notNull();
        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        protected static Dictionary<HashObject<System.Type>, T> cache = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, T>();
        /// <summary>
        /// 泛型类型元数据缓存 访问锁
        /// </summary>
        protected static AutoCSer.Threading.SpinLock cacheLock;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static T get(Type type)
        {
            var value = default(T);
            try
            {
                if (cache.TryGetValue(type, out value) && value.CurrentType == type) return value;
            }
            catch { }

            cacheLock.EnterYield();
            try
            {
                if (!cache.TryGetValue(type, out value))
                {
                    value = (T)createMethod.MakeGenericMethod(type).Invoke(null, null).notNull();
                    cache.Add(type, value);
                }
            }
            finally { cacheLock.Exit(); }
            return value;
        }
#if !AOT
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static T getEnum(Type type)
        {
            var value = default(T);
            try
            {
                if (cache.TryGetValue(type, out value) && value.CurrentType == type) return value;
            }
            catch { }

            Type underlyingType = System.Enum.GetUnderlyingType(type);
            cacheLock.EnterYield();
            try
            {
                if (!cache.TryGetValue(type, out value))
                {
                    value = (T)createMethod.MakeGenericMethod(type, underlyingType).Invoke(null, null).notNull();
                    cache.Add(type, value);
                }
            }
            finally { cacheLock.Exit(); }
            return value;
        }
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">IDictionary 类型</param>
        /// <returns></returns>
        protected static T getDictionary(Type type, Type interfaceType)
        {
            var value = default(T);
            try
            {
                if (cache.TryGetValue(type, out value) && value.CurrentType == type) return value;
            }
            catch { }

            Type[] types = interfaceType.GetGenericArguments();
            cacheLock.EnterYield();
            try
            {
                if (!cache.TryGetValue(type, out value))
                {
                    value = (T)createMethod.MakeGenericMethod(type, types[0], types[1]).Invoke(null, null).notNull();
                    cache.Add(type, value);
                }
            }
            finally { cacheLock.Exit(); }
            return value;
        }
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">IDictionary 类型</param>
        /// <returns></returns>
        protected static T getCollection(Type type, Type interfaceType)
        {
            var value = default(T);
            try
            {
                if (cache.TryGetValue(type, out value) && value.CurrentType == type) return value;
            }
            catch { }

            Type[] types = interfaceType.GetGenericArguments();
            cacheLock.EnterYield();
            try
            {
                if (!cache.TryGetValue(type, out value))
                {
                    value = (T)createMethod.MakeGenericMethod(type, types[0]).Invoke(null, null).notNull();
                    cache.Add(type, value);
                }
            }
            finally { cacheLock.Exit(); }
            return value;
        }
#endif

        /// <summary>
        /// Clear cache data at regular intervals
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            if (cache.Count != 0) TaskQueue.AddDefault(clearCacheTask);
        }
        /// <summary>
        /// Clear cache data at regular intervals
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCacheTask()
        {
            if (cacheLock.TryEnter())
            {
                try
                {
                    if (cache.Count != 0) cache = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, T>();
                }
                finally { cacheLock.Exit(); }
            }
        }

        static GenericTypeCache()
        {
            int clearSeconds = AutoCSer.Common.Config.GetMemoryCacheClearSeconds();
            if (clearSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(clearCache, clearSeconds, Threading.SecondTimerKeepModeEnum.After, clearSeconds);
            //AutoCSer.Memory.Common.AddClearCache(clearCache, AutoCSer.Common.Config.GetMemoryCacheClearSeconds());
        }
    }
}
