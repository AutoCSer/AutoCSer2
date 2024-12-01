//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase
{
        /// <summary>
        /// 服务端测试接口 客户端接口
        /// </summary>
        public partial interface IServerTaskQueueControllerClientController
        {
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.Net.ReturnCommand TaskQueue();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="Ref"></param>
            AutoCSer.Net.ReturnCommand TaskQueue(int Value, int Ref);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> TaskQueueReturn();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="Ref"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> TaskQueueReturn(int Value, int Ref);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> TaskQueueReturnSocket();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="Ref"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> TaskQueueReturnSocket(int Value, int Ref);
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.Net.ReturnCommand TaskQueueSocket();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="Ref"></param>
            AutoCSer.Net.ReturnCommand TaskQueueSocket(int Value, int Ref);
        }
}namespace AutoCSer.TestCase.ServerBindContext
{
        /// <summary>
        /// 服务端测试接口（套接字上下文绑定服务端） 客户端接口
        /// </summary>
        public partial interface IServerTaskQueueControllerClientController
        {
            /// <summary>
            /// 
            /// </summary>
            AutoCSer.Net.ReturnCommand TaskQueue();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="Ref"></param>
            AutoCSer.Net.ReturnCommand TaskQueue(int Value, int Ref);
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> TaskQueueReturn();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="Ref"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<string> TaskQueueReturn(int Value, int Ref);
        }
}namespace AutoCSer.TestCase
{
    public enum DefinedDissymmetryControllerMethodEnum
    {
            /// <summary>
            /// [0] 
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.TestCase.Data.ORM.BusinessModel value 
            /// </summary>
            SetSocket = 0,
            /// <summary>
            /// [1] 
            /// AutoCSer.Net.CommandServerSocket socket 
            /// 返回值 AutoCSer.TestCase.Data.ORM.BusinessModel 
            /// </summary>
            GetSocket = 1,
            /// <summary>
            /// [2] 
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.TestCase.Data.ORM.BusinessModel value 
            /// </summary>
            SetSocketTask = 2,
            /// <summary>
            /// [3] 
            /// AutoCSer.Net.CommandServerSocket socket 
            /// 返回值 AutoCSer.TestCase.Data.ORM.BusinessModel 
            /// </summary>
            GetSocketTask = 3,
    }
}namespace AutoCSer.TestCase.ServerBindContext
{
    public enum ServerBindContextDefinedDissymmetryControllerMethodEnum
    {
            /// <summary>
            /// [0] 
            /// AutoCSer.TestCase.Data.ORM.BusinessModel value 
            /// </summary>
            SetSocket = 0,
            /// <summary>
            /// [1] 
            /// 返回值 AutoCSer.TestCase.Data.ORM.BusinessModel 
            /// </summary>
            GetSocket = 1,
            /// <summary>
            /// [2] 
            /// AutoCSer.TestCase.Data.ORM.BusinessModel value 
            /// </summary>
            SetSocketTask = 2,
            /// <summary>
            /// [3] 
            /// 返回值 AutoCSer.TestCase.Data.ORM.BusinessModel 
            /// </summary>
            GetSocketTask = 3,
    }
}
#endif