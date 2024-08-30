using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;

namespace AutoCSer.ORM.Cache.Synchronous
{
    /// <summary>
    /// 缓存数据同步客户端关键字缓存
    /// </summary>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">缓存关键字类型</typeparam>
    public sealed class DictionaryCache<T, KT>
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 缓存数据同步客户端
        /// </summary>
        private readonly CacheClient<DictionaryCache<T, KT>, T, KT> client;
        /// <summary>
        /// 客户端字典缓存
        /// </summary>
        private readonly Dictionary<RandomKey<KT>, T> cache;
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get { return cache.Count; } }
        /// <summary>
        /// 获取关键字集合
        /// </summary>
        public IEnumerable<KT> Keys
        {
            get
            {
                foreach (RandomKey<KT> key in cache.Keys) yield return key;
            }
        }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        public IEnumerable<T> Values { get { return cache.Values; } }
        /// <summary>
        /// 根据关键字获取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[KT key]
        {
            get
            {
                T value;
                return cache.TryGetValue(key, out value) ? value : null;
            }
        }
        /// <summary>
        /// 当前命令
        /// </summary>
        private KeepCallbackCommand keepCallbackCommand;
        /// <summary>
        /// 获取缓存异常次数
        /// </summary>
        private int exceptionCount;
        /// <summary>
        /// 缓存数据同步客户端关键字缓存
        /// </summary>
        /// <param name="client">缓存数据同步客户端</param>
        private DictionaryCache(CacheClient<DictionaryCache<T, KT>, T, KT> client)
        {
            this.client = client;
            cache = DictionaryCreator<RandomKey<KT>>.Create<T>();
            tryGetCache();
        }
        /// <summary>
        /// 尝试请求缓存数据
        /// </summary>
        private void tryGetCache()
        {
            try
            {
                keepCallbackCommand = client.GetCache(callback);
            }
            catch (Exception exception)
            {
                if (exceptionCount++ == 0) LogHelper.ExceptionIgnoreException(exception);
            }
            finally
            {
                if (keepCallbackCommand == null) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(tryGetCache, 1);
            }
        }
        /// <summary>
        /// 缓存数据回调处理
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="keepCallbackCommand"></param>
        private void callback(CommandClientReturnValue<CallbackValue<T>> returnValue, KeepCallbackCommand keepCallbackCommand)
        {
            if (returnValue.IsSuccess)
            {
                T value = returnValue.Value.Value;
                switch (returnValue.Value.OperationType)
                {
                    case OperationTypeEnum.Cache: cache.Add(client.GetKey(value), value); return;
                    case OperationTypeEnum.Loaded: client.Loaded(this); return;
                    case OperationTypeEnum.Insert: cache.Add(client.GetKey(value), value); return;
                    case OperationTypeEnum.Update: cache[client.GetKey(value)] = value; return;
                    case OperationTypeEnum.Delete: cache.Remove(client.GetKey(value)); return;
                }
            }
            client.Create();
        }

        /// <summary>
        /// 创建缓存数据同步客户端关键字缓存
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static DictionaryCache<T, KT> createCache(CacheClient<DictionaryCache<T, KT>, T, KT> client)
        {
            return new DictionaryCache<T, KT>(client);
        }
        /// <summary>
        /// 创建缓存数据同步客户端
        /// </summary>
        /// <param name="getCache"></param>
        /// <param name="getKey"></param>
        /// <returns></returns>
        public static CacheClient<DictionaryCache<T, KT>, T, KT> CreateClient(Func<Action<CommandClientReturnValue<CallbackValue<T>>, KeepCallbackCommand>, KeepCallbackCommand> getCache, Func<T, KT> getKey)
        {
            return new CacheClient<DictionaryCache<T, KT>, T, KT>(getCache, getKey, createCache);
        }
    }
}
