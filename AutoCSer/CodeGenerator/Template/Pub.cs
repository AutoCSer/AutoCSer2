using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    /// <summary>
    /// CSharp 模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        public unsafe partial class FullName : Pub, IEquatable<FullName>
        {
            public string MemberName;
            public static implicit operator int(FullName value) { return 0; }
            public static implicit operator FullName(int value) { return null; }
            public static implicit operator string(FullName value) { return null; }
            public void simpleSerialize(object value) { }
            public byte* simpleDeserialize(params byte*[] values) { return null; }
            public bool Equals(FullName other) { return false; }
            public void jsonSerialize(params object[] value) { }
            public void jsonDeserialize(object value, ref AutoCSer.Memory.Pointer names, object memberMap = null) { }
            public void fixedBinarySerialize(params object[] values) { }
            public void binarySerialize(params object[] values) { }
            public void fixedBinaryDeserialize(params object[] values) { }
            public void binaryDeserialize(params object[] values) { }
            public void xmlSerialize(params object[] value) { }
            public bool fieldEquals(params object[] values) { return false; }
            public void memberCopyFrom(params object[] values) { }
            public void createRandomObject(params object[] values) { }

            public static void MethodName() { }
        }
        public partial class GenericDefinitionFullName : Pub { }
        public partial class CurrentType : Pub { }
        public partial class GenericTypeName : Pub { }
        public partial class BaseType : Pub { }
        public partial class UnderlyingType : Pub { }
        public partial class MemberType : Pub { }
        public partial class MethodReturnType : Pub { }
        public partial class ParameterType : Pub { }
#if !DotNet45 && !AOT
        public partial class NetCoreWebViewTypeFullName : AutoCSer.NetCoreWeb.View { }
#endif
        public static readonly string ParameterName = null;
    }
}
