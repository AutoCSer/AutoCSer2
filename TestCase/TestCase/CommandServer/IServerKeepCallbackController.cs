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
    public interface IServerKeepCallbackController
    {
        void KeepCallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallbackSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback);
        void KeepCallbackSocket(CommandServerSocket socket, CommandServerKeepCallback Callback);
        void KeepCallbackReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallback(int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackReturn(CommandServerKeepCallback<string> Callback);
        void KeepCallback(CommandServerKeepCallback Callback);

        void KeepCallbackCountSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCount(int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountReturn(CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCount(CommandServerKeepCallbackCount Callback);

        void KeepCallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallbackQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerKeepCallback<string> Callback);
        void KeepCallbackQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback Callback);
        void KeepCallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallback<string> Callback);
        void KeepCallbackQueue(CommandServerCallQueue queue, CommandServerKeepCallback Callback);

        void KeepCallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountQueueReturn(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountQueue(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountQueueReturn(CommandServerCallQueue queue, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountQueue(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallbackCount Callback);

        void KeepCallbackReadWriteQueueReturn(CommandServerCallReadQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallbackReadWriteQueue(CommandServerCallWriteQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackReadWriteQueueReturn(CommandServerCallWriteQueue queue, CommandServerKeepCallback<string> Callback);
        void KeepCallbackReadWriteQueue(CommandServerCallReadQueue queue, CommandServerKeepCallback Callback);
        void KeepCallbackCountReadWriteQueueReturn(CommandServerCallWriteQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountReadWriteQueue(CommandServerCallReadQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountReadWriteQueueReturn(CommandServerCallReadQueue queue, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountReadWriteQueue(CommandServerCallWriteQueue queue, CommandServerKeepCallbackCount Callback);

        void KeepCallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallbackConcurrencyReadQueue(CommandServerCallConcurrencyReadWriteQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, CommandServerKeepCallback<string> Callback);
        void KeepCallbackConcurrencyReadQueue(CommandServerCallConcurrencyReadQueue queue, CommandServerKeepCallback Callback);
        void KeepCallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountConcurrencyReadQueue(CommandServerCallConcurrencyReadQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountConcurrencyReadQueue(CommandServerCallConcurrencyReadWriteQueue queue, CommandServerKeepCallbackCount Callback);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerKeepCallbackController : IServerKeepCallbackController
    {
        internal const int KeepCallbackCount = 4;
        //internal static void KeepCallback(long value, CommandServerKeepCallback<string> callback)
        //{
        //    for (long endValue = value + KeepCallbackCount; value != endValue; ++value) callback.Callback(value.ToString());
        //    callback.CancelKeep();
        //}
        internal static void AutoKeepCallback(long value, CommandServerKeepCallback<string> callback)
        {
            for (long endValue = value + KeepCallbackCount; value != endValue; ++value) callback.Callback(value.ToString());
        }
        internal static IEnumerable<string> KeepCallbackEnumerable(long value)
        {
            for (long endValue = value + KeepCallbackCount; value != endValue; ++value) yield return value.ToString();
        }
#if NetStandard21
        internal static async IAsyncEnumerable<string> AsyncEnumerable(long value)
        {
            for (long endValue = value + KeepCallbackCount; value != endValue; ++value) yield return value.ToString();
        }
#endif
        //internal static void KeepCallback(CommandServerKeepCallback callback)
        //{
        //    for (int count = KeepCallbackCount; count != 0; --count) callback.Callback();
        //    callback.CancelKeep();
        //}
        internal static void AutoKeepCallback(CommandServerKeepCallback callback)
        {
            for (int count = KeepCallbackCount; count != 0; --count) callback.Callback();
        }
        internal static async Task KeepCallback(long value, CommandServerKeepCallbackCount<string> callback)
        {
            for (long endValue = value + KeepCallbackCount; value != endValue; ++value) await callback.CallbackAsync(value.ToString());
            callback.CancelKeep();
        }
        internal static async Task AutoKeepCallback(long value, CommandServerKeepCallbackCount<string> callback)
        {
            for (long endValue = value + KeepCallbackCount; value != endValue; ++value) await callback.CallbackAsync(value.ToString());
        }
        internal static async Task KeepCallback(CommandServerKeepCallbackCount callback)
        {
            for (int count = KeepCallbackCount; count != 0; --count) await callback.CallbackAsync();
            callback.CancelKeep();
        }
        internal static async Task AutoKeepCallback(CommandServerKeepCallbackCount callback)
        {
            for (int count = KeepCallbackCount; count != 0; --count) await callback.CallbackAsync();
        }

        void IServerKeepCallbackController.KeepCallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackSocket(CommandServerSocket socketf, CommandServerKeepCallback Callback)
        {
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallback(int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackReturn(CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallback(CommandServerKeepCallback Callback)
        {
            AutoKeepCallback(Callback);
        }

        void IServerKeepCallbackController.KeepCallbackCountSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback)
        {
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCount(int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountReturn(CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCount(CommandServerKeepCallbackCount Callback)
        {
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }

        void IServerKeepCallbackController.KeepCallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback Callback)
        {
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueue(CommandServerCallQueue queue, CommandServerKeepCallback Callback)
        {
            AutoKeepCallback(Callback);
        }

        void IServerKeepCallbackController.KeepCallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerKeepCallbackCount Callback)
        {
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueueReturn(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueue(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueueReturn(CommandServerCallQueue queue, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueue(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallbackCount Callback)
        {
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }

        void IServerKeepCallbackController.KeepCallbackReadWriteQueueReturn(CommandServerCallReadQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackReadWriteQueue(CommandServerCallWriteQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackReadWriteQueueReturn(CommandServerCallWriteQueue queue, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackReadWriteQueue(CommandServerCallReadQueue queue, CommandServerKeepCallback Callback)
        {
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackCountReadWriteQueueReturn(CommandServerCallWriteQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountReadWriteQueue(CommandServerCallReadQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountReadWriteQueueReturn(CommandServerCallReadQueue queue, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountReadWriteQueue(CommandServerCallWriteQueue queue, CommandServerKeepCallbackCount Callback)
        {
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }

        void IServerKeepCallbackController.KeepCallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackConcurrencyReadQueue(CommandServerCallConcurrencyReadWriteQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, CommandServerKeepCallback<string> Callback)
        {
            AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackConcurrencyReadQueue(CommandServerCallConcurrencyReadQueue queue, CommandServerKeepCallback Callback)
        {
            AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountConcurrencyReadQueue(CommandServerCallConcurrencyReadQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountConcurrencyReadQueueReturn(CommandServerCallConcurrencyReadQueue queue, CommandServerKeepCallbackCount<string> Callback)
        {
            KeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountConcurrencyReadQueue(CommandServerCallConcurrencyReadWriteQueue queue, CommandServerKeepCallbackCount Callback)
        {
            KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
    }
}
