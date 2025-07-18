﻿using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AutoCSer.Net
{
    /// <summary>
    /// Service authentication helper function based on incremental login timestamp verification
    /// 基于递增登录时间戳验证的服务认证辅助函数
    /// </summary>
    public unsafe static class TimestampVerify
    {
        /// <summary>
        /// MD5 encryption
        /// MD5 加密
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="verifyString">Verify string
        /// 验证字符串</param>
        /// <param name="randomPrefix">Random prefix
        /// 随机前缀</param>
        /// <param name="timestamp">Timestamp to be verified
        /// 待验证时间戳</param>
        /// <returns></returns>
        public static byte[] Md5(MD5 md5, string verifyString, ulong randomPrefix, long timestamp)
        {
            int size = (verifyString.Length << 1) + (sizeof(ulong) + sizeof(long));
            ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(size
#if DEBUG
            , false
#endif
                );
            try
            {
                fixed (byte* dataFixed = buffer.GetFixedBuffer())
                {
                    byte* start = dataFixed + buffer.StartIndex;
                    *(ulong*)start = randomPrefix;
                    *(long*)(start + sizeof(ulong)) = timestamp;
                    AutoCSer.Common.CopyTo(verifyString, start + (sizeof(ulong) + sizeof(long)));
                }
                return md5.ComputeHash(buffer.Buffer.notNull().Buffer, buffer.StartIndex, size);
            }
            finally { buffer.Free(); }
        }
        /// <summary>
        /// Determine whether the MD5 values are equal
        /// 判断 MD5 值是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ulong Md5Equals(byte[] left, byte[] right)
        {
            fixed (byte* leftFixed = left, rightRixed = right)
            {
                return (*(ulong*)leftFixed ^ *(ulong*)rightRixed) | (*(ulong*)(leftFixed + sizeof(ulong)) ^ *(ulong*)(rightRixed + sizeof(ulong)));
            }
        }
    }
}
