//本文件由程序自动生成，请不要自行修改
using System;
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
            /// <param name="id"></param>
            /// <param name="speed"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetSpeed(int id, int speed);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="speeds"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetSpeeds(AutoCSer.KeyValue<int,int>[] speeds);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICallbackNode))]
        public partial interface ICallbackNodeClientNode
        {
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>> GetJsonValue();
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetJsonValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
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
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter CallCustomPersistence(int value);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICustomServiceNode))]
        public partial interface ICustomServiceNodeClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateCallbackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <param name="key"></param>
            /// <param name="nodeInfo"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateGameNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.IGameNode))]
        public partial interface IGameNodeLocalClientNode
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="monster"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> AddMonster(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster monster);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="monsters"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> AddMonsters(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game.Monster[] monsters);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="speed"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetSpeed(int id, int speed);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="speeds"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetSpeeds(AutoCSer.KeyValue<int,int>[] speeds);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> Clear();
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICallbackNode))]
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass>>> GetJsonValue();
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray>> GetServerByteArray();
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> PersistenceCallbackException();
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
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetJsonValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue<AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass> value);
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> SetServerByteArray(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
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
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
        /// <summary>
        ///  客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.TestCase.StreamPersistenceMemoryDatabase.ICustomServiceNode))]
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
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateGameNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
        }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Game
{
    public enum GameNodeMethodEnum
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
            /// int id 
            /// int speed 
            /// </summary>
            SetSpeed = 2,
            /// <summary>
            /// [3] 
            /// AutoCSer.KeyValue{int,int}[] speeds 
            /// </summary>
            SetSpeeds = 3,
            /// <summary>
            /// [4] 
            /// </summary>
            Clear = 4,
    }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    public enum CallbackNodeMethodEnum
    {
            BindNodeMethodTest = 0,
            /// <summary>
            /// [1] 
            /// </summary>
            Callback = 1,
            /// <summary>
            /// [2] 
            /// 返回值 bool 
            /// </summary>
            CallbackBeforePersistence = 2,
            /// <summary>
            /// [3] 
            /// int value 
            /// </summary>
            CustomPersistence = 3,
            /// <summary>
            /// [4] 
            /// 返回值 int 
            /// </summary>
            Enumerable = 4,
            /// <summary>
            /// [5] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            EnumerableBeforePersistence = 5,
            /// <summary>
            /// [6] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} 
            /// </summary>
            GetJsonValue = 6,
            /// <summary>
            /// [7] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray 
            /// </summary>
            GetServerByteArray = 7,
            /// <summary>
            /// [8] 
            /// 返回值 int 
            /// </summary>
            GetValue = 8,
            /// <summary>
            /// [9] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            GetValueBeforePersistence = 9,
            /// <summary>
            /// [10] 
            /// int value 
            /// int count 
            /// 返回值 int 
            /// </summary>
            InputEnumerable = 10,
            /// <summary>
            /// [11] 
            /// int value 
            /// int count 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            InputEnumerableBeforePersistence = 11,
            /// <summary>
            /// [12] 
            /// int value 
            /// int count 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            InputKeepCallback = 12,
            /// <summary>
            /// [13] 
            /// int value 
            /// int count 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            InputKeepCallbackBeforePersistence = 13,
            /// <summary>
            /// [14] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            KeepCallback = 14,
            /// <summary>
            /// [15] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            KeepCallbackBeforePersistence = 15,
            /// <summary>
            /// [16] 
            /// </summary>
            PersistenceCallbackException = 16,
            /// <summary>
            /// [17] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetCallback = 17,
            /// <summary>
            /// [18] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetCallbackPersistence = 18,
            /// <summary>
            /// [19] 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            SetCallbackPersistenceBeforePersistence = 19,
            /// <summary>
            /// [20] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.JsonValue{AutoCSer.TestCase.StreamPersistenceMemoryDatabase.TestClass} value 
            /// </summary>
            SetJsonValue = 20,
            /// <summary>
            /// [21] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// </summary>
            SetServerByteArray = 21,
            /// <summary>
            /// [22] 
            /// int value 
            /// </summary>
            SetValue = 22,
            /// <summary>
            /// [23] 
            /// int value 
            /// 返回值 bool 
            /// </summary>
            SetValueBeforePersistence = 23,
            /// <summary>
            /// [24] 
            /// int value 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetValueCallback = 24,
            /// <summary>
            /// [25] 
            /// int value 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback{int} callback 
            /// 返回值 int 
            /// </summary>
            SetValueCallbackPersistence = 25,
            /// <summary>
            /// [26] 
            /// int value 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 
            /// </summary>
            SetValueCallbackPersistenceBeforePersistence = 26,
            /// <summary>
            /// [27] 
            /// int value 
            /// </summary>
            SetValuePersistenceSendOnly = 27,
            /// <summary>
            /// [28] 
            /// int value 
            /// 返回值 bool 
            /// </summary>
            SetValuePersistenceSendOnlyBeforePersistence = 28,
            /// <summary>
            /// [29] 
            /// int value 
            /// </summary>
            SetValueSendOnly = 29,
            /// <summary>
            /// [30] 
            /// int value 
            /// </summary>
            SnapshotSet = 30,
            /// <summary>
            /// [31] 
            /// int value 
            /// </summary>
            CallCustomPersistence = 31,
    }
}namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    public enum CustomServiceNodeMethodEnum
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
            /// [25] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateCallbackNode = 25,
            /// <summary>
            /// [26] 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// string key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            CreateGameNode = 26,
    }
}
#endif