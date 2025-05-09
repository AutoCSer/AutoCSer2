using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 流序列化数据库本地客户端节点
    /// </summary>
    [Generator(Name = "流序列化数据库本地客户端节点", IsAuto = true)]
    internal partial class StreamPersistenceMemoryDatabaseLocalClientNodeInterface : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
    {
        /// <summary>
        /// 生成类型名称后缀
        /// </summary>
        protected override string typeNameSuffix { get { return "LocalClientNode"; } }
        /// <summary>
        /// 输出类定义开始段代码是否包含当前类型
        /// </summary>
        protected override bool isStartClass { get { return false; } }
        /// <summary>
        /// 节点方法信息
        /// </summary>
        public sealed class NodeMethod
        {
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod ServerNodeMethod;
            /// <summary>
            /// 接口方法信息
            /// </summary>
            public MethodIndex Method;
            /// <summary>
            /// 方法名称
            /// </summary>
            public string MethodName { get { return Method.MethodName; } }
            /// <summary>
            /// 返回值类型
            /// </summary>
            public ExtensionType MethodReturnType;
            /// <summary>
            /// 是否存在返回值
            /// </summary>
            public bool MethodIsReturn { get { return ServerNodeMethod.ReturnValueType != typeof(void); } }
            /// <summary>
            /// 回调委托类型
            /// </summary>
            public ExtensionType CallbackType;
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="nodeAttribute"></param>
            /// <param name="method"></param>
            /// <param name="isServiceNode"></param>
            public NodeMethod(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod method, bool isServiceNode)
            {
                if (method != null && method.IsClientCall
                    && (isServiceNode || method.Type != typeof(IServiceNode)))
                {
                    this.ServerNodeMethod = method;
                    Method = new MethodIndex(method.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, method.MethodIndex, method.ParameterStartIndex, method.ParameterEndIndex);
                    switch (method.CallType)
                    {
                        case CallTypeEnum.SendOnly:
                            MethodReturnType = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNodeMethod.LocalClientSendOnlyMethodReturnType;
                            break;
                        case CallTypeEnum.KeepCallback:
                        case CallTypeEnum.InputKeepCallback:
                        case CallTypeEnum.Enumerable:
                        case CallTypeEnum.InputEnumerable:
                            if (method.MethodAttribute.IsCallbackClient)
                            {
                                CallbackType = typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
                                MethodReturnType = typeof(LocalServiceQueueNode<IDisposable>);
                            }
                            else MethodReturnType = typeof(LocalServiceQueueNode<>).MakeGenericType(typeof(LocalKeepCallback<>).MakeGenericType(method.ReturnValueType));
                            break;
                        default:
                            if (method.MethodAttribute.IsCallbackClient)
                            {
                                CallbackType = method.ReturnValueType == typeof(void) ? typeof(Action<LocalResult>) : typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
                                MethodReturnType = typeof(void);
                            }
                            else MethodReturnType = method.ReturnValueType == typeof(void) ? typeof(LocalServiceQueueNode<LocalResult>) : typeof(LocalServiceQueueNode<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(method.ReturnValueType));
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 是否自定义基础服务节点
        /// </summary>
        public bool IsCustomServiceNode { get { return CurrentType.Type != typeof(IServiceNode) && typeof(IServiceNode).IsAssignableFrom(CurrentType.Type); } }
        /// <summary>
        /// 节点方法集合
        /// </summary>
        public NodeMethod[] Methods;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            if (!CurrentAttribute.IsLocalClient || !type.IsInterface) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType nodeType = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = nodeType.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            Methods = methods.getArray(p => new NodeMethod(nodeType.NodeAttribute, p, type == typeof(IServiceNode)));
            create(true);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
