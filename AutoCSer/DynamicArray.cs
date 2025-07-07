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
    /// Dynamic array information
    /// 动态数组信息
    /// </summary>
    internal static class DynamicArray
    {
        /// <summary>
        /// Default array container size
        /// 默认数组容器大小
        /// </summary>
        internal const int DefalutArrayCapacity = sizeof(int);

        /// <summary>
        /// Type cache of does the array element need to be cleared (If there are reference type members, a clearing operation is required to prevent memory leaks)
        /// 数组元素是否需要清除操作的类型缓存（存在引用类型成员则需要清除操作避免内存泄露）
        /// </summary>
#if NetStandard21
        private static Dictionary<HashObject<System.Type>, bool>? isClearArrayCache;
#else
        private static Dictionary<HashObject<System.Type>, bool> isClearArrayCache;
#endif
        /// <summary>
        /// The access lock of the array type cache
        /// 数组类型缓存的访问锁
        /// </summary>
        private static LockObject isClearArrayLock = new LockObject(new object());
        /// <summary>
        /// Does the array element need to be cleared
        /// 数组元素是否需要清除操作
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Does the array element need to be cleared
        /// 数组元素是否需要清除操作</returns>
        public static bool IsClearArray(Type type)
        {
            if (type.IsPointer) return false;
            if (type.IsClass || type.IsInterface) return true;
            if (type.IsEnum) return false;
            if (type.IsValueType)
            {
#if AOT
                if (!AutoCSer.Common.IsCodeGenerator) return !AutoCSer.BinarySerialize.FieldSize.IsFixedSize(type);
#endif
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
        /// Does the array element need to be cleared
        /// 数组元素是否需要清除操作
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isClearArrayCache"></param>
        /// <returns>Does the array element need to be cleared
        /// 数组元素是否需要清除操作</returns>
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
#if !AOT
        /// <summary>
        /// Clear cache data at regular intervals
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            if (isClearArrayCache != null) TaskQueue.AddDefault(clearCacheTask);
        }
        /// <summary>
        /// Clear cache data at regular intervals
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
#endif
    }
    /// <summary>
    /// Dynamic array base class
    /// 动态数组基类
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    internal static class DynamicArray<T>
    {
        /// <summary>
        /// Does the array element need to be cleared
        /// 数组元素是否需要清除操作
        /// </summary>
        internal static readonly bool IsClearArray = DynamicArray.IsClearArray(typeof(T));
        /// <summary>
        /// Create a new array
        /// 创建新数组
        /// </summary>
        /// <param name="capacity">Expected array container size
        /// 预期数组容器大小</param>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T[] GetNewArray(int capacity)
        {
            return new T[(uint)capacity <= ((int.MaxValue >> 1) + 1) ? (int)((uint)capacity).upToPower2() : int.MaxValue];
        }
    }
}
