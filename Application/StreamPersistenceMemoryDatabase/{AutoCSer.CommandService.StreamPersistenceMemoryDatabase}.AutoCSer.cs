//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    public enum ServiceMethodEnum
    {
            /// <summary>
            /// [0] 从节点添加修复方法目录与文件信息
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// long timestamp 创建从节点客户端信息时间戳
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RepairNodeMethodDirectory directory 修复方法目录信息
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RepairNodeMethodFile file 修复方法文件信息
            /// </summary>
            AppendRepairNodeMethodDirectoryFile = 0,
            /// <summary>
            /// [1] 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// byte[] rawAssembly 程序集文件数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RepairNodeMethodName methodName 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum 
            /// </summary>
            BindNodeMethod = 1,
            /// <summary>
            /// [2] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// int methodIndex 调用方法编号
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum 
            /// </summary>
            Call = 2,
            /// <summary>
            /// [3] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum 
            /// </summary>
            CallInput = 3,
            /// <summary>
            /// [4] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            CallInputOutput = 4,
            /// <summary>
            /// [5] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// int methodIndex 调用方法编号
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            CallOutput = 5,
            /// <summary>
            /// [6] 检查持久化回调异常位置文件头部是否匹配
            /// uint fileHeadVersion 持久化回调异常位置文件头部版本信息
            /// ulong rebuildPosition 持久化流重建起始位置
            /// 返回值 long 持久化回调异常位置文件已写入位置，失败返回 -1
            /// </summary>
            CheckPersistenceCallbackExceptionPositionFileHead = 6,
            /// <summary>
            /// [7] 检查持久化文件头部是否匹配
            /// uint fileHeadVersion 持久化文件头部版本信息
            /// ulong rebuildPosition 持久化流重建起始位置
            /// 返回值 long 持久化流已写入位置，失败返回 -1
            /// </summary>
            CheckPersistenceFileHead = 7,
            /// <summary>
            /// [8] 创建从节点
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// bool isBackup 是否备份客户端
            /// 返回值 long 从节点验证时间戳，负数表示 CallStateEnum 错误状态
            /// </summary>
            CreateSlave = 8,
            /// <summary>
            /// [9] 获取节点标识
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// string key 节点全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 节点信息
            /// bool isCreate 关键字不存在时创建空闲节点标识
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 关键字不存在时返回一个空闲节点标识用于创建节点
            /// </summary>
            GetNodeIndex = 9,
            /// <summary>
            /// [10] 获取持久化回调异常位置数据
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// long timestamp 创建从节点客户端信息时间戳
            /// AutoCSer.Net.CommandServerKeepCallback{long} callback 
            /// 返回值 long 
            /// </summary>
            GetPersistenceCallbackExceptionPosition = 10,
            /// <summary>
            /// [11] 获取持久化回调异常位置文件数据
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// long timestamp 创建从节点客户端信息时间戳
            /// uint fileHeadVersion 持久化回调异常位置文件头部版本信息
            /// ulong rebuildPosition 持久化流重建起始位置
            /// long position 读取文件起始位置
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.PersistenceFileBuffer} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.PersistenceFileBuffer 
            /// </summary>
            GetPersistenceCallbackExceptionPositionFile = 11,
            /// <summary>
            /// [12] 获取持久化文件数据
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// long timestamp 创建从节点客户端信息时间戳
            /// uint fileHeadVersion 持久化文件头部版本信息
            /// ulong rebuildPosition 持久化流重建起始位置
            /// long position 读取文件起始位置
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.PersistenceFileBuffer} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.PersistenceFileBuffer 
            /// </summary>
            GetPersistenceFile = 12,
            /// <summary>
            /// [13] 获取持久化流已当前写入位置
            /// 返回值 long 
            /// </summary>
            GetPersistencePosition = 13,
            /// <summary>
            /// [14] 获取重建快照结束位置
            /// 返回值 long 重建快照结束位置
            /// </summary>
            GetRebuildSnapshotPosition = 14,
            /// <summary>
            /// [15] 从节点获取修复节点方法信息
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// long timestamp 创建从节点客户端信息时间戳
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RepairNodeMethodPosition} callback 获取修复节点方法信息委托
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RepairNodeMethodPosition 
            /// </summary>
            GetRepairNodeMethodPosition = 15,
            /// <summary>
            /// [16] 获取服务端 UTC 时间
            /// 返回值 System.DateTime 
            /// </summary>
            GetUtcNow = 16,
            /// <summary>
            /// [17] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter 
            /// </summary>
            InputKeepCallback = 17,
            /// <summary>
            /// [18] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// int methodIndex 调用方法编号
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter 
            /// </summary>
            KeepCallback = 18,
            /// <summary>
            /// [19] 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RebuildResult 
            /// </summary>
            Rebuild = 19,
            /// <summary>
            /// [20] 移除从节点客户端信息
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// long timestamp 创建从节点客户端信息时间戳
            /// </summary>
            RemoveSlave = 20,
            /// <summary>
            /// [21] 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 
            /// byte[] rawAssembly 程序集文件数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RepairNodeMethodName methodName 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum 
            /// </summary>
            RepairNodeMethod = 21,
            /// <summary>
            /// [22] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// </summary>
            SendOnly = 22,
            /// <summary>
            /// [23] 获取所有匹配节点的节点索引信息
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 匹配服务端节点信息
            /// AutoCSer.Net.CommandServerKeepCallbackCount{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex 
            /// </summary>
            GetNodeIndexs = 23,
            /// <summary>
            /// [24] 获取所有匹配节点的全局关键字与节点索引信息
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 匹配服务端节点信息
            /// AutoCSer.Net.CommandServerKeepCallbackCount{AutoCSer.BinarySerializeKeyValue{string,AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex}} callback 
            /// 返回值 AutoCSer.BinarySerializeKeyValue{string,AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex} 
            /// </summary>
            GetNodeKeyIndexs = 24,
            /// <summary>
            /// [25] 获取所有匹配节点的全局关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo 匹配服务端节点信息
            /// AutoCSer.Net.CommandServerKeepCallbackCount{string} callback 
            /// 返回值 string 
            /// </summary>
            GetNodeKeys = 25,
            /// <summary>
            /// [26] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            CallInputOutputWrite = 26,
            /// <summary>
            /// [27] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum 
            /// </summary>
            CallInputWrite = 27,
            /// <summary>
            /// [28] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// int methodIndex 调用方法编号
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            CallOutputWrite = 28,
            /// <summary>
            /// [29] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// int methodIndex 调用方法编号
            /// AutoCSer.Net.CommandServerCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum} callback 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum 
            /// </summary>
            CallWrite = 29,
            /// <summary>
            /// [30] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter 
            /// </summary>
            InputKeepCallbackWrite = 30,
            /// <summary>
            /// [31] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index 节点索引信息
            /// int methodIndex 调用方法编号
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter} callback 返回参数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter 
            /// </summary>
            KeepCallbackWrite = 31,
            /// <summary>
            /// [32] 调用节点方法
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallConcurrencyReadWriteQueue queue 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.RequestParameter parameter 请求参数
            /// </summary>
            SendOnlyWrite = 32,
    }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
        /// <summary>
        /// 超时任务消息节点接口（用于分布式事务数据一致性检查） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.ITimeoutMessageNode<>))]
        public partial interface ITimeoutMessageNodeClientNode<T>
        {
            /// <summary>
            /// 添加任务
            /// </summary>
            /// <param name="task"></param>
            /// <returns>任务标识，失败返回 0</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> Append(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage<T> task);
            /// <summary>
            /// 取消任务
            /// </summary>
            /// <param name="identity">任务标识</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Cancel(long identity);
            /// <summary>
            /// 获取任务总数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCount();
            /// <summary>
            /// 获取执行失败任务数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetFailedCount();
            /// <summary>
            /// 失败任务重试
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RetryFailed();
            /// <summary>
            /// 触发任务执行
            /// </summary>
            /// <param name="identity">任务标识</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter RunTask(long identity);
            /// <summary>
            /// 添加立即执行任务
            /// </summary>
            /// <param name="task"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendRun(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage<T> task);
            /// <summary>
            /// 获取执行任务消息数据
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand GetRunTask(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<T>,AutoCSer.Net.KeepCallbackCommand> callback);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IArrayNode<>))]
        public partial interface IArrayNodeClientNode<T>
        {
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter FillArray(T value);
            /// <summary>
            /// 获取数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetLength();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValue(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfArray(T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOfArray(T value);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReverseArray();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SortArray();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNode))]
        public partial interface IBitmapNodeClientNode
        {
            /// <summary>
            /// 清除位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ClearBit(uint index);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearMap();
            /// <summary>
            /// 读取位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBit(uint index);
            /// <summary>
            /// 清除位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBitClearBit(uint index);
            /// <summary>
            /// 状态取反并返回操作之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBitInvertBit(uint index);
            /// <summary>
            /// 设置位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>> GetBitSetBit(uint index);
            /// <summary>
            /// 获取二进制位数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<uint> GetCapacity();
            /// <summary>
            /// 状态取反
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> InvertBit(uint index);
            /// <summary>
            /// 设置位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> SetBit(uint index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayDictionaryNode<>))]
        public partial interface IByteArrayDictionaryNodeClientNode<KT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<byte[][]> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayFragmentDictionaryNode<>))]
        public partial interface IByteArrayFragmentDictionaryNodeClientNode<KT>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<byte[][]> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayQueueNode))]
        public partial interface IByteArrayQueueNodeClientNode
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Enqueue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryDequeue();
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryDequeueResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryPeek();
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryPeekResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IByteArrayStackNode))]
        public partial interface IByteArrayStackNodeClientNode
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到栈
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Push(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryPeek();
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryPeekResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryPop();
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryPopResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNode<,>))]
        public partial interface IDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReusableClear();
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, VT value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<VT[]> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 分布式锁节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNode<>))]
        public partial interface IDistributedLockNodeClientNode<T>
        {
            /// <summary>
            /// 申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> Enter(T key, ushort timeoutSeconds);
            /// <summary>
            /// 释放锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="identity">锁操作标识</param>
            AutoCSer.Net.SendOnlyCommand Release(T key, long identity);
            /// <summary>
            /// 尝试申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns>失败返回 0</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> TryEnter(T key, ushort timeoutSeconds);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNode<,>))]
        public partial interface IFragmentDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, VT value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<VT[]> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveKeys(KT[] keys);
            /// <summary>
            /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReusableClear();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNode<>))]
        public partial interface IFragmentHashSetNodeClientNode<T>
        {
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveValues(T[] values);
            /// <summary>
            /// 可重用哈希表重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReusableClear();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesDictionaryNode))]
        public partial interface IHashBytesDictionaryNodeClientNode
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 HashBytes 字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashBytesFragmentDictionaryNode))]
        public partial interface IHashBytesFragmentDictionaryNodeClientNode
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> GetRemove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> GetRemoveResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> TryGetResponseParameter(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter returnValue, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<byte[]>> TryGetValue(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashSetNode<>))]
        public partial interface IHashSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Renew(int capacity);
            /// <summary>
            /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReusableClear();
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveValues(T[] values);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNode))]
        public partial interface IIdentityGeneratorNodeClientNode
        {
            /// <summary>
            /// 获取下一个自增ID
            /// </summary>
            /// <returns>下一个自增ID，失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> Next();
            /// <summary>
            /// 获取自增 ID 分段
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>自增 ID 分段</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment> NextFragment(int count);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNode<>))]
        public partial interface ILeftArrayNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Add(T value);
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearLength();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter FillArray(T value);
            /// <summary>
            /// 获取数组容器初大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCapacity();
            /// <summary>
            /// 获取容器空闲数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetFreeCount();
            /// <summary>
            /// 获取有效数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetLength();
            /// <summary>
            /// 移除最后一个数据并返回该数据
            /// </summary>
            /// <returns>没有可移除数据则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetTryPopValue();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValue(int index);
            /// <summary>
            /// 移除指定索引位置数据并返回被移除的数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueRemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueRemoveToEnd(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfArray(T value);
            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="index">插入位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Insert(int index, T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> LastIndexOfArray(T value);
            /// <summary>
            /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">数据</param>
            /// <returns>是否存在移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
            /// <summary>
            /// 移除指定索引位置数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveToEnd(int index);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ReverseArray();
            /// <summary>
            /// 置空并释放数组
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetEmpty();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SortArray();
            /// <summary>
            /// 当有空闲位置时添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>如果数组已满则添加失败并返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(T value);
            /// <summary>
            /// 移除最后一个数据
            /// </summary>
            /// <returns>是否存在可移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IManyHashBitMapClientFilterNode))]
        public partial interface IManyHashBitMapClientFilterNodeClientNode
        {
            /// <summary>
            /// 获取设置新位操作
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand GetBit(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<int>,AutoCSer.Net.KeepCallbackCommand> callback);
            /// <summary>
            /// 获取当前位图数据
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap> GetData();
            /// <summary>
            /// 设置位
            /// </summary>
            /// <param name="bit">位置</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter SetBit(int bit);
            /// <summary>
            /// 获取位图大小（位数量）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetSize();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IManyHashBitMapFilterNode))]
        public partial interface IManyHashBitMapFilterNodeClientNode
        {
            /// <summary>
            /// 获取位图大小（位数量）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetSize();
            /// <summary>
            /// 设置位
            /// </summary>
            /// <param name="size">位图大小（位数量）</param>
            /// <param name="bits">位置集合</param>
            /// <returns>返回 false 表示位图大小不匹配</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> SetBits(int size, uint[] bits);
            /// <summary>
            /// 检查位图数据
            /// </summary>
            /// <param name="size">位图大小（位数量）</param>
            /// <param name="bits">位置集合</param>
            /// <returns>返回 false 表示数据不存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.NullableBoolEnum> CheckBits(int size, uint[] bits);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNode<>))]
        public partial interface IMessageNodeClientNode<T>
        {
            /// <summary>
            /// 生产者添加新消息
            /// </summary>
            /// <param name="message"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter AppendMessage(T message);
            /// <summary>
            /// 清除所有消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 清除所有失败消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter ClearFailed();
            /// <summary>
            /// 消息完成处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.Net.SendOnlyCommand Completed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 消息失败处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.Net.SendOnlyCommand Failed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 获取消费者回调数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCallbackCount();
            /// <summary>
            /// 获取未处理完成消息数量（不包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCount();
            /// <summary>
            /// 获取失败消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetFailedCount();
            /// <summary>
            /// 消费客户端获取消息
            /// </summary>
            /// <param name="maxCount">当前客户端最大并发消息数量</param>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand GetMessage(int maxCount, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<T>,AutoCSer.Net.KeepCallbackCommand> callback);
            /// <summary>
            /// 获取未完成处理超时消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetTimeoutCount();
            /// <summary>
            /// 获取未处理完成消息数量（包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetTotalCount();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 进程守护节点接口（服务端需要以管理员身份运行，否则可能异常） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IProcessGuardNode))]
        public partial interface IProcessGuardNodeClientNode
        {
            /// <summary>
            /// 添加待守护进程
            /// </summary>
            /// <param name="processInfo">进程信息</param>
            /// <returns>是否添加成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Guard(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ProcessGuardInfo processInfo);
            /// <summary>
            /// 删除被守护进程
            /// </summary>
            /// <param name="processId">进程标识</param>
            /// <param name="startTime">进程启动时间</param>
            /// <param name="processName">进程名称</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Remove(int processId, System.DateTime startTime, string processName);
            /// <summary>
            /// 切换进程
            /// </summary>
            /// <param name="key">切换进程关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Switch(string key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IQueueNode<>))]
        public partial interface IQueueNodeClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Enqueue(T value);
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryDequeue();
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryPeek();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNode<,>))]
        public partial interface ISearchTreeDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断是否包含关键字
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否包含关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 获取节点数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountLess(KT key);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountThan(KT key);
            /// <summary>
            /// 获取树高度，时间复杂度 O(n)
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetHeight();
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 获取范围数据集合
            /// </summary>
            /// <param name="skipCount">跳过记录数</param>
            /// <param name="getCount">获取记录数</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetValues(int skipCount, byte getCount);
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(KT key);
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Set(KT key, VT value);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 获取第一个关键字数据
            /// </summary>
            /// <returns>第一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>> TryGetFirstKey();
            /// <summary>
            /// 获取第一组数据
            /// </summary>
            /// <returns>第一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>> TryGetFirstKeyValue();
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>第一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetFirstValue();
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>> TryGetKeyValueByIndex(int index);
            /// <summary>
            /// 获取最后一个关键字数据
            /// </summary>
            /// <returns>最后一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>> TryGetLastKey();
            /// <summary>
            /// 获取最后一组数据
            /// </summary>
            /// <returns>最后一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>> TryGetLastKeyValue();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>最后一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetLastValue();
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>目标数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValueByIndex(int index);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<VT[]> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeSetNode<>))]
        public partial interface ISearchTreeSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountLess(T value);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> CountThan(T value);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetByIndex(int index);
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetFrist();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetLast();
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOf(T value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveValues(T[] values);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务注册节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServerRegistryNode))]
        public partial interface IServerRegistryNodeClientNode
        {
            /// <summary>
            /// 添加服务注册日志
            /// </summary>
            /// <param name="log"></param>
            /// <returns>服务注册结果</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryStateEnum> Append(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog log);
            /// <summary>
            /// 获取服务会话标识ID
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> GetSessionID();
            /// <summary>
            /// 获取服务注册日志
            /// </summary>
            /// <param name="serverName">监视服务名称，空字符串表示所有服务</param>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand LogCallback(string serverName, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog>,AutoCSer.Net.KeepCallbackCommand> callback);
            /// <summary>
            /// 服务注册回调委托，主要用于注册组件检查服务的在线状态
            /// </summary>
            /// <param name="sessionID">服务会话标识ID</param>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand ServiceCallback(long sessionID, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum>,AutoCSer.Net.KeepCallbackCommand> callback);
            /// <summary>
            /// 获取服务主日志
            /// </summary>
            /// <param name="serverName">服务名称</param>
            /// <returns>返回 null 表示没有找到服务主日志</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog> GetLog(string serverName);
            /// <summary>
            /// 检查服务在线状态
            /// </summary>
            /// <param name="sessionID">服务会话标识ID</param>
            /// <param name="serverName">服务名称</param>
            AutoCSer.Net.SendOnlyCommand Check(long sessionID, string serverName);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode))]
        public partial interface IServiceNodeClientNode
        {
            /// <summary>
            /// 创建数组节点 ArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="length">数组长度</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int length);
            /// <summary>
            /// 创建位图节点 BitmapNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">二进制位数量</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity);
            /// <summary>
            /// 创建字典节点 ByteArrayDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建队列节点（先进先出） ByteArrayQueueNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建栈节点（后进先出） ByteArrayStackNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateByteArrayStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建字典节点 DictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建分布式锁节点 DistributedLockNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateDistributedLockNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateFragmentHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 HashBytesDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateHashBytesDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建字典节点 HashBytesFragmentDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateHashBytesFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 创建哈希表节点 HashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="identity">起始分配 ID</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateIdentityGeneratorNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, long identity);
            /// <summary>
            /// 创建数组节点 LeftArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateLeftArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建消息处理节点 MessageNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="messageType">消息数据类型</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType messageType, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建队列节点（先进先出） QueueNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSearchTreeSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateServerByteArrayMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSortedDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建排序列表节点 SortedListNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSortedListNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
            /// <summary>
            /// 创建排序集合节点 SortedSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateSortedSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建栈节点（后进先出） StackNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index);
            /// <summary>
            /// 创建服务注册节点 IServerRegistryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="loadTimeoutSeconds">冷启动会话超时秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateServerRegistryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int loadTimeoutSeconds);
            /// <summary>
            /// 创建服务进程守护节点 IProcessGuardNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateProcessGuardNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateManyHashBitMapClientFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size);
            /// <summary>
            /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex> CreateManyHashBitMapFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveNodeByKey(string key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNode<,>))]
        public partial interface ISortedDictionaryNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<VT[]> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNode<,>))]
        public partial interface ISortedListNodeClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 获取容器大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetCapacity();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> GetRemove(KT key);
            /// <summary>
            /// 获取关键字排序位置
            /// </summary>
            /// <param name="key"></param>
            /// <returns>负数表示没有找到关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfKey(KT key);
            /// <summary>
            /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>负数表示没有找到匹配数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> IndexOfValue(VT value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(KT key);
            /// <summary>
            /// 删除指定排序索引位置数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>索引超出范围返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> RemoveAt(int index);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNode<>))]
        public partial interface ISortedSetNodeClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 获取最大值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetMax();
            /// <summary>
            /// 获取最小值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> GetMin();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Remove(T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> RemoveValues(T[] values);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNode<>))]
        public partial interface IStackNodeClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Clear();
            /// <summary>
            /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<bool> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> Count();
            /// <summary>
            /// 将数据添加到栈
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Push(T value);
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryPeek();
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
        /// <summary>
        /// 超时任务消息节点接口（用于分布式事务数据一致性检查） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.ITimeoutMessageNode<>))]
        public partial interface ITimeoutMessageNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加任务
            /// </summary>
            /// <param name="task"></param>
            /// <returns>任务标识，失败返回 0</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<long>> Append(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage<T> task);
            /// <summary>
            /// 取消任务
            /// </summary>
            /// <param name="identity">任务标识</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Cancel(long identity);
            /// <summary>
            /// 获取任务总数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetCount();
            /// <summary>
            /// 获取执行失败任务数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetFailedCount();
            /// <summary>
            /// 失败任务重试
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RetryFailed();
            /// <summary>
            /// 触发任务执行
            /// </summary>
            /// <param name="identity">任务标识</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> RunTask(long identity);
            /// <summary>
            /// 添加立即执行任务
            /// </summary>
            /// <param name="task"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendRun(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage<T> task);
            /// <summary>
            /// 获取执行任务消息数据
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<System.IDisposable> GetRunTask(System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<T>> callback);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IArrayNode<>))]
        public partial interface IArrayNodeLocalClientNode<T>
        {
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ClearArray();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> FillArray(T value);
            /// <summary>
            /// 获取数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetLength();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValue(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOfArray(T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> LastIndexOfArray(T value);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ReverseArray();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SortArray();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IBitmapNode))]
        public partial interface IBitmapNodeLocalClientNode
        {
            /// <summary>
            /// 清除位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ClearBit(uint index);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ClearMap();
            /// <summary>
            /// 读取位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>非 0 表示二进制位为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBit(uint index);
            /// <summary>
            /// 清除位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitClearBit(uint index);
            /// <summary>
            /// 状态取反并返回操作之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitInvertBit(uint index);
            /// <summary>
            /// 设置位状态并返回设置之前的状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<int>>> GetBitSetBit(uint index);
            /// <summary>
            /// 获取二进制位数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<uint>> GetCapacity();
            /// <summary>
            /// 状态取反
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> InvertBit(uint index);
            /// <summary>
            /// 设置位状态
            /// </summary>
            /// <param name="index">位索引</param>
            /// <returns>是否设置成功，失败表示索引超出范围</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> SetBit(uint index);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDictionaryNode<,>))]
        public partial interface IDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ReusableClear();
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(KT key);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">新容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Renew(int capacity);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 尝试添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<VT[]>> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 分布式锁节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IDistributedLockNode<>))]
        public partial interface IDistributedLockNodeLocalClientNode<T>
        {
            /// <summary>
            /// 申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<long>> Enter(T key, ushort timeoutSeconds);
            /// <summary>
            /// 释放锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="identity">锁操作标识</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter Release(T key, long identity);
            /// <summary>
            /// 尝试申请锁
            /// </summary>
            /// <param name="key">锁关键字</param>
            /// <param name="timeoutSeconds">超时秒数</param>
            /// <returns>失败返回 0</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<long>> TryEnter(T key, ushort timeoutSeconds);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentDictionaryNode<,>))]
        public partial interface IFragmentDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(KT key);
            /// <summary>
            /// 强制设置数据，如果关键字已存在则覆盖
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否设置成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<VT[]>> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveKeys(KT[] keys);
            /// <summary>
            /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ReusableClear();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IFragmentHashSetNode<>))]
        public partial interface IFragmentHashSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Add(T value);
            /// <summary>
            /// 清除数据（保留分片数组）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ClearArray();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveValues(T[] values);
            /// <summary>
            /// 可重用哈希表重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ReusableClear();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IHashSetNode<>))]
        public partial interface IHashSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(T value);
            /// <summary>
            /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            /// <param name="capacity">容器初始化大小</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Renew(int capacity);
            /// <summary>
            /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ReusableClear();
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveValues(T[] values);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IIdentityGeneratorNode))]
        public partial interface IIdentityGeneratorNodeLocalClientNode
        {
            /// <summary>
            /// 获取下一个自增ID
            /// </summary>
            /// <returns>下一个自增ID，失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<long>> Next();
            /// <summary>
            /// 获取自增 ID 分段
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>自增 ID 分段</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment>> NextFragment(int count);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ILeftArrayNode<>))]
        public partial interface ILeftArrayNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">数据</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Add(T value);
            /// <summary>
            /// 清除指定位置数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">清除数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Clear(int startIndex, int count);
            /// <summary>
            /// 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ClearLength();
            /// <summary>
            /// 用数据填充数组指定位置
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">填充数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Fill(T value, int startIndex, int count);
            /// <summary>
            /// 用数据填充整个数组
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> FillArray(T value);
            /// <summary>
            /// 获取数组容器初大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetCapacity();
            /// <summary>
            /// 获取容器空闲数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetFreeCount();
            /// <summary>
            /// 获取有效数组长度
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetLength();
            /// <summary>
            /// 移除最后一个数据并返回该数据
            /// </summary>
            /// <returns>没有可移除数据则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetTryPopValue();
            /// <summary>
            /// 根据索引位置获取数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValue(int index);
            /// <summary>
            /// 移除指定索引位置数据并返回被移除的数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueRemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则无数据返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueRemoveToEnd(int index);
            /// <summary>
            /// 根据索引位置设置数据并返回设置之前的数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>设置之前的数据，超出索引返回则无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetValueSet(int index, T value);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOfArray(T value);
            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="index">插入位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Insert(int index, T value);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <param name="startIndex">最后一个匹配位置（起始位置）</param>
            /// <param name="count">查找匹配数据数量</param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> LastIndexOf(T value, int startIndex, int count);
            /// <summary>
            /// 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>失败返回负数</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> LastIndexOfArray(T value);
            /// <summary>
            /// 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">数据</param>
            /// <returns>是否存在移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(T value);
            /// <summary>
            /// 移除指定索引位置数据
            /// </summary>
            /// <param name="index">数据位置</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> RemoveAt(int index);
            /// <summary>
            /// 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// </summary>
            /// <param name="index"></param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> RemoveToEnd(int index);
            /// <summary>
            /// 反转指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">反转数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Reverse(int startIndex, int count);
            /// <summary>
            /// 反转整个数组数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ReverseArray();
            /// <summary>
            /// 置空并释放数组
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SetEmpty();
            /// <summary>
            /// 根据索引位置设置数据
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="value">数据</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> SetValue(int index, T value);
            /// <summary>
            /// 排序指定位置数组数据
            /// </summary>
            /// <param name="startIndex">起始位置</param>
            /// <param name="count">排序数据数量</param>
            /// <returns>超出索引范围则返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Sort(int startIndex, int count);
            /// <summary>
            /// 数组排序
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> SortArray();
            /// <summary>
            /// 当有空闲位置时添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>如果数组已满则添加失败并返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> TryAdd(T value);
            /// <summary>
            /// 移除最后一个数据
            /// </summary>
            /// <returns>是否存在可移除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNode<>))]
        public partial interface IMessageNodeLocalClientNode<T>
        {
            /// <summary>
            /// 生产者添加新消息
            /// </summary>
            /// <param name="message"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> AppendMessage(T message);
            /// <summary>
            /// 清除所有消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 清除所有失败消息
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> ClearFailed();
            /// <summary>
            /// 消息完成处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter Completed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 消息失败处理
            /// </summary>
            /// <param name="identity"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter Failed(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity);
            /// <summary>
            /// 获取消费者回调数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetCallbackCount();
            /// <summary>
            /// 获取未处理完成消息数量（不包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetCount();
            /// <summary>
            /// 获取失败消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetFailedCount();
            /// <summary>
            /// 消费客户端获取消息
            /// </summary>
            /// <param name="maxCount">当前客户端最大并发消息数量</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<System.IDisposable> GetMessage(int maxCount, System.Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<T>> callback);
            /// <summary>
            /// 获取未完成处理超时消息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetTimeoutCount();
            /// <summary>
            /// 获取未处理完成消息数量（包括失败消息）
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetTotalCount();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IQueueNode<>))]
        public partial interface IQueueNodeLocalClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Contains(T value);
            /// <summary>
            /// 获取队列数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 将数据添加到队列
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Enqueue(T value);
            /// <summary>
            /// 从队列中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryDequeue();
            /// <summary>
            /// 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPeek();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeDictionaryNode<,>))]
        public partial interface ISearchTreeDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断是否包含关键字
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否包含关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 获取节点数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> CountLess(KT key);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> CountThan(KT key);
            /// <summary>
            /// 获取树高度，时间复杂度 O(n)
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetHeight();
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 获取范围数据集合
            /// </summary>
            /// <param name="skipCount">跳过记录数</param>
            /// <param name="getCount">获取记录数</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalKeepCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetValues(int skipCount, byte getCount);
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOf(KT key);
            /// <summary>
            /// 根据关键字删除节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>是否存在关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(KT key);
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Set(KT key, VT value);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <param name="value">数据</param>
            /// <returns>是否添加了数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 获取第一个关键字数据
            /// </summary>
            /// <returns>第一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>>> TryGetFirstKey();
            /// <summary>
            /// 获取第一组数据
            /// </summary>
            /// <returns>第一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetFirstKeyValue();
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>第一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetFirstValue();
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetKeyValueByIndex(int index);
            /// <summary>
            /// 获取最后一个关键字数据
            /// </summary>
            /// <returns>最后一个关键字数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<KT>>> TryGetLastKey();
            /// <summary>
            /// 获取最后一组数据
            /// </summary>
            /// <returns>最后一组数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.KeyValue<KT,VT>>>> TryGetLastKeyValue();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>最后一个数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetLastValue();
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>目标数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValueByIndex(int index);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<VT[]>> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISearchTreeSetNode<>))]
        public partial interface ISearchTreeSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 根据关键字比它小的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> CountLess(T value);
            /// <summary>
            /// 根据关键字比它大的节点数量
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>节点数量，失败返回 -1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> CountThan(T value);
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetByIndex(int index);
            /// <summary>
            /// 获取第一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetFrist();
            /// <summary>
            /// 获取最后一个数据
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetLast();
            /// <summary>
            /// 根据关键字获取一个匹配节点位置
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>一个匹配节点位置,失败返回-1</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOf(T value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value">关键字</param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveValues(T[] values);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode))]
        public partial interface IServiceNodeLocalClientNode
        {
            /// <summary>
            /// 创建数组节点 ArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="length">数组长度</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int length);
            /// <summary>
            /// 创建位图节点 BitmapNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">二进制位数量</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateBitmapNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, uint capacity);
            /// <summary>
            /// 创建字典节点 ByteArrayDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建字典节点 ByteArrayFragmentDictionaryNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建队列节点（先进先出） ByteArrayQueueNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建栈节点（后进先出） ByteArrayStackNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateByteArrayStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
            /// <summary>
            /// 创建字典节点 DictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建分布式锁节点 DistributedLockNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateDistributedLockNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 FragmentDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateFragmentHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建字典节点 HashBytesDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashBytesDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建字典节点 HashBytesFragmentDictionaryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashBytesFragmentDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 创建哈希表节点 HashSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <param name="groupType">可重用字典重组操作类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateHashSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, AutoCSer.ReusableDictionaryGroupTypeEnum groupType);
            /// <summary>
            /// 创建 64 位自增ID 节点 IdentityGeneratorNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="identity">起始分配 ID</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateIdentityGeneratorNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, long identity);
            /// <summary>
            /// 创建数组节点 LeftArrayNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateLeftArrayNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建消息处理节点 MessageNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="messageType">消息数据类型</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType messageType, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建队列节点（先进先出） QueueNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateQueueNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSearchTreeSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建消息处理节点 MessageNode{ServerByteArrayMessage}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="arraySize">正在处理消息数组大小</param>
            /// <param name="timeoutSeconds">消息处理超时秒数</param>
            /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerByteArrayMessageNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds);
            /// <summary>
            /// 创建排序字典节点 SortedDictionaryNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedDictionaryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType);
            /// <summary>
            /// 创建排序列表节点 SortedListNode{KT,VT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="valueType">数据类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedListNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity);
            /// <summary>
            /// 创建排序集合节点 SortedSetNode{KT}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateSortedSetNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType);
            /// <summary>
            /// 创建栈节点（后进先出） StackNode{T}
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="keyType">关键字类型</param>
            /// <param name="capacity">容器初始化大小</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateStackNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> RemoveNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index);
            /// <summary>
            /// 创建服务注册节点 IServerRegistryNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="loadTimeoutSeconds">冷启动会话超时秒数</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateServerRegistryNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int loadTimeoutSeconds);
            /// <summary>
            /// 创建服务进程守护节点 IProcessGuardNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateProcessGuardNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
            /// <summary>
            /// 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateManyHashBitMapClientFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size);
            /// <summary>
            /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
            /// </summary>
            /// <param name="index">节点索引信息</param>
            /// <param name="key">节点全局关键字</param>
            /// <param name="nodeInfo">节点信息</param>
            /// <param name="size">位图大小（位数量）</param>
            /// <returns>节点标识，已经存在节点则直接返回</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>> CreateManyHashBitMapFilterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int size);
            /// <summary>
            /// 删除节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> RemoveNodeByKey(string key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedDictionaryNode<,>))]
        public partial interface ISortedDictionaryNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(KT key);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="keys"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<VT[]>> GetValueArray(KT[] keys);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="keys"></param>
            /// <returns>删除关键字数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveKeys(KT[] keys);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedListNode<,>))]
        public partial interface ISortedListNodeLocalClientNode<KT,VT>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ContainsKey(KT key);
            /// <summary>
            /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> ContainsValue(VT value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 获取容器大小
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> GetCapacity();
            /// <summary>
            /// 删除关键字并返回被删除数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> GetRemove(KT key);
            /// <summary>
            /// 获取关键字排序位置
            /// </summary>
            /// <param name="key"></param>
            /// <returns>负数表示没有找到关键字</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOfKey(KT key);
            /// <summary>
            /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value"></param>
            /// <returns>负数表示没有找到匹配数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> IndexOfValue(VT value);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(KT key);
            /// <summary>
            /// 删除指定排序索引位置数据
            /// </summary>
            /// <param name="index"></param>
            /// <returns>索引超出范围返回 false</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> RemoveAt(int index);
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> TryAdd(KT key, VT value);
            /// <summary>
            /// 根据关键字获取数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<VT>>> TryGetValue(KT key);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序集合节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ISortedSetNode<>))]
        public partial interface ISortedSetNodeLocalClientNode<T>
        {
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否添加成功，否则表示关键字已经存在</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Add(T value);
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断关键字是否存在
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 获取最大值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetMax();
            /// <summary>
            /// 获取最小值
            /// </summary>
            /// <returns>没有数据时返回无返回值</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> GetMin();
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="value"></param>
            /// <returns>是否删除成功</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Remove(T value);
            /// <summary>
            /// 如果关键字不存在则添加数据
            /// </summary>
            /// <param name="values"></param>
            /// <returns>添加数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> AddValues(T[] values);
            /// <summary>
            /// 删除关键字
            /// </summary>
            /// <param name="values"></param>
            /// <returns>删除数据数量</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> RemoveValues(T[] values);
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出） 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IStackNode<>))]
        public partial interface IStackNodeLocalClientNode<T>
        {
            /// <summary>
            /// 清除所有数据
            /// </summary>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Clear();
            /// <summary>
            /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// </summary>
            /// <param name="value">匹配数据</param>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<bool>> Contains(T value);
            /// <summary>
            /// 获取数据数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<int>> Count();
            /// <summary>
            /// 将数据添加到栈
            /// </summary>
            /// <param name="value"></param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> Push(T value);
            /// <summary>
            /// 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPeek();
            /// <summary>
            /// 从栈中弹出一个数据
            /// </summary>
            /// <returns>没有可弹出数据则返回无数据</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<T>>> TryPop();
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
        /// <summary>
        /// 超时任务消息节点接口（用于分布式事务数据一致性检查）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ITimeoutMessageNodeMethodEnum))]
        public partial interface ITimeoutMessageNode<T> { }
        /// <summary>
        /// 超时任务消息节点接口（用于分布式事务数据一致性检查） 节点方法序号映射枚举类型
        /// </summary>
        public enum ITimeoutMessageNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加任务
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage{T} task 
            /// 返回值 long 任务标识，失败返回 0
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 添加任务 持久化前检查
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage{T} task 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{long} 
            /// </summary>
            AppendBeforePersistence = 1,
            /// <summary>
            /// [2] 取消任务
            /// long identity 任务标识
            /// </summary>
            Cancel = 2,
            /// <summary>
            /// [3] 完成任务
            /// long identity 
            /// bool isSuccess 
            /// </summary>
            Completed = 3,
            /// <summary>
            /// [4] 获取任务总数量
            /// 返回值 int 
            /// </summary>
            GetCount = 4,
            /// <summary>
            /// [5] 获取执行失败任务数量
            /// 返回值 int 
            /// </summary>
            GetFailedCount = 5,
            /// <summary>
            /// [6] 失败任务重试
            /// </summary>
            RetryFailed = 6,
            /// <summary>
            /// [7] 触发任务执行
            /// long identity 任务标识
            /// </summary>
            RunTask = 7,
            /// <summary>
            /// [8] 触发任务执行
            /// long identity 任务标识
            /// </summary>
            RunTaskLoadPersistence = 8,
            /// <summary>
            /// [9] 快照设置数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessageData{T} value 数据
            /// </summary>
            SnapshotAdd = 9,
            /// <summary>
            /// [10] 添加立即执行任务
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage{T} task 
            /// </summary>
            AppendRun = 10,
            /// <summary>
            /// [11] 添加立即执行任务 持久化前检查
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage{T} task 
            /// 返回值 bool 是否继续持久化操作
            /// </summary>
            AppendRunBeforePersistence = 11,
            /// <summary>
            /// [12] 添加立即执行任务
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode.TimeoutMessage{T} task 
            /// </summary>
            AppendRunLoadPersistence = 12,
            /// <summary>
            /// [13] 获取执行任务消息数据
            /// 返回值 T 
            /// </summary>
            GetRunTask = 13,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IArrayNodeMethodEnum))]
        public partial interface IArrayNode<T> { }
        /// <summary>
        /// 数组节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IArrayNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除指定位置数据
            /// int startIndex 起始位置
            /// int count 清除数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 用数据填充数组指定位置
            /// T value 
            /// int startIndex 起始位置
            /// int count 填充数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Fill = 2,
            /// <summary>
            /// [3] 用数据填充整个数组
            /// T value 
            /// </summary>
            FillArray = 3,
            /// <summary>
            /// [4] 获取数组长度
            /// 返回值 int 
            /// </summary>
            GetLength = 4,
            /// <summary>
            /// [5] 根据索引位置获取数据
            /// int index 索引位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引返回则无返回值
            /// </summary>
            GetValue = 5,
            /// <summary>
            /// [6] 根据索引位置设置数据并返回设置之前的数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 设置之前的数据，超出索引返回则无返回值
            /// </summary>
            GetValueSet = 6,
            /// <summary>
            /// [7] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 起始位置
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOf = 7,
            /// <summary>
            /// [8] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOfArray = 8,
            /// <summary>
            /// [9] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 最后一个匹配位置（起始位置）
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOf = 9,
            /// <summary>
            /// [10] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOfArray = 10,
            /// <summary>
            /// [11] 反转指定位置数组数据
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Reverse = 11,
            /// <summary>
            /// [12] 反转整个数组数据
            /// </summary>
            ReverseArray = 12,
            /// <summary>
            /// [13] 根据索引位置设置数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            SetValue = 13,
            /// <summary>
            /// [14] 快照设置数据
            /// AutoCSer.KeyValue{int,T} value 数据
            /// </summary>
            SnapshotSet = 14,
            /// <summary>
            /// [15] 排序指定位置数组数据
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Sort = 15,
            /// <summary>
            /// [16] 数组排序
            /// </summary>
            SortArray = 16,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 位图节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IBitmapNodeMethodEnum))]
        public partial interface IBitmapNode { }
        /// <summary>
        /// 位图节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IBitmapNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            ClearBit = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            ClearMap = 1,
            /// <summary>
            /// [2] 读取位状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 非 0 表示二进制位为已设置状态，索引超出则无返回值
            /// </summary>
            GetBit = 2,
            /// <summary>
            /// [3] 清除位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 清除操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitClearBit = 3,
            /// <summary>
            /// [4] 状态取反并返回操作之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 取反操作之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitInvertBit = 4,
            /// <summary>
            /// [5] 设置位状态并返回设置之前的状态
            /// uint index 位索引
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{int} 设置之前的状态，非 0 表示二进制位之前为已设置状态，索引超出则无返回值
            /// </summary>
            GetBitSetBit = 5,
            /// <summary>
            /// [6] 获取二进制位数量
            /// 返回值 uint 
            /// </summary>
            GetCapacity = 6,
            /// <summary>
            /// [7] 状态取反
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            InvertBit = 7,
            /// <summary>
            /// [8] 设置位状态
            /// uint index 位索引
            /// 返回值 bool 是否设置成功，失败表示索引超出范围
            /// </summary>
            SetBit = 8,
            /// <summary>
            /// [9] 快照添加数据
            /// byte[] map 
            /// </summary>
            SnapshotSet = 9,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayDictionaryNodeMethodEnum))]
        public partial interface IByteArrayDictionaryNode<KT> { }
        /// <summary>
        /// 字典节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 新容器初始化大小
            /// </summary>
            Renew = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 快照添加数据
            /// AutoCSer.BinarySerializeKeyValue{KT,byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 尝试添加数据
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
            /// <summary>
            /// [12] 根据关键字获取数据
            /// KT[] keys 
            /// 返回值 byte[][] 
            /// </summary>
            GetValueArray = 12,
            /// <summary>
            /// [13] 删除关键字
            /// KT[] keys 
            /// 返回值 int 删除关键字数量
            /// </summary>
            RemoveKeys = 13,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayFragmentDictionaryNodeMethodEnum))]
        public partial interface IByteArrayFragmentDictionaryNode<KT> { }
        /// <summary>
        /// 256 基分片字典 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayFragmentDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据（保留分片数组）
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 5,
            /// <summary>
            /// [6] 删除关键字
            /// KT key 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 快照添加数据
            /// AutoCSer.BinarySerializeKeyValue{KT,byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 如果关键字不存在则添加数据
            /// KT key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
            /// <summary>
            /// [12] 根据关键字获取数据
            /// KT[] keys 
            /// 返回值 byte[][] 
            /// </summary>
            GetValueArray = 12,
            /// <summary>
            /// [13] 删除关键字
            /// KT[] keys 
            /// 返回值 int 删除关键字数量
            /// </summary>
            RemoveKeys = 13,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayQueueNodeMethodEnum))]
        public partial interface IByteArrayQueueNode { }
        /// <summary>
        /// 队列节点接口（先进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayQueueNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 获取队列数据数量
            /// 返回值 int 
            /// </summary>
            Count = 1,
            /// <summary>
            /// [2] 将数据添加到队列
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// </summary>
            Enqueue = 2,
            /// <summary>
            /// [3] 快照添加数据
            /// byte[] value 
            /// </summary>
            SnapshotAdd = 3,
            /// <summary>
            /// [4] 从队列中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryDequeue = 4,
            /// <summary>
            /// [5] 从队列中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryDequeueResponseParameter = 5,
            /// <summary>
            /// [6] 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 6,
            /// <summary>
            /// [7] 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryPeekResponseParameter = 7,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IByteArrayStackNodeMethodEnum))]
        public partial interface IByteArrayStackNode { }
        /// <summary>
        /// 栈节点（后进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IByteArrayStackNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 1,
            /// <summary>
            /// [2] 将数据添加到栈
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// </summary>
            Push = 2,
            /// <summary>
            /// [3] 快照添加数据
            /// byte[] value 
            /// </summary>
            SnapshotAdd = 3,
            /// <summary>
            /// [4] 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 4,
            /// <summary>
            /// [5] 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryPeekResponseParameter = 5,
            /// <summary>
            /// [6] 从栈中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 没有可弹出数据则返回无数据
            /// </summary>
            TryPop = 6,
            /// <summary>
            /// [7] 从栈中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 没有可弹出数据则返回无数据
            /// </summary>
            TryPopResponseParameter = 7,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IDictionaryNodeMethodEnum))]
        public partial interface IDictionaryNode<KT,VT> { }
        /// <summary>
        /// 字典节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            ReusableClear = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 被删除数据
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 新容器初始化大小
            /// </summary>
            Renew = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 快照添加数据
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 尝试添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// KT[] keys 
            /// 返回值 VT[] 
            /// </summary>
            GetValueArray = 11,
            /// <summary>
            /// [12] 删除关键字
            /// KT[] keys 
            /// 返回值 int 删除关键字数量
            /// </summary>
            RemoveKeys = 12,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 分布式锁节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IDistributedLockNodeMethodEnum))]
        public partial interface IDistributedLockNode<T> { }
        /// <summary>
        /// 分布式锁节点 节点方法序号映射枚举类型
        /// </summary>
        public enum IDistributedLockNodeMethodEnum
        {
            /// <summary>
            /// [0] 申请锁
            /// T key 锁关键字
            /// ushort timeoutSeconds 超时秒数
            /// 返回值 long 
            /// </summary>
            Enter = 0,
            /// <summary>
            /// [1] 释放锁
            /// T key 锁关键字
            /// long identity 锁操作标识
            /// </summary>
            Release = 1,
            /// <summary>
            /// [2] 快照设置数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.DistributedLockIdentity{T} value 数据
            /// </summary>
            SnapshotSet = 2,
            /// <summary>
            /// [3] 尝试申请锁
            /// T key 锁关键字
            /// ushort timeoutSeconds 超时秒数
            /// 返回值 long 失败返回 0
            /// </summary>
            TryEnter = 3,
            /// <summary>
            /// [4] 快照设置数据
            /// long value 数据
            /// </summary>
            SnapshotSetIdentity = 4,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片字典 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IFragmentDictionaryNodeMethodEnum))]
        public partial interface IFragmentDictionaryNode<KT,VT> { }
        /// <summary>
        /// 256 基分片字典 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IFragmentDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据（保留分片数组）
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 强制设置数据，如果关键字已存在则覆盖
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 6,
            /// <summary>
            /// [7] 快照添加数据
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 7,
            /// <summary>
            /// [8] 如果关键字不存在则添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 8,
            /// <summary>
            /// [9] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// KT[] keys 
            /// 返回值 VT[] 
            /// </summary>
            GetValueArray = 10,
            /// <summary>
            /// [11] 删除关键字
            /// KT[] keys 
            /// 返回值 int 删除关键字数量
            /// </summary>
            RemoveKeys = 11,
            /// <summary>
            /// [12] 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            ReusableClear = 12,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 哈希表 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IFragmentHashSetNodeMethodEnum))]
        public partial interface IFragmentHashSetNode<T> { }
        /// <summary>
        /// 256 基分片 哈希表 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IFragmentHashSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 如果关键字不存在则添加数据
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除数据（保留分片数组）
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 2,
            /// <summary>
            /// [3] 判断关键字是否存在
            /// T value 
            /// 返回值 bool 
            /// </summary>
            Contains = 3,
            /// <summary>
            /// [4] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 4,
            /// <summary>
            /// [5] 删除关键字
            /// T value 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 如果关键字不存在则添加数据
            /// T[] values 
            /// 返回值 int 添加数据数量
            /// </summary>
            AddValues = 6,
            /// <summary>
            /// [7] 删除关键字
            /// T[] values 
            /// 返回值 int 删除数据数量
            /// </summary>
            RemoveValues = 7,
            /// <summary>
            /// [8] 可重用哈希表重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            ReusableClear = 8,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 字典节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashBytesDictionaryNodeMethodEnum))]
        public partial interface IHashBytesDictionaryNode { }
        /// <summary>
        /// 字典节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashBytesDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 4,
            /// <summary>
            /// [5] 删除关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 新容器初始化大小
            /// </summary>
            Renew = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 快照添加数据
            /// AutoCSer.BinarySerializeKeyValue{byte[],byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 尝试添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 256 基分片 HashBytes 字典 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashBytesFragmentDictionaryNodeMethodEnum))]
        public partial interface IHashBytesFragmentDictionaryNode { }
        /// <summary>
        /// 256 基分片 HashBytes 字典 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashBytesFragmentDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据（保留分片数组）
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// </summary>
            ClearArray = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 被删除数据
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字并返回被删除数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 被删除数据
            /// </summary>
            GetRemoveResponseParameter = 5,
            /// <summary>
            /// [6] 删除关键字
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 强制设置数据，如果关键字已存在则覆盖
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否设置成功
            /// </summary>
            Set = 7,
            /// <summary>
            /// [8] 快照添加数据
            /// AutoCSer.BinarySerializeKeyValue{byte[],byte[]} value 
            /// </summary>
            SnapshotAdd = 8,
            /// <summary>
            /// [9] 如果关键字不存在则添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 9,
            /// <summary>
            /// [10] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter 
            /// </summary>
            TryGetResponseParameter = 10,
            /// <summary>
            /// [11] 根据关键字获取数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArray key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{byte[]} 
            /// </summary>
            TryGetValue = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 哈希表节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IHashSetNodeMethodEnum))]
        public partial interface IHashSetNode<T> { }
        /// <summary>
        /// 哈希表节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IHashSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// T value 
            /// 返回值 bool 
            /// </summary>
            Contains = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字
            /// T value 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 4,
            /// <summary>
            /// [5] 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
            /// int capacity 容器初始化大小
            /// </summary>
            Renew = 5,
            /// <summary>
            /// [6] 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
            /// </summary>
            ReusableClear = 6,
            /// <summary>
            /// [7] 如果关键字不存在则添加数据
            /// T[] values 
            /// 返回值 int 添加数据数量
            /// </summary>
            AddValues = 7,
            /// <summary>
            /// [8] 删除关键字
            /// T[] values 
            /// 返回值 int 删除数据数量
            /// </summary>
            RemoveValues = 8,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 64 位自增ID 节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IIdentityGeneratorNodeMethodEnum))]
        public partial interface IIdentityGeneratorNode { }
        /// <summary>
        /// 64 位自增ID 节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IIdentityGeneratorNodeMethodEnum
        {
            /// <summary>
            /// [0] 获取下一个自增ID
            /// 返回值 long 下一个自增ID，失败返回负数
            /// </summary>
            Next = 0,
            /// <summary>
            /// [1] 获取自增 ID 分段
            /// int count 获取数量
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IdentityFragment 自增 ID 分段
            /// </summary>
            NextFragment = 1,
            /// <summary>
            /// [2] 快照添加数据
            /// long identity 
            /// </summary>
            SnapshotSet = 2,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 数组节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ILeftArrayNodeMethodEnum))]
        public partial interface ILeftArrayNode<T> { }
        /// <summary>
        /// 数组节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ILeftArrayNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 数据
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除指定位置数据
            /// int startIndex 起始位置
            /// int count 清除数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 清除所有数据并将数据有效长度设置为 0
            /// </summary>
            ClearLength = 2,
            /// <summary>
            /// [3] 用数据填充数组指定位置
            /// T value 
            /// int startIndex 起始位置
            /// int count 填充数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Fill = 3,
            /// <summary>
            /// [4] 用数据填充整个数组
            /// T value 
            /// </summary>
            FillArray = 4,
            /// <summary>
            /// [5] 获取数组容器初大小
            /// 返回值 int 
            /// </summary>
            GetCapacity = 5,
            /// <summary>
            /// [6] 获取容器空闲数量
            /// 返回值 int 
            /// </summary>
            GetFreeCount = 6,
            /// <summary>
            /// [7] 获取有效数组长度
            /// 返回值 int 
            /// </summary>
            GetLength = 7,
            /// <summary>
            /// [8] 移除最后一个数据并返回该数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可移除数据则无数据返回
            /// </summary>
            GetTryPopValue = 8,
            /// <summary>
            /// [9] 根据索引位置获取数据
            /// int index 索引位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引返回则无返回值
            /// </summary>
            GetValue = 9,
            /// <summary>
            /// [10] 移除指定索引位置数据并返回被移除的数据
            /// int index 数据位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引范围则无数据返回
            /// </summary>
            GetValueRemoveAt = 10,
            /// <summary>
            /// [11] 移除指定索引位置数据，将最后一个数据移动到该指定位置，并返回被移除的数据
            /// int index 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 超出索引范围则无数据返回
            /// </summary>
            GetValueRemoveToEnd = 11,
            /// <summary>
            /// [12] 根据索引位置设置数据并返回设置之前的数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 设置之前的数据，超出索引返回则无返回值
            /// </summary>
            GetValueSet = 12,
            /// <summary>
            /// [13] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 起始位置
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOf = 13,
            /// <summary>
            /// [14] 从数组中查找第一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            IndexOfArray = 14,
            /// <summary>
            /// [15] 插入数据
            /// int index 插入位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Insert = 15,
            /// <summary>
            /// [16] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// int startIndex 最后一个匹配位置（起始位置）
            /// int count 查找匹配数据数量
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOf = 16,
            /// <summary>
            /// [17] 从数组中查找最后一个匹配数据的位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 
            /// 返回值 int 失败返回负数
            /// </summary>
            LastIndexOfArray = 17,
            /// <summary>
            /// [18] 移除第一个匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 数据
            /// 返回值 bool 是否存在移除数据
            /// </summary>
            Remove = 18,
            /// <summary>
            /// [19] 移除指定索引位置数据
            /// int index 数据位置
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            RemoveAt = 19,
            /// <summary>
            /// [20] 移除指定索引位置数据并将最后一个数据移动到该指定位置
            /// int index 
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            RemoveToEnd = 20,
            /// <summary>
            /// [21] 反转指定位置数组数据
            /// int startIndex 起始位置
            /// int count 反转数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Reverse = 21,
            /// <summary>
            /// [22] 反转整个数组数据
            /// </summary>
            ReverseArray = 22,
            /// <summary>
            /// [23] 置空并释放数组
            /// </summary>
            SetEmpty = 23,
            /// <summary>
            /// [24] 根据索引位置设置数据
            /// int index 索引位置
            /// T value 数据
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            SetValue = 24,
            /// <summary>
            /// [25] 排序指定位置数组数据
            /// int startIndex 起始位置
            /// int count 排序数据数量
            /// 返回值 bool 超出索引范围则返回 false
            /// </summary>
            Sort = 25,
            /// <summary>
            /// [26] 数组排序
            /// </summary>
            SortArray = 26,
            /// <summary>
            /// [27] 当有空闲位置时添加数据
            /// T value 
            /// 返回值 bool 如果数组已满则添加失败并返回 false
            /// </summary>
            TryAdd = 27,
            /// <summary>
            /// [28] 移除最后一个数据
            /// 返回值 bool 是否存在可移除数据
            /// </summary>
            TryPop = 28,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IManyHashBitMapClientFilterNodeMethodEnum))]
        public partial interface IManyHashBitMapClientFilterNode { }
        /// <summary>
        /// 多哈希位图客户端同步过滤节点接口（类似布隆过滤器，适合小容器） 节点方法序号映射枚举类型
        /// </summary>
        public enum IManyHashBitMapClientFilterNodeMethodEnum
        {
            /// <summary>
            /// [0] 获取设置新位操作
            /// 返回值 int 
            /// </summary>
            GetBit = 0,
            /// <summary>
            /// [1] 获取当前位图数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap 
            /// </summary>
            GetData = 1,
            /// <summary>
            /// [2] 设置位
            /// int bit 位置
            /// </summary>
            SetBit = 2,
            /// <summary>
            /// [3] 设置位 持久化前检查
            /// int bit 位置
            /// 返回值 bool 是否继续持久化操作
            /// </summary>
            SetBitBeforePersistence = 3,
            /// <summary>
            /// [4] 快照添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap map 多哈希位图数据
            /// </summary>
            SnapshotSet = 4,
            /// <summary>
            /// [5] 获取位图大小（位数量）
            /// 返回值 int 
            /// </summary>
            GetSize = 5,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IManyHashBitMapFilterNodeMethodEnum))]
        public partial interface IManyHashBitMapFilterNode { }
        /// <summary>
        /// 多哈希位图过滤节点接口（类似布隆过滤器） 节点方法序号映射枚举类型
        /// </summary>
        public enum IManyHashBitMapFilterNodeMethodEnum
        {
            /// <summary>
            /// [0] 获取位图大小（位数量）
            /// 返回值 int 
            /// </summary>
            GetSize = 0,
            /// <summary>
            /// [1] 设置位
            /// int size 位图大小（位数量）
            /// uint[] bits 位置集合
            /// 返回值 bool 返回 false 表示位图大小不匹配
            /// </summary>
            SetBits = 1,
            /// <summary>
            /// [2] 设置位 持久化前检查
            /// int size 位图大小（位数量）
            /// uint[] bits 位置集合
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{bool} 返回 false 表示位图大小不匹配
            /// </summary>
            SetBitsBeforePersistence = 2,
            /// <summary>
            /// [3] 快照添加数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ManyHashBitMap map 多哈希位图数据
            /// </summary>
            SnapshotSet = 3,
            /// <summary>
            /// [4] 检查位图数据
            /// int size 位图大小（位数量）
            /// uint[] bits 位置集合
            /// 返回值 AutoCSer.NullableBoolEnum 返回 false 表示数据不存在
            /// </summary>
            CheckBits = 4,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 消息处理节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IMessageNodeMethodEnum))]
        public partial interface IMessageNode<T> { }
        /// <summary>
        /// 消息处理节点 节点方法序号映射枚举类型
        /// </summary>
        public enum IMessageNodeMethodEnum
        {
            /// <summary>
            /// [0] 生产者添加新消息
            /// T message 
            /// </summary>
            AppendMessage = 0,
            /// <summary>
            /// [1] 清除所有消息
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 清除所有失败消息
            /// </summary>
            ClearFailed = 2,
            /// <summary>
            /// [3] 消息完成处理
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// </summary>
            Completed = 3,
            /// <summary>
            /// [4] 消息失败处理
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MessageIdeneity identity 
            /// </summary>
            Failed = 4,
            /// <summary>
            /// [5] 获取消费者回调数量
            /// 返回值 int 
            /// </summary>
            GetCallbackCount = 5,
            /// <summary>
            /// [6] 获取未处理完成消息数量（不包括失败消息）
            /// 返回值 int 
            /// </summary>
            GetCount = 6,
            /// <summary>
            /// [7] 获取失败消息数量
            /// 返回值 int 
            /// </summary>
            GetFailedCount = 7,
            /// <summary>
            /// [8] 消费客户端获取消息
            /// int maxCount 当前客户端最大并发消息数量
            /// 返回值 T 
            /// </summary>
            GetMessage = 8,
            /// <summary>
            /// [9] 获取未完成处理超时消息数量
            /// 返回值 int 
            /// </summary>
            GetTimeoutCount = 9,
            /// <summary>
            /// [10] 获取未处理完成消息数量（包括失败消息）
            /// 返回值 int 
            /// </summary>
            GetTotalCount = 10,
            /// <summary>
            /// [11] 快照设置数据
            /// T value 数据
            /// </summary>
            SnapshotAdd = 11,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 进程守护节点接口（服务端需要以管理员身份运行，否则可能异常）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IProcessGuardNodeMethodEnum))]
        public partial interface IProcessGuardNode { }
        /// <summary>
        /// 进程守护节点接口（服务端需要以管理员身份运行，否则可能异常） 节点方法序号映射枚举类型
        /// </summary>
        public enum IProcessGuardNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加待守护进程
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ProcessGuardInfo processInfo 进程信息
            /// 返回值 bool 是否添加成功
            /// </summary>
            Guard = 0,
            /// <summary>
            /// [1] 删除被守护进程
            /// int processId 进程标识
            /// System.DateTime startTime 进程启动时间
            /// string processName 进程名称
            /// </summary>
            Remove = 1,
            /// <summary>
            /// [2] 快照设置数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ProcessGuardInfo value 数据
            /// </summary>
            SnapshotSet = 2,
            /// <summary>
            /// [3] 切换进程
            /// string key 切换进程关键字
            /// 返回值 bool 
            /// </summary>
            Switch = 3,
            /// <summary>
            /// [4] 初始化添加待守护进程
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ProcessGuardInfo processInfo 进程信息
            /// 返回值 bool 是否添加成功
            /// </summary>
            GuardLoadPersistence = 4,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 队列节点接口（先进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IQueueNodeMethodEnum))]
        public partial interface IQueueNode<T> { }
        /// <summary>
        /// 队列节点接口（先进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IQueueNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 匹配数据
            /// 返回值 bool 
            /// </summary>
            Contains = 1,
            /// <summary>
            /// [2] 获取队列数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 将数据添加到队列
            /// T value 
            /// </summary>
            Enqueue = 3,
            /// <summary>
            /// [4] 从队列中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryDequeue = 4,
            /// <summary>
            /// [5] 获取队列中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 5,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISearchTreeDictionaryNodeMethodEnum))]
        public partial interface ISearchTreeDictionaryNode<KT,VT> { }
        /// <summary>
        /// 二叉搜索树节点 节点方法序号映射枚举类型
        /// </summary>
        public enum ISearchTreeDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断是否包含关键字
            /// KT key 关键字
            /// 返回值 bool 是否包含关键字
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 获取节点数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 根据关键字比它小的节点数量
            /// KT key 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountLess = 3,
            /// <summary>
            /// [4] 根据关键字比它大的节点数量
            /// KT key 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountThan = 4,
            /// <summary>
            /// [5] 获取树高度，时间复杂度 O(n)
            /// 返回值 int 
            /// </summary>
            GetHeight = 5,
            /// <summary>
            /// [6] 根据关键字删除节点
            /// KT key 关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 被删除数据
            /// </summary>
            GetRemove = 6,
            /// <summary>
            /// [7] 获取范围数据集合
            /// int skipCount 跳过记录数
            /// byte getCount 获取记录数
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetValues = 7,
            /// <summary>
            /// [8] 根据关键字获取一个匹配节点位置
            /// KT key 关键字
            /// 返回值 int 一个匹配节点位置,失败返回-1
            /// </summary>
            IndexOf = 8,
            /// <summary>
            /// [9] 根据关键字删除节点
            /// KT key 关键字
            /// 返回值 bool 是否存在关键字
            /// </summary>
            Remove = 9,
            /// <summary>
            /// [10] 设置数据
            /// KT key 关键字
            /// VT value 数据
            /// 返回值 bool 是否添加了关键字
            /// </summary>
            Set = 10,
            /// <summary>
            /// [11] 快照添加数据
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 11,
            /// <summary>
            /// [12] 添加数据
            /// KT key 关键字
            /// VT value 数据
            /// 返回值 bool 是否添加了数据
            /// </summary>
            TryAdd = 12,
            /// <summary>
            /// [13] 获取第一个关键字数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{KT} 第一个关键字数据
            /// </summary>
            TryGetFirstKey = 13,
            /// <summary>
            /// [14] 获取第一组数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 第一组数据
            /// </summary>
            TryGetFirstKeyValue = 14,
            /// <summary>
            /// [15] 获取第一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 第一个数据
            /// </summary>
            TryGetFirstValue = 15,
            /// <summary>
            /// [16] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 数据
            /// </summary>
            TryGetKeyValueByIndex = 16,
            /// <summary>
            /// [17] 获取最后一个关键字数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{KT} 最后一个关键字数据
            /// </summary>
            TryGetLastKey = 17,
            /// <summary>
            /// [18] 获取最后一组数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{AutoCSer.KeyValue{KT,VT}} 最后一组数据
            /// </summary>
            TryGetLastKeyValue = 18,
            /// <summary>
            /// [19] 获取最后一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 最后一个数据
            /// </summary>
            TryGetLastValue = 19,
            /// <summary>
            /// [20] 根据关键字获取数据
            /// KT key 关键字
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 目标数据
            /// </summary>
            TryGetValue = 20,
            /// <summary>
            /// [21] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 数据
            /// </summary>
            TryGetValueByIndex = 21,
            /// <summary>
            /// [22] 根据关键字获取数据
            /// KT[] keys 
            /// 返回值 VT[] 
            /// </summary>
            GetValueArray = 22,
            /// <summary>
            /// [23] 删除关键字
            /// KT[] keys 
            /// 返回值 int 删除关键字数量
            /// </summary>
            RemoveKeys = 23,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 二叉搜索树集合节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISearchTreeSetNodeMethodEnum))]
        public partial interface ISearchTreeSetNode<T> { }
        /// <summary>
        /// 二叉搜索树集合节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ISearchTreeSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 关键字
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// T value 关键字
            /// 返回值 bool 
            /// </summary>
            Contains = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 根据关键字比它小的节点数量
            /// T value 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountLess = 4,
            /// <summary>
            /// [5] 根据关键字比它大的节点数量
            /// T value 关键字
            /// 返回值 int 节点数量，失败返回 -1
            /// </summary>
            CountThan = 5,
            /// <summary>
            /// [6] 根据节点位置获取数据
            /// int index 节点位置
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 数据
            /// </summary>
            GetByIndex = 6,
            /// <summary>
            /// [7] 获取第一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetFrist = 7,
            /// <summary>
            /// [8] 获取最后一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetLast = 8,
            /// <summary>
            /// [9] 根据关键字获取一个匹配节点位置
            /// T value 关键字
            /// 返回值 int 一个匹配节点位置,失败返回-1
            /// </summary>
            IndexOf = 9,
            /// <summary>
            /// [10] 删除关键字
            /// T value 关键字
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 10,
            /// <summary>
            /// [11] 如果关键字不存在则添加数据
            /// T[] values 
            /// 返回值 int 添加数据数量
            /// </summary>
            AddValues = 11,
            /// <summary>
            /// [12] 删除关键字
            /// T[] values 
            /// 返回值 int 删除数据数量
            /// </summary>
            RemoveValues = 12,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务注册节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IServerRegistryNodeMethodEnum))]
        public partial interface IServerRegistryNode { }
        /// <summary>
        /// 服务注册节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IServerRegistryNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加服务注册日志
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog log 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryStateEnum 服务注册结果
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 获取服务会话标识ID
            /// 返回值 long 
            /// </summary>
            GetSessionID = 1,
            /// <summary>
            /// [2] 获取服务注册日志
            /// string serverName 监视服务名称，空字符串表示所有服务
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog 
            /// </summary>
            LogCallback = 2,
            /// <summary>
            /// [3] 服务注册回调委托，主要用于注册组件检查服务的在线状态
            /// long sessionID 服务会话标识ID
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum 
            /// </summary>
            ServiceCallback = 3,
            /// <summary>
            /// [4] 快照设置数据
            /// AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog value 数据
            /// </summary>
            SnapshotSet = 4,
            /// <summary>
            /// [5] 获取服务主日志
            /// string serverName 服务名称
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog 返回 null 表示没有找到服务主日志
            /// </summary>
            GetLog = 5,
            /// <summary>
            /// [6] 检查服务在线状态
            /// long sessionID 服务会话标识ID
            /// string serverName 服务名称
            /// </summary>
            Check = 6,
            /// <summary>
            /// [7] 服务失联持久化
            /// long sessionID 服务会话标识ID
            /// string serverName 服务名称
            /// </summary>
            LostContact = 7,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 服务基础操作接口方法映射枚举
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IServiceNodeMethodEnum))]
        public partial interface IServiceNode { }
        /// <summary>
        /// 服务基础操作接口方法映射枚举 节点方法序号映射枚举类型
        /// </summary>
        public enum IServiceNodeMethodEnum
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
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序字典节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISortedDictionaryNodeMethodEnum))]
        public partial interface ISortedDictionaryNode<KT,VT> { }
        /// <summary>
        /// 排序字典节点 节点方法序号映射枚举类型
        /// </summary>
        public enum ISortedDictionaryNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 bool 
            /// </summary>
            ContainsValue = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetRemove = 4,
            /// <summary>
            /// [5] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 5,
            /// <summary>
            /// [6] 快照添加数据
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 6,
            /// <summary>
            /// [7] 添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 7,
            /// <summary>
            /// [8] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 8,
            /// <summary>
            /// [9] 根据关键字获取数据
            /// KT[] keys 
            /// 返回值 VT[] 
            /// </summary>
            GetValueArray = 9,
            /// <summary>
            /// [10] 删除关键字
            /// KT[] keys 
            /// 返回值 int 删除关键字数量
            /// </summary>
            RemoveKeys = 10,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序列表节点
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISortedListNodeMethodEnum))]
        public partial interface ISortedListNode<KT,VT> { }
        /// <summary>
        /// 排序列表节点 节点方法序号映射枚举类型
        /// </summary>
        public enum ISortedListNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断关键字是否存在
            /// KT key 
            /// 返回值 bool 
            /// </summary>
            ContainsKey = 1,
            /// <summary>
            /// [2] 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 bool 
            /// </summary>
            ContainsValue = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 获取容器大小
            /// 返回值 int 
            /// </summary>
            GetCapacity = 4,
            /// <summary>
            /// [5] 删除关键字并返回被删除数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            GetRemove = 5,
            /// <summary>
            /// [6] 获取关键字排序位置
            /// KT key 
            /// 返回值 int 负数表示没有找到关键字
            /// </summary>
            IndexOfKey = 6,
            /// <summary>
            /// [7] 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// VT value 
            /// 返回值 int 负数表示没有找到匹配数据
            /// </summary>
            IndexOfValue = 7,
            /// <summary>
            /// [8] 删除关键字
            /// KT key 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 8,
            /// <summary>
            /// [9] 删除指定排序索引位置数据
            /// int index 
            /// 返回值 bool 索引超出范围返回 false
            /// </summary>
            RemoveAt = 9,
            /// <summary>
            /// [10] 快照添加数据
            /// AutoCSer.KeyValue{KT,VT} value 
            /// </summary>
            SnapshotAdd = 10,
            /// <summary>
            /// [11] 添加数据
            /// KT key 
            /// VT value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            TryAdd = 11,
            /// <summary>
            /// [12] 根据关键字获取数据
            /// KT key 
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{VT} 
            /// </summary>
            TryGetValue = 12,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 排序集合节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(ISortedSetNodeMethodEnum))]
        public partial interface ISortedSetNode<T> { }
        /// <summary>
        /// 排序集合节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum ISortedSetNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加数据
            /// T value 
            /// 返回值 bool 是否添加成功，否则表示关键字已经存在
            /// </summary>
            Add = 0,
            /// <summary>
            /// [1] 清除所有数据
            /// </summary>
            Clear = 1,
            /// <summary>
            /// [2] 判断关键字是否存在
            /// T value 
            /// 返回值 bool 
            /// </summary>
            Contains = 2,
            /// <summary>
            /// [3] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 3,
            /// <summary>
            /// [4] 获取最大值
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetMax = 4,
            /// <summary>
            /// [5] 获取最小值
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有数据时返回无返回值
            /// </summary>
            GetMin = 5,
            /// <summary>
            /// [6] 删除关键字
            /// T value 
            /// 返回值 bool 是否删除成功
            /// </summary>
            Remove = 6,
            /// <summary>
            /// [7] 如果关键字不存在则添加数据
            /// T[] values 
            /// 返回值 int 添加数据数量
            /// </summary>
            AddValues = 7,
            /// <summary>
            /// [8] 删除关键字
            /// T[] values 
            /// 返回值 int 删除数据数量
            /// </summary>
            RemoveValues = 8,
        }
}namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
        /// <summary>
        /// 栈节点（后进先出）
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IStackNodeMethodEnum))]
        public partial interface IStackNode<T> { }
        /// <summary>
        /// 栈节点（后进先出） 节点方法序号映射枚举类型
        /// </summary>
        public enum IStackNodeMethodEnum
        {
            /// <summary>
            /// [0] 清除所有数据
            /// </summary>
            Clear = 0,
            /// <summary>
            /// [1] 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
            /// T value 匹配数据
            /// 返回值 bool 
            /// </summary>
            Contains = 1,
            /// <summary>
            /// [2] 获取数据数量
            /// 返回值 int 
            /// </summary>
            Count = 2,
            /// <summary>
            /// [3] 将数据添加到栈
            /// T value 
            /// </summary>
            Push = 3,
            /// <summary>
            /// [4] 获取栈中下一个弹出数据（不弹出数据仅查看）
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryPeek = 4,
            /// <summary>
            /// [5] 从栈中弹出一个数据
            /// 返回值 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult{T} 没有可弹出数据则返回无数据
            /// </summary>
            TryPop = 5,
        }
}
#endif