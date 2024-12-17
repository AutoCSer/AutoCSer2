using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 回调测试节点
    /// </summary>
    public class CallbackNode : MethodParameterCreatorNode<ICallbackNode>, ICallbackNode, ISnapshot<int>
    {
        public int Value;
        public MethodCallback<int> MethodCallback;
        public LeftArray<int> GetSnapshotArray()
        {
            LeftArray<int> snapshotArray = new LeftArray<int>(1);
            snapshotArray.Add(Value);
            return snapshotArray;
        }
        public void SnapshotSet(int value)
        {
            Value = value;
        }

        public void SetValueCallback(int value, MethodCallback<int> callback)
        {
            this.Value = value;
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
            this.Value = value;
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
            this.Value = value;
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
            int value = this.Value;
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
            this.Value = value;
            for (int end = value + count; value != end; ++value) yield return value;
        }
        public ValueResult<int> EnumerableBeforePersistence()
        {
            return default(ValueResult<int>);
        }
        public IEnumerable<int> Enumerable()
        {
            int value = this.Value;
            for (int end = value + 10; value != end; ++value) yield return value;
        }

        public bool CallbackBeforePersistence()
        {
            return true;
        }
        public void Callback()
        {
            MethodCallback?.Callback(Value + 1);
        }
        public bool SetValueBeforePersistence(int value)
        {
            return true;
        }
        public void SetValue(int value)
        {
            this.Value = value;
        }
        public ValueResult<int> GetValueBeforePersistence()
        {
            return default(ValueResult<int>);
        }
        public int GetValue()
        {
            return Value + 1;
        }

        public void CustomPersistence(int value)
        {
            this.Value = value;
        }
        public void CallCustomPersistence(int value)
        {
            methodParameterCreator.Creator.CustomPersistence(value);
        }
        public void PersistenceCallbackException()
        {
            throw new AutoCSer.Log.IgnoreException();
            //持久化成功，回调程序产生异常，在没有写入忽略序列化异常位置以前被意外中断，会导致重新初始化的时候节点会变成异常不可用状态
        }

        private ServerByteArray serverByteArray;
        public void SetServerByteArray(ServerByteArray value)
        {
            serverByteArray = value;
        }
        public ServerByteArray GetServerByteArray()
        {
            return serverByteArray;
        }
        private TestClass jsonValue;
        public void SetJsonValue(JsonValue<TestClass> value)
        {
            jsonValue = value;
        }
        public JsonValue<TestClass> GetJsonValue()
        {
            return jsonValue;
        }
    }
}
