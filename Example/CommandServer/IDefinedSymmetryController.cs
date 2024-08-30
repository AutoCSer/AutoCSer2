using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer
{
    /// <summary>
    /// 客户端与服务端定义一致的 定义对称 示例接口
    /// </summary>
    public interface IDefinedSymmetryController
    {
        /// <summary>
        /// 同步返回数据，支持 ref / out 参数
        /// </summary>
        /// <param name="parameter">普通参数</param>
        /// <param name="refParameter">ref 参数</param>
        /// <param name="outParameter">out 参数</param>
        /// <returns></returns>
        int SynchronousReturn(int parameter, ref int refParameter, out long outParameter);
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        void SynchronousCall(int parameter1, int parameter2);
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task<int> SynchronousReturnTask(int parameter1, int parameter2);
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task SynchronousCallTask(int parameter1, int parameter2);
    }
    /// <summary>
    /// 定义对称示例接口 服务端实例
    /// </summary>
    internal sealed class DefinedSymmetryController : IDefinedSymmetryController
    {
        /// <summary>
        /// 同步返回数据，支持 ref / out 参数
        /// </summary>
        /// <param name="parameter">普通参数</param>
        /// <param name="refParameter">ref 参数</param>
        /// <param name="outParameter">out 参数</param>
        /// <returns></returns>
        int IDefinedSymmetryController.SynchronousReturn(int parameter, ref int refParameter, out long outParameter)
        {
            refParameter = parameter + 1;
            outParameter = (long)parameter * refParameter;
            return parameter + 2;
        }
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        void IDefinedSymmetryController.SynchronousCall(int parameter1, int parameter2)
        {
            Console.WriteLine(parameter1 + parameter2);
        }
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task<int> IDefinedSymmetryController.SynchronousReturnTask(int parameter1, int parameter2)
        {
            return Task.FromResult(parameter1 + parameter2);
        }
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task IDefinedSymmetryController.SynchronousCallTask(int parameter1, int parameter2)
        {
            Console.WriteLine(parameter1 + parameter2);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
