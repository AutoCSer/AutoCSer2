//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICallbackNode))]
        public partial interface ICallbackNodeClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Callback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> Enumerable();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetValue();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> InputEnumerable(int value, int count);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> InputKeepCallback(int value, int count);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> KeepCallback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetCallback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetCallbackPersistence();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetValue(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetValueCallback(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetValueCallbackPersistence(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.Net.SendOnlyCommand SetValuePersistenceSendOnly(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.Net.SendOnlyCommand SetValueSendOnly(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> CallCustomPersistence(int value);
            /// <summary>
            /// 
            /// </summary>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> PersistenceCallbackException();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJson<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetServerJson();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetServerJson(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJson<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetJsonValue();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetJsonValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetServerBinary();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetServerBinary(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetServerJsonBinary();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetServerJsonBinary(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICustomServiceNode))]
        public partial interface ICustomServiceNodeClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateCallbackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="length"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int length);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateLeftArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedListNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateBinaryMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerBinaryMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerJsonMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateStringMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerJsonBinaryMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformanceDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformanceMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformancePersistenceDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformancePersistenceMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformancePersistenceSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformanceSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDistributedLockNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICallbackNode))]
        public partial interface ICallbackNodeLocalClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Callback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> Enumerable();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> GetValue();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> InputEnumerable(int value, int count);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> InputKeepCallback(int value, int count);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> KeepCallback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetCallback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetCallbackPersistence();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetValue(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetValueCallback(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> SetValueCallbackPersistence(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter SetValuePersistenceSendOnly(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter SetValueSendOnly(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> CallCustomPersistence(int value);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> PersistenceCallbackException();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJson<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetServerJson();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetServerJson(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJson<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetJsonValue();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetJsonValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetServerBinary();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetServerBinary(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetServerJsonBinary();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetServerJsonBinary(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinary<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(ServerNodeType = typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICustomServiceNode))]
        public partial interface ICustomServiceNodeLocalClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateCallbackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="length"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int length);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateLeftArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedListNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateBinaryMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerBinaryMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerJsonMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateStringMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerJsonBinaryMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformanceDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformanceMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="capacity"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformancePersistenceDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <param name="arraySize"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="checkTimeoutSeconds"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformancePersistenceMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformancePersistenceSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreatePerformanceSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDistributedLockNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    public enum CallbackNodeMethodEnum
    {
            /// <summary>
            /// [0] 
            /// </summary>
            Callback = 0,
            /// <summary>
            /// [1] 
            /// 返回值 bool 
            /// </summary>
            CallbackBeforePersistence = 1,
            /// <summary>
            /// [2] 
            /// 返回值 int 
            /// </summary>
            Enumerable = 2,
            /// <summary>
            /// [3] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            EnumerableBeforePersistence = 3,
            /// <summary>
            /// [4] 
            /// 返回值 int 
            /// </summary>
            GetValue = 4,
            /// <summary>
            /// [5] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            GetValueBeforePersistence = 5,
            /// <summary>
            /// [6] 
            /// int value 
            /// int count 
            /// 返回值 int 
            /// </summary>
            InputEnumerable = 6,
            /// <summary>
            /// [7] 
            /// int value 
            /// int count 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            InputEnumerableBeforePersistence = 7,
            /// <summary>
            /// [8] 
            /// int value 
            /// int count 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            InputKeepCallback = 8,
            /// <summary>
            /// [9] 
            /// int value 
            /// int count 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            InputKeepCallbackBeforePersistence = 9,
            /// <summary>
            /// [10] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            KeepCallback = 10,
            /// <summary>
            /// [11] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            KeepCallbackBeforePersistence = 11,
            /// <summary>
            /// [12] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetCallback = 12,
            /// <summary>
            /// [13] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetCallbackPersistence = 13,
            /// <summary>
            /// [14] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            SetCallbackPersistenceBeforePersistence = 14,
            /// <summary>
            /// [15] 
            /// int value 
            /// </summary>
            SetValue = 15,
            /// <summary>
            /// [16] 
            /// int value 
            /// 返回值 bool 
            /// </summary>
            SetValueBeforePersistence = 16,
            /// <summary>
            /// [17] 
            /// int value 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetValueCallback = 17,
            /// <summary>
            /// [18] 
            /// int value 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetValueCallbackPersistence = 18,
            /// <summary>
            /// [19] 
            /// int value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            SetValueCallbackPersistenceBeforePersistence = 19,
            /// <summary>
            /// [20] 
            /// int value 
            /// </summary>
            SetValuePersistenceSendOnly = 20,
            /// <summary>
            /// [21] 
            /// int value 
            /// 返回值 bool 
            /// </summary>
            SetValuePersistenceSendOnlyBeforePersistence = 21,
            /// <summary>
            /// [22] 
            /// int value 
            /// </summary>
            SetValueSendOnly = 22,
            /// <summary>
            /// [23] 
            /// int value 
            /// </summary>
            CallCustomPersistence = 23,
            /// <summary>
            /// [24] 
            /// int value 
            /// </summary>
            CustomPersistence = 24,
            /// <summary>
            /// [25] 
            /// </summary>
            PersistenceCallbackException = 25,
            BindNodeMethodTest = 26,
            /// <summary>
            /// [27] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJson{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} 
            /// </summary>
            GetServerJson = 27,
            /// <summary>
            /// [28] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJson{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} value 
            /// </summary>
            SetServerJson = 28,
            /// <summary>
            /// [29] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} 
            /// </summary>
            GetJsonValue = 29,
            /// <summary>
            /// [30] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} value 
            /// </summary>
            SetJsonValue = 30,
            /// <summary>
            /// [31] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerBinary{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} 
            /// </summary>
            GetServerBinary = 31,
            /// <summary>
            /// [32] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerBinary{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} value 
            /// </summary>
            SetServerBinary = 32,
            /// <summary>
            /// [33] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinary{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} 
            /// </summary>
            GetServerJsonBinary = 33,
            /// <summary>
            /// [34] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinary{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} value 
            /// </summary>
            SetServerJsonBinary = 34,
            /// <summary>
            /// [35] 
            /// int value 
            /// </summary>
            SnapshotSet = 35,
    }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    public enum CustomServiceNodeMethodEnum
    {
            /// <summary>
            /// [0] 创建字典节点 FragmentHashStringDictionary256{HashString,string}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentHashStringDictionaryNode = 0,
            /// <summary>
            /// [1] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 1,
            /// <summary>
            /// [2] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateCallbackNode = 2,
            /// <summary>
            /// [3] 删除节点持久化参数检查
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 无返回值表示需要继续调用持久化方法
            /// </summary>
            RemoveNodeBeforePersistence = 3,
            /// <summary>
            /// [4] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int length 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateArrayNode = 4,
            /// <summary>
            /// [5] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// uint capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateBitmapNode = 5,
            /// <summary>
            /// [6] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateDictionaryNode = 6,
            /// <summary>
            /// [7] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateFragmentDictionaryNode = 7,
            /// <summary>
            /// [8] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateFragmentHashSetNode = 8,
            /// <summary>
            /// [9] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateHashSetNode = 9,
            /// <summary>
            /// [10] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateLeftArrayNode = 10,
            /// <summary>
            /// [11] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateQueueNode = 11,
            /// <summary>
            /// [12] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateSearchTreeDictionaryNode = 12,
            /// <summary>
            /// [13] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateSearchTreeSetNode = 13,
            /// <summary>
            /// [14] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateSortedDictionaryNode = 14,
            /// <summary>
            /// [15] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateSortedListNode = 15,
            /// <summary>
            /// [16] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateSortedSetNode = 16,
            /// <summary>
            /// [17] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateStackNode = 17,
            /// <summary>
            /// [18] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int arraySize 
            /// int timeoutSeconds 
            /// int checkTimeoutSeconds 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateBinaryMessageNode = 18,
            /// <summary>
            /// [19] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int arraySize 
            /// int timeoutSeconds 
            /// int checkTimeoutSeconds 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateServerBinaryMessageNode = 19,
            /// <summary>
            /// [20] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int arraySize 
            /// int timeoutSeconds 
            /// int checkTimeoutSeconds 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateServerJsonMessageNode = 20,
            /// <summary>
            /// [21] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int arraySize 
            /// int timeoutSeconds 
            /// int checkTimeoutSeconds 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateStringMessageNode = 21,
            /// <summary>
            /// [22] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int arraySize 
            /// int timeoutSeconds 
            /// int checkTimeoutSeconds 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateServerJsonBinaryMessageNode = 22,
            /// <summary>
            /// [23] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreatePerformanceDictionaryNode = 23,
            /// <summary>
            /// [24] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int arraySize 
            /// int timeoutSeconds 
            /// int checkTimeoutSeconds 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreatePerformanceMessageNode = 24,
            /// <summary>
            /// [25] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int capacity 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreatePerformancePersistenceDictionaryNode = 25,
            /// <summary>
            /// [26] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// int arraySize 
            /// int timeoutSeconds 
            /// int checkTimeoutSeconds 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreatePerformancePersistenceMessageNode = 26,
            /// <summary>
            /// [27] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreatePerformancePersistenceSearchTreeDictionaryNode = 27,
            /// <summary>
            /// [28] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreatePerformanceSearchTreeDictionaryNode = 28,
            /// <summary>
            /// [29] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateDistributedLockNode = 29,
    }
}
#endif