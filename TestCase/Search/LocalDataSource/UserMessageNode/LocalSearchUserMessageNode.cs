using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 用户搜索数据更新消息节点
    /// </summary>
    internal sealed class LocalSearchUserMessageNode : TimeoutMessageNode<OperationData<int>>
    {
        /// <summary>
        /// 用户搜索数据更新消息节点
        /// </summary>
        /// <param name="timeoutSeconds">触发任务执行超时秒数</param>
        internal LocalSearchUserMessageNode(int timeoutSeconds = 60) : base(timeoutSeconds) { }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns>是否执行成功</returns>
        public override async Task<bool> RunTask(TimeoutMessage<OperationData<int>> task, TimeoutMessageRunTaskTypeEnum type)
        {
            OperationData<int> data = task.TaskData;
            SearchQueryCommandClientSocketEvent client = (SearchQueryCommandClientSocketEvent)await SearchQueryCommandClientSocketEvent.CommandClient.Client.GetSocketEvent();
            CommandClientReturnValue<bool> result = await client.SearchUserClient.UserMessage(data);
            if (result.IsSuccess && result.Value)
            {
                callback(data);
                return true;
            }
            return false;
        }
    }
}
