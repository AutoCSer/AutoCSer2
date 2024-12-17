using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 客户端节点方法信息
    /// </summary>
    internal sealed class ClientNodeMethod : NodeMethod
    {
        /// <summary>
        /// 本地客户端 SendOnly 方法返回值类型
        /// </summary>
        internal static readonly Type LocalClientSendOnlyMethodReturnType = typeof(MethodParameter);
        /// <summary>
        /// 是否返回参数类型
        /// </summary>
        internal readonly bool IsReturnResponseParameter;

        /// <summary>
        /// 客户端节点方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="isLocalClient"></param>
        internal ClientNodeMethod(Type type, MethodInfo method, bool isLocalClient) : base(type, method)
        {
            MethodIndex = int.MinValue;
            Parameters = method.GetParameters();
            ParameterEndIndex = Parameters.Length;
            ReturnValueType = method.ReturnType;
            bool isReturnType = false, isKeepCallback = false;
            if (ReturnValueType == (isLocalClient ? LocalClientSendOnlyMethodReturnType : typeof(SendOnlyCommand)))
            {
                ReturnValueType = typeof(void);
                CallType = CallTypeEnum.SendOnly;
            }
            else
            {
                if (ReturnValueType.IsGenericType)
                {
                    Type genericTypeDefinition = ReturnValueType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == (isLocalClient ? typeof(LocalServiceQueueNode<>) : typeof(Task<>)))
                    {
                        ReturnValueType = ReturnValueType.GetGenericArguments()[0];
                        if (ReturnValueType == typeof(ResponseResult))
                        {
                            ReturnValueType = typeof(void);
                            isReturnType = true;
                        }
                        else if (ReturnValueType.IsGenericType)
                        {
                            genericTypeDefinition = ReturnValueType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(ResponseResult<>))
                            {
                                ReturnValueType = ReturnValueType.GetGenericArguments()[0];
                                isReturnType = true;
                            }
                            else if (genericTypeDefinition == typeof(KeepCallbackResponse<>))
                            {
                                ReturnValueType = ReturnValueType.GetGenericArguments()[0];
                                isReturnType = isKeepCallback = true;
                            }
                        }
                    }
                }
                if (!isReturnType)
                {
                    Type awaitType = isLocalClient ? typeof(LocalServiceQueueNode<ResponseResult>) : typeof(Task<ResponseResult>);
                    Error = $"节点方法 {type.fullName()}.{method.Name} 返回值类型必须是 {awaitType.fullName()}";
                    return;
                }
                if (!isLocalClient)
                {
                    if (ParameterStartIndex != ParameterEndIndex && ReturnValueType == (isKeepCallback ? typeof(ResponseParameterSerializer) : typeof(ResponseParameter)))
                    {
                        ParameterInfo parameter = Parameters[ParameterStartIndex];
                        if (parameter.ParameterType == ReturnValueType && string.Equals(parameter.Name, nameof(ServerReturnValue<int>.ReturnValue), StringComparison.OrdinalIgnoreCase))
                        {
                            IsReturnResponseParameter = true;
                            ++ParameterStartIndex;
                        }
                    }
                }
                setCallType();
                if (isKeepCallback)
                {
                    switch (CallType)
                    {
                        case CallTypeEnum.CallOutput: CallType = CallTypeEnum.KeepCallback; break;
                        case CallTypeEnum.CallInputOutput: CallType = CallTypeEnum.InputKeepCallback; break;
                    }
                }
            }
            if (!checkParameter()) return;
            if (ParameterStartIndex != ParameterEndIndex)
            {
                InputParameterType = AutoCSer.Net.CommandServer.ServerMethodParameter.Get(ParameterCount, InputParameters, typeof(void)).notNull();
                InputParameterFields = InputParameterType.GetFields(InputParameters);
                IsSimpleSerializeParamter = InputParameterType.IsSimpleSerialize;
            }
            if (ReturnValueType != typeof(void)) IsSimpleDeserializeParamter = SimpleSerialize.Serializer.IsType(ReturnValueType);
        }
        /// <summary>
        /// 设置服务端节点方法数据
        /// </summary>
        /// <param name="method"></param>
        /// <returns>错误信息</returns>
#if NetStandard21
        internal string? Set(ServerNodeMethod method)
#else
        internal string Set(ServerNodeMethod method)
#endif
        {
            MethodIndex = method.MethodIndex;
            if (CallType == method.CallType) return null;
            switch (CallType)
            {
                case CallTypeEnum.CallOutput:
                    if (method.CallType == CallTypeEnum.Callback) return null;
                    break;
                case CallTypeEnum.CallInputOutput:
                    if (method.CallType == CallTypeEnum.InputCallback) return null;
                    break;
                case CallTypeEnum.KeepCallback:
                    if (method.CallType == CallTypeEnum.Enumerable) return null;
                    break;
                case CallTypeEnum.InputKeepCallback:
                    if (method.CallType == CallTypeEnum.InputEnumerable) return null;
                    break;
            }
            return $"客户端节点方法 {method.Method.Name} 调用类型 {CallType} 与服务端节点方法调用类型 {method.CallType} 不匹配";
        }

        /// <summary>
        /// 获取客户端节点接口方法集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methods"></param>
        /// <param name="isLocalClient"></param>
        /// <returns></returns>
#if NetStandard21
        internal static string? GetMethod(Type type, ref LeftArray<ClientNodeMethod> methods, bool isLocalClient)
#else
        internal static string GetMethod(Type type, ref LeftArray<ClientNodeMethod> methods, bool isLocalClient)
#endif
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                var error = AutoCSer.Net.CommandServer.InterfaceController.CheckMethod(type, method);
                if (error != null) return error;
                methods.Add(new ClientNodeMethod(type, method, isLocalClient));
            }
            return null;
        }
    }
}
