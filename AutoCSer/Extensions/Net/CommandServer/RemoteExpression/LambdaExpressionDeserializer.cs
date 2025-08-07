using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程 Lambda 表达式反序列化
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LambdaExpressionDeserializer
    {
        /// <summary>
        /// Remote expression server metadata information
        /// 远程表达式服务端元数据信息
        /// </summary>
        private readonly ServerMetadata metadata;
        /// <summary>
        /// Binary data deserialization
        /// 二进制数据反序列化
        /// </summary>
        private BinaryDeserializer deserializer;
        /// <summary>
        /// 表达式参数集合
        /// </summary>
        internal ParameterExpression[] Parameters;
        /// <summary>
        /// 当前读取位置
        /// </summary>
        private byte* read;
        /// <summary>
        /// 当前结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 远程表达式序列化状态
        /// </summary>
        internal RemoteExpressionSerializeStateEnum State;
        /// <summary>
        /// 远程 Lambda 表达式反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="metadata"></param>
        internal LambdaExpressionDeserializer(BinaryDeserializer deserializer, ServerMetadata metadata)
        {
            this.metadata = metadata;
            this.deserializer = deserializer;
            Parameters = EmptyArray<ParameterExpression>.Array;
            if (deserializer.ReadBuffer(out read, out end)) State = RemoteExpressionSerializeStateEnum.Success;
            else State = RemoteExpressionSerializeStateEnum.FormatReadIndexOutOfRange;
        }
        /// <summary>
        /// 设置错误状态
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setError(RemoteExpressionSerializeStateEnum state)
        {
            State = state;
            deserializer.SetCustomError(string.Empty);
        }
        /// <summary>
        /// 设置反序列化数据超出预期范围错误状态
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setIndexOutOfRange()
        {
            State = RemoteExpressionSerializeStateEnum.FormatReadIndexOutOfRange;
            deserializer.State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal System.Linq.Expressions.Expression? Deserialize()
#else
        internal System.Linq.Expressions.Expression Deserialize()
#endif
        {
            if (State == RemoteExpressionSerializeStateEnum.Success && deserialize(out Parameters))
            {
                var expression = deserializeExpression();
                if (expression != null)
                {
                    if (read == end)
                    {
                        deserializer.Current = end;
                        return expression;
                    }
                    setError(RemoteExpressionSerializeStateEnum.DeserializeFailed);
                }
            }
            return null;
        }
        /// <summary>
        /// 反序列化参数集合
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private bool deserialize(out ParameterExpression[] parameters)
        {
            int count = *(int*)read;
            if (count > 0)
            {
                parameters = new ParameterExpression[count];
                read += sizeof(int);
                var parameterName = default(string);
                for (int index = 0; index != count; ++index)
                {
                    var parameterType = deserializeType();
                    if (parameterType == null || !deserialize(out parameterName)) return false;
                    parameters[index] = System.Linq.Expressions.Expression.Parameter(parameterType, parameterName);
                }
                return true;
            }
            parameters = EmptyArray<ParameterExpression>.Array;
            if (count == 0)
            {
                read += sizeof(int);
                return true;
            }
            State = RemoteExpressionSerializeStateEnum.DeserializeFailed;
            return false;
        }
        /// <summary>
        /// 反序列化类型信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private Type? deserializeType()
#else
        private Type deserializeType()
#endif
        {
            int header = *(int*)read;
            read += sizeof(int);
            if ((header & (int)NodeHeaderEnum.TypeIndex) != 0)
            {
                Type type = AutoCSer.Reflection.RemoteType.FixedTypes[(byte)header];
                return (header & (int)NodeHeaderEnum.IsArray) == 0 ? type : type.MakeArrayType();
            }
            if ((header & (int)NodeHeaderEnum.Type) != 0)
            {
                HashBuffer hashBuffer = new HashBuffer(read, end);
                if (hashBuffer.Buffer.Data != null)
                {
                    Dictionary<HashBuffer, Type> types = metadata.Types;
                    var assemblyName = default(string);
                    var typeName = default(string);
                    var value = default(Type);
                    int step = 0;
                    Monitor.Enter(types);
                    try
                    {
                        if (!types.TryGetValue(hashBuffer, out value))
                        {
                            read += sizeof(int);
                            if (deserialize(out assemblyName) && deserialize(out typeName))
                            {
                                if (new AutoCSer.Reflection.RemoteType(assemblyName.notNull(), typeName.notNull()).TryGet(out value, false))
                                {
                                    types.Add(new HashBuffer(ref hashBuffer), value);
                                }
                                else step = 1;
                            }
                            else step = 2;
                        }
                    }
                    finally { Monitor.Exit(types); }
                    switch (step)
                    {
                        case 0:
                            read = hashBuffer.Buffer.End + sizeof(int);
                            return value;
                        case 1:
                            State = RemoteExpressionSerializeStateEnum.NotFoundType;
                            return null;
                        case 2: return null;
                    }
                }
                else setIndexOutOfRange();
            }
            State = RemoteExpressionSerializeStateEnum.DeserializeFailed;
            return null;
        }
        /// <summary>
        /// 反序列化类型集合
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
#if NetStandard21
        private Type[]? deserializeTypeArray(int count)
#else
        private Type[] deserializeTypeArray(int count)
#endif
        {
            if (count == 0) return EmptyArray<Type>.Array;
            Type[] types = new Type[count];
            for (int index = 0; index != count; ++index)
            {
                var type = deserializeType();
                if (type != null) types[index] = type;
                else return null;
            }
            return types;
        }
        /// <summary>
        /// 反序列化方法信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private Type? deserializeType(uint header)
#else
        private Type deserializeType(uint header)
#endif
        {
            if (State == RemoteExpressionSerializeStateEnum.Success)
            {
                if ((header & (int)NodeHeaderEnum.Type) != 0) return deserializeType();
                setError(RemoteExpressionSerializeStateEnum.DeserializeFailed);
            }
            return null;
        }
        /// <summary>
        /// 反序列化方法信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private MethodInfo? deserializeMethod(uint header)
#else
        private MethodInfo deserializeMethod(uint header)
#endif
        {
            if ((header & (int)NodeHeaderEnum.Method) != 0 && State == RemoteExpressionSerializeStateEnum.Success)
            {
                HashBuffer hashBuffer = new HashBuffer(read, end);
                if (hashBuffer.Buffer.Data != null)
                {
                    Dictionary<HashBuffer, MethodInfo> methods = metadata.Methods;
                    var methodName = default(string);
                    var value = default(MethodInfo);
                    int step = 0;
                    Monitor.Enter(methods);
                    try
                    {
                        if (!methods.TryGetValue(hashBuffer, out value))
                        {
                            read += sizeof(int);
                            var type = deserializeType();
                            if (type != null && deserialize(out methodName))
                            {
                                BindingFlags bindingFlags = (BindingFlags)(*(int*)read);
                                int typeCount = *(int*)(read + sizeof(int));
                                read += sizeof(int) * 2;
                                var types = deserializeTypeArray(typeCount & 0xff);
                                if (types != null)
                                {
                                    var parameterTypes = deserializeTypeArray(typeCount >> 8);
                                    if (parameterTypes != null)
                                    {
                                        if (types.Length == 0) value = type.GetMethod(methodName.notNull(), bindingFlags, null, parameterTypes, null);
                                        else value = RemoteMetadataMethodIndex.GetMethod(type, methodName.notNull(), bindingFlags, types, parameterTypes);
                                        if (value != null) methods.Add(new HashBuffer(ref hashBuffer), value);
                                        else step = 1;
                                    }
                                    else step = 2;
                                }
                                else step = 2;
                            }
                            else step = 2;
                        }
                    }
                    finally { Monitor.Exit(methods); }
                    switch (step)
                    {
                        case 0:
                            read = hashBuffer.Buffer.End + sizeof(int);
                            return value;
                        case 1:
                            State = RemoteExpressionSerializeStateEnum.NotFoundMethod;
                            return null;
                        case 2: return null;
                    }
                }
                else setIndexOutOfRange();
           }
            return null;
        }
        /// <summary>
        /// 反序列化属性信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private PropertyInfo? deserializeProperty(uint header)
#else
        private PropertyInfo deserializeProperty(uint header)
#endif
        {
            return (header & (int)NodeHeaderEnum.Property) != 0 && State == RemoteExpressionSerializeStateEnum.Success ? deserializeProperty() : null;
        }
        /// <summary>
        /// 反序列化属性信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private PropertyInfo? deserializeProperty()
#else
        private PropertyInfo deserializeProperty()
#endif
        {
            HashBuffer hashBuffer = new HashBuffer(read, end);
            if (hashBuffer.Buffer.Data != null)
            {
                Dictionary<HashBuffer, PropertyInfo> properties = metadata.Properties;
                var propertyName = default(string);
                var value = default(PropertyInfo);
                int step = 0;
                Monitor.Enter(properties);
                try
                {
                    if (!properties.TryGetValue(hashBuffer, out value))
                    {
                        read += sizeof(int);
                        var type = deserializeType();
                        if (type != null && deserialize(out propertyName))
                        {
                            value = type.GetProperty(propertyName.notNull(), (BindingFlags)(*(int*)read));
                            if (value != null) properties.Add(new HashBuffer(ref hashBuffer), value);
                            else step = 1;
                        }
                        else step = 2;
                    }
                }
                finally { Monitor.Exit(properties); }
                switch (step)
                {
                    case 0:
                        read = hashBuffer.Buffer.End + sizeof(int);
                        return value;
                    case 1:
                        State = RemoteExpressionSerializeStateEnum.NotFoundProperty;
                        return null;
                    case 2: return null;
                }
            }
            setIndexOutOfRange();
            return null;
        }
        /// <summary>
        /// 反序列化成员信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private MemberInfo? deserializeMember(uint header)
#else
        private MemberInfo deserializeMember(uint header)
#endif
        {
            if (State == RemoteExpressionSerializeStateEnum.Success)
            {
                if ((header & (int)NodeHeaderEnum.Property) != 0) return deserializeProperty();
                if ((header & (int)NodeHeaderEnum.Field) != 0)
                {
                    HashBuffer hashBuffer = new HashBuffer(read, end);
                    if (hashBuffer.Buffer.Data != null)
                    {
                        Dictionary<HashBuffer, FieldInfo> fields = metadata.Fields;
                        var fieldName = default(string);
                        var value = default(FieldInfo);
                        int step = 0;
                        Monitor.Enter(fields);
                        try
                        {
                            if (!fields.TryGetValue(hashBuffer, out value))
                            {
                                read += sizeof(int);
                                var type = deserializeType();
                                if (type != null && deserialize(out fieldName))
                                {
                                    value = type.GetField(fieldName.notNull(), (BindingFlags)(*(int*)read));
                                    if (value != null) fields.Add(new HashBuffer(ref hashBuffer), value);
                                    else step = 1;
                                }
                                else step = 2;
                            }
                        }
                        finally { Monitor.Exit(fields); }
                        switch (step)
                        {
                            case 0:
                                read = hashBuffer.Buffer.End + sizeof(int);
                                return value;
                            case 1:
                                State = RemoteExpressionSerializeStateEnum.NotFoundProperty;
                                return null;
                            case 2: return null;
                        }
                    }
                    else setIndexOutOfRange();
                }
                else setError(RemoteExpressionSerializeStateEnum.DeserializeFailed);
            }
            return null;
        }
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private bool deserialize(out string? value)
#else
        private bool deserialize(out string value)
#endif
        {
            uint header = *(uint*)read;
            if ((byte)header == (int)ExpressionType.Constant)
            {
                read += sizeof(int);
                switch ((byte)(header >> 8) - (byte)ConstantTypeEnum.String)
                {
                    case (byte)ConstantTypeEnum.String - (byte)ConstantTypeEnum.String:
                        string stringValue = string.Empty;
                        read = AutoCSer.SimpleSerialize.Deserializer.NotNull(read, ref stringValue, end);
                        if (read != null)
                        {
                            value = stringValue;
                            return true;
                        }
                        break;
                    case (byte)ConstantTypeEnum.NullString - (byte)ConstantTypeEnum.String:
                        value = null;
                        return true;
                    case (byte)ConstantTypeEnum.EmptyString - (byte)ConstantTypeEnum.String:
                        value = string.Empty;
                        return true;
                    case (byte)ConstantTypeEnum.CharString - (byte)ConstantTypeEnum.String:
                        value = ((char)(header >> 16)).ToString();
                        return true;
                }
            }
            value = null;
            State = RemoteExpressionSerializeStateEnum.ConstantError;
            return false;
        }
        /// <summary>
        /// 常量反序列化
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private System.Linq.Expressions.Expression? deserializeConstant(uint header)
#else
        private System.Linq.Expressions.Expression deserializeConstant(uint header)
#endif
        {
            if (State == RemoteExpressionSerializeStateEnum.Success)
            {
                switch ((byte)(header >> 8))
                {
                    case (byte)ConstantTypeEnum.Unknown:
                        var type = deserializeType();
                        if(type != null)
                        {
                            if ((header & (uint)NodeHeaderEnum.NullValue) == 0)
                            {
                                deserializer.Current = read;
                                var constantValue = AutoCSer.Extensions.Metadata.GenericType.Get(type).BinaryDeserialize(deserializer);
                                if (constantValue != null)
                                {
                                    read = deserializer.Current;
                                    return System.Linq.Expressions.Expression.Constant(constantValue, type);
                                }
                                State = RemoteExpressionSerializeStateEnum.ConstantError;
                            }
                            return System.Linq.Expressions.Expression.Constant(null, type);
                        }
                        return null;
                    case (byte)ConstantTypeEnum.Bool: return System.Linq.Expressions.Expression.Constant(false, typeof(bool));
                    case (byte)ConstantTypeEnum.Byte: return System.Linq.Expressions.Expression.Constant((byte)(header >> 16), typeof(byte));
                    case (byte)ConstantTypeEnum.SByte: return System.Linq.Expressions.Expression.Constant((sbyte)(byte)(header >> 16), typeof(sbyte));
                    case (byte)ConstantTypeEnum.Short: return System.Linq.Expressions.Expression.Constant((short)(ushort)(header >> 16), typeof(short));
                    case (byte)ConstantTypeEnum.UShort: return System.Linq.Expressions.Expression.Constant((ushort)(header >> 16), typeof(ushort));
                    case (byte)ConstantTypeEnum.Int:
                        object value = *(int*)read;
                        read += sizeof(int);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(int));
                    case (byte)ConstantTypeEnum.UInt:
                        value = *(uint*)read;
                        read += sizeof(uint);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(uint));
                    case (byte)ConstantTypeEnum.Long:
                        value = *(long*)read;
                        read += sizeof(long);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(long));
                    case (byte)ConstantTypeEnum.ULong:
                        value = *(ulong*)read;
                        read += sizeof(ulong);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(ulong));
                    case (byte)ConstantTypeEnum.Float:
                        value = *(float*)read;
                        read += sizeof(float);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(float));
                    case (byte)ConstantTypeEnum.Double:
                        value = *(double*)read;
                        read += sizeof(double);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(double));
                    case (byte)ConstantTypeEnum.Decimal:
                        value = *(decimal*)read;
                        read += sizeof(decimal);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(decimal));
                    case (byte)ConstantTypeEnum.Char: return System.Linq.Expressions.Expression.Constant((char)(header >> 16), typeof(char));
                    case (byte)ConstantTypeEnum.DateTime:
                        value = *(DateTime*)read;
                        read += sizeof(DateTime);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(DateTime));
                    case (byte)ConstantTypeEnum.TimeSpan:
                        value = *(TimeSpan*)read;
                        read += sizeof(TimeSpan);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(TimeSpan));
                    case (byte)ConstantTypeEnum.Guid:
                        value = *(Guid*)read;
                        read += sizeof(Guid);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(Guid));
                    case (byte)ConstantTypeEnum.NullableBool: return System.Linq.Expressions.Expression.Constant((bool?)false, typeof(bool?));
                    case (byte)ConstantTypeEnum.NullableByte: return System.Linq.Expressions.Expression.Constant((byte)(header >> 16), typeof(byte?));
                    case (byte)ConstantTypeEnum.NullableSByte: return System.Linq.Expressions.Expression.Constant((sbyte)(byte)(header >> 16), typeof(sbyte?));
                    case (byte)ConstantTypeEnum.NullableShort: return System.Linq.Expressions.Expression.Constant((short)(ushort)(header >> 16), typeof(short?));
                    case (byte)ConstantTypeEnum.NullableUShort: return System.Linq.Expressions.Expression.Constant((ushort)(header >> 16), typeof(ushort?));
                    case (byte)ConstantTypeEnum.NullableInt:
                        value = *(int*)read;
                        read += sizeof(int);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(int?));
                    case (byte)ConstantTypeEnum.NullableUInt:
                        value = *(uint*)read;
                        read += sizeof(uint);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(uint?));
                    case (byte)ConstantTypeEnum.NullableLong:
                        value = *(long*)read;
                        read += sizeof(long);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(long?));
                    case (byte)ConstantTypeEnum.NullableULong:
                        value = *(ulong*)read;
                        read += sizeof(ulong);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(ulong?));
                    case (byte)ConstantTypeEnum.NullableFloat:
                        value = *(float*)read;
                        read += sizeof(float);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(float?));
                    case (byte)ConstantTypeEnum.NullableDouble:
                        value = *(double*)read;
                        read += sizeof(double);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(double?));
                    case (byte)ConstantTypeEnum.NullableDecimal:
                        value = *(decimal*)read;
                        read += sizeof(decimal);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(decimal?));
                    case (byte)ConstantTypeEnum.NullableChar: return System.Linq.Expressions.Expression.Constant((ushort)(header >> 16), typeof(char?));
                    case (byte)ConstantTypeEnum.NullableDateTime:
                        value = *(DateTime*)read;
                        read += sizeof(DateTime);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(DateTime?));
                    case (byte)ConstantTypeEnum.NullableTimeSpan:
                        value = *(TimeSpan*)read;
                        read += sizeof(TimeSpan);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(TimeSpan?));
                    case (byte)ConstantTypeEnum.NullableGuid:
                        value = *(Guid*)read;
                        read += sizeof(Guid);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(Guid?));
                    case (byte)ConstantTypeEnum.Complex:
                        value = *(System.Numerics.Complex*)read;
                        read += sizeof(System.Numerics.Complex);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Complex));
                    case (byte)ConstantTypeEnum.Plane:
                        value = *(System.Numerics.Plane*)read;
                        read += sizeof(System.Numerics.Plane);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Plane));
                    case (byte)ConstantTypeEnum.Quaternion:
                        value = *(System.Numerics.Quaternion*)read;
                        read += sizeof(System.Numerics.Quaternion);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Quaternion));
                    case (byte)ConstantTypeEnum.Matrix3x2:
                        value = *(System.Numerics.Matrix3x2*)read;
                        read += sizeof(System.Numerics.Matrix3x2);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Matrix3x2));
                    case (byte)ConstantTypeEnum.Matrix4x4:
                        value = *(System.Numerics.Matrix4x4*)read;
                        read += sizeof(System.Numerics.Matrix4x4);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Matrix4x4));
                    case (byte)ConstantTypeEnum.Vector2:
                        value = *(System.Numerics.Vector2*)read;
                        read += sizeof(System.Numerics.Vector2);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Vector2));
                    case (byte)ConstantTypeEnum.Vector3:
                        value = *(System.Numerics.Vector3*)read;
                        read += sizeof(System.Numerics.Vector3);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Vector3));
                    case (byte)ConstantTypeEnum.Vector4:
                        value = *(System.Numerics.Vector4*)read;
                        read += sizeof(System.Numerics.Vector4);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(System.Numerics.Vector4));
                    case (byte)ConstantTypeEnum.Half:
                        value = *(Half*)read;
                        read += sizeof(Half);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(Half));
                    case (byte)ConstantTypeEnum.Int128:
                        value = *(Int128*)read;
                        read += sizeof(Int128);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(Int128));
                    case (byte)ConstantTypeEnum.UInt128:
                        value = *(UInt128*)read;
                        read += sizeof(UInt128);
                        return System.Linq.Expressions.Expression.Constant(value, typeof(UInt128));
                    case (byte)ConstantTypeEnum.ByteArray:
                        var byteArray = default(byte[]);
                        read = AutoCSer.SimpleSerialize.Deserializer.Deserialize(read, ref byteArray, end);
                        if (read != null) return System.Linq.Expressions.Expression.Constant(byteArray, typeof(byte[]));
                        State = RemoteExpressionSerializeStateEnum.FormatReadIndexOutOfRange;
                        return null;
                    case (byte)ConstantTypeEnum.String:
                        string stringValue = string.Empty;
                        read = AutoCSer.SimpleSerialize.Deserializer.NotNull(read, ref stringValue, end);
                        if (read != null) return System.Linq.Expressions.Expression.Constant(stringValue, typeof(string));
                        State = RemoteExpressionSerializeStateEnum.FormatReadIndexOutOfRange;
                        return null;
                    case (byte)ConstantTypeEnum.NullString: return System.Linq.Expressions.Expression.Constant(null, typeof(string));
                    case (byte)ConstantTypeEnum.EmptyString: return System.Linq.Expressions.Expression.Constant(string.Empty, typeof(string));
                    case (byte)ConstantTypeEnum.CharString: return System.Linq.Expressions.Expression.Constant(((char)(header >> 16)).ToString(), typeof(string));
                    case (byte)ConstantTypeEnum.NullByteArray: return System.Linq.Expressions.Expression.Constant(null, typeof(byte[]));
                    case (byte)ConstantTypeEnum.EmptyByteArray: return System.Linq.Expressions.Expression.Constant(EmptyArray<byte>.Array, typeof(byte[]));
                    case (byte)ConstantTypeEnum.ByteArray1: return System.Linq.Expressions.Expression.Constant(new byte[] { (byte)(header >> 16) }, typeof(byte[]));
                    case (byte)ConstantTypeEnum.ByteArray2: return System.Linq.Expressions.Expression.Constant(new byte[] { (byte)(header >> 16), (byte)(header >> 24) }, typeof(byte[]));
                    case (byte)ConstantTypeEnum.True: return System.Linq.Expressions.Expression.Constant(true, typeof(bool));
                    case (byte)ConstantTypeEnum.NullableBoolTrue: return System.Linq.Expressions.Expression.Constant((bool?)true, typeof(bool?));
                    case (byte)ConstantTypeEnum.NullBool: return System.Linq.Expressions.Expression.Constant(null, typeof(bool?));
                    case (byte)ConstantTypeEnum.NullByte: return System.Linq.Expressions.Expression.Constant(null, typeof(byte?));
                    case (byte)ConstantTypeEnum.NullSByte: return System.Linq.Expressions.Expression.Constant(null, typeof(sbyte?));
                    case (byte)ConstantTypeEnum.NullShort: return System.Linq.Expressions.Expression.Constant(null, typeof(short?));
                    case (byte)ConstantTypeEnum.NullUShort: return System.Linq.Expressions.Expression.Constant(null, typeof(ushort?));
                    case (byte)ConstantTypeEnum.NullInt: return System.Linq.Expressions.Expression.Constant(null, typeof(int?));
                    case (byte)ConstantTypeEnum.NullUInt: return System.Linq.Expressions.Expression.Constant(null, typeof(uint?));
                    case (byte)ConstantTypeEnum.NullLong: return System.Linq.Expressions.Expression.Constant(null, typeof(long?));
                    case (byte)ConstantTypeEnum.NullULong: return System.Linq.Expressions.Expression.Constant(null, typeof(ulong?));
                    case (byte)ConstantTypeEnum.NullFloat: return System.Linq.Expressions.Expression.Constant(null, typeof(float?));
                    case (byte)ConstantTypeEnum.NullDouble: return System.Linq.Expressions.Expression.Constant(null, typeof(double?));
                    case (byte)ConstantTypeEnum.NullDecimal: return System.Linq.Expressions.Expression.Constant(null, typeof(decimal?));
                    case (byte)ConstantTypeEnum.NullChar: return System.Linq.Expressions.Expression.Constant(null, typeof(char?));
                    case (byte)ConstantTypeEnum.NullDateTime: return System.Linq.Expressions.Expression.Constant(null, typeof(DateTime?));
                    case (byte)ConstantTypeEnum.NullTimeSpan: return System.Linq.Expressions.Expression.Constant(null, typeof(TimeSpan?));
                    case (byte)ConstantTypeEnum.NullGuid: return System.Linq.Expressions.Expression.Constant(null, typeof(Guid?));
                    default:
                        State = RemoteExpressionSerializeStateEnum.ConstantError;
                        return null;
                }
            }
            return null;
        }
        /// <summary>
        /// 反序列化方法调用参数集合
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private System.Linq.Expressions.Expression[]? deserializeArguments()
#else
        private System.Linq.Expressions.Expression[] deserializeArguments()
