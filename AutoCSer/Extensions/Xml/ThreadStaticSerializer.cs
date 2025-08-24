using System;
using System.Runtime.CompilerServices;

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
        /// 释放资源
        /// </summary>
        ~ThreadStaticSerializer()
        {
            Serializer.Dispose();
        }

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
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static XmlSerializer Get()
        {
            return (value ?? (value = new ThreadStaticSerializer())).Serializer;
        }
    }
}
