﻿using AutoCSer.Net;
using System;
using System.IO;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 修复节点方法队列回调
    /// </summary>
    internal sealed class RepairNodeMethodCallback : CommandServerCallQueueNode
    {
        /// <summary>
        /// 生成服务端节点
        /// </summary>
        private readonly ServerNodeCreator nodeCreator;
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        private readonly RepairNodeMethod repairNodeMethod;
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        private readonly ServerNodeMethod nodeMethod;
        /// <summary>
        /// 修复节点方法
        /// </summary>
        private readonly Method method;
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        private readonly MethodInfo methodInfo;
        /// <summary>
        /// 服务端节点方法自定义属性
        /// </summary>
        private readonly ServerMethodAttribute methodAttribute;
        /// <summary>
        /// 修复节点方法目录
        /// </summary>
        private readonly DirectoryInfo methodDirectory;
        /// <summary>
        /// 修复节点方法回调返回状态
        /// </summary>
        private readonly CommandServerCallback<CallStateEnum> callback;
        /// <summary>
        /// 修复节点方法队列回调
        /// </summary>
        /// <param name="nodeCreator">生成服务端节点</param>
        /// <param name="repairNodeMethod">修复节点方法信息</param>
        /// <param name="nodeMethod">修复节点方法信息</param>
        /// <param name="method">修复节点方法</param>
        /// <param name="methodInfo">修复节点方法信息</param>
        /// <param name="methodAttribute">服务端节点方法自定义属性</param>
        /// <param name="callback">修复节点方法回调返回状态</param>
        internal RepairNodeMethodCallback(ServerNodeCreator nodeCreator, RepairNodeMethod repairNodeMethod, ServerNodeMethod nodeMethod, Method method, MethodInfo methodInfo, ServerMethodAttribute methodAttribute, ref CommandServerCallback<CallStateEnum> callback)
        {
            this.callback = callback;
            this.repairNodeMethod = repairNodeMethod;
            this.nodeCreator = nodeCreator;
            this.nodeMethod = nodeMethod;
            this.method = method;
            this.methodInfo = methodInfo;
            this.methodAttribute = methodAttribute;
            this.methodDirectory = new DirectoryInfo(repairNodeMethod.MethodDirectoryName);
            callback = null;
        }
        /// <summary>
        /// 回调操作
        /// </summary>
        public override void RunTask()
        {
            nodeCreator.Repair(repairNodeMethod, nodeMethod, method, methodInfo, methodAttribute, methodDirectory, callback);
        }
    }
}