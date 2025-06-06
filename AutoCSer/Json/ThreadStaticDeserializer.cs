﻿using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 反序列化线程静态变量
    /// </summary>
    internal sealed class ThreadStaticDeserializer
    {
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        internal readonly JsonDeserializer Deserializer = new JsonDeserializer();

        /// <summary>
        /// 线程静态变量
        /// </summary>
        [ThreadStatic]
#if NetStandard21
        private static ThreadStaticDeserializer? value;
#else
        private static ThreadStaticDeserializer value;
#endif
        /// <summary>
        /// 创建线程静态变量访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock createLock;
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        internal static ThreadStaticDeserializer Get()
        {
            return value ?? get();
        }
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        private static ThreadStaticDeserializer get()
        {
            createLock.EnterSleep();
            try
            {
                if (value == null) value = new ThreadStaticDeserializer();
            }
            finally { createLock.Exit(); }
            return value;
        }
    }
}
