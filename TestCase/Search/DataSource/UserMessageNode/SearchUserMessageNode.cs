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
        /// 分词结果磁盘块索引信息节点接口集合
        /// </summary>
        private readonly LeftArray<StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>>> wordIdentityBlockIndexNodes;
        /// <summary>
        /// 用户搜索数据更新消息节点
        /// </summary>
        /// <param name="timeoutSeconds">触发任务执行超时秒数</param>
        internal SearchUserMessageNode(int timeoutSeconds = 60) : base(timeoutSeconds)
        {
            wordIdentityBlockIndexNodes.SetEmpty();
            wordIdentityBlockIndexNodes.Add(WordIdentityBlockIndex.CommandClientSocketEvent.UserNameNodeCache);
            wordIdentityBlockIndexNodes.Add(WordIdentityBlockIndex.CommandClientSocketEvent.UserRemarkNodeCache);
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="type"></param>
        /// <returns>是否执行成功</returns>
        public override async Task<bool> RunTask(TimeoutMessage<OperationData<int>> task, TimeoutMessageRunTaskTypeEnum type)
        {
            ResponseResult<ISearchUserNodeClientNode> userNode = await WordIdentityBlockIndex.CommandClientSocketEvent.SearchUserNodeCache.GetSynchronousNode();
            if (!userNode.IsSuccess) return false;

            OperationData<int> data = task.TaskData;
            switch (data.Type)
            {
                case OperationTypeEnum.Insert:
                    ResponseResult<ConditionDataUpdateStateEnum> userResult = await userNode.Value.Create(data.Key);
                    if (!userResult.IsSuccess || userResult.Value != ConditionDataUpdateStateEnum.Success) return false;

                    foreach (StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> wordIdentityBlockIndexNode in wordIdentityBlockIndexNodes)
                    {
                        ResponseResult<IWordIdentityBlockIndexNodeClientNode<int>> nodeResult = await wordIdentityBlockIndexNode.GetSynchronousNode();
                        if (!nodeResult.IsSuccess) return false;
                        ResponseResult<WordIdentityBlockIndexUpdateStateEnum> result = await nodeResult.Value.Create(data.Key);
                        if (!result.IsSuccess) return false;
                        switch (result.Value)
                        {
                            case WordIdentityBlockIndexUpdateStateEnum.Success:
                            case WordIdentityBlockIndexUpdateStateEnum.DeletedNotFoundKey:
                                break;
                            default: return false;
                        }
                    }
                    break;
                case OperationTypeEnum.Delete:
                    userResult = await userNode.Value.Delete(data.Key);
                    if (!userResult.IsSuccess || userResult.Value != ConditionDataUpdateStateEnum.Success) return false;

                    foreach (StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> wordIdentityBlockIndexNode in wordIdentityBlockIndexNodes)
                    {
                        ResponseResult<IWordIdentityBlockIndexNodeClientNode<int>> nodeResult = await wordIdentityBlockIndexNode.GetSynchronousNode();
                        if (!nodeResult.IsSuccess) return false;
                        ResponseResult<WordIdentityBlockIndexUpdateStateEnum> result = await nodeResult.Value.Delete(data.Key);
                        if (!result.IsSuccess) return false;
                        switch (result.Value)
                        {
                            case WordIdentityBlockIndexUpdateStateEnum.Success:
                            case WordIdentityBlockIndexUpdateStateEnum.DeletedNotFoundKey:
                            case WordIdentityBlockIndexUpdateStateEnum.NotSupportDeleteKey:
                                break;
                            default: return false;
                        }
                    }
                    break;
                default:
                    userResult = await userNode.Value.Update(data.Key);
                    if (!userResult.IsSuccess || userResult.Value != ConditionDataUpdateStateEnum.Success) return false;

                    foreach (StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> wordIdentityBlockIndexNode in wordIdentityBlockIndexNodes)
                    {
                        ResponseResult<IWordIdentityBlockIndexNodeClientNode<int>> nodeResult = await wordIdentityBlockIndexNode.GetSynchronousNode();
                        if (!nodeResult.IsSuccess) return false;
                        ResponseResult<WordIdentityBlockIndexUpdateStateEnum> result = await nodeResult.Value.Update(data.Key);
                        if (!result.IsSuccess) return false;
                        switch (result.Value)
                        {
                            case WordIdentityBlockIndexUpdateStateEnum.Success:
                            case WordIdentityBlockIndexUpdateStateEnum.DeletedNotFoundKey:
                                break;
                            default: return false;
                        }
                    }
                    break;
            }
            callback(data);
            return true;
        }
    }
}
