﻿using System;
using System.Security.Cryptography;

namespace AutoCSer.CommandService.TimestampVerify
{
    /// <summary>
    /// 反向服务验证数据
    /// </summary>
    /// <typeparam name="T">附加数据类型</typeparam>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReverseServiceVerifyData<T>
    {
        /// <summary>
        /// 随机值
        /// </summary>
        private ulong random;
        /// <summary>
        /// 附加数据
        /// </summary>
        public T Data;
        /// <summary>
        /// MD5 数据
        /// </summary>
        private byte[] hashData;
        /// <summary>
        /// 反向服务验证数据
        /// </summary>
        /// <param name="timestamp">待验证时间戳</param>
        /// <param name="data">附加数据</param>
        public ReverseServiceVerifyData(long timestamp, T data)
        {
            ReverseServiceClientData<T> clientData = new ReverseServiceClientData<T>(timestamp, data);
            random = clientData.Random;
            Data = data;
            using (MD5 md5 = MD5.Create()) hashData = md5.ComputeHash(AutoCSer.BinarySerializer.Serialize(clientData));
        }
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="timestamp">待验证时间戳</param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public bool Verify(long timestamp, MD5 md5)
        {
            if (this.hashData != null)
            {
                ReverseServiceClientData<T> clientData = new ReverseServiceClientData<T>(timestamp, random, ref Data);
                byte[] hashData = md5.ComputeHash(AutoCSer.BinarySerializer.Serialize(clientData));
                return AutoCSer.Net.TimestampVerify.Md5Equals(this.hashData, hashData) == 0;
            }
            return false;
        }
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="timestamp">待验证时间戳</param>
        /// <returns></returns>
        public bool Verify(long timestamp)
        {
            using (MD5 md5 = MD5.Create()) return Verify(timestamp, md5);
        }
    }
}