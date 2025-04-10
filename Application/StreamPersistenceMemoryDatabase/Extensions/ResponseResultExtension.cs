﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 缓存返回结果扩展操作
    /// </summary>
    public static class ResponseResultExtension
    {
        ///// <summary>
        ///// JSON 字符串转换为对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static ResponseResult<T> FromJson<T>(this ResponseResult<string> json)
        //{
        //    if (json.IsSuccess)
        //    {
        //        var value = default(T);
        //        AutoCSer.Json.DeserializeResult result = AutoCSer.JsonDeserializer.Deserialize(json.Value.notNull(), ref value);
        //        if (result.State == AutoCSer.Json.DeserializeStateEnum.Success) return value;
        //        return CommandClientReturnTypeEnum.ClientDeserializeError;
        //    }
        //    return json.Cast<T>();
        //}
        ///// <summary>
        ///// JSON 字符串转换为对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static ResponseResult<T> FromJson<T>(this ResponseResult<ValueResult<string>> json)
        //{
        //    if (json.IsSuccess)
        //    {
        //        if (json.Value.IsValue)
        //        {
        //            var value = default(T);
        //            AutoCSer.Json.DeserializeResult result = AutoCSer.JsonDeserializer.Deserialize(json.Value.Value, ref value);
        //            if (result.State == AutoCSer.Json.DeserializeStateEnum.Success) return value;
        //            return CommandClientReturnTypeEnum.ClientDeserializeError;
        //        }
        //        return CallStateEnum.NoReturnValue;
        //    }
        //    return json.Cast<T>();
        //}
        ///// <summary>
        ///// 二进制序列化数据转换为对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static ResponseResult<T> FromBinary<T>(this ResponseResult<byte[]> data)
        //{
        //    if (data.IsSuccess)
        //    {
        //        var value = default(T);
        //        AutoCSer.BinarySerialize.DeserializeResult result = AutoCSer.BinaryDeserializer.Deserialize(data.Value.notNull(), ref value);
        //        if (result.State == AutoCSer.BinarySerialize.DeserializeStateEnum.Success) return value;
        //        return CommandClientReturnTypeEnum.ClientDeserializeError;
        //    }
        //    return data.Cast<T>();
        //}
        ///// <summary>
        ///// 二进制序列化数据转换为对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static ResponseResult<T> FromBinary<T>(this ResponseResult<ValueResult<byte[]>> data)
        //{
        //    if (data.IsSuccess)
        //    {
        //        if (data.Value.IsValue)
        //        {
        //            var value = default(T);
        //            AutoCSer.BinarySerialize.DeserializeResult result = AutoCSer.BinaryDeserializer.Deserialize(data.Value.Value, ref value);
        //            if (result.State == AutoCSer.BinarySerialize.DeserializeStateEnum.Success) return value;
        //            return CommandClientReturnTypeEnum.ClientDeserializeError;
        //        }
        //        return CallStateEnum.NoReturnValue;
        //    }
        //    return data.Cast<T>();
        //}
        /// <summary>
        /// 二进制位数据转 bool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ResponseResult<bool> ToBool(this ResponseResult<ValueResult<int>> value)
        {
            if (value.IsSuccess)
            {
                if (value.Value.IsValue) return value.Value.Value != 0;
                return CallStateEnum.NoReturnValue;
            }
            return value.Cast<bool>();
        }
    }
}
