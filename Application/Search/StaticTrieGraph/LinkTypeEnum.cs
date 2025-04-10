﻿using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// Trie 图失败节点类型
    /// </summary>
    internal enum LinkTypeEnum : byte
    {
        /// <summary>
        /// 找不到失败节点
        /// </summary>
        None,
        /// <summary>
        /// 二级节点
        /// </summary>
        HashNode,
        /// <summary>
        /// 三级及以下节点
        /// </summary>
        Node,
    }
}
