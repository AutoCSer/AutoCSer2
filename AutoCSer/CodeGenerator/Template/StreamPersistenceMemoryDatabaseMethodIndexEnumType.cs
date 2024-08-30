using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseMethodIndexEnumType : Pub
    {
        public enum MethodIndexEnum
        {
            #region PART CLASS
            #region LOOP Methods
            #region IF EnumName
            #region IF Method
            /// <summary>
            /// [@MethodIndex] @Method.XmlDocument
            #region LOOP Method.Parameters
            /// @ParameterType.XmlFullName @ParameterName @XmlDocument
            #endregion LOOP Method.Parameters
            #region IF MethodIsReturn
            /// 返回值 @MethodReturnType.XmlFullName @Method.ReturnXmlDocument
            #endregion IF MethodIsReturn
            /// </summary>
            #endregion IF Method
            @EnumName = @MethodIndex,
            #endregion IF EnumName
            #endregion LOOP Methods
            #endregion PART CLASS
        }
    }
}
