using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 回调测试节点
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsMethodParameterCreator = true, IsLocalClient = true)]
    public partial interface ICallbackNode
    {
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(int value);
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSet64(long value);
        [ServerMethod(IsPersistence = false)]
        bool CheckSnapshot();
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

        void SetServerByteArray(ServerByteArray value);
        [ServerMethod(IsPersistence = false)]
        ServerByteArray GetServerByteArray();
        void SetJsonValue(Game.Monster value);
        [ServerMethod(IsPersistence = false)]
        Game.Monster GetJsonValue();
    }
}
