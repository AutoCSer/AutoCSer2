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
        private string stackTrace;
        /// <summary>
        /// 判断异常信息是否和上一次重复
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public bool IsRepeat(Exception exception)
        {
            try
            {
                string exceptionMessage = exception.Message, exceptionStackTrace = exception.StackTrace;
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
