﻿using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库回调操作
    /// </summary>
    internal sealed class PersistenceCallback : CommandServerCallQueueNode
    {
        /// <summary>
        /// 持久化流已写入位置
        /// </summary>
        internal long PersistencePosition;
        /// <summary>
        /// 持久化回调头节点
        /// </summary>
        private readonly MethodParameter head;
        /// <summary>
        /// 持久化回调尾节点
        /// </summary>
        private readonly MethodParameter end;
        /// <summary>
        /// 日志流持久化内存数据库回调操作
        /// </summary>
        /// <param name="head">持久化回调头节点</param>
        /// <param name="end">持久化回调尾节点</param>
        internal PersistenceCallback(MethodParameter head, MethodParameter end)
        {
            this.head = head;
            this.end = end;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            head.Node.NodeCreator.Service.PersistenceCallback(head, end, PersistencePosition);
        }
    }
}