using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 保持回调输出扩展
    /// </summary>
    public static class LocalKeepCallbackExtension
    {
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <param name="response"></param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns></returns>
        public static async Task<LocalResult<LeftArray<T>>> GetLeftArray<T>(this LocalKeepCallback<ValueResult<T>> response, int capacity = 0)
        {
            LeftArray<T> array = new LeftArray<T>(capacity);
#if NetStandard21
            await foreach (LocalResult<ValueResult<T>> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<T>>();
                if (!value.Value.IsValue) return CallStateEnum.IllegalInputParameter;
                array.Add(value.Value.Value);
            }
#else
            while (await response.MoveNext())
            {
                LocalResult<ValueResult<T>> value = response.Current;
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
#if NetStandard21
        public static async Task<LocalResult<LeftArray<T?>>> GetLeftArray<T>(this LocalKeepCallback<T> response, int capacity = 0)
#else
        public static async Task<LocalResult<LeftArray<T>>> GetLeftArray<T>(this LocalKeepCallback<T> response, int capacity = 0)
#endif
        {
#if NetStandard21
            LeftArray<T?> array = new LeftArray<T?>(capacity);
            await foreach (LocalResult<T> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<T?>>();
                array.Add(value.Value);
            }
#else
            LeftArray<T> array = new LeftArray<T>(capacity);
            while (await response.MoveNext())
            {
                LocalResult<T> value = response.Current;
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
        public static async Task<LocalResult<LeftArray<VT>>> GetLeftArray<T, VT>(this LocalKeepCallback<ValueResult<T>> response, Func<T, VT> getValue, int capacity = 0)
        {
            LeftArray<VT> array = new LeftArray<VT>(capacity);
#if NetStandard21
            await foreach (LocalResult<ValueResult<T>> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<VT>>();
                if (!value.Value.IsValue) return CallStateEnum.IllegalInputParameter;
                array.Add(getValue(value.Value.Value));
            }
#else
            while (await response.MoveNext())
            {
                LocalResult<ValueResult<T>> value = response.Current;
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
#if NetStandard21
        public static async Task<LocalResult<LeftArray<VT?>>> GetLeftArray<T, VT>(this LocalKeepCallback<T> response, Func<T?, VT> getValue, int capacity = 0)
#else
        public static async Task<LocalResult<LeftArray<VT>>> GetLeftArray<T, VT>(this LocalKeepCallback<T> response, Func<T, VT> getValue, int capacity = 0)
#endif
        {
#if NetStandard21
            LeftArray<VT?> array = new LeftArray<VT?>(capacity);
            await foreach (LocalResult<T> value in response.GetAsyncEnumerable())
            {
                if (!value.IsSuccess) return value.Cast<LeftArray<VT?>>();
                array.Add(getValue(value.Value));
            }
#else
            LeftArray<VT> array = new LeftArray<VT>(capacity);
            while (await response.MoveNext())
            {
                LocalResult<T> value = response.Current;
                if (!value.IsSuccess) return value.Cast<LeftArray<VT>>();
                array.Add(getValue(value.Value));
            }
#endif
            return array;
        }
    }
}
