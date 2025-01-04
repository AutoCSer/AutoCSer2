using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseClientNode : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.XmlDocument 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(@CurrentType.GenericDefinitionFullName))]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition/*IF:IsCustomServiceNode*/ : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode/*IF:IsCustomServiceNode*/
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
            @MethodReturnType.FullName @MethodName(/*PUSH:ReturnRequestParameterType*/@FullName returnValue/*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*//*PUSH:ReturnRequestParameterType*//*LOOP:Method.Parameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*//*PUSH:KeepCallbackType*//*IF:Method.Parameters.Length*/, /*IF:Method.Parameters.Length*/@FullName callback/*PUSH:KeepCallbackType*/);
            #endregion IF Method
            #endregion LOOP Methods
        }
        #endregion PART CLASS
    }
    #region NOTE
    /// <summary>
    /// CSharp 模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        public partial class FullName : Pub, IEquatable<FullName>
        {
            public bool Equals(FullName other) { return false; }
        }
        public partial class GenericDefinitionFullName : Pub
        {
        }
        public partial class CurrentType : Pub
        {
        }
        public partial class MethodReturnType : Pub
        {
        }
        public partial class ParameterType : Pub
        {

        }
    }
    #endregion NOTE
}
