using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseLocalClientNode : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.XmlDocument 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(@CurrentType.GenericDefinitionFullName))]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition/*IF:IsCustomServiceNode*/ : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeLocalClientNode/*IF:IsCustomServiceNode*/
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
            /// <returns>@Method.ReturnXmlDocument</returns>
            #endregion IF MethodIsReturn
            @MethodReturnType.FullName @MethodName(/*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:CallbackType*//*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*/@FullName callback/*PUSH:CallbackType*/);
            #endregion IF Method
            #endregion LOOP Methods
        }
        #endregion PART CLASS
    }
}
