﻿using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerSendOnlyController
    {
        CommandServerSendOnly SendOnly(int Value, int Ref);
        CommandServerSendOnly SendOnly();

        CommandServerSendOnly SendOnlyQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref);
        CommandServerSendOnly SendOnlyQueue(CommandServerCallQueue queue);

        Task<CommandServerSendOnly> SendOnlyTask(int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTask();

        Task<CommandServerSendOnly> SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
    }
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerSendOnlyController : CommandServerBindContextController, IServerSendOnlyController
    {
        CommandServerSendOnly IServerSendOnlyController.SendOnly(int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            return SendOnly();
        }
        public CommandServerSendOnly SendOnly()
        {
            AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.Release();
            return null;
        }

        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            return SendOnly();
        }
        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueue(CommandServerCallQueue queue)
        {
            return SendOnly();
        }

        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask(int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask()
        {
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }

        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
    }
}