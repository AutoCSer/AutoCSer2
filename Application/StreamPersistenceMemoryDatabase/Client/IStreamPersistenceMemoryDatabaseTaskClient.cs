using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库客户端接口
    /// </summary>
    public interface IStreamPersistenceMemoryDatabaseTaskClient : IStreamPersistenceMemoryDatabaseClientBase
    {
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> Call(NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallOutput(ResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        ReturnCommand<CallStateEnum> CallInput(RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        ReturnCommand<ResponseParameter> CallInputOutput(ResponseParameter returnValue, RequestParameter parameter);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> KeepCallback(KeepCallbackResponseParameter returnValue, NodeIndex index, int methodIndex);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="returnValue">接口返回初始值，这里用于返回值的自定义反序列化操作，参数名称必须是 ReturnValue 不区分大小写，必须放在第一个数据参数之前，类型必须与返回值类型一致</param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        EnumeratorCommand<KeepCallbackResponseParameter> InputKeepCallback(KeepCallbackResponseParameter returnValue, RequestParameter parameter);
    }
}
