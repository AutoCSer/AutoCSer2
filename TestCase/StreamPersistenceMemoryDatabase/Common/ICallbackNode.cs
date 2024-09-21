using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 回调测试节点
    /// </summary>
    [ServerNode(MethodIndexEnumType = typeof(CallbackNodeMethodEnum), IsAutoMethodIndex = false, IsMethodParameterCreator = true, IsLocalClient = true)]
    public interface ICallbackNode
    {
        [ServerMethod(IsClientCall = false, IsSnapshotMethod = true)]
        void SnapshotSet(int value);
        [ServerMethod(IsPersistence = false)]
        void SetValueCallback(int value, MethodCallback<int> callback);
        [ServerMethod(IsPersistence = false)]
        void SetCallback(MethodCallback<int> callback);
        [ServerMethod(IsPersistence = false, IsSendOnly = true)]
        void SetValueSendOnly(int value);
        ValueResult<int> SetValueCallbackPersistenceBeforePersistence(int value);
        void SetValueCallbackPersistence(int value, MethodCallback<int> callback);
        ValueResult<int> SetCallbackPersistenceBeforePersistence();
        void SetCallbackPersistence(MethodCallback<int> callback);
        bool SetValuePersistenceSendOnlyBeforePersistence(int value);
        [ServerMethod(IsSendOnly = true)]
        void SetValuePersistenceSendOnly(int value);

        ValueResult<int> InputKeepCallbackBeforePersistence(int value, int count);
        void InputKeepCallback(int value, int count, MethodKeepCallback<int> callback);
        ValueResult<int> KeepCallbackBeforePersistence();
        void KeepCallback(MethodKeepCallback<int> callback);
        ValueResult<int> InputEnumerableBeforePersistence(int value, int count);
        IEnumerable<int> InputEnumerable(int value, int count);
        ValueResult<int> EnumerableBeforePersistence();
        IEnumerable<int> Enumerable();

        bool CallbackBeforePersistence();
        void Callback();
        bool SetValueBeforePersistence(int value);
        void SetValue(int value);
        ValueResult<int> GetValueBeforePersistence();
        int GetValue();

        [ServerMethod(IsPersistence = false, IsClientCall = false)]
        void CustomPersistence(int value);
        [ServerMethod(IsPersistence = false)]
        void CallCustomPersistence(int value);

        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void PersistenceCallbackException();

        void SetServerJsonBinary(ServerJsonBinary<TestClass> value);
        [ServerMethod(IsPersistence = false)]
        ServerJsonBinary<TestClass> GetServerJsonBinary();
        void SetServerJson(ServerJson<TestClass> value);
        [ServerMethod(IsPersistence = false)]
        ServerJson<TestClass> GetServerJson();
        [ServerMethod(IsPersistence = false)]
        void SetJsonValue(JsonValue<TestClass> value);
        [ServerMethod(IsPersistence = false)]
        JsonValue<TestClass> GetJsonValue();
        void SetServerBinary(ServerBinary<TestClass> value);
        [ServerMethod(IsPersistence = false)]
        ServerBinary<TestClass> GetServerBinary();
    }
}
