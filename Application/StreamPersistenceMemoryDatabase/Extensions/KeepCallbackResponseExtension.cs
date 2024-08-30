using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 保持回调输出扩展
    /// </summary>
    public static class KeepCallbackResponseExtension
    {
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="response"></param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns></returns>
        public static async Task<ResponseResult<LeftArray<T>>> GetLeftArray<T>(this KeepCallbackResponse<ValueResult<T>> response, int capacity = 0)
        {
            LeftArray<T> array = new LeftArray<T>(capacity);
#if DotNet45 || NetStandard2
            while (await response.MoveNextAsync())
            {
                ResponseResult<ValueResult<T>> value = response.Current;
                if (!value.IsSuccess) return value.Cast<LeftArray<T>>();
                if (!value.Value.IsValue) return CallStateEnum.IllegalInputParameter;
                array.Add(value.Value.Value);
            }
#else
            await foreach (ResponseResult<ValueResult<T>> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<T>>();
                if (!value.Value.IsValue) return CallStateEnum.IllegalInputParameter;
                array.Add(value.Value.Value);
            }
#endif
            return array;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="response"></param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns></returns>
        public static async Task<ResponseResult<LeftArray<T>>> GetLeftArray<T>(this KeepCallbackResponse<T> response, int capacity = 0)
        {
            LeftArray<T> array = new LeftArray<T>(capacity);
#if DotNet45 || NetStandard2
            while (await response.MoveNextAsync())
            {
                ResponseResult<T> value = response.Current;
                if (!value.IsSuccess) return value.Cast<LeftArray<T>>();
                array.Add(value.Value);
            }
#else
            await foreach (ResponseResult<T> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<T>>();
                array.Add(value.Value);
            }
#endif
            return array;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="response"></param>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns></returns>
        public static async Task<ResponseResult<LeftArray<VT>>> GetLeftArray<T, VT>(this KeepCallbackResponse<ValueResult<T>> response, Func<T, VT> getValue, int capacity = 0)
        {
            LeftArray<VT> array = new LeftArray<VT>(capacity);
#if DotNet45 || NetStandard2
            while (await response.MoveNextAsync())
            {
                ResponseResult<ValueResult<T>> value = response.Current;
                if (!value.IsSuccess) return value.Cast<LeftArray<VT>>();
                if (!value.Value.IsValue) return CallStateEnum.IllegalInputParameter;
                array.Add(getValue(value.Value.Value));
            }
#else
            await foreach (ResponseResult<ValueResult<T>> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<VT>>();
                if (!value.Value.IsValue) return CallStateEnum.IllegalInputParameter;
                array.Add(getValue(value.Value.Value));
            }
#endif
            return array;
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="response"></param>
        /// <param name="getValue">数据转换委托</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns></returns>
        public static async Task<ResponseResult<LeftArray<VT>>> GetLeftArray<T, VT>(this KeepCallbackResponse<T> response, Func<T, VT> getValue, int capacity = 0)
        {
            LeftArray<VT> array = new LeftArray<VT>(capacity);
#if DotNet45 || NetStandard2
            while (await response.MoveNextAsync())
            {
                ResponseResult<T> value = response.Current;
                if (!value.IsSuccess) return value.Cast<LeftArray<VT>>();
                array.Add(getValue(value.Value));
            }
#else
            await foreach (ResponseResult<T> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<VT>>();
                array.Add(getValue(value.Value));
            }
#endif
            return array;
        }
    }
}
