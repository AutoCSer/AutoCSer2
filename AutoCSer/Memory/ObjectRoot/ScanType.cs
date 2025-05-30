﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.Memory.ObjectRoot
{
    /// <summary>
    /// 待检测类型集合
    /// </summary>
    public static class ScanType
    {
        /// <summary>
        /// 待检测类型集合
        /// </summary>
        private static LeftArray<Type> types = new LeftArray<Type>(0);
        /// <summary>
        /// 待检测类型集合
        /// </summary>
        public static IEnumerable<Type> Types { get { return types; } }
        /// <summary>
        /// 待检测类型集合访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock typeLock;
        /// <summary>
        /// 添加待检测类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryAdd(Type type)
        {
            typeLock.EnterSleep();
            try
            {
                if (types.IndexOf(type) < 0)
                {
                    types.Add(type);
                    return true;
                }
            }
            finally { typeLock.Exit(); }
            return false;
        }
        /// <summary>
        /// 添加待检测类型
        /// </summary>
        /// <param name="type"></param>
        internal static void Add(Type type)
        {
            if (!AutoCSer.Common.Config.IsMemoryScanStaticType) return;
            typeLock.EnterSleep();
            try
            {
                types.Add(type);
            }
            finally { typeLock.Exit(); }
        }
    }
}
