using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 流序列化数据库本地客户端节点
    /// </summary>
    [Generator(Name = "流序列化数据库本地客户端节点", IsAuto = true)]
    internal partial class StreamPersistenceMemoryDatabaseLocalClientNode : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
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
            private InterfaceMethodBase interfaceMethod;
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
            public bool MethodIsReturn { get { return interfaceMethod.ReturnValueType != typeof(void); } }
            /// <summary>
            /// 回调委托类型
            /// </summary>
            public ExtensionType KeepCallbackType;
            /// <summary>
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="nodeAttribute"></param>
            /// <param name="interfaceMethod"></param>
            /// <param name="isServiceNode"></param>
            public NodeMethod(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod interfaceMethod, bool isServiceNode)
            {
                if (interfaceMethod != null && interfaceMethod.IsClientCall
                    && (isServiceNode || interfaceMethod.Type != typeof(IServiceNode)))
                {
                    this.interfaceMethod = interfaceMethod;
                    Method = new MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex, interfaceMethod.ParameterStartIndex, interfaceMethod.ParameterEndIndex);
                    switch (interfaceMethod.CallType)
                    {
                        case CallTypeEnum.SendOnly:
                            MethodReturnType = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNodeMethod.LocalClientSendOnlyMethodReturnType;
                            break;
                        case CallTypeEnum.KeepCallback:
                        case CallTypeEnum.InputKeepCallback:
                        case CallTypeEnum.Enumerable:
                        case CallTypeEnum.InputEnumerable:
                            if (interfaceMethod.MethodAttribute.IsKeepCallbackCommand)
                            {
                                KeepCallbackType = typeof(Action<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(interfaceMethod.ReturnValueType));
                                MethodReturnType = typeof(LocalServiceQueueNode<IDisposable>);
                            }
                            else MethodReturnType = typeof(LocalServiceQueueNode<>).MakeGenericType(typeof(LocalKeepCallback<>).MakeGenericType(interfaceMethod.ReturnValueType));
                            break;
                        default:
                            MethodReturnType = interfaceMethod.ReturnValueType == typeof(void) ? typeof(LocalServiceQueueNode<LocalResult>) : typeof(LocalServiceQueueNode<>).MakeGenericType(typeof(LocalResult<>).MakeGenericType(interfaceMethod.ReturnValueType));
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
            if (!CurrentAttribute.IsLocalClient || !CurrentType.Type.IsInterface) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType type = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(CurrentType.Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = type.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            Methods = methods.getArray(p => new NodeMethod(type.NodeAttribute, p, CurrentType.Type == typeof(IServiceNode)));

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
