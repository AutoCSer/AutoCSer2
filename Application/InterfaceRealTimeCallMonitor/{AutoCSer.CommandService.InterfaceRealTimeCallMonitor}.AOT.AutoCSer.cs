//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 实时调用信息序列化数据
        /// </summary>
    public partial class CallData
    {
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.CompletedTimestamp);
                __deserializer__.BinaryDeserialize(ref this.StartTimestamp);
                __deserializer__.BinaryDeserialize(ref this.TimeoutTimestamp);
                __deserializer__.BinaryDeserialize(ref this.StepTimestamp);
                __deserializer__.BinaryDeserialize(ref this.Step);
                __deserializer__.BinaryDeserialize(ref this.Type);
                __deserializer__.BinaryDeserialize(ref this.IsException);
                __deserializer__.FixedFillSize(1);
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.CallName);
                __deserializer__.BinaryDeserialize(ref this.CallType);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 0, 1073741833);
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData value = default(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData>();
            }
    }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 实时调用信息序列化数据
        /// </summary>
    public partial class CallData
    {
            /// <summary>
            /// 默认构造函数
            /// </summary>
            internal static AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData DefaultConstructor()
            {
                return new AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData();
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void DefaultConstructorReflection()
            {
                DefaultConstructor();
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData>();
            }
    }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 实时调用时间戳信息
        /// </summary>
    public partial struct CallTimestamp
    {
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp value)
            {
                value.binaryDeserialize(deserializer);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                binaryFieldDeserialize(__deserializer__);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            private void binaryFieldDeserialize(AutoCSer.BinaryDeserializer __deserializer__)
            {
                __deserializer__.BinaryDeserialize(ref this.CallData);
                __deserializer__.Simple(ref this.ServerTimestamp);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                AutoCSer.BinarySerialize.TypeInfo typeInfo = new AutoCSer.BinarySerialize.TypeInfo(false, 0, 1073741826);
                return typeInfo;
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp value = default(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp>();
            }
    }
}namespace AutoCSer.CommandService
{
        /// <summary>
        /// 接口实时调用监视服务接口 客户端接口
        /// </summary>
        public partial interface IInterfaceRealTimeCallMonitorServiceClientController
        {
            /// <summary>
            /// 接口监视服务在线检查
            /// </summary>
            AutoCSer.Net.EnumeratorCommand Check();
            /// <summary>
            /// 调用完成
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="isException">接口是否执行异常</param>
            AutoCSer.Net.SendOnlyCommand Completed(long callIdentity, bool isException);
            /// <summary>
            /// 获取未完成调用数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> GetCount();
            /// <summary>
            /// 获取异常调用数据
            /// </summary>
            /// <returns>实时调用时间戳信息回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetException();
            /// <summary>
            /// 获取超时调用数据
            /// </summary>
            /// <returns>实时调用时间戳信息回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetTimeout();
            /// <summary>
            /// 获取指定数量的超时调用
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>超时调用回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetTimeoutCalls(int count);
            /// <summary>
            /// 获取超时调用数量
            /// </summary>
            /// <returns>超时调用数量</returns>
            AutoCSer.Net.ReturnCommand<int> GetTimeoutCount();
            /// <summary>
            /// 设置自定义调用步骤
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="step">自定义调用步骤</param>
            AutoCSer.Net.SendOnlyCommand SetStep(long callIdentity, int step);
            /// <summary>
            /// 新增一个实时调用信息
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            /// <param name="timeoutMilliseconds">超时毫秒数</param>
            /// <param name="type">调用类型</param>
            AutoCSer.Net.SendOnlyCommand Start(long callIdentity, string callType, string callName, int timeoutMilliseconds, ushort type);
        }
}namespace AutoCSer.CommandService
{
        /// <summary>
        /// 接口实时调用监视服务接口方法序号映射枚举类型
        /// </summary>
    public enum InterfaceRealTimeCallMonitorServiceMethodEnum
    {
            /// <summary>
            /// [0] 接口监视服务在线检查
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// AutoCSer.Net.CommandServerKeepCallback callback 在线检查回调
            /// </summary>
            Check = 0,
            /// <summary>
            /// [1] 调用完成
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// long callIdentity 调用标识
            /// bool isException 接口是否执行异常
            /// </summary>
            Completed = 1,
            /// <summary>
            /// [2] 获取未完成调用数量
            /// 返回值 int 
            /// </summary>
            GetCount = 2,
            /// <summary>
            /// [3] 获取异常调用数据
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp} callback 实时调用时间戳信息回调
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp 
            /// </summary>
            GetException = 3,
            /// <summary>
            /// [4] 获取超时调用数据
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp} callback 实时调用时间戳信息回调
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp 
            /// </summary>
            GetTimeout = 4,
            /// <summary>
            /// [5] 获取指定数量的超时调用
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// int count 获取数量
            /// AutoCSer.Net.CommandServerKeepCallback{AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp} callback 超时调用回调
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp 
            /// </summary>
            GetTimeoutCalls = 5,
            /// <summary>
            /// [6] 获取超时调用数量
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// 返回值 int 超时调用数量
            /// </summary>
            GetTimeoutCount = 6,
            /// <summary>
            /// [7] 设置自定义调用步骤
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// long callIdentity 调用标识
            /// int step 自定义调用步骤
            /// </summary>
            SetStep = 7,
            /// <summary>
            /// [8] 新增一个实时调用信息
            /// AutoCSer.Net.CommandServerSocket socket 
            /// AutoCSer.Net.CommandServerCallQueue queue 
            /// long callIdentity 调用标识
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// int timeoutMilliseconds 超时毫秒数
            /// ushort type 调用类型
            /// </summary>
            Start = 8,
    }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 触发 AOT 编译
    /// </summary>
    public static class AotMethod
    {
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            /// <returns></returns>
            public static bool Call()
            {
                if (AutoCSer.Date.StartTimestamp == long.MinValue)
                {
                    AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData/**/.BinarySerialize();
                    AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData/**/.DefaultConstructorReflection();
                    AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp/**/.BinarySerialize();

                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<long>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<int>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<ushort>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<bool>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<string>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallData>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.BinarySerialize.TypeSerializer<AutoCSer.Diagnostics.ServerTimestamp>));
                    AutoCSer.BinarySerializer.Simple<AutoCSer.Diagnostics.ServerTimestamp>(null, default(AutoCSer.Diagnostics.ServerTimestamp));
                    binaryDeserializeMemberTypes();



                    return true;
                }
                return false;
            }
            /// <summary>
            /// 二进制反序列化成员类型代码生成调用激活 AOT 反射
            /// </summary>
            private static void binaryDeserializeMemberTypes()
            {
                AutoCSer.Diagnostics.ServerTimestamp t1 = default(AutoCSer.Diagnostics.ServerTimestamp);
                AutoCSer.BinaryDeserializer.Simple<AutoCSer.Diagnostics.ServerTimestamp>(null, ref t1);
            }
    }
}
#endif