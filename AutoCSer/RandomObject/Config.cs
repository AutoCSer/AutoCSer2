using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer;
using AutoCSer.Threading;

namespace AutoCSer.RandomObject
{
    /// <summary>
    /// 随机对象生成配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 最小时间值
        /// </summary>
        public DateTime MinDateTime = DateTime.MinValue;
        /// <summary>
        /// 最大时间值
        /// </summary>
        public DateTime MaxDateTime = DateTime.MaxValue;
        /// <summary>
        /// 最小数字
        /// </summary>
        public decimal MinDecimal = decimal.MinValue;
        /// <summary>
        /// 最大数字
        /// </summary>
        public decimal MaxDecimal = decimal.MaxValue;
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
        /// 字符是否ASCII
        /// </summary>
        public bool IsAscii;
        /// <summary>
        /// 是否生成字符0
        /// </summary>
        public bool IsNullChar = true;
        /// <summary>
        /// 是否保存历史对象
        /// </summary>
        public bool IsHistory = true;
        /// <summary>
        /// 获取自定义生成委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        public virtual Delegate? GetCustomCreator(Type type)
#else
        public virtual Delegate GetCustomCreator(Type type)
#endif
        {
            return null;
        }
        /// <summary>
        /// 创建不支持对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NetStandard21
        public virtual T? CreateNotSupport<T>()
#else
        public virtual T CreateNotSupport<T>()
#endif
        {
            return default(T);
        }
        /// <summary>
        /// 历史对象集合
        /// </summary>
#if NetStandard21
        private Dictionary<HashObject<System.Type>, ListArray<object>>? history;
#else
        private Dictionary<HashObject<System.Type>, ListArray<object>> history;
#endif
        /// <summary>
        /// 获取历史对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        internal object? TryGetValue(Type type)
#else
        internal object TryGetValue(Type type)
#endif
        {
            if (history != null && AutoCSer.Random.Default.NextBit() == 0)
            {
                Monitor.Enter(this);
                try
                {
                    if (history != null)
                    {
                        var objects = default(ListArray<object>);
                        if (history.TryGetValue(type, out objects)) return objects.Array.Array[AutoCSer.Random.Default.Next(objects.Array.Length)];
                    }
                }
                finally { Monitor.Exit(this); }
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
            var objects = default(ListArray<object>);
            Monitor.Enter(this);
            try
            {
                if (history == null) history = DictionaryCreator.CreateHashObject<System.Type, ListArray<object>>();
                if (!history.TryGetValue(type, out objects)) history.Add(type, objects = new ListArray<object>());
                objects.Add(value);
            }
            finally { Monitor.Exit(this); }
        }
        /// <summary>
        /// 清理历史对象集合
        /// </summary>
        internal void ClearHistory()
        {
            Monitor.Enter(this);
            try
            {
                if (history?.Count > 0) history = DictionaryCreator.CreateHashObject<System.Type, ListArray<object>>();
            }
            finally { Monitor.Exit(this); }
        }

        /// <summary>
        /// 创建字符串
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public string CreateString(char start, char end)
        {
            int length = Math.Abs(AutoCSer.Random.Default.Next(MaxArraySize)) + 1;
            char[] charArray = new char[length];
            int mod = end - start + 1;
            if (mod <= 0) mod += char.MaxValue + 1;
            while (length != 0) charArray[--length] = (char)(AutoCSer.Random.Default.NextUShort() % mod + start);
            return new string(charArray);
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DateTime CreateDateTime()
        {
            return Creator.CreateDateTime(this);
        }
    }
}
