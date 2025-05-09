using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game;
using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 回调测试节点
    /// </summary>
    public class CallbackNode : MethodParameterCreatorNode<ICallbackNode>, ICallbackNode
        , ISnapshot<int>
        , ISnapshot<long>
    {
        private int value;
        private long value64;
        public MethodCallback<int> MethodCallback;
        public int GetSnapshotCapacity(ref object customObject)
        {
            return 1;
        }
        public SnapshotResult<int> GetSnapshotResult(int[] snapshotArray, object customObject)
        {
            snapshotArray[0] = value;
            return new SnapshotResult<int>(1);
        }
        public void SetSnapshotResult(ref LeftArray<int> array, ref LeftArray<int> newArray) { }
        public void SnapshotSet(int value)
        {
            SetValue64(value);
        }
        public SnapshotResult<long> GetSnapshotResult(long[] snapshotArray, object customObject)
        {
            snapshotArray[0] = value64;
            return new SnapshotResult<long>(1);
        }
        public void SetSnapshotResult(ref LeftArray<long> array, ref LeftArray<long> newArray) { }
        public void SnapshotSet64(long value)
        {
            this.value64 = value;
        }
        public void SetValue64(int value)
        {
            this.value = value;
            value64 = value;
        }

        public bool CheckSnapshot()
        {
            return value == value64;
        }

        public void SetValueCallback(int value, MethodCallback<int> callback)
        {
            SetValue64(value);
            callback.Callback(value + 1);
        }
        public void SetCallback(MethodCallback<int> callback)
        {
            this.MethodCallback = callback;
        }
        public void SetValueSendOnly(int value)
        {
            MethodCallback?.Callback(value + 1);
        }
        public ValueResult<int> SetValueCallbackPersistenceBeforePersistence(int value)
        {
            return default(ValueResult<int>);
        }
        public void SetValueCallbackPersistence(int value, MethodCallback<int> callback)
        {
            SetValue64(value);
            callback.Callback(value + 1);
        }
        public ValueResult<int> SetCallbackPersistenceBeforePersistence()
        {
            return default(ValueResult<int>);
        }
        public void SetCallbackPersistence(MethodCallback<int> callback)
        {
            this.MethodCallback = callback;
        }
        public bool SetValuePersistenceSendOnlyBeforePersistence(int value)
        {
            return true;
        }
        public void SetValuePersistenceSendOnly(int value)
        {
            MethodCallback?.Callback(value + 1);
        }

        public ValueResult<int> InputKeepCallbackBeforePersistence(int value, int count)
        {
            return default(ValueResult<int>);
        }
        public void InputKeepCallback(int value, int count, MethodKeepCallback<int> callback)
        {
            if (callback.IsCancelKeepCallback) return;
            SetValue64(value);
            for (int end = value + count; value != end; ++value)
            {
                if (!callback.Callback(value)) return;
            }
            callback.CancelKeep();
        }
        public ValueResult<int> KeepCallbackBeforePersistence()
        {
            return default(ValueResult<int>);
        }
        public void KeepCallback(MethodKeepCallback<int> callback)
        {
            if (callback.IsCancelKeepCallback) return;
            int value = this.value;
            for (int end = value + 10; value != end; ++value)
            {
                if (!callback.Callback(value)) return;
            }
            callback.CancelKeep();
        }
        public ValueResult<int> InputEnumerableBeforePersistence(int value, int count)
        {
            return default(ValueResult<int>);
        }
        public IEnumerable<int> InputEnumerable(int value, int count)
        {
            SetValue64(value);
            for (int end = value + count; value != end; ++value) yield return value;
        }
        public ValueResult<int> EnumerableBeforePersistence()
        {
            return default(ValueResult<int>);
        }
        public IEnumerable<int> Enumerable()
        {
            int value = this.value;
            for (int end = value + 10; value != end; ++value) yield return value;
        }

        public bool CallbackBeforePersistence()
        {
            return true;
        }
        public void Callback()
        {
            MethodCallback?.Callback(value + 1);
        }
        public void CallbackCommand()
        {
            MethodCallback?.Callback(value + 1);
        }
        public bool SetValueBeforePersistence(int value)
        {
            return true;
        }
        public void SetValue(int value)
        {
            SetValue64(value);
        }
        public void SetValueCommand(int value)
        {
            SetValue64(value);
        }
        public ValueResult<int> GetValueBeforePersistence()
        {
            return default(ValueResult<int>);
        }
        public int GetValue()
        {
            return value + 1;
        }
        public int GetValueCommand()
        {
            return value + 1;
        }
        public int CallInoutOutputCommand(int value)
        {
            return value + 1;
        }

        public void CustomPersistence(int value)
        {
            SetValue64(value);
        }
        public void CallCustomPersistence(int value)
        {
            StreamPersistenceMemoryDatabaseMethodParameterCreator.CustomPersistence(value);
        }
        public void PersistenceCallbackException()
        {
            throw new AutoCSer.Log.IgnoreException();
            //持久化成功，回调程序产生异常，在没有写入忽略序列化异常位置以前被意外中断，会导致重新初始化的时候节点会变成异常不可用状态
        }
#if !AOT
        private ServerByteArray serverByteArray;
        public void SetServerByteArray(ServerByteArray value)
        {
            serverByteArray = value;
        }
        public ServerByteArray GetServerByteArray()
        {
            return serverByteArray;
        }
#endif
        private Game.Monster jsonValue;
        public void SetJsonValue(Game.Monster value)
        {
            jsonValue = value;
        }
        public Game.Monster GetJsonValue()
        {
            return jsonValue;
        }
    }
}
