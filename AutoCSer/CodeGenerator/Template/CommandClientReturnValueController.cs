using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class CommandClientReturnValueController : Pub
    {
        #region PART CLASS
        #region LOOP ControllerInterfaces
        /// <summary>
        /// @Property.CodeGeneratorXmlDocument (Direct return value API encapsulation)
        /// </summary>
        public sealed class @ReturnValueControllerTypeName
        {
            /// <summary>
            /// Command client socket event
            /// </summary>
            private readonly @CurrentType.FullName __client__;
            /// <summary>
            /// Whether errors and exceptions are ignored
            /// </summary>
            private readonly bool __isIgnoreError__;
            /// <summary>
            /// @Property.CodeGeneratorXmlDocument (Direct return value API encapsulation)
            /// </summary>
            /// <param name="client">Command client socket event</param>
            /// <param name="isIgnoreError">Whether errors and exceptions are ignored</param>
            public @ReturnValueControllerTypeName(@CurrentType.FullName client, bool isIgnoreError = false)
            {
                __client__ = client;
                __isIgnoreError__ = isIgnoreError;
            }
            #region LOOP Methods
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF IsSynchronous
            #region IF IsReturnValue
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            #endregion IF IsReturnValue
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @MethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*//*AT:RefOutString*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*/)
            {
                /*IF:IsReturnValue*/
                return /*IF:IsReturnValue*/this.__client__.@MemberName/**/.@MethodName(/*LOOP:Method.Parameters*//*AT:RefOutString*/@ParameterJoinName/*LOOP:Method.Parameters*/)/*IF:IsGetReturnValue*/.GetValue(__isIgnoreError__)/*IF:IsGetReturnValue*/;
            }
            #endregion IF IsSynchronous
            #region NOT IsSynchronous
            #region IF IsReturnValue
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            #endregion IF IsReturnValue
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @MethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:CallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName @CallbackParameterName/*PUSH:CallbackType*//*PUSH:KeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName @KeepCallbackParameterName/*PUSH:KeepCallbackType*/)
            {
                return this.__client__.@MemberName/**/.@MethodName(/*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*//*IF:CallbackParameterName*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/new @CallbackReturnValueType.FullName(@CallbackParameterName)/*IF:CallbackParameterName*//*IF:KeepCallbackParameterName*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/new @KeepCallbackReturnValueType.FullName(@KeepCallbackParameterName)/*IF:KeepCallbackParameterName*/);
            }
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF IsReturnValue
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            #endregion IF IsReturnValue
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public @MethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:CallbackType*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/@FullName @CallbackParameterName/*NOT:IsTwoStageReturnValueParameter*/, Action<AutoCSer.Net.CommandClientReturnValue> @ErrorCallbackParameterName/*NOT:IsTwoStageReturnValueParameter*//*PUSH:CallbackType*//*PUSH:KeepCallbackType*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/@FullName @KeepCallbackParameterName, Action<AutoCSer.Net.CommandClientReturnValue> @ErrorKeepCallbackParameterName/*PUSH:KeepCallbackType*/)
            {
                return this.__client__.@MemberName/**/.@MethodName(/*LOOP:Method.Parameters*/@ParameterJoinName/*LOOP:Method.Parameters*//*IF:CallbackParameterName*//*IF:IsJoinCallback*/, /*IF:IsJoinCallback*/new @CallbackReturnValueType.FullName(@CallbackParameterName/*NOT:IsTwoStageReturnValueParameter*/, @ErrorCallbackParameterName/*NOT:IsTwoStageReturnValueParameter*/)/*IF:CallbackParameterName*//*IF:KeepCallbackParameterName*//*IF:IsJoinKeepCallback*/, /*IF:IsJoinKeepCallback*/new @KeepCallbackReturnValueType.FullName(@KeepCallbackParameterName, @ErrorKeepCallbackParameterName)/*IF:KeepCallbackParameterName*/);
            }
            #endregion NOT IsSynchronous
            #endregion LOOP Methods
            #region NOTE
            internal @ReturnValueControllerTypeName(params object[] values) { }
            #endregion NOTE
        }
        /// <summary>
        /// Get the direct return value API encapsulation (@MemberName)
        /// </summary>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored</param>
        /// <returns>@Property.CodeGeneratorXmlDocument (Direct return value API encapsulation)</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public @ReturnValueControllerTypeName @GetReturnValueControllerMethodName(bool isIgnoreError = false) { return new @ReturnValueControllerTypeName(this, isIgnoreError); }
        #endregion LOOP ControllerInterfaces
        #endregion PART CLASS
    }
}
