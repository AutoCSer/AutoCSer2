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
            public string ParameterName;
            public string ReturnValueParameterName;
            public MethodReturnType.FullName ReturnValue;
            public static implicit operator int(FullName value) { return 0; }
            public static implicit operator FullName(int value) { return null; }
            public static implicit operator string(FullName value) { return null; }
            public static implicit operator FullName(AutoCSer.Net.CommandClientReturnValue value) { return null; }
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
            public static object Get(params object[] values) { return null; }
            public static MethodReturnType.FullName Create<T>(params object[] values) { return null; }
        }
        public partial class GenericDefinitionFullName : FullName { }
        public partial class GenericTypeName : FullName { }
        public partial class ParameterTypeName : FullName { }
        public partial class TypeFullName : FullName { }
        public partial class MethodIndexEnumTypeName : FullName { }
        public partial class CurrentType : Pub { }
        public partial class BaseType : Pub { }
        public partial class UnderlyingType : Pub { }
        public partial class MemberType : Pub { }
        public partial class MethodReturnType : Pub { }
        public partial class ReturnValueType : Pub { }
        public partial class ParameterType : Pub { }
        public partial class ServerType : Pub { }
        public partial class ClientType : Pub { }
        public partial class CallbackType : Pub { }
        public partial class EnumType : Pub { }
        public partial class SnapshotType : Pub { }
        public interface InterfaceTypeName
        {
            MethodReturnType.FullName MethodName(params object[] values);
        }
#if !DotNet45 && !AOT
        public partial class NetCoreWebViewTypeFullName : AutoCSer.NetCoreWeb.View { }
#endif
    }
}
