using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 枚举命令接口
    /// </summary>
    public interface IEnumeratorCommand : IEnumeratorTask
    {
        /// <summary>
        /// 判断是否存在下一个数据 await bool
        /// </summary>
        /// <returns></returns>
        EnumeratorCommandMoveNext MoveNext();
    }
    /// <summary>
    /// 枚举命令接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEnumeratorCommand<T> : IEnumeratorCommand
    {
        /// <summary>
        /// 获取当前数据
        /// </summary>
        T Current { get; }
    }
}
