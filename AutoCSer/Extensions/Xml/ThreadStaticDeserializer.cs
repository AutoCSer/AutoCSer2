using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 反序列化线程静态变量
    /// </summary>
    internal sealed class ThreadStaticDeserializer
    {
        /// <summary>
        /// XML 反序列化
        /// </summary>
        internal readonly XmlDeserializer Deserializer = new XmlDeserializer();

        /// <summary>
        /// 线程静态变量
        /// </summary>
        [ThreadStatic]
        private static ThreadStaticDeserializer value;
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
