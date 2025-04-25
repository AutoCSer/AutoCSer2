//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.TimestampVerifyClient
{
        [AutoCSer.Net.CommandClientControllerType(typeof(TimestampVerifyClient))]
        public partial interface ITimestampVerifyClient { }
        /// <summary>
        ///  客户端控制器
        /// </summary>
        internal unsafe partial class TimestampVerifyClient : AutoCSer.Net.CommandClientController<AutoCSer.TestCase.TimestampVerifyClient.ITimestampVerifyClient, AutoCSer.TestCase.TimestampVerify.ITimestampVerify>, ITimestampVerifyClient
        {
            private TimestampVerifyClient(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames) : base(socket, controllerName, startMethodIndex, serverMethodNames, 0) { }
            internal static AutoCSer.Net.CommandClientController __CommandClientControllerConstructor__(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
            {
                return new TimestampVerifyClient(socket, controllerName, startMethodIndex, serverMethodNames);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __pi1__
            {
                internal int left;
                internal int right;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi1__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(8))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.left);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.right);
                }
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi1__ value = default(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi1__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi1__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __po1__
            {
                internal int ReturnValue;
                private static int getReturnValue(__po1__ parameter)
                {
                    return parameter.ReturnValue;
                }
                internal static readonly Func<__po1__, int> GetReturnValue = getReturnValue;
                
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po1__ value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.ReturnValue);
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po1__ value = default(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po1__);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po1__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __pi0__
            {
                internal ulong randomPrefix;
                internal byte[] hashData;
                internal long timestamp;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi0__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(20))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.randomPrefix);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.timestamp);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.hashData);
                }
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi0__ value = default(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi0__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi0__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __po0__
            {
                internal long timestamp;
                internal AutoCSer.Net.CommandServerVerifyStateEnum ReturnValue;
                
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po0__ value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.timestamp);
                byte ReturnValue = 0;
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref ReturnValue);
                this.ReturnValue = (AutoCSer.Net.CommandServerVerifyStateEnum)ReturnValue;
                __start__ += 3;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po0__ value = default(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po0__);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po0__));
            }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.TimestampVerifyClient.ITimestampVerifyClient/**/.Add(int left, int right)
            {
                __pi1__ __inputParameter__ = new __pi1__
                {
                    left = left,
                    right = right,
                };
                __po1__ __outputParameter__ = new __po1__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__pi1__, __po1__>(0
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 验证函数
            /// </summary>
            /// <param name="randomPrefix">随机前缀</param>
            /// <param name="hashData">验证 Hash 数据</param>
            /// <param name="timestamp">待验证时间戳</param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<AutoCSer.Net.CommandServerVerifyStateEnum> AutoCSer.CommandService.ITimestampVerifyClient/**/.Verify(ulong randomPrefix, byte[] hashData, ref long timestamp)
            {
                __pi0__ __inputParameter__ = new __pi0__
                {
                    randomPrefix = randomPrefix,
                    hashData = hashData,
                    timestamp = timestamp,
                };
                __po0__ __outputParameter__ = new __po0__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__pi0__, __po0__>(1
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                timestamp = __outputParameter__.timestamp;
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 获取客户端接口方法信息集合
            /// </summary>
            internal static AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> __CommandClientControllerMethods__()
            {
                AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> methods = new AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod>(2);
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ITimestampVerifyClient), "Add", 1, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ITimestampVerifyClient), "Verify", 0, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                return methods;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void __CommandClientControllerConstructor__()
            {
                __CommandClientControllerConstructor__(null, null, 0, null);
                __CommandClientControllerMethods__();
                AutoCSer.AotReflection.Interfaces(typeof(TimestampVerifyClient));
            }
        }
}namespace AutoCSer.TestCase.TimestampVerifyClient
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
                    AutoCSer.AotMethod.Call();
                    AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi1__.SimpleSerialize();
                    AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po1__.SimpleSerialize();
                    AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__pi0__.SimpleSerialize();
                    AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__po0__.SimpleSerialize();
                    AutoCSer.TestCase.TimestampVerifyClient.TimestampVerifyClient.__CommandClientControllerConstructor__();



                    return true;
                }
                return false;
            }
    }
}
#endif