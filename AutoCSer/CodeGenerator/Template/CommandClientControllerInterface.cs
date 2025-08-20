using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class CommandClientControllerInterface : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument client interface
        /// </summary>
        #region IF IsCodeGeneratorControllerAttribute
        [AutoCSer.CodeGenerator.CommandClientController(typeof(@CurrentType.FullName))]
        #endregion IF IsCodeGeneratorControllerAttribute
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition
        {
            #region LOOP Methods
            #region IF Method
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region IF TaskQueueKeyType
            /// <param name="queueKey">Queue keyword</param>
            #endregion IF TaskQueueKeyType
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF TwoStageReturnValueType
            /// <param name="callback">@CallbackCodeGeneratorXmlDocument</param>
            /// <param name="keepCallback">@KeepCallbackCodeGeneratorXmlDocument</param>
            #endregion IF TwoStageReturnValueType
            #region IF MethodIsReturn
            /// <returns>@CodeGeneratorReturnXmlDocument</returns>
            #endregion IF MethodIsReturn
            @MethodReturnType.FullName @MethodName(/*PUSH:TaskQueueKeyType*/@FullName queueKey/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:TaskQueueKeyType*//*LOOP:Method.Parameters*//*AT:RefOutString*//*IF:IsRedirectType*/[AutoCSer.Net.CommandServer.MethodParameterType(typeof(@ParameterType.FullName))]/*IF:IsRedirectType*/@RedirectType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*IF:TwoStageReturnValueType*//*IF:IsTwoStageInputParameter*/, /*IF:IsTwoStageInputParameter*/Action<AutoCSer.Net.CommandClientReturnValue<@TwoStageReturnValueType.FullName>> callback, Action<AutoCSer.Net.CommandClientReturnValue<@ReturnValueType.FullName>, AutoCSer.Net.KeepCallbackCommand> keepCallback/*IF:TwoStageReturnValueType*/);
            #endregion IF Method
            #endregion LOOP Methods
        }
        #endregion PART CLASS
    }
}
