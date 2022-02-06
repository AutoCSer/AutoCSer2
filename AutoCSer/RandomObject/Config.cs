using System;
using System.Collections.Generic;
using AutoCSer;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机对象生成配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 数组最大容量，默认为 15
        /// </summary>
        public int MaxArraySize = (1 << 4) - 1;
        /// <summary>
        /// 时间是否精确到秒
        /// </summary>
        public bool IsSecondDateTime;
        /// <summary>
        /// 浮点数是否转换成字符串
        /// </summary>
        public bool IsParseFloat;
        /// <summary>
        /// 是否生成字符0
        /// </summary>
        public bool IsNullChar = true;
        /// <summary>
        /// 是否保存历史对象
        /// </summary>
        public bool IsHistory = true;
        /// <summary>
        /// 创建不支持对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T CreateNotSupport<T>()
        {
            return default(T);
        }
        /// <summary>
        /// 历史对象集合
        /// </summary>
        private Dictionary<HashType, ListArray<object>> history;
        /// <summary>
        /// 获取历史对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal object TryGetValue(Type type)
        {
            if (history != null && AutoCSer.Random.Default.NextBit() == 0)
            {
                ListArray<object> objects;
                if (history.TryGetValue(type, out objects)) return objects.Array.Array[AutoCSer.Random.Default.Next(objects.Array.Length)];
            }
            return null;
        }
        /// <summary>
        /// 保存历史对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        internal void SaveHistory(Type type, object value)
        {
            if (!IsHistory || value == null) return;
            ListArray<object> objects;
            if (history == null) history = DictionaryCreator.CreateHashType<ListArray<object>>();
            if (!history.TryGetValue(type, out objects)) history.Add(type, objects = new ListArray<object>());
            objects.Add(value);
        }
        /// <summary>
        /// 清理历史对象集合
        /// </summary>
        internal void ClearHistory()
        {
            if (history?.Count > 0) history = DictionaryCreator.CreateHashType<ListArray<object>>();
        }
    }
}
