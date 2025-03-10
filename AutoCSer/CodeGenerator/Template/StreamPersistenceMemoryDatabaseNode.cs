using AutoCSer.CodeGenerator.Metadata;
using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseNode : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(@MethodIndexEnumTypeName))]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition { }
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument 节点方法序号映射枚举类型
        /// </summary>
        public enum @MethodIndexEnumTypeName
        {
            #region LOOP Methods
            #region IF EnumName
            #region IF Method
            /// <summary>
            /// [@MethodIndex] @Method.CodeGeneratorXmlDocument
            #region LOOP Method.Parameters
            /// @ParameterType.XmlFullName @ParameterName @CodeGeneratorXmlDocument
            #endregion LOOP Method.Parameters
            #region IF MethodIsReturn
            /// 返回值 @MethodReturnType.XmlFullName @Method.CodeGeneratorReturnXmlDocument
            #endregion IF MethodIsReturn
            /// </summary>
            #endregion IF Method
            @EnumName = @MethodIndex,
            #endregion IF EnumName
            #endregion LOOP Methods
        }
        #endregion PART CLASS
    }
}
