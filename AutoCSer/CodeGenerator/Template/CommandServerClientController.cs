using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class CommandServerClientController : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.XmlDocument 客户端接口
        /// </summary>
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition
        {
            #region LOOP Methods
            #region IF Method
            /// <summary>
            /// @Method.XmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@XmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF MethodIsReturn
            /// <returns>@ReturnXmlDocument</returns>
            #endregion IF MethodIsReturn
            @MethodReturnType.FullName @MethodName(/*PUSH:TaskQueueKeyType*/@FullName queueKey/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:TaskQueueKeyType*//*LOOP:Method.Parameters*//*AT:RefOutString*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*/);
            #endregion IF Method
            #endregion LOOP Methods
        }
        #endregion PART CLASS
    }
}
