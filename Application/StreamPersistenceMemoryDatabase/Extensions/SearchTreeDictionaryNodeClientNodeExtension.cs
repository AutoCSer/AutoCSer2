using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 二叉搜索树字典客户端节点扩展
    /// </summary>
    public static class SearchTreeDictionaryNodeClientNodeExtension
    {
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pageIndex">分页编号，从 1 开始</param>
        /// <param name="pageSize">分页记录数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<KeepCallbackResponse<ValueResult<VT>>> GetPage<KT, VT>(this ISearchTreeDictionaryNodeClientNode<KT, VT> node, int pageIndex, byte pageSize) where KT : IComparable<KT>
        {
            return node.GetValues(Math.Max(pageIndex - 1, 0) * pageSize, pageSize);
        }
        /// <summary>
        /// 获取分页数据数组
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pageIndex">分页编号，从 1 开始</param>
        /// <param name="pageSize">分页记录数</param>
        /// <returns></returns>
        public static async Task<ResponseResult<LeftArray<VT>>> GetPageArray<KT, VT>(this ISearchTreeDictionaryNodeClientNode<KT, VT> node, int pageIndex, byte pageSize) where KT : IComparable<KT>
        {
            KeepCallbackResponse<ValueResult<VT>> response = await node.GetPage(pageIndex, pageSize);
            return await response.GetLeftArray(pageSize);
        }

        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pageIndex">分页编号，从 1 开始</param>
        /// <param name="pageSize">分页记录数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LocalServiceQueueNode<LocalKeepCallback<ValueResult<VT>>> GetPage<KT, VT>(this ISearchTreeDictionaryNodeLocalClientNode<KT, VT> node, int pageIndex, byte pageSize) where KT : IComparable<KT>
        {
            return node.GetValues(Math.Max(pageIndex - 1, 0) * pageSize, pageSize);
        }
        /// <summary>
        /// 获取分页数据数组
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pageIndex">分页编号，从 1 开始</param>
        /// <param name="pageSize">分页记录数</param>
        /// <returns></returns>
        public static async Task<LocalResult<LeftArray<VT>>> GetPageArray<KT, VT>(this ISearchTreeDictionaryNodeLocalClientNode<KT, VT> node, int pageIndex, byte pageSize) where KT : IComparable<KT>
        {
            LocalKeepCallback<ValueResult<VT>> response = await node.GetPage(pageIndex, pageSize);
            return await response.GetLeftArray(pageSize);
        }
    }
}
