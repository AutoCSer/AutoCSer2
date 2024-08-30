using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// API 接口返回状态
    /// </summary>
    public struct ResponseResult
    {
        /// <summary>
        /// 返回值状态，1 / Success 表示成功
        /// </summary>
        public ResponseStateEnum State;
        /// <summary>
        /// 附加错误信息
        /// </summary>
        public string Message;
        /// <summary>
        /// 返回状态是否成功
        /// </summary>
        public bool IsSuccess { get { return State == ResponseStateEnum.Success; } }
        /// <summary>
        /// 返回 JSON 值
        /// </summary>
        /// <param name="state">返回状态</param>
        /// <param name="message">附加错误信息</param>
        public ResponseResult(ResponseStateEnum state, string message = null)
        {
            State = state;
            Message = message;
        }
        /// <summary>
        /// 状态转换
        /// </summary>
        /// <param name="state">返回状态</param>
        public static implicit operator ResponseResult(ResponseStateEnum state) { return new ResponseResult(state); }
        /// <summary>
        /// 错误状态转换
        /// </summary>
        /// <param name="message">附加错误信息</param>
        public static implicit operator ResponseResult(string message) { return new ResponseResult(ResponseStateEnum.Unknown, message); }
    }
    /// <summary>
    /// API 接口返回值
    /// </summary>
    /// <typeparam name="T">返回值数据类型</typeparam>
    public struct ResponseResult<T>
    {
        /// <summary>
        /// 返回值状态，1 / Success 表示成功
        /// </summary>
        public ResponseStateEnum State;
        /// <summary>
        /// 返回结果
        /// </summary>
        public T Result;
        /// <summary>
        /// 附加错误信息
        /// </summary>
        public string Message;
        /// <summary>
        /// 返回状态是否成功
        /// </summary>
        public bool IsSuccess { get { return State == ResponseStateEnum.Success; } }
        /// <summary>
        /// 成功返回值
        /// </summary>
        /// <param name="result">返回值</param>
        public ResponseResult(T result)
        {
            State = ResponseStateEnum.Success;
            Result = result;
            Message = null;
        }
        /// <summary>
        /// 错误状态
        /// </summary>
        /// <param name="state">错误状态</param>
        /// <param name="message">附加错误信息</param>
        public ResponseResult(ResponseStateEnum state, string message = null)
        {
            State = state;
            Result = default(T);
            Message = message;
        }
        /// <summary>
        /// 错误状态
        /// </summary>
        /// <param name="result">返回值</param>
        private ResponseResult(ResponseResult result)
        {
            State = result.State;
            Result = default(T);
            Message = result.Message;
        }
        /// <summary>
        /// 成功返回值转换
        /// </summary>
        /// <param name="result">返回值</param>
        public static implicit operator ResponseResult<T>(T result) { return new ResponseResult<T>(result); }
        /// <summary>
        /// 错误状态转换
        /// </summary>
        /// <param name="state">错误状态</param>
        public static implicit operator ResponseResult<T>(ResponseStateEnum state) { return new ResponseResult<T>(state); }
        /// <summary>
        /// 错误状态转换
        /// </summary>
        /// <param name="result">错误状态</param>
        public static implicit operator ResponseResult<T>(ResponseResult result) { return new ResponseResult<T>(result); }
        /// <summary>
        /// 错误状态转换
        /// </summary>
        /// <param name="result">错误状态</param>
        public static implicit operator ResponseResult(ResponseResult<T> result) { return new ResponseResult(result.State, result.Message); }
    }
}
