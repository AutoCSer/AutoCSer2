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
    /// 流序列化缓存客户端节点
    /// </summary>
    [Generator(Name = "流序列化缓存客户端节点", IsAuto = true)]
    internal partial class StreamPersistenceMemoryDatabaseClientNode : AttributeGenerator<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute>
    {
        /// <summary>
        /// 生成类型名称后缀
        /// </summary>
        protected override string typeNameSuffix { get { return "ClientNode"; } }
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
            /// 接口方法与枚举信息
            /// </summary>
            /// <param name="nodeAttribute"></param>
            /// <param name="interfaceMethod"></param>
            public NodeMethod(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeAttribute nodeAttribute, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod interfaceMethod)
            {
                if (interfaceMethod != null && interfaceMethod.IsClientCall
                    && (interfaceMethod.IsDeclaringMethod || !nodeAttribute.IsClientCodeGeneratorOnlyDeclaringMethod))
                {
                    this.interfaceMethod = interfaceMethod;
                    Method = new MethodIndex(interfaceMethod.Method, AutoCSer.Metadata.MemberFiltersEnum.Instance, interfaceMethod.MethodIndex, interfaceMethod.ParameterStartIndex, interfaceMethod.ParameterEndIndex);
                    switch (interfaceMethod.CallType)
                    {
                        case CallTypeEnum.SendOnly:
                            MethodReturnType = typeof(SendOnlyCommand);
                            break;
                        case CallTypeEnum.KeepCallback:
                        case CallTypeEnum.InputKeepCallback:
                        case CallTypeEnum.Enumerable:
                        case CallTypeEnum.InputEnumerable:
                            MethodReturnType = typeof(Task<>).MakeGenericType(typeof(KeepCallbackResponse<>).MakeGenericType(interfaceMethod.ReturnValueType));
                            break;
                        default:
                            MethodReturnType = interfaceMethod.ReturnValueType == typeof(void) ? typeof(Task<ResponseResult>) : typeof(Task<>).MakeGenericType(typeof(ResponseResult<>).MakeGenericType(interfaceMethod.ReturnValueType));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 节点方法集合
        /// </summary>
        public NodeMethod[] Methods;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            if (!CurrentType.Type.IsInterface) return AutoCSer.Common.CompletedTask;
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType type = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeType(CurrentType.Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethod[] methods = type.Methods;
            if (methods == null || methods.Length == 0) return AutoCSer.Common.CompletedTask;
            Methods = methods.getArray(p => new NodeMethod(type.NodeAttribute, p));

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
