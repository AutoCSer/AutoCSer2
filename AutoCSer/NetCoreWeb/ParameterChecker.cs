using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 代理控制器方法调用参数检查
    /// </summary>
    public struct ParameterChecker
    {
        /// <summary>
        /// 参数检查错误信息
        /// </summary>
        internal string Message;
        /// <summary>
        /// 错误返回值
        /// </summary>
        public ResponseResult ErrorResult
        {
            get { return new ResponseResult(ResponseStateEnum.ParameterConstraint, Message); }
        }

        /// <summary>
        /// 检查参数不允许为 null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public static bool CheckNull<T>(T value, string name, string summary, ref ParameterChecker checker)
            where T : class
        {
            if (value != null) return true;
            checker.Message = $"参数 {summary} {name} 不允许为 null";
            return false;
        }
        /// <summary>
        /// 检查参数不允许为默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public static bool CheckEquatable<T>(T value, string name, string summary, ref ParameterChecker checker)
            where T : IEquatable<T>
        {
            if (!value.Equals(default(T))) return true;
            checker.Message = $"参数 {summary} {name} 不允许为默认值";
            return false;
        }
        /// <summary>
        /// 检查参数不允许为空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public static bool CheckCollection<T>(T value, string name, string summary, ref ParameterChecker checker)
            where T : System.Collections.ICollection
        {
            if (value?.Count > 0) return true;
            checker.Message = $"参数 {summary} {name} 不允许为空集合";
            return false;
        }
        /// <summary>
        /// 检查参数不允许为空字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public static bool CheckString(string value, string name, string summary, ref ParameterChecker checker)
        {
            if (!string.IsNullOrEmpty(value)) return true;
            checker.Message = $"参数 {summary} {name} 不允许为空字符串";
            return false;
        }
        /// <summary>
        /// 自定义约束参数检查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="summary"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public static bool CheckConstraint<T>(T value, string name, string summary, ref ParameterChecker checker)
            where T : IParameterConstraint
        {
            if (value != null)
            {
                string message = value.Check(name, summary);
                if (message == null) return true;
                checker.Message = message;
                return false;
            }
            checker.Message = $"参数 {summary} {name} 不允许为 null";
            return false;
        }
    }
}
