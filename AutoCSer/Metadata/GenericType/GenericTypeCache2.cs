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
    /// <typeparam name="T"></typeparam>
    internal abstract class GenericTypeCache2<T> where T : GenericTypeCache2<T>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal abstract Type CurrentType1 { get; }
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal abstract Type CurrentType2 { get; }

        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(T).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic).notNull();
        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static Dictionary<HashKey<HashObject<System.Type>, HashObject<System.Type>>, T> cache = AutoCSer.DictionaryCreator<HashKey<HashObject<System.Type>, HashObject<System.Type>>>.Create<T>();
        /// <summary>
        /// 泛型类型元数据缓存 访问锁
        /// </summary>
        protected static AutoCSer.Threading.SpinLock cacheLock;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        protected static T get(Type type1, Type type2)
        {
            var value = default(T);
            HashKey<HashObject<System.Type>, HashObject<System.Type>> hashKey = new HashKey<HashObject<System.Type>, HashObject<System.Type>>(type1, type2);
            try
            {
                if (cache.TryGetValue(hashKey, out value) && value.CurrentType2 == type2 && value.CurrentType1 == type1) return value;
            }
            catch { }

            cacheLock.EnterYield();
            try
            {
                if (!cache.TryGetValue(hashKey, out value))
                {
                    value = (T)createMethod.MakeGenericMethod(type1, type2).Invoke(null, null).notNull();
                    cache.Add(hashKey, value);
                }
            }
            finally { cacheLock.Exit(); }
            return value;
        }

        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            if (cache.Count != 0) TaskQueue.AddDefault(clearCacheTask);
        }
        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCacheTask()
        {
            if (cacheLock.TryEnter())
            {
                try
                {
                    if (cache.Count != 0) cache = AutoCSer.DictionaryCreator<HashKey<HashObject<System.Type>, HashObject<System.Type>>>.Create<T>();
                }
                finally { cacheLock.Exit(); }
            }
        }

        static GenericTypeCache2()
        {
            int clearSeconds = AutoCSer.Common.Config.GetMemoryCacheClearSeconds();
            if (clearSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(clearCache, clearSeconds, Threading.SecondTimerKeepModeEnum.After, clearSeconds);
            //AutoCSer.Memory.Common.AddClearCache(clearCache, AutoCSer.Common.Config.GetMemoryCacheClearSeconds());
        }
    }
}
