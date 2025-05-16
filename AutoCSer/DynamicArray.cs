using AutoCSer.Configuration;
using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 动态数组信息
    /// </summary>
    internal static class DynamicArray
    {
        /// <summary>
        /// 默认数组容器长度
        /// </summary>
        internal const int DefalutArrayCapacity = sizeof(int);
        /// <summary>
        /// 是否需要清除数组缓存信息
        /// </summary>
#if NetStandard21
        private static Dictionary<HashObject<System.Type>, bool>? isClearArrayCache;
#else
        private static Dictionary<HashObject<System.Type>, bool> isClearArrayCache;
#endif
        /// <summary>
        /// 是否需要清除数组缓存 访问锁
        /// </summary>
        private static LockObject isClearArrayLock = new LockObject(new object());
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>需要清除数组</returns>
        public static bool IsClearArray(Type type)
        {
            if (type.IsPointer) return false;
            if (type.IsClass || type.IsInterface) return true;
            if (type.IsEnum) return false;
            if (type.IsValueType)
            {
                bool isClear;
                isClearArrayLock.Enter();
                try
                {
                    if (isClearArrayCache != null)
                    {
                        if (isClearArrayCache.TryGetValue(type, out isClear)) return isClear;
                    }
                    else isClearArrayCache = DictionaryCreator.CreateHashObject<System.Type, bool>();
                    isClearArrayCache.Add(type, true);
                    isClearArrayCache[type] = isClear = isClearArray(type, isClearArrayCache);
                }
                finally { isClearArrayLock.Exit(); }
                return isClear;
            }
            return true;
        }
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isClearArrayCache"></param>
        /// <returns>需要清除数组</returns>
        private static bool isClearArray(Type type, Dictionary<HashObject<System.Type>, bool> isClearArrayCache)
        {
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Type fieldType = field.FieldType;
                if (fieldType != type && !fieldType.IsPointer)
                {
                    if (fieldType.IsClass || fieldType.IsInterface) return true;
                    if (!fieldType.IsEnum)
                    {
                        if (fieldType.IsValueType)
                        {
                            bool isClear;
                            if (!isClearArrayCache.TryGetValue(fieldType, out isClear))
                            {
                                isClearArrayCache.Add(fieldType, true);
                                isClearArrayCache[fieldType] = isClear = isClearArray(fieldType, isClearArrayCache);
                            }
                            if (isClear) return true;
                        }
                        else return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            if (isClearArrayCache != null) TaskQueue.AddDefault(clearCacheTask);
        }
        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCacheTask()
        {
            isClearArrayLock.Enter();
            isClearArrayCache = null;
            isClearArrayLock.Exit();
        }
        static DynamicArray()
        {
            int clearSeconds = AutoCSer.Common.Config.GetMemoryCacheClearSeconds();
            if (clearSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(clearCache, clearSeconds, Threading.SecondTimerKeepModeEnum.After, clearSeconds);
            //AutoCSer.Memory.Common.AddClearCache(clearCache, AutoCSer.Common.Config.GetMemoryCacheClearSeconds());
        }
    }
    /// <summary>
    /// 动态数组基类
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    internal static class DynamicArray<T>
    {
        /// <summary>
        /// 是否需要清除数组
        /// </summary>
        internal static readonly bool IsClearArray = DynamicArray.IsClearArray(typeof(T));
        /// <summary>
        /// 创建新数组
        /// </summary>
        /// <param name="capacity">数组长度</param>
        /// <returns>数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T[] GetNewArray(int capacity)
        {
            return new T[(uint)capacity <= ((int.MaxValue >> 1) + 1) ? (int)((uint)capacity).upToPower2() : int.MaxValue];
        }
    }
}
