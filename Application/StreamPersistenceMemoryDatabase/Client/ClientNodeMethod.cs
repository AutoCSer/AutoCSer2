using AutoCSer.Extensions;
using AutoCSer.Net;
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
        /// 客户端节点方法信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        internal ClientNodeMethod(Type type, MethodInfo method) : base(type, method)
        {
            MethodIndex = int.MinValue;
            Parameters = method.GetParameters();
            ParameterEndIndex = Parameters.Length;
            ReturnValueType = method.ReturnType;
            bool isReturnType = false, isKeepCallback = false;
            if (ReturnValueType == typeof(SendOnlyCommand))
            {
                ReturnValueType = typeof(void);
                CallType = CallTypeEnum.SendOnly;
            }
            else
            {
                if (ReturnValueType.IsGenericType)
                {
                    Type genericTypeDefinition = ReturnValueType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Task<>))
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
                    Error = $"节点方法 {type.fullName()}.{method.Name} 返回值类型必须是 {typeof(Task<ResponseResult>).fullName()}";
                    return;
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
                InputParameterType = AutoCSer.Net.CommandServer.ServerMethodParameter.Get(ParameterCount, InputParameters, typeof(void));
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
        internal string Set(ServerNodeMethod method)
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
        /// <returns></returns>
        internal static string GetMethod(Type type, ref LeftArray<ClientNodeMethod> methods)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                string error = AutoCSer.Net.CommandServer.InterfaceController.CheckMethod(type, method);
                if (error != null) return error;
                methods.Add(new ClientNodeMethod(type, method));
            }
            return null;
        }
    }
}
