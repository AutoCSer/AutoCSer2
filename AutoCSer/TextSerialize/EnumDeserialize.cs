﻿using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using AutoCSer.Extensions;

namespace AutoCSer.TextSerialize
{
    /// <summary>
    /// Deserialization of enumeration values
    /// 枚举值反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class EnumDeserialize<T> where T : struct, IConvertible
    {
        /// <summary>
        /// 枚举值集合
        /// </summary>
        protected static readonly T[] enumValues;
        /// <summary>
        /// 枚举名称查找数据
        /// </summary>
        protected static Pointer enumSearchData;

        static EnumDeserialize()
        {
            Dictionary<string, T> values = ((T[])System.Enum.GetValues(typeof(T))).getDictionary(value => value.ToString().notNull());
            enumValues = values.getArray(value => value.Value);
            enumSearchData = AutoCSer.StateSearcher.CharBuilder.Create(values.getArray(value => value.Key), true);
        }
    }
}
