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
        /// 添加到持久化队列
        /// </summary>
        /// <param name="methodParameter"></param>
        private void pushPersistence(MethodParameter methodParameter)
        {
            if (methodParameter.Node.IsPersistence) node.NodeCreator.Service.PushPersistenceMethodParameter(methodParameter);
            else
            {
                CommandServerCallQueue queue = node.NodeCreator.Service.CommandServerCallQueue;
                if (queue.ThreadId == System.Environment.CurrentManagedThreadId) methodParameter.PersistenceCallback();
                else queue.AddOnly(new MethodParameterPersistenceCallback(methodParameter));
            }
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
        internal static void CreateCallMethodParameter(MethodParameterCreator creator, int methodIndex)
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
        internal static void CreateCallOutputMethodParameter(MethodParameterCreator creator, int methodIndex)
        {
            creator.createCallOutputMethodParameter(methodIndex);
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void createCallOutputMethodParameter(int methodIndex, CommandServerCallback<ResponseParameter>? callback)
#else
        private void createCallOutputMethodParameter(int methodIndex, CommandServerCallback<ResponseParameter> callback)
#endif
        {
            pushPersistence(new CallOutputMethodParameter(node, methodIndex, callback));
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
#if NetStandard21
        internal static void CreateCallOutputCallbackMethodParameter(MethodParameterCreator creator, int methodIndex, CommandServerCallback<ResponseParameter>? callback)
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
        internal static void CreateCallInputMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
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
        internal static void CreateCallInputOutputMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private void createCallInputOutputMethodParameter<T>(int methodIndex, ref T parameter, CommandServerCallback<ResponseParameter>? callback) where T : struct
#else
        private void createCallInputOutputMethodParameter<T>(int methodIndex, ref T parameter, CommandServerCallback<ResponseParameter> callback) where T : struct
#endif
        {
            pushPersistence(new CallInputOutputMethodParameter<T>(node, methodIndex, ref parameter, callback));
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
        internal static void CreateCallInputOutputCallbackMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter, CommandServerCallback<ResponseParameter>? callback) where T : struct
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
        internal static void CreateSendOnlyMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
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
        internal static void CreateKeepCallbackMethodParameter(MethodParameterCreator creator, int methodIndex)
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
        internal static void CreateInputKeepCallbackMethodParameter<T>(MethodParameterCreator creator, int methodIndex, T parameter) where T : struct
        {
            creator.createInputKeepCallbackMethodParameter(methodIndex, ref parameter);
        }
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
        internal MethodParameterCreator(ServerNode<T> node) : base(node)
        {
            Creator = (T)(object)this;
        }
    }
}
