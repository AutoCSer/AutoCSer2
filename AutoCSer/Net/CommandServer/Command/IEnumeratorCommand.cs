using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// Collection enumeration command interface
    /// 集合枚举命令接口
    /// </summary>
    public interface IEnumeratorCommand : IAsyncDisposable// : IEnumeratorTask
    {
        /// <summary>
        /// await bool, the collection enumeration command returns true when the next data exists
        /// await bool，集合枚举命令存在下一个数据返回 true
        /// </summary>
        /// <returns></returns>
        EnumeratorCommandMoveNext MoveNext();
    }
    /// <summary>
    /// Collection enumeration command interface
    /// 集合枚举命令接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEnumeratorCommand<T> : IEnumeratorCommand
    {
        /// <summary>
        /// Get current data
        /// 获取当前数据
        /// </summary>
        T Current { get; }
    }
}
