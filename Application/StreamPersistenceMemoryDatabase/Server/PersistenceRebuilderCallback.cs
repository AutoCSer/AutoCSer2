using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化文件重建队列回调
    /// </summary>
    internal sealed class PersistenceRebuilderCallback : CommandServerCallQueueNode
    {
        /// <summary>
        /// 日志流持久化文件重建
        /// </summary>
        private readonly PersistenceRebuilder rebuilder;
        /// <summary>
        /// 日志流持久化文件重建队列回调任务类型
        /// </summary>
        private readonly PersistenceRebuilderCallbackTypeEnum type;
        /// <summary>
        /// 日志流持久化文件重建队列回调
        /// </summary>
        /// <param name="rebuilder">日志流持久化文件重建</param>
        /// <param name="type">日志流持久化文件重建队列回调任务类型</param>
        internal PersistenceRebuilderCallback(PersistenceRebuilder rebuilder, PersistenceRebuilderCallbackTypeEnum type)
        {
            this.rebuilder = rebuilder;
            this.type = type;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            switch (type)
            {
                case PersistenceRebuilderCallbackTypeEnum.Close: rebuilder.Close(); return;
                case PersistenceRebuilderCallbackTypeEnum.NextNode: rebuilder.NextNode(); return;
                case PersistenceRebuilderCallbackTypeEnum.CheckQueue: rebuilder.CheckQueue(); return;
                case PersistenceRebuilderCallbackTypeEnum.Completed: rebuilder.Service.RebuildCompleted(); return;
            }
        }
    }
}
