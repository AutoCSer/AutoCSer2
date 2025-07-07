using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct PageResult<T>
    {
        /// <summary>
        /// 当前分页数据
        /// </summary>
        public readonly T[] Values;
        /// <summary>
        /// 查询记录总数
        /// </summary>
        public readonly int TotalCount;
        /// <summary>
        /// 当前分页编号（从 0 开始）
        /// </summary>
        public readonly int PageIndex;
        /// <summary>
        /// 当前分页编号（从 1 开始）
        /// </summary>
        public int PageIndex1
        {
            get { return PageIndex + 1; }
        }
        /// <summary>
        /// 查询分页记录数量
        /// </summary>
        public readonly int PageSize;
        /// <summary>
        /// 分页总数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (TotalCount > 0) return (TotalCount - 1) / PageSize + 1;
                return 0;
            }
        }
        /// <summary>
        /// The return value type of the network client
        /// 网络客户端返回值类型
        /// </summary>
        public CommandClientReturnTypeEnum ReturnType;
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum CallState;
        /// <summary>
        /// Is the call successful
        /// 是否调用成功
        /// </summary>
        public bool IsSuccess { get { return CallState == CallStateEnum.Success && ReturnType == CommandClientReturnTypeEnum.Success; } }
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="values">当前分页数据</param>
        /// <param name="totalCount">查询记录总数</param>
        /// <param name="pageIndex">当前分页编号（从 0 开始）</param>
        /// <param name="pageSize">查询分页记录数量</param>
        public PageResult(T[] values, int totalCount, int pageIndex, int pageSize)
        {
            Values = values;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
            ReturnType = CommandClientReturnTypeEnum.Success;
            CallState = CallStateEnum.Success;
        }
        /// <summary>
        /// 错误分页数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="state"></param>
        internal PageResult(CommandClientReturnTypeEnum returnType, CallStateEnum state)
        {
            Values = EmptyArray<T>.Array;
            PageSize = PageIndex = TotalCount = 0;
            ReturnType = returnType;
            CallState = state;
        }
        /// <summary>
        /// 错误分页数据
        /// </summary>
        /// <param name="returnType"></param>
        public PageResult(CommandClientReturnTypeEnum returnType)
        {
            Values = EmptyArray<T>.Array;
            PageSize = PageIndex = TotalCount = 0;
            ReturnType = returnType;
            CallState = CallStateEnum.Unknown;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="values"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        //public PageResult(LeftArray<T> values, int pageIndex, int pageSize)
        //{
        //    int startIndex = pageIndex * pageSize;
        //    Values = new T[Math.Min(values.Count - startIndex, PageSize)];
        //    TotalCount = values.Count;
        //    PageIndex = pageIndex;
        //    PageSize = pageSize;
        //    Array.Copy(values.Array, startIndex, Values, 0, Values.Length);
        //}
        /// <summary>
        /// 转换分页数据类型
        /// </summary>
        /// <typeparam name="VT">Data type</typeparam>
        /// <param name="values">当前分页数据</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public PageResult<VT> Cast<VT>(VT[] values)
        {
            return new PageResult<VT>(values, TotalCount, PageIndex, PageSize);
        }
        /// <summary>
        /// 错误数据转换
        /// </summary>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public PageResult<VT> Cast<VT>()
        {
            return new PageResult<VT>(ReturnType, CallState);
        }
    }
}
