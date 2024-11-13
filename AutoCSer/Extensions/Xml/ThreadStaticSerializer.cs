using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 序列化线程静态变量
    /// </summary>
    internal sealed class ThreadStaticSerializer
    {
        /// <summary>
        /// XML 序列化
        /// </summary>
        internal readonly XmlSerializer Serializer = new XmlSerializer(true);

        /// <summary>
        /// 线程静态变量
        /// </summary>
        [ThreadStatic]
#if NetStandard21
        private static ThreadStaticSerializer? value;
#else
        private static ThreadStaticSerializer value;
#endif
        /// <summary>
        /// 创建线程静态变量访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock createLock;
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        internal static ThreadStaticSerializer Get()
        {
            return value ?? get();
        }
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        private static ThreadStaticSerializer get()
        {
            createLock.EnterSleep();
            try
            {
                if (value == null) value = new ThreadStaticSerializer();
            }
            finally { createLock.Exit(); }
            return value;
        }
    }
}
