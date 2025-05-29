using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 创建调用方法与参数信息
    /// </summary>
    public abstract class MethodParameterCreator
    {
        /// <summary>
        /// 服务端节点
        /// </summary>
        private readonly ServerNode node;
        ///// <summary>
        ///// 最后一次调用生成的调用方法与参数信息
        ///// </summary>
        //internal MethodParameter MethodParameter;
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        protected MethodParameterCreator(ServerNode node)
        {
            this.node = node;
        }
        ///// <summary>
        ///// 添加到持久化队列
        ///// </summary>
        ///// <returns></returns>
        //public bool PushPersistence()
        //{
        //    MethodParameter methodParameter = MethodParameter;
        //    if(methodParameter != null)
        //    {
        //        MethodParameter = null;
        //        service.PushPersistenceMethodParameter(methodParameter);
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// 添加到持久化队列（持久化 API 先持久化请求数据再执行请求保证持久化的可靠性，避免出现反馈客户端成功以后出现持久化失败丢失数据的情况）
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <returns></returns>
        private bool pushPersistence(MethodParameter methodParameter)
        {
            if (methodParameter.Node.IsPersistence) return node.NodeCreator.Service.PushPersistenceMethodParameter(methodParameter);
            node.NodeCreator.Service.CommandServerCallQueue.AppendWriteOnly(new MethodParameterPersistenceCallback(methodParameter));
            return true;
        }

        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="methodIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void createCallMethodParameter(int methodIndex)
        {
            pushPersistence(new CallMethodParameter(node, methodIndex));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        public static void CreateCallMethodParameter(MethodParameterCreator creator, int methodIndex)
        {
            creator.createCallMethodParameter(methodIndex);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="methodIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void createCallOutputMethodParameter(int methodIndex)
        {
            pushPersistence(new CallOutputMethodParameter(node, methodIndex));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        public static void CreateCallOutputMethodParameter(MethodParameterCreator creator, int methodIndex)
        {
            creator.createCallOutputMethodParameter(methodIndex);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
#if NetStandard21
        private void createCallOutputMethodParameter(int methodIndex, CommandServerCallback<ResponseParameter>? callback)
#else
        private void createCallOutputMethodParameter(int methodIndex, CommandServerCallback<ResponseParameter> callback)
#endif
        {
            if (!pushPersistence(new CallOutputMethodParameter(node, methodIndex, callback))) callback?.Callback(ResponseParameter.CallStates[(byte)CallStateEnum.NotSupportPersistence]);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
#if NetStandard21
        public static void CreateCallOutputCallbackMethodParameter(MethodParameterCreator creator, int methodIndex, CommandServerCallback<ResponseParameter>? callback)
#else
        internal static void CreateCallOutputCallbackMethodParameter(MethodParameterCreator creator, int methodIndex, CommandServerCallback<ResponseParameter> callback)
#endif
        {
            creator.createCallOutputMethodParameter(methodIndex, callback);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void createCallInputMethodParameter<T>(int methodIndex, ref T parameter) where T : struct
        {
            pushPersistence(new CallInputMethodParameter<T>(node, methodIndex, ref parameter));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public static void CreateCallInputMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
        {
            creator.createCallInputMethodParameter(methodIndex, ref parameter);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void createCallInputOutputMethodParameter<T>(int methodIndex, ref T parameter) where T : struct
        {
            pushPersistence(new CallInputOutputMethodParameter<T>(node, methodIndex, ref parameter));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public static void CreateCallInputOutputMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
        {
            creator.createCallInputOutputMethodParameter(methodIndex, ref parameter);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        /// <param name="callback"></param>
#if NetStandard21
        private void createCallInputOutputMethodParameter<T>(int methodIndex, ref T parameter, CommandServerCallback<ResponseParameter>? callback) where T : struct
#else
        private void createCallInputOutputMethodParameter<T>(int methodIndex, ref T parameter, CommandServerCallback<ResponseParameter> callback) where T : struct
#endif
        {
            if (!pushPersistence(new CallInputOutputMethodParameter<T>(node, methodIndex, ref parameter, callback))) callback?.Callback(ResponseParameter.CallStates[(byte)CallStateEnum.NotSupportPersistence]);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        /// <param name="callback"></param>
#if NetStandard21
        public static void CreateCallInputOutputCallbackMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter, CommandServerCallback<ResponseParameter>? callback) where T : struct
#else
        internal static void CreateCallInputOutputCallbackMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter, CommandServerCallback<ResponseParameter> callback) where T : struct
#endif
        {
            creator.createCallInputOutputMethodParameter(methodIndex, ref parameter, callback);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void createSendOnlyMethodParameter<T>(int methodIndex, ref T parameter) where T : struct
        {
            pushPersistence(new SendOnlyMethodParameter<T>(node, methodIndex, ref parameter));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public static void CreateSendOnlyMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
        {
            creator.createSendOnlyMethodParameter(methodIndex, ref parameter);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="methodIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void createKeepCallbackMethodParameter(int methodIndex)
        {
            pushPersistence(new KeepCallbackMethodParameter(node, methodIndex));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        public static void CreateKeepCallbackMethodParameter(MethodParameterCreator creator, int methodIndex)
        {
            creator.createKeepCallbackMethodParameter(methodIndex);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void createInputKeepCallbackMethodParameter<T>(int methodIndex, ref T parameter) where T : struct
        {
            pushPersistence(new InputKeepCallbackMethodParameter<T>(node, methodIndex, ref parameter));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public static void CreateInputKeepCallbackMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
        {
            creator.createInputKeepCallbackMethodParameter(methodIndex, ref parameter);
        }
        /// <summary>
        /// 代码生成模板
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static object MethodParameterCreatorCallMethodName(params object[] values) { return AutoCSer.Common.EmptyObject; }
    }
    /// <summary>
    /// 创建调用方法与参数信息，用于服务端自定义持久化调用，调用接口方法会添加到持久化队列
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    public abstract class MethodParameterCreator<T> : MethodParameterCreator
    {
        /// <summary>
        /// 创建调用方法与参数信息，用于服务端自定义持久化调用，调用接口方法会添加到持久化队列
        /// </summary>
        public readonly T Creator;
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        public MethodParameterCreator(ServerNode<T> node) : base(node)
        {
            Creator = (T)(object)this;
        }
    }
}
