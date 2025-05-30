﻿//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.IGameNode))]
        public partial interface IGameNodeClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="monster"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AddMonster(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster monster);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="monsters"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AddMonsters(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster[] monsters);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="speed"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetSpeed(int id, int speed);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="speeds"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetSpeeds(AutoCSer.KeyValue<int,int>[] speeds);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 回调测试节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICallbackNode))]
        public partial interface ICallbackNodeClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter CallCustomPersistence(int value);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Callback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<int>> Enumerable();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster> GetJsonValue();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray> GetServerByteArray();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetValue();
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter PersistenceCallbackException();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> SetCallback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> SetCallbackPersistence();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetJsonValue(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetServerByteArray(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetValue(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> SetValueCallback(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> SetValueCallbackPersistence(int value);
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
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> CheckSnapshot();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand CallInoutOutputCommand(int value, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> callback);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.Net.CallbackCommand CallbackCommand(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> callback);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand GetValueCommand(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>> callback);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.Net.CallbackCommand SetValueCommand(int value, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> callback);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作自定义扩展接口（用于添加自定义节点创建接口） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICustomServiceNode))]
        public partial interface ICustomServiceNodeClientNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode
        {
            /// <summary>
            /// 创建回调测试节点 ICallbackNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateCallbackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 创建游戏测试节点 GameNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateGameNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
        /// <summary>
        ///  本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.IGameNode))]
        public partial interface IGameNodeLocalClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="monster"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AddMonster(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster monster);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="monsters"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AddMonsters(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster[] monsters);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="speed"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SetSpeed(int id, int speed);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="speeds"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SetSpeeds(AutoCSer.KeyValue<int,int>[] speeds);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 回调测试节点 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICallbackNode))]
        public partial interface ICallbackNodeLocalClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> CallCustomPersistence(int value);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Callback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalKeepCallback<int>> Enumerable();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster>> GetJsonValue();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray>> GetServerByteArray();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetValue();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalKeepCallback<int>> InputEnumerable(int value, int count);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalKeepCallback<int>> InputKeepCallback(int value, int count);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalKeepCallback<int>> KeepCallback();
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> PersistenceCallbackException();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> SetCallback();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> SetCallbackPersistence();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SetJsonValue(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SetServerByteArray(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SetValue(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> SetValueCallback(int value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> SetValueCallbackPersistence(int value);
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
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> CheckSnapshot();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            void CallInoutOutputCommand(int value, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> __callback__);
            /// <summary>
            /// 
            /// </summary>
            void CallbackCommand(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> __callback__);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            void GetValueCommand(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> __callback__);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            void SetValueCommand(int value, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> __callback__);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作自定义扩展接口（用于添加自定义节点创建接口） 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICustomServiceNode))]
        public partial interface ICustomServiceNodeLocalClientNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode
        {
            /// <summary>
            /// 创建回调测试节点 ICallbackNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateCallbackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 创建游戏测试节点 GameNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateGameNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
        /// <summary>
        /// 
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(IGameNodeMethodEnum))]
        public partial interface IGameNode { }
        /// <summary>
        ///  节点方法序号映射枚举类型
        /// </summary>
        public enum IGameNodeMethodEnum
        {
            /// <summary>
            /// [0] 
            /// AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster monster 
            /// </summary>
            AddMonster = 0,
            /// <summary>
            /// [1] 
            /// AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster[] monsters 
            /// </summary>
            AddMonsters = 1,
            /// <summary>
            /// [2] 
            /// </summary>
            Clear = 2,
            /// <summary>
            /// [3] 
            /// int id 
            /// int speed 
            /// </summary>
            SetSpeed = 3,
            /// <summary>
            /// [4] 
            /// AutoCSer.KeyValue{int,int}[] speeds 
            /// </summary>
            SetSpeeds = 4,
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 回调测试节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(ICallbackNodeMethodEnum))]
        public partial interface ICallbackNode { }
        /// <summary>
        /// 回调测试节点 节点方法序号映射枚举类型
        /// </summary>
        public enum ICallbackNodeMethodEnum
        {
            BindNodeMethodTest = 0,
            /// <summary>
            /// [1] 
            /// int value 
            /// </summary>
            CallCustomPersistence = 1,
            /// <summary>
            /// [2] 
            /// </summary>
            Callback = 2,
            /// <summary>
            /// [3] 
            /// 返回值 bool 
            /// </summary>
            CallbackBeforePersistence = 3,
            /// <summary>
            /// [4] 
            /// int value 
            /// </summary>
            CustomPersistence = 4,
            /// <summary>
            /// [5] 
            /// 返回值 int 
            /// </summary>
            Enumerable = 5,
            /// <summary>
            /// [6] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            EnumerableBeforePersistence = 6,
            /// <summary>
            /// [7] 
            /// 返回值 AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster 
            /// </summary>
            GetJsonValue = 7,
            /// <summary>
            /// [8] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray 
            /// </summary>
            GetServerByteArray = 8,
            /// <summary>
            /// [9] 
            /// 返回值 int 
            /// </summary>
            GetValue = 9,
            /// <summary>
            /// [10] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            GetValueBeforePersistence = 10,
            /// <summary>
            /// [11] 
            /// int value 
            /// int count 
            /// 返回值 int 
            /// </summary>
            InputEnumerable = 11,
            /// <summary>
            /// [12] 
            /// int value 
            /// int count 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            InputEnumerableBeforePersistence = 12,
            /// <summary>
            /// [13] 
            /// int value 
            /// int count 
            /// </summary>
            InputKeepCallback = 13,
            /// <summary>
            /// [14] 
            /// int value 
            /// int count 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            InputKeepCallbackBeforePersistence = 14,
            /// <summary>
            /// [15] 
            /// </summary>
            KeepCallback = 15,
            /// <summary>
            /// [16] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            KeepCallbackBeforePersistence = 16,
            /// <summary>
            /// [17] 
            /// </summary>
            PersistenceCallbackException = 17,
            /// <summary>
            /// [18] 
            /// </summary>
            SetCallback = 18,
            /// <summary>
            /// [19] 
            /// </summary>
            SetCallbackPersistence = 19,
            /// <summary>
            /// [20] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            SetCallbackPersistenceBeforePersistence = 20,
            /// <summary>
            /// [21] 
            /// AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster value 
            /// </summary>
            SetJsonValue = 21,
            /// <summary>
            /// [22] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// </summary>
            SetServerByteArray = 22,
            /// <summary>
            /// [23] 
            /// int value 
            /// </summary>
            SetValue = 23,
            /// <summary>
            /// [24] 
            /// int value 
            /// 返回值 bool 
            /// </summary>
            SetValueBeforePersistence = 24,
            /// <summary>
            /// [25] 
            /// int value 
            /// </summary>
            SetValueCallback = 25,
            /// <summary>
            /// [26] 
            /// int value 
            /// </summary>
            SetValueCallbackPersistence = 26,
            /// <summary>
            /// [27] 
            /// int value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            SetValueCallbackPersistenceBeforePersistence = 27,
            /// <summary>
            /// [28] 
            /// int value 
            /// </summary>
            SetValuePersistenceSendOnly = 28,
            /// <summary>
            /// [29] 
            /// int value 
            /// 返回值 bool 
            /// </summary>
            SetValuePersistenceSendOnlyBeforePersistence = 29,
            /// <summary>
            /// [30] 
            /// int value 
            /// </summary>
            SetValueSendOnly = 30,
            /// <summary>
            /// [31] 
            /// int value 
            /// </summary>
            SnapshotSet = 31,
            /// <summary>
            /// [32] 
            /// 返回值 bool 
            /// </summary>
            CheckSnapshot = 32,
            /// <summary>
            /// [33] 
            /// long value 
            /// </summary>
            SnapshotSet64 = 33,
            /// <summary>
            /// [34] 
            /// int value 
            /// 返回值 int 
            /// </summary>
            CallInoutOutputCommand = 34,
            /// <summary>
            /// [35] 
            /// </summary>
            CallbackCommand = 35,
            /// <summary>
            /// [36] 
            /// 返回值 int 
            /// </summary>
            GetValueCommand = 36,
            /// <summary>
            /// [37] 
            /// int value 
            /// </summary>
            SetValueCommand = 37,
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作自定义扩展接口（用于添加自定义节点创建接口）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(ICustomServiceNodeMethodEnum))]
        public partial interface ICustomServiceNode { }
        /// <summary>
        /// 服务基础操作自定义扩展接口（用于添加自定义节点创建接口） 节点方法序号映射枚举类型
        /// </summary>
        public enum ICustomServiceNodeMethodEnum
        {
            /// <summary>
            /// [0] 创建数组节点 ArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int length 数组长度
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateArrayNode = 0,
            /// <summary>
            /// [1] 创建位图节点 BitmapNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// uint capacity 二进制位数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateBitmapNode = 1,
            /// <summary>
            /// [2] 创建字典节点 ByteArrayDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayDictionaryNode = 2,
            /// <summary>
            /// [3] 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayFragmentDictionaryNode = 3,
            /// <summary>
            /// [4] 创建队列节点（先进先出） ByteArrayQueueNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayQueueNode = 4,
            /// <summary>
            /// [5] 创建栈节点（后进先出） ByteArrayStackNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateByteArrayStackNode = 5,
            /// <summary>
            /// [6] 创建字典节点 DictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDictionaryNode = 6,
            /// <summary>
            /// [7] 创建分布式锁节点 DistributedLockNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateDistributedLockNode = 7,
            /// <summary>
            /// [8] 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentDictionaryNode = 8,
            /// <summary>
            /// [9] 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateFragmentHashSetNode = 9,
            /// <summary>
            /// [10] 创建字典节点 HashBytesDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesDictionaryNode = 10,
            /// <summary>
            /// [11] 创建字典节点 HashBytesFragmentDictionaryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashBytesFragmentDictionaryNode = 11,
            /// <summary>
            /// [12] 创建哈希表节点 HashSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// AutoCSer.ReusableDictionaryGroupTypeEnum groupType 可重用字典重组操作类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateHashSetNode = 12,
            /// <summary>
            /// [13] 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// long identity 起始分配 ID
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateIdentityGeneratorNode = 13,
            /// <summary>
            /// [14] 创建数组节点 LeftArrayNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateLeftArrayNode = 14,
            /// <summary>
            /// [15] 创建消息处理节点 MessageNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType messageType 消息数据类型
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateMessageNode = 15,
            /// <summary>
            /// [16] 创建队列节点（先进先出） QueueNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateQueueNode = 16,
            /// <summary>
            /// [17] 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeDictionaryNode = 17,
            /// <summary>
            /// [18] 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSearchTreeSetNode = 18,
            /// <summary>
            /// [19] 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int arraySize 正在处理消息数组大小
            /// int timeoutSeconds 消息处理超时秒数
            /// int checkTimeoutSeconds 消息超时检查间隔秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerByteArrayMessageNode = 19,
            /// <summary>
            /// [20] 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedDictionaryNode = 20,
            /// <summary>
            /// [21] 创建排序列表节点 SortedListNode{KT,VT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// AutoCSer.Reflection.RemoteType valueType 数据类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedListNode = 21,
            /// <summary>
            /// [22] 创建排序集合节点 SortedSetNode{KT}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateSortedSetNode = 22,
            /// <summary>
            /// [23] 创建栈节点（后进先出） StackNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType keyType 关键字类型
            /// int capacity 容器初始化大小
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateStackNode = 23,
            /// <summary>
            /// [24] 删除节点
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNode = 24,
            /// <summary>
            /// [25] 创建服务注册节点 IServerRegistryNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int loadTimeoutSeconds 冷启动会话超时秒数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateServerRegistryNode = 25,
            /// <summary>
            /// [26] 创建服务进程守护节点 IProcessGuardNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateProcessGuardNode = 26,
            /// <summary>
            /// [27] 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapClientFilterNode = 27,
            /// <summary>
            /// [28] 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// int size 位图大小（位数量）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateManyHashBitMapFilterNode = 28,
            /// <summary>
            /// [29] 删除节点
            /// string key 节点全局关键字
            /// 返回值 bool 是否成功删除节点，否则表示没有找到节点
            /// </summary>
            RemoveNodeByKey = 29,
            /// <summary>
            /// [30] 创建仅存档节点 PersistenceNode{T}
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// AutoCSer.Reflection.RemoteType valueType 存档数据类型
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateOnlyPersistenceNode = 30,
            /// <summary>
            /// [256] 创建回调测试节点 ICallbackNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateCallbackNode = 256,
            /// <summary>
            /// [257] 创建游戏测试节点 GameNode
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 节点标识，已经存在节点则直接返回
            /// </summary>
            CreateGameNode = 257,
        }
}
#endif