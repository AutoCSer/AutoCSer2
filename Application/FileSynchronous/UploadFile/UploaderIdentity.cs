﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件上传标识
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct UploaderIdentity
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        internal FileUploader Uploader;
        /// <summary>
        /// 上传标识
        /// </summary>
        private uint identity;
        /// <summary>
        /// 设置文件上传实例
        /// </summary>
        /// <param name="uploader"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(FileUploader uploader)
        {
            this.Uploader = uploader;
            uploader.UploaderInfo.Index.Identity = identity;
        }
        /// <summary>
        /// 获取文件上传实例
        /// </summary>
        /// <param name="identity">上传标识</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal FileUploader Get(uint identity)
        {
            return this.identity == identity ? Uploader : null;
        }
        /// <summary>
        /// 释放文件上传实例
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            Uploader = null;
            ++identity;
        }
    }
}