using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class CommandServerMethodIndexEnumType : Pub
    {
        #region PART CLASS
        #region IF IsGeneratorAttribute
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument
        /// </summary>
        [AutoCSer.Net.CommandServer.ServerControllerInterface(typeof(@MethodIndexEnumTypeName))]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition { }
        #endregion IF IsGeneratorAttribute
        #region IF IsGeneratorEnum
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument (The method sequence number maps the enumeration type)
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
        #endregion IF IsGeneratorEnum
        #endregion PART CLASS
        public const int MethodIndex = 0;
    }
}
