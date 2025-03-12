using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode;
using AutoCSer.TestCase.SearchCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchDataSource
{
    /// <summary>
    /// 用户搜索数据更新消息节点
    /// </summary>
    internal sealed class SearchUserMessageNode : TimeoutMessageNode<OperationData<int>>
    {
        /// <summary>
        /// 用户搜索数据更新消息节点
        /// </summary>
        /// <param name="timeoutSeconds">触发任务执行超时秒数</param>
        internal SearchUserMessageNode(int timeoutSeconds = 60) : base(timeoutSeconds) { }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns>是否执行成功</returns>
        public override async Task<bool> RunTask(TimeoutMessage<OperationData<int>> task, TimeoutMessageRunTaskTypeEnum type)
        {
            OperationData<int> data = task.TaskData;
            if ((data.DataType & OperationDataTypeEnum.SearchUserNode) != 0)
            {
                if (!await searchUser(data)) return false;
            }
            if ((data.DataType & OperationDataTypeEnum.UserNameNode) != 0)
            {
                if (!await wordIdentity(data, WordIdentityBlockIndex.CommandClientSocketEvent.UserNameNodeCache)) return false;
            }
            if ((data.DataType & OperationDataTypeEnum.UserRemarkNode) != 0)
            {
                if (!await wordIdentity(data, WordIdentityBlockIndex.CommandClientSocketEvent.UserRemarkNodeCache)) return false;
            }
            callback(data);
            return true;
        }
        /// <summary>
        /// 用户搜索非索引条件数据更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<bool> searchUser(OperationData<int> data)
        {
            ResponseResult<ISearchUserNodeClientNode> userNode = await WordIdentityBlockIndex.CommandClientSocketEvent.SearchUserNodeCache.GetSynchronousNode();
            if (!userNode.IsSuccess) return false;
            ResponseResult<ConditionDataUpdateStateEnum> userResult;
            switch (data.OperationType)
            {
                case OperationTypeEnum.Insert: userResult = await userNode.Value.Create(data.Key); break;
                case OperationTypeEnum.Delete: userResult = await userNode.Value.Delete(data.Key); break;
                default: userResult = await userNode.Value.Update(data.Key); break;
            }
            return userResult.IsSuccess && userResult.Value == ConditionDataUpdateStateEnum.Success;
        }
        /// <summary>
        /// 分词数据更新
        /// </summary>
        /// <param name="data"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private async Task<bool> wordIdentity(OperationData<int> data, StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> node)
        {
            ResponseResult<IWordIdentityBlockIndexNodeClientNode<int>> nodeResult = await node.GetSynchronousNode();
            if (!nodeResult.IsSuccess) return false;
            ResponseResult<WordIdentityBlockIndexUpdateStateEnum> result;
            switch (data.OperationType)
            {
                case OperationTypeEnum.Insert: result = await nodeResult.Value.Create(data.Key); break;
                case OperationTypeEnum.Delete: result = await nodeResult.Value.Delete(data.Key); break;
                default: result = await nodeResult.Value.Update(data.Key); break;
            }
            if (!result.IsSuccess) return false;
            switch (result.Value)
            {
                case WordIdentityBlockIndexUpdateStateEnum.Success:
                case WordIdentityBlockIndexUpdateStateEnum.DeletedNotFoundKey:
                case WordIdentityBlockIndexUpdateStateEnum.NotSupportDeleteKey:
                    return true;
                default: return false;
            }
        }
    }
}
