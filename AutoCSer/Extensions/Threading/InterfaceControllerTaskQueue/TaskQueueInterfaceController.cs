using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    internal static class TaskQueueInterfaceController
    {
        /// <summary>
        /// object 构造函数信息
        /// </summary>
        internal static readonly ConstructorInfo ObjectConstructorInfo = typeof(object).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyArray<Type>.Array , null).notNull();
        /// <summary>
        /// 接口任务队列节点构造函数参数集合信息
        /// </summary>
        internal static readonly Type[] NodeConstructorParameterTypes = new Type[] { typeof(ClientCallbackTypeEnum) };
        /// <summary>
        /// 接口任务队列节点构造函数信息
        /// </summary>
        internal static readonly ConstructorInfo NodeConstructorInfo = typeof(InterfaceControllerTaskQueueNode).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, NodeConstructorParameterTypes, null).notNull();
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        internal static readonly Action<InterfaceControllerTaskQueue, InterfaceControllerTaskQueueNodeBase> InterfaceControllerTaskQueueAdd = InterfaceControllerTaskQueue.Add;
        /// <summary>
        /// Set the return value
        /// 设置返回值
        /// </summary>
        internal static readonly Action<InterfaceControllerTaskQueueNode> InterfaceControllerTaskQueueNodeSetReturnType = InterfaceControllerTaskQueueNode.SetReturnType;

        /// <summary>
        /// 任务节点类型序号
        /// </summary>
        private static int nodeTypeIndex;
        /// <summary>
        /// 获取任务节点类型序号
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static string GetNodeTypeName()
        {
            return AutoCSer.Common.NamePrefix + ".Threading.InterfaceControllerTaskQueueNode" + Interlocked.Increment(ref nodeTypeIndex).toString();
        }
    }
    /// <summary>
    /// 控制器接口信息
    /// </summary>
    /// <typeparam name="T">调用接口类型</typeparam>
    /// <typeparam name="ST">服务实现实例</typeparam>
    internal static class TaskQueueInterfaceController<T, ST> where ST : class
    {
        /// <summary>
        /// 创建接口任务队列控制器
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T Create(InterfaceControllerTaskQueue queue, ST service)
        {
            if (controllerConstructorException == null) return callConstructor(queue, service);
            throw controllerConstructorException;
        }
        /// <summary>
        /// 控制器构造函数
        /// </summary>
        private static readonly Func<InterfaceControllerTaskQueue, ST , T> callConstructor;
        /// <summary>
        /// 控制器构造错误
        /// </summary>
#if NetStandard21
        private static readonly Exception? controllerConstructorException;
#else
        private static readonly Exception controllerConstructorException;
#endif
#pragma warning disable CS8618
        static TaskQueueInterfaceController()
#pragma warning restore CS8618
        {
            Type type = typeof(T), serviceType = typeof(ST);
            try
            {
                var error = default(string);
                LeftArray<TaskQueueInterfaceControllerMethod> methodArray = new LeftArray<TaskQueueInterfaceControllerMethod>(0);
                if (InterfaceController.CheckType(type, false, out error))
                {
                    Dictionary<TaskQueueInterfaceControllerMatchMethod, MethodInfo> serviceMethods = DictionaryCreator<TaskQueueInterfaceControllerMatchMethod>.Create<MethodInfo>();
                    foreach (MethodInfo method in serviceType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        serviceMethods.Add(new TaskQueueInterfaceControllerMatchMethod(method), method);
                    }
                    error = TaskQueueInterfaceControllerMethod.GetMethod(type, serviceMethods, ref methodArray);
                    if (error == null)
                    {
                        foreach (Type interfaceType in type.GetInterfaces())
                        {
                            error = TaskQueueInterfaceControllerMethod.GetMethod(interfaceType, serviceMethods, ref methodArray);
                            if (error != null) break;
                        }
                        if (error == null && methodArray.Length == 0) error = $"没有找到接口方法定义 {type.fullName()}";
                    }
                }
                if (error != null)
                {
                    controllerConstructorException = new Exception($"{type.fullName()} 接口任务队列控制器生成失败 {error}");
                    return;
                }

                Type[] constructorParameterTypes = new Type[] { typeof(InterfaceControllerTaskQueue), serviceType };
                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".Threading.TaskQueueInterfaceController." + type.FullName + "." + serviceType.FullName, TypeAttributes.Class | TypeAttributes.Sealed, typeof(object), new Type[] { type });
                FieldBuilder queueField = typeBuilder.DefineField("queue", typeof(InterfaceControllerTaskQueue), FieldAttributes.Public | FieldAttributes.InitOnly);
                FieldBuilder controllerField = typeBuilder.DefineField("controller", serviceType, FieldAttributes.Public | FieldAttributes.InitOnly);
                #region 构造函数
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                #region base()
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Call, TaskQueueInterfaceController.ObjectConstructorInfo);
                #endregion
                #region this.queue = queue;
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Stfld, queueField);
                #endregion
                #region this.controller = controller;
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Stfld, controllerField);
                #endregion
                constructorGenerator.ret();
                #endregion
                LeftArray<Type> nodeConstructorParameterTypes = new LeftArray<Type>(4);
                nodeConstructorParameterTypes.Add(typeof(InterfaceControllerTaskQueue));
                nodeConstructorParameterTypes.Add(serviceType);
                LeftArray<FieldBuilder> parameterFieldBuilders = new LeftArray<FieldBuilder>(4);
                foreach (TaskQueueInterfaceControllerMethod method in methodArray)
                {
                    TypeBuilder nodeTypeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(TaskQueueInterfaceController.GetNodeTypeName(), TypeAttributes.Class | TypeAttributes.Sealed, method.Method.ReturnType);
                    FieldBuilder nodeControllerField = nodeTypeBuilder.DefineField("__controller__", serviceType, FieldAttributes.Public | FieldAttributes.InitOnly);
                    nodeConstructorParameterTypes.Length = 2;
                    parameterFieldBuilders.Length = 0;
                    foreach (ParameterInfo parameter in method.Parameters)
                    {
                        nodeConstructorParameterTypes.Add(parameter.ParameterType);
                        parameterFieldBuilders.Add(nodeTypeBuilder.DefineField(parameter.Name.notNull(), parameter.ParameterType, FieldAttributes.Public | FieldAttributes.InitOnly));
                    }
                    #region 构造函数
                    ConstructorBuilder nodeConstructorBuilder = nodeTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, nodeConstructorParameterTypes.ToArray());
                    constructorGenerator = nodeConstructorBuilder.GetILGenerator();
                    #region base(ClientCallbackTypeEnum.RunTask)
                    constructorGenerator.Emit(OpCodes.Ldarg_0);
                    constructorGenerator.int32((byte)method.MethodAttribute.CallbackType);
                    if (method.Method.ReturnType == typeof(InterfaceControllerTaskQueueNode))
                    {
                        constructorGenerator.Emit(OpCodes.Call, TaskQueueInterfaceController.NodeConstructorInfo);
                    }
                    else
                    {
                        constructorGenerator.Emit(OpCodes.Call, method.Method.ReturnType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, TaskQueueInterfaceController.NodeConstructorParameterTypes, null).notNull());
                    }
                    #endregion
                    #region this.controller = controller;
                    constructorGenerator.Emit(OpCodes.Ldarg_0);
                    constructorGenerator.Emit(OpCodes.Ldarg_2);
                    constructorGenerator.Emit(OpCodes.Stfld, nodeControllerField);
                    #endregion
                    #region this.parameter = parameter;
                    int parameterIndex = 0;
                    foreach (FieldBuilder parameterField in parameterFieldBuilders)
                    {
                        constructorGenerator.Emit(OpCodes.Ldarg_0);
                        constructorGenerator.ldarg(parameterIndex + 3);
                        constructorGenerator.Emit(OpCodes.Stfld, parameterField);
                        ++parameterIndex;
                    }
                    #endregion
                    #region InterfaceControllerTaskQueue.Add(queue, this);
                    constructorGenerator.Emit(OpCodes.Ldarg_1);
                    constructorGenerator.Emit(OpCodes.Ldarg_0);
                    constructorGenerator.call(TaskQueueInterfaceController.InterfaceControllerTaskQueueAdd.Method);
                    #endregion
                    constructorGenerator.ret();
                    #endregion
                    #region public override void RunTask()
                    MethodBuilder methodBuilder = nodeTypeBuilder.DefineMethod(nameof(InterfaceControllerTaskQueueNodeBase.RunTask), MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot, typeof(void), EmptyArray<Type>.Array);
                    ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                    #endregion
                    if (method.Method.ReturnType != typeof(InterfaceControllerTaskQueueNode)) methodGenerator.Emit(OpCodes.Ldarg_0);
                    #region controller.Call(value);
                    methodGenerator.Emit(OpCodes.Ldarg_0);
                    methodGenerator.Emit(OpCodes.Ldfld, nodeControllerField);
                    foreach (FieldBuilder parameterField in parameterFieldBuilders)
                    {
                        methodGenerator.Emit(OpCodes.Ldarg_0);
                        methodGenerator.Emit(OpCodes.Ldfld, parameterField);
                    }
                    methodGenerator.call(method.ServiceMethod);
                    #endregion
                    #region InterfaceControllerTaskQueueNode.SetReturnType(this);
                    if (method.Method.ReturnType == typeof(InterfaceControllerTaskQueueNode))
                    {
                        methodGenerator.Emit(OpCodes.Ldarg_0);
                        methodGenerator.call(TaskQueueInterfaceController.InterfaceControllerTaskQueueNodeSetReturnType.Method);
                    }
                    else
                    {
                        methodGenerator.call(AutoCSer.Extensions.Metadata.GenericType.Get(method.ServiceMethod.ReturnType).InterfaceControllerTaskQueueNodeSetReturn.Method);
                    }
                    methodGenerator.ret();
                    #endregion
                    nodeTypeBuilder.CreateType();

                    methodBuilder = typeBuilder.DefineMethod(method.Method.Name, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, method.Method.ReturnType, method.Parameters.getArray(parameter => parameter.ParameterType));
                    typeBuilder.DefineMethodOverride(methodBuilder, method.Method);
                    methodGenerator = methodBuilder.GetILGenerator();
                    #region return new TaskQueueReturnIL(queue, controller, value);
                    methodGenerator.Emit(OpCodes.Ldarg_0);
                    methodGenerator.Emit(OpCodes.Ldfld, queueField);
                    methodGenerator.Emit(OpCodes.Ldarg_0);
                    methodGenerator.Emit(OpCodes.Ldfld, controllerField);
                    for (parameterIndex = 0; parameterIndex != method.Parameters.Length; methodGenerator.ldarg(++parameterIndex)) ;
                    methodGenerator.Emit(OpCodes.Newobj, nodeConstructorBuilder);
                    methodGenerator.ret();
                    #endregion
                }
                Type controllerType = typeBuilder.CreateType();

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "CallConstructor", type, constructorParameterTypes, controllerType, true);
                ILGenerator callConstructorGenerator = dynamicMethod.GetILGenerator();
                callConstructorGenerator.Emit(OpCodes.Ldarg_0);
                callConstructorGenerator.Emit(OpCodes.Ldarg_1);
                callConstructorGenerator.Emit(OpCodes.Newobj, controllerType.GetConstructor(constructorParameterTypes).notNull());
                callConstructorGenerator.ret();
                callConstructor = (Func<InterfaceControllerTaskQueue, ST, T>)dynamicMethod.CreateDelegate(typeof(Func<InterfaceControllerTaskQueue, ST, T>));

            }
            catch (Exception exception)
            {
                controllerConstructorException = new Exception($"{serviceType.fullName()} 接口任务队列控制器 {type.fullName()} 生成失败", exception);
            }
        }
    }
