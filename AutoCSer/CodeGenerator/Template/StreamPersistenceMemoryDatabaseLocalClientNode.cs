using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseLocalClientNode : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument 本地客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(@CurrentType.GenericDefinitionFullName), typeof(@TypeName))]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition/*IF:IsCustomServiceNode*/ : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode/*IF:IsCustomServiceNode*/
        {
            #region LOOP Methods
            #region IF IsInterfaceMethod
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF MethodIsReturn
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            #endregion IF MethodIsReturn
            @MethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:CallbackType*//*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*/@FullName __callback__/*PUSH:CallbackType*/);
            #endregion IF IsInterfaceMethod
            #endregion LOOP Methods
        }
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument 本地客户端节点
        /// </summary>
        internal unsafe partial class @TypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientNode<@InterfaceTypeName>, @InterfaceTypeName/*NOTE*/, MethodInterfaceTypeName/*NOTE*/
        {
            /// <summary>
            /// 本地客户端节点
            /// </summary>
            /// <param name="key">节点全局关键字</param>
            /// <param name="creator">创建节点操作对象委托</param>
            /// <param name="client">日志流持久化内存数据库客户端</param>
            /// <param name="index">节点索引信息</param>
            /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
            private @TypeName(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
                : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            internal static @InterfaceTypeName @LocalClientNodeConstructorMethodName(string key, Func<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex, string, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalServiceQueueNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex>>> creator, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClient client, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new @TypeName(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
            #region LOOP ParameterTypes
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct @ParameterTypeName
            {
                #region LOOP Parameters
                internal @ParameterType.FullName @ParameterName;
                #endregion LOOP Parameters
                /*AT:SerializeCode*/
            }
            #endregion LOOP ParameterTypes
            #region LOOP Methods
            #region IF Method
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF MethodIsReturn
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            #endregion IF MethodIsReturn
            @MethodReturnType.FullName @MethodInterfaceTypeName/**/.@MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:CallbackType*//*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*/@FullName __callback__/*PUSH:CallbackType*/)
            {
                /*IF:IsMethodReturnType*/
                return /*IF:IsMethodReturnType*/@ClientType.FullName/**/.Create/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(this, @MethodIndex
                #region PUSH InputParameterType
                    , new @ParameterTypeName
                    {
                        #region LOOP Parameters
                        @ParameterName = @ParameterName,
                        #endregion LOOP Parameters
                    }
                    #endregion PUSH InputParameterType
                    #region IF CallbackType
                    , __callback__
                #endregion IF CallbackType
                #region IF IsReadWriteNodeParameter
                    , @IsReadWriteNode
                #endregion IF IsReadWriteNodeParameter
                    );
            }

            #endregion IF Method
            #endregion LOOP Methods
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void @LocalClientNodeMethodName()
            {
                @LocalClientNodeConstructorMethodName(null, null, null, default(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex), false);
                AutoCSer.AotReflection.NonPublicFields(typeof(@MethodIndexEnumTypeName));
                AutoCSer.AotReflection.NonPublicMethods(typeof(@TypeName));
                AutoCSer.AotReflection.Interfaces(typeof(@TypeName));
            }
        }
        #endregion PART CLASS
        internal const int MethodIndex = 0;
        internal const bool IsReadWriteNode = false;
        internal const string CallbackParameterName = null;
        private static readonly ParameterType.FullName ParameterName = null;
        public interface InterfaceTypeName { }
        public interface MethodInterfaceTypeName
        {
            MethodReturnType.FullName MethodName(ParameterType.FullName ParameterJoinName, FullName __callback__);
        }
    }
}
