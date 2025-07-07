using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分页数组
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct PageArray<T>
    {
        /// <summary>
        /// 数据数组
        /// </summary>
        private T[] array;
        /// <summary>
        /// 分页查询参数
        /// </summary>
        private readonly PageParameter parameter;
        /// <summary>
        /// 跳过记录数量
        /// </summary>
        private long skipCount;
        /// <summary>
        /// 当前数组写入索引位置
        /// </summary>
        private int index;
        /// <summary>
        /// 数据总数
        /// </summary>
        private int count;
        /// <summary>
        /// 分页数组
        /// </summary>
        /// <param name="parameter">分页查询参数</param>
        internal PageArray(PageParameter parameter)
        {
            this.parameter = parameter;
            index = -1;
            array = EmptyArray<T>.Array;
            skipCount = (long)parameter.PageIndex * parameter.PageSize;
            count = 0;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <returns>是否需要将数据写入数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Add()
        {
            ++count;
            return index != array.Length && --skipCount < 0;
        }
        /// <summary>
        /// 将数据写入数组
        /// </summary>
        /// <param name="value"></param>
        public void Add(T value)
        {
            if (array.Length != 0) array[index++] = value;
            else
            {
                array = new T[parameter.PageSize];
                array[0] = value;
                index = 1;
            }
        }
        /// <summary>
        /// Get page data
        /// 获取分页数据
        /// </summary>
        /// <returns></returns>
        public PageResult<T> GetPageResult()
        {
            if (array.Length != 0 && array.Length != index) Array.Resize(ref array, index);
            return new PageResult<T>(array, count, parameter.PageIndex, parameter.PageSize);
        }
    }
}
