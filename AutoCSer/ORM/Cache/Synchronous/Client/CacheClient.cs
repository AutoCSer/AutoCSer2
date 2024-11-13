using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Cache.Synchronous
{
    /// <summary>
    /// 缓存数据同步客户端
    /// </summary>
    /// <typeparam name="T">客户端缓存对象类型</typeparam>
    public abstract class CacheClient<T> where T : class
    {
        /// <summary>
        /// 当前客户端缓存对象
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public T Cache { get; private set; }
        /// <summary>
        /// 创建中的缓存
        /// </summary>
#if NetStandard21
        protected T? createCache;
#else
        protected T createCache;
#endif
        /// <summary>
        /// 缓存数据初始化传输完毕
        /// </summary>
        /// <param name="cache"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Loaded(T cache)
        {
            if (object.ReferenceEquals(System.Threading.Interlocked.CompareExchange(ref createCache, null, cache), cache)) Cache = cache;
        }
    }
    /// <summary>
    /// 缓存数据同步客户端
    /// </summary>
    /// <typeparam name="CT">客户端缓存对象类型</typeparam>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">缓存关键字类型</typeparam>
    public sealed class CacheClient<CT, T, KT> : CacheClient<CT>
        where CT : class
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 获取缓存数据委托
        /// </summary>
        public readonly Func<Action<CommandClientReturnValue<CallbackValue<T>>, KeepCallbackCommand>, KeepCallbackCommand> GetCache;
        /// <summary>
        /// 获取缓存关键字委托
        /// </summary>
        public readonly Func<T, KT> GetKey;
        /// <summary>
        /// 创建客户端缓存对象委托
        /// </summary>
        private readonly Func<CacheClient<CT, T, KT>, CT> createClientCache;
        /// <summary>
        /// 缓存数据同步客户端
        /// </summary>
        /// <param name="getCache">获取缓存数据委托</param>
        /// <param name="getKey">获取缓存数据委托</param>
        /// <param name="createClientCache">创建客户端缓存对象委托</param>
        public CacheClient(Func<Action<CommandClientReturnValue<CallbackValue<T>>, KeepCallbackCommand>, KeepCallbackCommand> getCache, Func<T, KT> getKey, Func<CacheClient<CT, T, KT>, CT> createClientCache)
        {
            GetCache = getCache;
            GetKey = getKey;
            this.createClientCache = createClientCache;
            Create();
        }
        /// <summary>
        /// 重新创建客户端缓存对象
        /// </summary>
        internal void Create()
        {
            try
            {
                createCache = createClientCache(this);
            }
            catch (Exception exception)
            {
                LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (createCache == null) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(Create, 1);
            }
        }
    }
}
