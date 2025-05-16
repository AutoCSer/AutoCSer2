using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Net.CommandServer;
using AutoCSer.Extensions;

namespace AutoCSer.Net
{
    /// <summary>
    /// 仅发送数据命令 await bool 是否成功添加输出队列
    /// </summary>
    public class SendOnlyCommand : CallbackCommand
    {
        /// <summary>
        /// 仅发送数据命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        internal SendOnlyCommand(CommandClientController controller, int methodIndex) : base(controller, methodIndex) 
        {
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
#if NetStandard21
        internal override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
            if (stream.Data.Pointer.FreeSize >= sizeof(uint) || buildInfo.Count == 0)
            {
                uint methodIndex = Controller.GetMethodIndex(Method.MethodIndex);
                if (methodIndex != 0)
                {
                    stream.Data.Pointer.Write(methodIndex);
                    buildInfo.AddCount();
                }
                else ++buildInfo.FreeCount;
                return LinkNext;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
        /// <summary>
        /// 丢弃命令，用于清除 async 内部提示 await 的警告，仅用于确定不会超过客户端最大未处理命令数量限制，如果是批量请求并且可能超过限制则应该 await 等待
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Discard() { }
    }
    /// <summary>
    /// 仅发送数据命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SendOnlyCommand<T> : SendOnlyCommand
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T inputParameter;
        /// <summary>
        /// 仅发送数据命令
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="methodIndex"></param>
        /// <param name="inputParameter"></param>
        internal SendOnlyCommand(CommandClientController controller, int methodIndex, ref T inputParameter) : base(controller, methodIndex)
        {
            this.inputParameter = inputParameter;
            Push();
        }
        /// <summary>
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
#if NetStandard21
        internal unsafe override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal unsafe override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            uint methodIndex = Controller.GetMethodIndex(Method.MethodIndex);
            if (methodIndex != 0)
            {
                UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
                int streamLength = stream.SetCanResizeGetCurrentIndex(buildInfo.Count == 0);
                if (stream.MoveSize(sizeof(uint) + sizeof(int)))
                {
                    if (Method.IsSimpleSerializeParamter) SimpleSerialize.Serializer<T>.DefaultSerializer(stream, ref inputParameter);
                    else Controller.Socket.OutputSerializer.IndependentSerialize(ref inputParameter);
                    if (!stream.IsResizeError)
                    {
                        inputParameter = default(T);
                        int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(uint) + sizeof(int));
                        byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
                        *(uint*)dataFixed = methodIndex | (uint)CommandFlagsEnum.SendData;
                        *(int*)(dataFixed + sizeof(uint)) = dataLength;
                        buildInfo.AddCount();
                        return LinkNext;
                    }
                }
                stream.Data.Pointer.CurrentIndex = streamLength;
                buildInfo.IsFullSend = 1;
                return this;
            }
            inputParameter = default(T);
            ++buildInfo.FreeCount;
            OnBuildError(CommandClientReturnTypeEnum.ControllerMethodIndexError);
            return LinkNext;
        }
        ///// <summary>
        ///// 创建命令输入数据
        ///// </summary>
        ///// <param name="buildInfo">TCP 客户端创建命令参数</param>
        ///// <returns>是否成功</returns>
        //internal unsafe override Command Build(ref ClientBuildInfo buildInfo)
        //{
        //    UnmanagedStream stream = Controller.Socket.OutputSerializer.Stream;
        //    ClientInterfaceMethod method = Controller.Methods[MethodIndex];
        //    if (buildInfo.Count == 0 || method.CheckMaxDataSize(buildInfo.SendBufferSize, stream.Data.Pointer.CurrentIndex))
        //    {
        //        uint methodIndex = Controller.GetMethodIndex(MethodIndex);
        //        if (methodIndex != 0)
        //        {
        //            int streamLength = stream.Data.Pointer.CurrentIndex;
        //            stream.MoveSize(sizeof(uint) + sizeof(int));
        //            methodIndex |= (uint)CommandFlags.SendData;
        //            if (method.IsSimpleSerializeParamter) SimpleSerialize.Serializer<T>.DefaultSerializer(stream, ref inputParameter);
        //            else if (!method.IsJsonSerializeParamter) Controller.Socket.BinarySerialize(ref inputParameter);
        //            else
        //            {
        //                Controller.Socket.JsonSerialize(ref inputParameter);
        //                methodIndex |= (uint)CommandFlags.JsonSerialize;
        //            }
        //            int dataSize = stream.Data.Pointer.CurrentIndex - streamLength, dataLength = dataSize - (sizeof(uint) + sizeof(int));
        //            byte* dataFixed = stream.Data.Pointer.Byte + streamLength;
        //            *(uint*)dataFixed = methodIndex;
        //            *(int*)(dataFixed + sizeof(uint)) = dataLength;
        //            method.SetMaxDataSize(dataSize);
        //            buildInfo.AddCount();
        //            return LinkNext;
        //        }
        //        ++buildInfo.FreeCount;
        //        OnBuildError(CommandClientReturnType.ControllerMethodIndexError);
        //        return LinkNext;
        //    }
        //    buildInfo.IsFullSend = 1;
        //    return this;
        //}
        /// <summary>
        /// 创建命令输入数据错误处理
        /// </summary>
        /// <param name="returnType"></param>
        protected override void OnBuildError(CommandClientReturnTypeEnum returnType) { }
    }
}
