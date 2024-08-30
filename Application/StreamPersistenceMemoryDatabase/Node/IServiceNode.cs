using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作接口方法映射枚举
    /// </summary>
    [ServerNode(MethodIndexEnumType = typeof(ServiceNodeMethodEnum), IsAutoMethodIndex = false)]
    public interface IServiceNode
    {
        /// <summary>
        /// 删除节点持久化参数检查
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> RemoveNodeBeforePersistence(NodeIndex index);
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        bool RemoveNode(NodeIndex index);
        /// <summary>
        /// 创建字典节点 FragmentHashStringDictionary256{HashString,string}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        NodeIndex CreateFragmentHashStringDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo);
    }
}
