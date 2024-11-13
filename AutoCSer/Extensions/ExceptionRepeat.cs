using System;

namespace AutoCSer
{
    /// <summary>
    /// 异常信息重复判断
    /// </summary>
    public struct ExceptionRepeat
    {
        /// <summary>
        /// 异常提示信息
        /// </summary>
        private string message;
        /// <summary>
        /// 异常调用栈
        /// </summary>
#if NetStandard21
        private string? stackTrace;
#else
        private string stackTrace;
#endif
        /// <summary>
        /// 判断异常信息是否和上一次重复
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public bool IsRepeat(Exception exception)
        {
            try
            {
                string exceptionMessage = exception.Message;
                var exceptionStackTrace = exception.StackTrace;
                if (exceptionMessage == message)
                {
                    if (exceptionStackTrace == stackTrace) return true;
                }
                else message = exceptionMessage;
                stackTrace = exceptionStackTrace;
            }
            catch { }
            return false;
        }
    }
}
