//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.CommandClientPerformance
{
        /// <summary>
        /// 
        /// </summary>
        [AutoCSer.Net.CommandClientControllerType(typeof(AwaiterClientController))]
        public partial interface IAwaiterClient { }
        /// <summary>
        ///  客户端控制器
        /// </summary>
        internal unsafe partial class AwaiterClientController : AutoCSer.Net.CommandClientController<AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient, AutoCSer.TestCase.CommandServerPerformance.IService>, IAwaiterClient
        {
            private AwaiterClientController(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames) : base(socket, controllerName, startMethodIndex, serverMethodNames, -2147483648) { }
            internal static AutoCSer.Net.CommandClientController __CommandClientControllerConstructor__(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
            {
                return new AwaiterClientController(socket, controllerName, startMethodIndex, serverMethodNames);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip8__
            {
                internal int left;
                internal int right;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip8__ value)
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
                AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip8__ value = default(AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip8__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip8__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __op8__
            {
                internal int ReturnValue;
                private static int getReturnValue(__op8__ parameter)
                {
                    return parameter.ReturnValue;
                }
                internal static readonly Func<__op8__, int> GetReturnValue = getReturnValue;
                
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__op8__ value, byte* end)
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
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__op8__ value = default(AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__op8__);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__op8__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip12__
            {
                internal int left;
                internal int right;
                internal int ReturnValue;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip12__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(12))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.ReturnValue);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.left);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.right);
                }
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip12__ value = default(AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip12__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip12__));
            }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.Synchronous(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(0
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.Callback(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(1
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.Queue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(2
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.ConcurrencyReadQueue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(3
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.ReadWriteQueue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(4
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.Task(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(5
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.SynchronousCallTask(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(6
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.TaskQueue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip8__, int, __op8__>(7
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="queueKey"></param>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.TaskQueueKey(int queueKey, int left, int right)
            {
                __ip12__ __inputParameter__ = new __ip12__
                {
                    left = left,
                    right = right,
                    ReturnValue = queueKey,
                };
                var __returnValue__ = base.ReturnValueOutput<__ip12__, int, __op8__>(8
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.EnumeratorCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.KeepCallback()
            {
                var __returnValue__ = base.Enumerator<int, __op8__>(9
                    , __op8__/**/.GetReturnValue
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.SendOnlyCommand AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.SendOnly(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.SendOnlyInput<__ip8__>(10
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.EnumeratorCommand<int> AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.KeepCallbackCount()
            {
                var __returnValue__ = base.Enumerator<int, __op8__>(11
                    , __op8__/**/.GetReturnValue
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.SendOnlyCommand AutoCSer.TestCase.CommandClientPerformance.IAwaiterClient/**/.SendOnlyTask(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.SendOnlyInput<__ip8__>(12
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 获取客户端接口方法信息集合
            /// </summary>
            internal static AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> __CommandClientControllerMethods__()
            {
                AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> methods = new AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod>(13);
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "Synchronous", 8, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "Callback", 0, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "Queue", 4, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "ConcurrencyReadQueue", 1, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "ReadWriteQueue", 5, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "Task", 10, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "SynchronousCallTask", 9, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "TaskQueue", 11, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "TaskQueueKey", 12, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "KeepCallback", 2, 0, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "SendOnly", 6, 1, 0, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "KeepCallbackCount", 3, 0, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(IAwaiterClient), "SendOnlyTask", 7, 1, 0, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                return methods;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void __CommandClientControllerConstructor__()
            {
                __CommandClientControllerConstructor__(null, null, 0, null);
                __CommandClientControllerMethods__();
                AutoCSer.AotReflection.Interfaces(typeof(AwaiterClientController));
            }
        }
}namespace AutoCSer.TestCase.CommandClientPerformance
{
        /// <summary>
        /// 
        /// </summary>
        [AutoCSer.Net.CommandClientControllerType(typeof(CallbackClientController))]
        public partial interface ICallbackClient { }
        /// <summary>
        ///  客户端控制器
        /// </summary>
        internal unsafe partial class CallbackClientController : AutoCSer.Net.CommandClientController<AutoCSer.TestCase.CommandClientPerformance.ICallbackClient, AutoCSer.TestCase.CommandServerPerformance.IService>, ICallbackClient
        {
            private CallbackClientController(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames) : base(socket, controllerName, startMethodIndex, serverMethodNames, -2147483648) { }
            internal static AutoCSer.Net.CommandClientController __CommandClientControllerConstructor__(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
            {
                return new CallbackClientController(socket, controllerName, startMethodIndex, serverMethodNames);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip8__
            {
                internal int left;
                internal int right;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip8__ value)
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
                AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip8__ value = default(AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip8__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip8__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __op8__
            {
                internal int ReturnValue;
                private static int getReturnValue(__op8__ parameter)
                {
                    return parameter.ReturnValue;
                }
                internal static readonly Func<__op8__, int> GetReturnValue = getReturnValue;
                
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__op8__ value, byte* end)
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
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__op8__ value = default(AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__op8__);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__op8__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip12__
            {
                internal int left;
                internal int right;
                internal int ReturnValue;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip12__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(12))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.ReturnValue);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.left);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.right);
                }
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip12__ value = default(AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip12__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip12__));
            }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.Synchronous(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(0
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.Callback(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(1
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.Queue(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(2
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.ConcurrencyReadQueue(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(3
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.ReadWriteQueue(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(4
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.Task(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(5
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.SynchronousCallTask(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(6
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.TaskQueue(int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.CallbackOutput<__ip8__, int, __op8__>(7
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="queueKey"></param>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.CallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.TaskQueueKey(int queueKey, int left, int right, System.Action<AutoCSer.Net.CommandClientReturnValue<int>> callback)
            {
                __ip12__ __inputParameter__ = new __ip12__
                {
                    left = left,
                    right = right,
                    ReturnValue = queueKey,
                };
                var __returnValue__ = base.CallbackOutput<__ip12__, int, __op8__>(8
                    , AutoCSer.Net.CommandClientCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.KeepCallback(System.Action<AutoCSer.Net.CommandClientReturnValue<int>,AutoCSer.Net.KeepCallbackCommand> callback)
            {
                var __returnValue__ = base.KeepCallback<int, __op8__>(9
                    , AutoCSer.Net.CommandClientKeepCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.SendOnlyCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.SendOnly(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.SendOnlyInput<__ip8__>(10
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="callback"></param>
            /// <returns></returns>
            AutoCSer.Net.KeepCallbackCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.KeepCallbackCount(System.Action<AutoCSer.Net.CommandClientReturnValue<int>,AutoCSer.Net.KeepCallbackCommand> callback)
            {
                var __returnValue__ = base.KeepCallback<int, __op8__>(11
                    , AutoCSer.Net.CommandClientKeepCallback<int>/**/.Get(callback)
                    , __op8__/**/.GetReturnValue
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.SendOnlyCommand AutoCSer.TestCase.CommandClientPerformance.ICallbackClient/**/.SendOnlyTask(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                var __returnValue__ = base.SendOnlyInput<__ip8__>(12
                    , ref __inputParameter__
                    );
                return __returnValue__;
            }
            /// <summary>
            /// 获取客户端接口方法信息集合
            /// </summary>
            internal static AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> __CommandClientControllerMethods__()
            {
                AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> methods = new AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod>(13);
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "Synchronous", 8, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "Callback", 0, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "Queue", 4, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "ConcurrencyReadQueue", 1, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "ReadWriteQueue", 5, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "Task", 10, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "SynchronousCallTask", 9, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "TaskQueue", 11, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "TaskQueueKey", 12, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "KeepCallback", 2, 0, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "SendOnly", 6, 1, 0, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "KeepCallbackCount", 3, 0, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ICallbackClient), "SendOnlyTask", 7, 1, 0, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                return methods;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void __CommandClientControllerConstructor__()
            {
                __CommandClientControllerConstructor__(null, null, 0, null);
                __CommandClientControllerMethods__();
                AutoCSer.AotReflection.Interfaces(typeof(CallbackClientController));
            }
        }
}namespace AutoCSer.TestCase.CommandClientPerformance
{
        /// <summary>
        /// 
        /// </summary>
        [AutoCSer.Net.CommandClientControllerType(typeof(SynchronousCllientClientController))]
        public partial interface ISynchronousCllient { }
        /// <summary>
        ///  客户端控制器
        /// </summary>
        internal unsafe partial class SynchronousCllientClientController : AutoCSer.Net.CommandClientController<AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient, AutoCSer.TestCase.CommandServerPerformance.IService>, ISynchronousCllient
        {
            private SynchronousCllientClientController(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames) : base(socket, controllerName, startMethodIndex, serverMethodNames, -2147483648) { }
            internal static AutoCSer.Net.CommandClientController __CommandClientControllerConstructor__(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
            {
                return new SynchronousCllientClientController(socket, controllerName, startMethodIndex, serverMethodNames);
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip8__
            {
                internal int left;
                internal int right;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip8__ value)
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
                AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip8__ value = default(AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip8__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip8__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __op8__
            {
                internal int ReturnValue;
                private static int getReturnValue(__op8__ parameter)
                {
                    return parameter.ReturnValue;
                }
                internal static readonly Func<__op8__, int> GetReturnValue = getReturnValue;
                
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__op8__ value, byte* end)
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
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__op8__ value = default(AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__op8__);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__op8__));
            }
            }
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct __ip12__
            {
                internal int left;
                internal int right;
                internal int ReturnValue;
                
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip12__ value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(12))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.ReturnValue);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.left);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.right);
                }
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip12__ value = default(AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip12__);
                SimpleSerialize(null, ref value);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip12__));
            }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.Synchronous(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(0
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.Callback(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(1
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.Queue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(2
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.ConcurrencyReadQueue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(3
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.ReadWriteQueue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(4
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.Task(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(5
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.SynchronousCallTask(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(6
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.TaskQueue(int left, int right)
            {
                __ip8__ __inputParameter__ = new __ip8__
                {
                    left = left,
                    right = right,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip8__, __op8__>(7
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="queueKey"></param>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            AutoCSer.Net.CommandClientReturnValue<int> AutoCSer.TestCase.CommandClientPerformance.ISynchronousCllient/**/.TaskQueueKey(int queueKey, int left, int right)
            {
                __ip12__ __inputParameter__ = new __ip12__
                {
                    left = left,
                    right = right,
                    ReturnValue = queueKey,
                };
                __op8__ __outputParameter__ = new __op8__
                {
                };
                var __returnValue__ = base.SynchronousInputOutput<__ip12__, __op8__>(8
                    , ref __inputParameter__
                    , ref __outputParameter__
                    );
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                return __returnValue__;
            }
            /// <summary>
            /// 获取客户端接口方法信息集合
            /// </summary>
            internal static AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> __CommandClientControllerMethods__()
            {
                AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> methods = new AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod>(9);
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "Synchronous", 8, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "Callback", 0, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "Queue", 4, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "ConcurrencyReadQueue", 1, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "ReadWriteQueue", 5, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "Task", 10, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "SynchronousCallTask", 9, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "TaskQueue", 11, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(ISynchronousCllient), "TaskQueueKey", 12, 1, 1, AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.CheckRunTask, 0, 0, 0));
                return methods;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void __CommandClientControllerConstructor__()
            {
                __CommandClientControllerConstructor__(null, null, 0, null);
                __CommandClientControllerMethods__();
                AutoCSer.AotReflection.Interfaces(typeof(SynchronousCllientClientController));
            }
        }
}namespace AutoCSer.TestCase.CommandClientPerformance
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
                    AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip8__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__op8__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__ip12__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.AwaiterClientController.__CommandClientControllerConstructor__();
                    AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip8__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__op8__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__ip12__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.CallbackClientController.__CommandClientControllerConstructor__();
                    AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip8__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__op8__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__ip12__.SimpleSerialize();
                    AutoCSer.TestCase.CommandClientPerformance.SynchronousCllientClientController.__CommandClientControllerConstructor__();




                    return true;
                }
                return false;
            }
    }
}
#endif