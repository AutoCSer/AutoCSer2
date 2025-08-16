using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerTwoStage‌CallbackController
    {
        void TwoStage‌CallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackReturn(CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);

        void TwoStage‌CallbackCountSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountReturn(CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);

        void TwoStage‌CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);

        void TwoStage‌CallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountQueueReturn(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountQueueReturn(CommandServerCallQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);

        void TwoStage‌CallbackReadWriteQueueReturn(CommandServerCallReadQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackReadWriteQueueReturn(CommandServerCallWriteQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackCountReadWriteQueueReturn(CommandServerCallWriteQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountReadWriteQueueReturn(CommandServerCallReadQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);

        void TwoStage‌CallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerTwoStage‌CallbackController : IServerTwoStage‌CallbackController
    {
        internal static void TwoStage‌Callback(long value, CommandServerCallback<long> callback, CommandServerKeepCallback<string> keepCallback)
        {
            callback.Callback(value);
            for (long endValue = value + ServerKeepCallbackController.KeepCallbackCount; value != endValue; ++value) keepCallback.Callback(value.ToString());
            keepCallback.CancelKeep();
        }
        internal static async Task TwoStage‌Callback(long value, CommandServerCallback<long> callback, CommandServerKeepCallbackCount<string> keepCallback)
        {
            callback.Callback(value);
            for (long endValue = value + ServerKeepCallbackController.KeepCallbackCount; value != endValue; ++value) await keepCallback.CallbackAsync(value.ToString());
            keepCallback.CancelKeep();
        }

        void IServerTwoStage‌CallbackController.TwoStage‌CallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackReturn(CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback);
        }

        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountReturn(CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }

        void IServerTwoStage‌CallbackController.TwoStage‌CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback);
        }

        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountQueueReturn(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountQueueReturn(CommandServerCallQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }

        void IServerTwoStage‌CallbackController.TwoStage‌CallbackReadWriteQueueReturn(CommandServerCallReadQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackReadWriteQueueReturn(CommandServerCallWriteQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountReadWriteQueueReturn(CommandServerCallWriteQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountReadWriteQueueReturn(CommandServerCallReadQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }

        void IServerTwoStage‌CallbackController.TwoStage‌CallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback);
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerTwoStage‌CallbackController.TwoStage‌CallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
    }
#endif
}
