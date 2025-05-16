using System;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 调用默认构造函数
    /// </summary>
    internal sealed class CallDefaultConstructor
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        private readonly ConstructorInfo constructor;
        /// <summary>
        /// 调用默认构造函数
        /// </summary>
        /// <param name="constructor"></param>
        internal CallDefaultConstructor(ConstructorInfo constructor)
        {
            this.constructor = constructor;
        }
        /// <summary>
        /// 调用默认构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal T Call<T>()
        {
            return (T)constructor.Invoke(null);
        }
    }
}
