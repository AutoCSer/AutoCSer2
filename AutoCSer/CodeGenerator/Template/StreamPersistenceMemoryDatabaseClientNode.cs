using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseClientNode : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument client node interface
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(@CurrentType.GenericDefinitionFullName))]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition/*IF:IsCustomServiceNode*/ : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode/*IF:IsCustomServiceNode*/
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
            @MethodReturnType.FullName @MethodName(/*PUSH:ReturnRequestParameterType*/@FullName returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:CallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName callback/*PUSH:CallbackType*//*PUSH:KeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName keepCallback/*PUSH:KeepCallbackType*/);
            #endregion IF Method
            #endregion LOOP Methods
        }
        #region IF IsReturnValueNode
        /// <summary>
        /// Get the direct return value API encapsulation (@CurrentType.XmlFullName)
        /// </summary>
        public sealed partial class @ReturnValueNodeTypeName/*IF:IsGenericTypeDefinition*/<@ReturnValueNodeCurrentType>/*IF:IsGenericTypeDefinition*/ : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientReturnValueNode<@ClientNodeTypeName>
        {
            /// <summary>
            /// Get the direct return value API encapsulation (@CurrentType.XmlFullName)
            /// </summary>
            /// <param name="node">Log stream persistence memory database client node cache for client singleton</param>
            /// <param name="isIgnoreError">A default value of false indicates that exceptions and error messages are not ignored</param>
            /// <param name="isSynchronousCallback">The default value of false indicates that the IO thread synchronization callback is not used; otherwise, the subsequent operations of the API call await are not allowed to have synchronization blocking logic or long-term CPU occupation operations</param>
            public @ReturnValueNodeTypeName(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<@ClientNodeTypeName> node, bool isIgnoreError = false, bool isSynchronousCallback = false) : base(node, isIgnoreError, isSynchronousCallback) { }
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
            public @ReturnValueMethodReturnType.FullName @MethodName(/*PUSH:ReturnRequestParameterType*/@FullName returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*/)
            {
                return base.node.@MethodName(/*PUSH:ReturnRequestParameterType*/returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*/)/*IF:IsGetReturnValue*/.GetValue(isIgnoreError)/*IF:IsGetReturnValue*/;
            }
            #endregion IF IsSynchronous
            #region NOT IsSynchronous
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @ReturnValueMethodReturnType.FullName @MethodName(/*PUSH:ReturnRequestParameterType*/@FullName returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName callback/*PUSH:ReturnValueCallbackType*//*PUSH:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName keepCallback/*PUSH:ReturnValueKeepCallbackType*/)
            {
                return base.node.@MethodName(/*PUSH:ReturnRequestParameterType*/returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*//*IF:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/new @CallbackReturnValueType.FullName(callback)/*IF:ReturnValueCallbackType*//*IF:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/new @KeepCallbackReturnValueType.FullName(keepCallback)/*IF:ReturnValueKeepCallbackType*/);
            }
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @ReturnValueMethodReturnType.FullName @MethodName(/*PUSH:ReturnRequestParameterType*/@FullName returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName callback, Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> @ErrorCallbackParameterName/*PUSH:ReturnValueCallbackType*//*PUSH:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName keepCallback, Action<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult> @ErrorKeepCallbackParameterName/*PUSH:ReturnValueKeepCallbackType*/)
            {
                return base.node.@MethodName(/*PUSH:ReturnRequestParameterType*/returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*//*IF:ReturnValueCallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/new @CallbackReturnValueType.FullName(callback, @ErrorCallbackParameterName)/*IF:ReturnValueCallbackType*//*IF:ReturnValueKeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/new @KeepCallbackReturnValueType.FullName(keepCallback, @ErrorKeepCallbackParameterName)/*IF:ReturnValueKeepCallbackType*/);
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
