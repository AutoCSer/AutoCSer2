﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序集合节点接口 客户端节点接口
    /// </summary>
    public partial interface ISortedSetNodeClientNode<T> where T : IComparable<T>
    {
    }
}