#endif
        {
            if (State == RemoteExpressionSerializeStateEnum.Success)
            {
                int count = *(int*)read;
                read += sizeof(int);
                if (count > 0)
                {
                    System.Linq.Expressions.Expression[] expressions = new System.Linq.Expressions.Expression[count];
                    for (int index = 0; index != count; ++index)
                    {
                        var expression = deserializeExpression();
                        if (expression != null) expressions[index] = expression;
                        else return null;
                    }
                    return expressions;
                }
                if (count == 0) return EmptyArray<System.Linq.Expressions.Expression>.Array;
                State = RemoteExpressionSerializeStateEnum.DeserializeFailed;
            }
            return null;
        }
        /// <summary>
        /// 反序列化表达式节点
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private System.Linq.Expressions.Expression? deserializeExpression(uint header)
#else
        private System.Linq.Expressions.Expression deserializeExpression(uint header)
#endif
        {
            return (header & (int)NodeHeaderEnum.NullValue) == 0 && State == RemoteExpressionSerializeStateEnum.Success ? deserializeExpression() : null;
        }
        /// <summary>
        /// 反序列化表达式节点
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private System.Linq.Expressions.Expression? deserializeExpression()
#else
        private System.Linq.Expressions.Expression deserializeExpression()
#endif
        {
            if (State == RemoteExpressionSerializeStateEnum.Success)
            {
                uint header = *(uint*)read;
                read += sizeof(uint);
                switch ((byte)header)
                {
                    case (int)ExpressionType.Add: return add(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.AddChecked: return addChecked(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.And: return and(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.AndAlso: return andAlso(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.ArrayIndex: return arrayIndex(deserializeExpression(), deserializeExpression());
                    case (int)ExpressionType.Divide: return divide(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Equal: return equal(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.ExclusiveOr: return exclusiveOr(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.GreaterThan: return greaterThan(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.GreaterThanOrEqual: return greaterThanOrEqual(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.LeftShift: return leftShift(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.LessThan: return lessThan(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.LessThanOrEqual: return lessThanOrEqual(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Modulo: return modulo(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Multiply: return multiply(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.MultiplyChecked: return multiplyChecked(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.NotEqual: return notEqual(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Or: return or(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.OrElse: return orElse(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Power: return power(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.RightShift: return rightShift(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Subtract: return subtract(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.SubtractChecked: return subtractChecked(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Assign: return assign(deserializeExpression(), deserializeExpression());
                    case (int)ExpressionType.Coalesce: return coalesce(deserializeExpression(), deserializeExpression());
                    case (int)ExpressionType.AddAssign: return addAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.AndAssign: return andAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.DivideAssign: return divideAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.ExclusiveOrAssign: return exclusiveOrAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.LeftShiftAssign: return leftShiftAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.ModuloAssign: return moduloAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.MultiplyAssign: return multiplyAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.OrAssign: return orAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.PowerAssign: return powerAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.RightShiftAssign: return rightShiftAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.SubtractAssign: return subtractAssign(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.AddAssignChecked: return addAssignChecked(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.MultiplyAssignChecked: return multiplyAssignChecked(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.SubtractAssignChecked: return subtractAssignChecked(deserializeExpression(), deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.TypeIs: return typeIs(deserializeExpression(), deserializeType(header));
                    case (int)ExpressionType.TypeEqual: return typeEqual(deserializeExpression(), deserializeType(header));
                    case (int)ExpressionType.Negate: return negate(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.UnaryPlus: return unaryPlus(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.NegateChecked: return negateChecked(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Not: return not(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Decrement: return decrement(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.Increment: return increment(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.PreIncrementAssign: return preIncrementAssign(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.PreDecrementAssign: return preDecrementAssign(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.PostIncrementAssign: return postIncrementAssign(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.PostDecrementAssign: return postDecrementAssign(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.OnesComplement: return onesComplement(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.IsTrue: return isTrue(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.IsFalse: return isFalse(deserializeExpression(), deserializeMethod(header));
                    case (int)ExpressionType.ArrayLength: return arrayLength(deserializeExpression());
                    case (int)ExpressionType.Quote: return quote(deserializeExpression());
                    case (int)ExpressionType.Convert: return convert(deserializeExpression(), deserializeType(header), deserializeMethod(header));
                    case (int)ExpressionType.ConvertChecked: return convertChecked(deserializeExpression(), deserializeType(header), deserializeMethod(header));
                    case (int)ExpressionType.TypeAs: return typeAs(deserializeExpression(), deserializeType(header));
                    case (int)ExpressionType.Unbox: return unbox(deserializeExpression(), deserializeType(header));
                    case (int)ExpressionType.Call: return call(deserializeExpression(header), deserializeMethod(header), deserializeArguments());
                    case (int)ExpressionType.Index: return index(deserializeExpression(header), deserializeProperty(header), deserializeArguments());
                    case (int)ExpressionType.MemberAccess: return memberAccess(deserializeExpression(header), deserializeMember(header));
                    case (int)ExpressionType.Conditional: return condition(deserializeExpression(), deserializeExpression(), deserializeExpression());
                    case (int)ExpressionType.Invoke: return invoke(deserializeExpression(), deserializeArguments());
                    case (int)ExpressionType.Default: return defaultExpression(deserializeType(header));
                    case (int)ExpressionType.Constant: return deserializeConstant(header);
                    case (int)ExpressionType.Parameter: return Parameters[header >> 8];
                    default:
                        State = RemoteExpressionSerializeStateEnum.UnknownNodeType;
                        return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? add(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression add(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Add(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// AddChecked
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? addChecked(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression addChecked(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.AddChecked(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// And
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? and(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression and(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.And(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// AndAlso
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? andAlso(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression andAlso(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.AndAlso(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// ArrayIndex
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? arrayIndex(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right)
#else
        private System.Linq.Expressions.Expression arrayIndex(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.ArrayIndex(left, right) : setNullParameter();
        }
        /// <summary>
        /// Divide
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? divide(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression divide(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Divide(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// Equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? equal(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression equal(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Equal(left, right, false, method) : setNullParameter();
        }
        /// <summary>
        /// ExclusiveOr
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? exclusiveOr(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression exclusiveOr(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.ExclusiveOr(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// GreaterThan
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? greaterThan(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression greaterThan(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.GreaterThan(left, right, false, method) : setNullParameter();
        }
        /// <summary>
        /// GreaterThanOrEqual
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? greaterThanOrEqual(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression greaterThanOrEqual(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.GreaterThanOrEqual(left, right, false, method) : setNullParameter();
        }
        /// <summary>
        /// LeftShift
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? leftShift(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression leftShift(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.LeftShift(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// LessThan
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? lessThan(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression lessThan(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.LessThan(left, right, false, method) : setNullParameter();
        }
        /// <summary>
        /// LessThanOrEqual
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? lessThanOrEqual(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression lessThanOrEqual(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.LessThanOrEqual(left, right, false, method) : setNullParameter();
        }
        /// <summary>
        /// Modulo
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? modulo(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression modulo(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Modulo(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// Multiply
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? multiply(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression multiply(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Multiply(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// MultiplyChecked
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? multiplyChecked(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression multiplyChecked(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.MultiplyChecked(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// NotEqual
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? notEqual(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression notEqual(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.NotEqual(left, right, false, method) : setNullParameter();
        }
        /// <summary>
        /// Or
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? or(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression or(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Or(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// OrElse
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? orElse(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression orElse(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.OrElse(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// Power
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? power(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression power(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Power(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// RightShift
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? rightShift(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression rightShift(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.RightShift(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// Subtract
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? subtract(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression subtract(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Subtract(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// SubtractChecked
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? subtractChecked(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression subtractChecked(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.SubtractChecked(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// Assign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? assign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right)
#else
        private System.Linq.Expressions.Expression assign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Assign(left, right) : setNullParameter();
        }
        /// <summary>
        /// Coalesce
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? coalesce(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right)
#else
        private System.Linq.Expressions.Expression coalesce(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.Coalesce(left, right) : setNullParameter();
        }
        /// <summary>
        /// AddAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? addAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression addAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.AddAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// AndAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? andAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression andAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.AndAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// DivideAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? divideAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression divideAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.DivideAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// ExclusiveOrAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? exclusiveOrAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression exclusiveOrAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.ExclusiveOrAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// LeftShiftAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? leftShiftAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression leftShiftAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.LeftShiftAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// ModuloAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? moduloAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression moduloAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.ModuloAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// MultiplyAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? multiplyAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression multiplyAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.MultiplyAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// OrAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? orAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression orAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.OrAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// PowerAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? powerAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression powerAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.PowerAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// RightShiftAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? rightShiftAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression rightShiftAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.RightShiftAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// SubtractAssign
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? subtractAssign(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression subtractAssign(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.SubtractAssign(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// AddAssignChecked
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? addAssignChecked(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression addAssignChecked(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.AddAssignChecked(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// MultiplyAssignChecked
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? multiplyAssignChecked(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression multiplyAssignChecked(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.MultiplyAssignChecked(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// SubtractAssignChecked
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? subtractAssignChecked(System.Linq.Expressions.Expression? left, System.Linq.Expressions.Expression? right, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression subtractAssignChecked(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, MethodInfo method)
#endif
        {
            return left != null && right != null ? System.Linq.Expressions.Expression.SubtractAssignChecked(left, right, method) : setNullParameter();
        }
        /// <summary>
        /// TypeIs
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? typeIs(System.Linq.Expressions.Expression? expression, Type? type)
#else
        private System.Linq.Expressions.Expression typeIs(System.Linq.Expressions.Expression expression, Type type)
#endif
        {
            return expression != null && type != null ? System.Linq.Expressions.Expression.TypeIs(expression, type) : setNullParameter();
        }
        /// <summary>
        /// TypeEqual
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? typeEqual(System.Linq.Expressions.Expression? expression, Type? type)
#else
        private System.Linq.Expressions.Expression typeEqual(System.Linq.Expressions.Expression expression, Type type)
#endif
        {
            return expression != null && type != null ? System.Linq.Expressions.Expression.TypeEqual(expression, type) : setNullParameter();
        }
        /// <summary>
        /// Negate
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? negate(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression negate(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.Negate(expression, method) : setNullParameter();
        }
        /// <summary>
        /// UnaryPlus
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? unaryPlus(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression unaryPlus(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.UnaryPlus(expression, method) : setNullParameter();
        }
        /// <summary>
        /// NegateChecked
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? negateChecked(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression negateChecked(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.NegateChecked(expression, method) : setNullParameter();
        }
        /// <summary>
        /// Not
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? not(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression not(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.Not(expression, method) : setNullParameter();
        }
        /// <summary>
        /// Decrement
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? decrement(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression decrement(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.Decrement(expression, method) : setNullParameter();
        }
        /// <summary>
        /// Increment
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? increment(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression increment(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.Increment(expression, method) : setNullParameter();
        }
        /// <summary>
        /// PreIncrementAssign
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? preIncrementAssign(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression preIncrementAssign(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.PreIncrementAssign(expression, method) : setNullParameter();
        }
        /// <summary>
        /// PreDecrementAssign
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? preDecrementAssign(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression preDecrementAssign(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.PreDecrementAssign(expression, method) : setNullParameter();
        }
        /// <summary>
        /// PostIncrementAssign
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? postIncrementAssign(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression postIncrementAssign(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.PostIncrementAssign(expression, method) : setNullParameter();
        }
        /// <summary>
        /// PostDecrementAssign
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? postDecrementAssign(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression postDecrementAssign(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.PostDecrementAssign(expression, method) : setNullParameter();
        }
        /// <summary>
        /// OnesComplement
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? onesComplement(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression onesComplement(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.OnesComplement(expression, method) : setNullParameter();
        }
        /// <summary>
        /// IsTrue
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? isTrue(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression isTrue(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.IsTrue(expression, method) : setNullParameter();
        }
        /// <summary>
        /// IsFalse
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? isFalse(System.Linq.Expressions.Expression? expression, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression isFalse(System.Linq.Expressions.Expression expression, MethodInfo method)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.IsFalse(expression, method) : setNullParameter();
        }
        /// <summary>
        /// ArrayLength
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? arrayLength(System.Linq.Expressions.Expression? expression)
#else
        private System.Linq.Expressions.Expression arrayLength(System.Linq.Expressions.Expression expression)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.ArrayLength(expression) : setNullParameter();
        }
        /// <summary>
        /// Quote
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? quote(System.Linq.Expressions.Expression? expression)
#else
        private System.Linq.Expressions.Expression quote(System.Linq.Expressions.Expression expression)
#endif
        {
            return expression != null ? System.Linq.Expressions.Expression.Quote(expression) : setNullParameter();
        }
        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? convert(System.Linq.Expressions.Expression? expression, Type? type, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression convert(System.Linq.Expressions.Expression expression, Type type, MethodInfo method)
#endif
        {
            return expression != null && type != null ? System.Linq.Expressions.Expression.Convert(expression, type, method) : setNullParameter();
        }
        /// <summary>
        /// ConvertChecked
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? convertChecked(System.Linq.Expressions.Expression? expression, Type? type, MethodInfo? method)
#else
        private System.Linq.Expressions.Expression convertChecked(System.Linq.Expressions.Expression expression, Type type, MethodInfo method)
#endif
        {
            return expression != null && type != null ? System.Linq.Expressions.Expression.ConvertChecked(expression, type, method) : setNullParameter();
        }
        /// <summary>
        /// TypeAs
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? typeAs(System.Linq.Expressions.Expression? expression, Type? type)
#else
        private System.Linq.Expressions.Expression typeAs(System.Linq.Expressions.Expression expression, Type type)
#endif
        {
            return expression != null && type != null ? System.Linq.Expressions.Expression.TypeAs(expression, type) : setNullParameter();
        }
        /// <summary>
        /// Unbox
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? unbox(System.Linq.Expressions.Expression? expression, Type? type)
#else
        private System.Linq.Expressions.Expression unbox(System.Linq.Expressions.Expression expression, Type type)
#endif
        {
            return expression != null && type != null ? System.Linq.Expressions.Expression.Unbox(expression, type) : setNullParameter();
        }
        /// <summary>
        /// Call
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? call(System.Linq.Expressions.Expression? expression, MethodInfo? method, System.Linq.Expressions.Expression[]? parameters)
#else
        private System.Linq.Expressions.Expression call(System.Linq.Expressions.Expression expression, MethodInfo method, System.Linq.Expressions.Expression[] parameters)
#endif
        {
            return method != null && parameters != null ? System.Linq.Expressions.Expression.Call(expression, method, parameters) : setNullParameter();
        }
        /// <summary>
        /// Index
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="property"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? index(System.Linq.Expressions.Expression? expression, PropertyInfo? property, System.Linq.Expressions.Expression[]? parameters)
#else
        private System.Linq.Expressions.Expression index(System.Linq.Expressions.Expression expression, PropertyInfo property, System.Linq.Expressions.Expression[] parameters)
#endif
        {
            return expression != null && parameters != null ? System.Linq.Expressions.Expression.MakeIndex(expression, property, parameters) : setNullParameter();
        }
        /// <summary>
        /// MemberAccess
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? memberAccess(System.Linq.Expressions.Expression? expression, MemberInfo? member)
#else
        private System.Linq.Expressions.Expression memberAccess(System.Linq.Expressions.Expression expression, MemberInfo member)
#endif
        {
            return member != null ? System.Linq.Expressions.Expression.MakeMemberAccess(expression, member) : setNullParameter();
        }
        /// <summary>
        /// Condition
        /// </summary>
        /// <param name="test"></param>
        /// <param name="ifTrue"></param>
        /// <param name="ifFalse"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? condition(System.Linq.Expressions.Expression? test, System.Linq.Expressions.Expression? ifTrue, System.Linq.Expressions.Expression? ifFalse)
#else
        private System.Linq.Expressions.Expression condition(System.Linq.Expressions.Expression test, System.Linq.Expressions.Expression ifTrue, System.Linq.Expressions.Expression ifFalse)
#endif
        {
            return test != null && ifTrue != null && ifFalse != null ? System.Linq.Expressions.Expression.Condition(test, ifTrue, ifFalse) : setNullParameter();
        }
        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? invoke(System.Linq.Expressions.Expression? expression, System.Linq.Expressions.Expression[]? parameters)
#else
        private System.Linq.Expressions.Expression invoke(System.Linq.Expressions.Expression expression, System.Linq.Expressions.Expression[] parameters)
#endif
        {
            return expression != null && parameters != null ? System.Linq.Expressions.Expression.Invoke(expression, parameters) : setNullParameter();
        }
        /// <summary>
        /// Default
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private System.Linq.Expressions.Expression? defaultExpression(Type? type)
#else
        private System.Linq.Expressions.Expression defaultExpression(Type type)
#endif
        {
            return type != null ? System.Linq.Expressions.Expression.Default(type) : setNullParameter();
        }
        /// <summary>
        /// 设置参数反序列化失败错误状态
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private System.Linq.Expressions.Expression? setNullParameter()
#else
        private System.Linq.Expressions.Expression setNullParameter()
#endif
        {
            if (State == RemoteExpressionSerializeStateEnum.Success) setError(RemoteExpressionSerializeStateEnum.NullParameter);
            return null;
        }
    }
}