#if DEBUG && NetStandard21
    internal interface ITaskQueueInterfaceControllerIL
    {
        InterfaceControllerTaskQueueNode Call(int value);
        InterfaceControllerTaskQueueNode<string> Return(int value);
    }
    internal sealed class TaskQueueInterfaceControllerObject
    {
        public void Call(int value) { }
        public string Return(int value) { return string.Empty; }
    }
    internal sealed class TaskQueueInterfaceControllerIL : ITaskQueueInterfaceControllerIL
    {
        private readonly InterfaceControllerTaskQueue queue;
        private readonly TaskQueueInterfaceControllerObject controller;
        public TaskQueueInterfaceControllerIL(InterfaceControllerTaskQueue queue, TaskQueueInterfaceControllerObject controller)
        {
            this.queue = queue;
            this.controller = controller;
        }
        public InterfaceControllerTaskQueueNode Call(int value)
        {
            return new TaskQueueCallIL(queue, controller, value);
        }
        public InterfaceControllerTaskQueueNode<string> Return(int value)
        {
            return new TaskQueueReturnIL(queue, controller, value);
        }
        internal static ITaskQueueInterfaceControllerIL CallConstructor(InterfaceControllerTaskQueue queue, TaskQueueInterfaceControllerObject controller)
        {
            return new TaskQueueInterfaceControllerIL(queue, controller);
        }
    }
    internal sealed class TaskQueueCallIL : InterfaceControllerTaskQueueNode
    {
        private readonly TaskQueueInterfaceControllerObject controller;
        private readonly int value;
        public TaskQueueCallIL(InterfaceControllerTaskQueue queue, TaskQueueInterfaceControllerObject controller, int value) : base(ClientCallbackTypeEnum.RunTask)
        {
            this.controller = controller;
            this.value = value;
            InterfaceControllerTaskQueue.Add(queue, this);
        }
        public override void RunTask()
        {
            controller.Call(value);
            InterfaceControllerTaskQueueNode.SetReturnType(this);
        }
    }
    internal sealed class TaskQueueReturnIL : InterfaceControllerTaskQueueNode<string>
    {
        private readonly TaskQueueInterfaceControllerObject controller;
        private readonly int value;
        public TaskQueueReturnIL(InterfaceControllerTaskQueue queue, TaskQueueInterfaceControllerObject controller, int value) : base(ClientCallbackTypeEnum.RunTask)
        {
            this.controller = controller;
            this.value = value;
            InterfaceControllerTaskQueue.Add(queue, this);
        }
        public override void RunTask()
        {
            InterfaceControllerTaskQueueNode<string>.SetReturn(this, controller.Return(value));
        }
    }
#endif
}
