using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseLocalClientNodeInterface : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument local client node interface
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(@CurrentType.GenericDefinitionFullName))]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition/*IF:IsCustomServiceNode*/ : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode/*IF:IsCustomServiceNode*/
        {
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
            @MethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:CallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName __callback__/*PUSH:CallbackType*//*PUSH:KeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName __keepCallback__/*PUSH:KeepCallbackType*/);
            #endregion IF Method
            #endregion LOOP Methods
        }
        #region IF IsReturnValueNode
        /// <summary>
        /// Get the direct return value API encapsulation (@CurrentType.XmlFullName)
        /// </summary>
        public sealed partial class @ReturnValueNodeTypeName/*IF:IsGenericTypeDefinition*/<@ReturnValueNodeCurrentType>/*IF:IsGenericTypeDefinition*/ : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalClientReturnValueNode<@ClientNodeTypeName>
        {
            /// <summary>
            /// Get the direct return value API encapsulation (@CurrentType.XmlFullName)
            /// </summary>
            /// <param name="node">Log stream persistence memory database local client node cache for client singleton</param>
            /// <param name="isIgnoreError">A default value of false indicates that exceptions and error messages are not ignored</param>
            /// <param name="isSynchronousCallback">The default value of false indicates that the IO thread synchronization callback is not used; otherwise, the subsequent operations of the API call await are not allowed to have synchronization blocking logic or long-term CPU occupation operations</param>
            public @ReturnValueNodeTypeName(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseLocalClientNodeCache<@ClientNodeTypeName> node, bool isIgnoreError = false, bool isSynchronousCallback = false) : base(node, isIgnoreError, isSynchronousCallback) { }
            #region LOOP Methods
            #region IF Method
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF IsSynchronous
            #region IF MethodIsReturn
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            #endregion IF MethodIsReturn
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @ReturnValueMethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*/)
            {
                return /*IF:IsGetReturnValue*/base.getReturnValue(/*IF:IsGetReturnValue*/base.node.@MethodName(/*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*/)/*IF:IsGetReturnValue*/)/*IF:IsGetReturnValue*/;
            }
            #endregion IF IsSynchronous
            #region NOT IsSynchronous
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @ReturnValueMethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName callback/*PUSH:ReturnValueCallbackType*//*PUSH:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName keepCallback/*PUSH:ReturnValueKeepCallbackType*/)
            {
                /*IF:IsReturnValue*/
                return /*IF:IsReturnValue*/base.node.@MethodName(/*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*//*IF:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/new @CallbackReturnValueType.FullName(callback)/*IF:ReturnValueCallbackType*//*IF:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/new @KeepCallbackReturnValueType.FullName(keepCallback)/*IF:ReturnValueKeepCallbackType*/);
            }
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @ReturnValueMethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName callback, Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> @ErrorCallbackParameterName/*PUSH:ReturnValueCallbackType*//*PUSH:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName keepCallback, Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalResult> @ErrorKeepCallbackParameterName/*PUSH:ReturnValueKeepCallbackType*/)
            {
                /*IF:IsReturnValue*/
                return /*IF:IsReturnValue*/base.node.@MethodName(/*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*//*IF:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/new @CallbackReturnValueType.FullName(callback, @ErrorCallbackParameterName)/*IF:ReturnValueCallbackType*//*IF:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/new @KeepCallbackReturnValueType.FullName(keepCallback, @ErrorKeepCallbackParameterName)/*IF:ReturnValueKeepCallbackType*/);
            }
            #endregion NOT IsSynchronous
            #endregion IF Method
            #endregion LOOP Methods
        }
        #endregion IF IsReturnValueNode
        #endregion PART CLASS
        internal interface ClientNodeTypeName
        {
            ReturnValueMethodReturnType.FullName MethodName(params object[] values);
        }
    }
}
