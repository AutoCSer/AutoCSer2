using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 建树器节点接口
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    /// <typeparam name="TT">节点标识类型</typeparam>
    internal interface ITreeBuilderNode<T, TT>
        where T : ITreeBuilderNode<T, TT>
        where TT : IEquatable<TT>
    {
        /// <summary>
        /// 树节点标识
        /// </summary>
        TT Tag { get; }
        /// <summary>
        /// 设置子节点集合
        /// </summary>
        /// <param name="childs">子节点集合</param>
        void SetChilds(T[] childs);
    }
}
