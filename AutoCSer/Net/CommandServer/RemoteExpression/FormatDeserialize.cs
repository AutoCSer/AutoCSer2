using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// Format the remote expression deserialization data
    /// 格式化远程表达式反序列化数据
    /// </summary>
    internal unsafe sealed class FormatDeserialize
    {
        /// <summary>
        /// Remote expression server metadata information
        /// 远程表达式服务端元数据信息
        /// </summary>
        private readonly ServerMetadata metadata;
        /// <summary>
        /// 新增类型编号集合
        /// </summary>
        internal LeftArray<int> NewTypes;
        /// <summary>
        /// 新增方法编号集合
        /// </summary>
        internal LeftArray<int> NewMethods;
        /// <summary>
        /// 新增属性编号集合
        /// </summary>
        internal LeftArray<int> NewProperties;
        /// <summary>
        /// 新增字段编号集合
        /// </summary>
        internal LeftArray<int> NewFields;
        /// <summary>
        /// A collection of generic parameter types
        /// 泛型参数类型集合
        /// </summary>
        private Type[] parameterTypes;
        /// <summary>
        /// Binary data deserialization
        /// 二进制数据反序列化
        /// </summary>
#if NetStandard21
        private BinaryDeserializer? deserializer;
#else
        private BinaryDeserializer deserializer;
#endif
        /// <summary>
        /// 当前写入位置
        /// </summary>
        private byte* write;
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
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// 参数表达式
        /// </summary>
#if NetStandard21
        private ParameterExpression? parameter;
#else
        private ParameterExpression parameter;
#endif
        /// <summary>
        /// 委托参数字段信息集合
        /// </summary>
        private FieldInfo[] parameterFields;
        /// <summary>
        /// 常量参数字段信息集合
        /// </summary>
        private FieldInfo[] constantParameterFields;
        /// <summary>
        /// Format the remote expression deserialization data
        /// 格式化远程表达式反序列化数据
        /// </summary>
        /// <param name="metadata">emote expression server metadata information
        /// 远程表达式服务端元数据信息</param>
        internal FormatDeserialize(ServerMetadata metadata)
        {
            this.metadata = metadata;
            parameterTypes = EmptyArray<Type>.Array;
            NewTypes = new LeftArray<int>(0);
            NewMethods = new LeftArray<int>(0);
            NewProperties = new LeftArray<int>(0);
            NewFields = new LeftArray<int>(0);
            constantParameterFields = parameterFields = EmptyArray<FieldInfo>.Array;
        }
        /// <summary>
        /// 格式化表达式序列化数据
        /// </summary>
        /// <param name="serializeInfo"></param>
        /// <param name="deserializer"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        internal RemoteExpressionSerializeStateEnum Format(ref SerializeInfo serializeInfo, BinaryDeserializer deserializer, Type[] parameterTypes)
        {
            byte* start = (byte*)serializeInfo.Key.DeserializeData.Data;
            this.deserializer = deserializer;
            this.parameterTypes = parameterTypes;
            NewTypes.Length = NewMethods.Length = NewProperties.Length = NewFields.Length = 0;
            state = RemoteExpressionSerializeStateEnum.Success;
            write = read = start + sizeof(int);
            end = read + *(int*)start;
            try
            {
                if (writeNode())
                {
                    if (read == end)
                    {
                        int size = (int)(write - start), constantCount = serializeInfo.ConstantParameterCount;
                        *(int*)write = constantCount;
                        *(int*)start = size - sizeof(int);
                        if (constantCount != 0)
                        {
                            read += sizeof(int) * 2;
                            write += sizeof(int) * 2;
                            end = read + *(int*)(read - sizeof(int));
                            byte* writeStart = write;
                            do
                            {
                                if (!writeType()) return state;
                            }
                            while (--constantCount != 0);
                            if (read == end)
                            {
                                *(int*)(writeStart - sizeof(int)) = (int)(write - writeStart);
                                serializeInfo.Set(start, (int)(write - start));
                            }
                            else state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
                        }
                        else serializeInfo.Set(start, size);
                    }
                    else state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
                }
            }
            finally
            {
                if ((NewTypes.Length | NewMethods.Length | NewProperties.Length | NewFields.Length) != 0) metadata.Output(this);
            }
            return state;
        }
        /// <summary>
        /// 格式化表达式节点序列化数据
        /// </summary>
        /// <returns></returns>
        private bool writeNode()
        {
            uint header = *(uint*)read;
            read += sizeof(uint);
            switch ((byte)header)
            {
                case (int)ExpressionType.Add:
                case (int)ExpressionType.AddChecked:
                case (int)ExpressionType.And:
                case (int)ExpressionType.AndAlso:
                case (int)ExpressionType.ArrayIndex:
                case (int)ExpressionType.Divide:
                case (int)ExpressionType.Equal:
                case (int)ExpressionType.ExclusiveOr:
                case (int)ExpressionType.GreaterThan:
                case (int)ExpressionType.GreaterThanOrEqual:
                case (int)ExpressionType.LeftShift:
                case (int)ExpressionType.LessThan:
                case (int)ExpressionType.LessThanOrEqual:
                case (int)ExpressionType.Modulo:
                case (int)ExpressionType.Multiply:
                case (int)ExpressionType.MultiplyChecked:
                case (int)ExpressionType.NotEqual:
                case (int)ExpressionType.Or:
                case (int)ExpressionType.OrElse:
                case (int)ExpressionType.Power:
                case (int)ExpressionType.RightShift:
                case (int)ExpressionType.Subtract:
                case (int)ExpressionType.SubtractChecked:
                case (int)ExpressionType.Assign:
                case (int)ExpressionType.Coalesce:
                case (int)ExpressionType.AddAssign:
                case (int)ExpressionType.AndAssign:
                case (int)ExpressionType.DivideAssign:
                case (int)ExpressionType.ExclusiveOrAssign:
                case (int)ExpressionType.LeftShiftAssign:
                case (int)ExpressionType.ModuloAssign:
                case (int)ExpressionType.MultiplyAssign:
                case (int)ExpressionType.OrAssign:
                case (int)ExpressionType.PowerAssign:
                case (int)ExpressionType.RightShiftAssign:
                case (int)ExpressionType.SubtractAssign:
                case (int)ExpressionType.AddAssignChecked:
                case (int)ExpressionType.MultiplyAssignChecked:
                case (int)ExpressionType.SubtractAssignChecked:
                    return writeHeader(header) && writeNode() && writeNode() && writeMethod(header);
                case (int)ExpressionType.TypeIs:
                case (int)ExpressionType.TypeEqual:
                    return writeHeader(header) && writeNode() && writeType(header);
                case (int)ExpressionType.Negate:
                case (int)ExpressionType.UnaryPlus:
                case (int)ExpressionType.NegateChecked:
                case (int)ExpressionType.Not:
                case (int)ExpressionType.Decrement:
                case (int)ExpressionType.Increment:
                case (int)ExpressionType.PreIncrementAssign:
                case (int)ExpressionType.PreDecrementAssign:
                case (int)ExpressionType.PostIncrementAssign:
                case (int)ExpressionType.PostDecrementAssign:
                case (int)ExpressionType.OnesComplement:
                case (int)ExpressionType.IsTrue:
                case (int)ExpressionType.IsFalse:
                case (int)ExpressionType.ArrayLength:
                case (int)ExpressionType.Quote:
                case (int)ExpressionType.Convert:
                case (int)ExpressionType.ConvertChecked:
                case (int)ExpressionType.TypeAs:
                case (int)ExpressionType.Unbox:
                    return writeHeader(header) && writeNode() && writeType(header) && writeMethod(header);
                case (int)ExpressionType.Call:
                    return writeHeader(header) && writeValue(header) && writeMethod(header) && writeArguments();
                case (int)ExpressionType.Index:
                    return writeHeader(header) && writeValue(header) && writeProperty(header)  && writeArguments();
                case (int)ExpressionType.MemberAccess:
                    return writeHeader(header) && writeValue(header) && writeField(header) && writeProperty(header);
                case (int)ExpressionType.Conditional:
                    return writeHeader(header) && writeNode() && writeNode() && writeNode();
                case (int)ExpressionType.Invoke:
                    return writeHeader(header) && writeNode() && writeArguments();
                case (int)ExpressionType.Default:
                    return writeHeader(header) && writeType(header);
                case (int)ExpressionType.Constant:
                case (int)ExpressionType.Parameter:
                    return writeIndex((int)header);
                default:
                    state = RemoteExpressionSerializeStateEnum.UnknownNodeType;
                    return false;
            }
        }
        /// <summary>
        /// 格式化节点头部数据
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool writeHeader(uint header)
        {
            if (read - write >= sizeof(uint) && read <= end)
            {
                if ((header & (int)NodeHeaderEnum.Type) != 0) header ^= (int)NodeHeaderEnum.Type | (int)NodeHeaderEnum.TypeIndex;
                if ((header & (int)NodeHeaderEnum.Method) != 0) header ^= (int)NodeHeaderEnum.Method | (int)NodeHeaderEnum.MethodIndex;
                if ((header & (int)NodeHeaderEnum.Property) != 0) header ^= (int)NodeHeaderEnum.Property | (int)NodeHeaderEnum.PropertyIndex;
                if ((header & (int)NodeHeaderEnum.Field) != 0) header ^= (int)NodeHeaderEnum.Field | (int)NodeHeaderEnum.FieldIndex;
                *(uint*)write = header;
                write += sizeof(uint);
                return true;
            }
            deserializer.notNull().SetIndexOutOfRange();
            state = RemoteExpressionSerializeStateEnum.FormatWriteIndexOutOfRange;
            return false;
        }
        /// <summary>
        /// 读取并写入元数据索引位置
        /// </summary>
        /// <returns></returns>
        private bool writeIndex()
        {
            if (read >= write && end - read >= sizeof(uint))
            {
                *(int*)write = *(int*)read;
                read += sizeof(int);
                write += sizeof(int);
                return true;
            }
            deserializer.notNull().SetIndexOutOfRange();
            state = RemoteExpressionSerializeStateEnum.FormatWriteIndexOutOfRange;
            return false;
        }
        /// <summary>
        /// 写入元数据索引位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool writeIndex(int index)
        {
            if (read - write >= sizeof(uint) && read <= end)
            {
                *(int*)write = index;
                write += sizeof(int);
                return true;
            }
            deserializer.notNull().SetIndexOutOfRange();
            state = RemoteExpressionSerializeStateEnum.FormatWriteIndexOutOfRange;
            return false;
        }
        /// <summary>
        /// 读取类型信息
        /// </summary>
        /// <returns></returns>
        private KeyValue<Type, int> readType()
        {
            int typeIndex = *(int*)read;
            read += sizeof(int);
            if (typeIndex > 0) return new KeyValue<Type, int>(metadata.Types.Array[typeIndex - 1], typeIndex);
            if (typeIndex != AutoCSer.BinarySerializer.NullValue)
            {
                Type type = parameterTypes[-typeIndex];
                return new KeyValue<Type, int>(type, apppendType(type));
            }
            else
            {
                var type = readRemoteType();
                if (type != null) return new KeyValue<Type, int>(type, apppendType(type));
            }
            return default(KeyValue<Type, int>);
        }
        /// <summary>
        /// 读取远程类型信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private Type? readRemoteType()
#else
        private Type readRemoteType()
#endif
        {
            var assemblyName = readString();
            if (assemblyName != null)
            {
                var typeName = readString();
                if (typeName != null)
                {
                    var parameterType = typeof(Type);
                    if (new AutoCSer.Reflection.RemoteType(assemblyName, typeName).TryGet(out parameterType, false)) return parameterType;
                    state = RemoteExpressionSerializeStateEnum.NotFoundType;
                }
            }
            return null;
        }
        /// <summary>
        /// 添加新类型编号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int apppendType(HashObject<Type> type)
        {
            Dictionary<HashObject<Type>, int> typeIndexs = metadata.TypeIndexs;
            int typeIndex;
            bool isNewType = false;
            Monitor.Enter(typeIndexs);
            try
            {
                if (!typeIndexs.TryGetValue(type, out typeIndex))
                {
                    metadata.Types.PrepLength(1);
                    typeIndexs.Add(type, typeIndex = typeIndexs.Count + 1);
                    metadata.Types.UnsafeAdd(type.Value);
                    isNewType = true;
                }
            }
            finally { Monitor.Exit(typeIndexs); }
            if (isNewType) NewTypes.Add(typeIndex);
            return typeIndex;
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private string? readString()
#else
        private string readString()
#endif
        {
            int type = *(int*)read;
            read += sizeof(int);
            switch ((byte)type)
            {
                case (byte)ConstantTypeEnum.EmptyString: return string.Empty;
                case (byte)ConstantTypeEnum.CharString: return ((char)(type >> 8)).ToString();
                case (byte)ConstantTypeEnum.String:
                    string stringValue = string.Empty;
                    read = AutoCSer.SimpleSerialize.Deserializer.NotNull(read, ref stringValue, end);
                    if (read != null) return stringValue;
                    break;
            }
            state = RemoteExpressionSerializeStateEnum.FormatReadIndexOutOfRange;
            return null;
        }
        /// <summary>
        /// 读取类型数组
        /// </summary>
        /// <param name="count"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
#if NetStandard21
        private Type[]? readTypeArray(int count, out int[] indexs)
#else
        private Type[] readTypeArray(int count, out int[] indexs)
#endif
        {
            if (count != 0)
            {
                Type[] types = new Type[count];
                indexs = AutoCSer.Common.GetUninitializedArray<int>(count);
                for (int index = 0; index != count; ++index)
                {
                    KeyValue<Type, int> type = readType();
                    if (type.Value != 0)
                    {
                        types[index] = type.Key;
                        indexs[index] = type.Value;
                    }
                    else return null;
                }
                return types;
            }
            indexs = EmptyArray<int>.Array;
            return EmptyArray<Type>.Array;
        }
        /// <summary>
        /// 格式化方法信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool writeMethod(uint header)
        {
            if ((header & (int)NodeHeaderEnum.Method) != 0)
            {
                if (*(int*)read == AutoCSer.BinarySerializer.NullValue)
                {
                    read += sizeof(int);
                    KeyValue<Type, int> type = readType();
                    if (type.Value != 0)
                    {
                        var methodName = readString();
                        if (methodName != null)
                        {
                            BindingFlags bindingFlags = (BindingFlags)(*(int*)read);
                            int typeCount = *(int*)(read + sizeof(int));
                            read += sizeof(int) * 2;
                            int[] typeIndexs;
                            var types = readTypeArray(typeCount & 0xff, out typeIndexs);
                            if (types != null)
                            {
                                int[] parameterTypeIndexs;
                                var parameterTypes = readTypeArray(typeCount >> 8, out parameterTypeIndexs);
                                if (parameterTypes != null)
                                {
                                    var method = default(MethodInfo);
                                    if (types.Length == 0) method = type.Key.GetMethod(methodName, bindingFlags, null, parameterTypes, null);
                                    else method = RemoteMetadataMethodIndex.GetMethod(type.Key, methodName, bindingFlags, types, parameterTypes);
                                    if (method != null)
                                    {
                                        RemoteMetadataMethodIndex methodIndex = new RemoteMetadataMethodIndex(methodName, type.Value, bindingFlags, parameterTypeIndexs, typeIndexs);
                                        return writeIndex(metadata.Append(method, ref methodIndex, ref NewMethods));
                                    }
                                    state = RemoteExpressionSerializeStateEnum.NotFoundMethod;
                                }
                            }
                        }
                    }
                }
                else state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
                return false;
            }
            return (header & (int)NodeHeaderEnum.MethodIndex) == 0 || writeIndex();
        }
        /// <summary>
        /// 格式化类型信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool writeType(uint header)
        {
            if ((header & (int)NodeHeaderEnum.Type) != 0)
            {
                if (*(int*)read == AutoCSer.BinarySerializer.NullValue)
                {
                    read += sizeof(int);
                    var type = readRemoteType();
                    if (type != null) return writeIndex(apppendType(type));
                }
                else state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
                return false;
            }
            if ((header & (int)NodeHeaderEnum.TypeIndex) != 0)
            {
                int typeIndex = *(int*)read;
                read += sizeof(int);
                return writeIndex(typeIndex > 0 ? typeIndex : apppendType(parameterTypes[-typeIndex]));
            }
            return true;
        }
        /// <summary>
        /// 格式化类型信息
        /// </summary>
        /// <returns></returns>
        private bool writeType()
        {
            int typeIndex = *(int*)read;
            read += sizeof(int);
            if (typeIndex > 0) return writeIndex(typeIndex);
            if (typeIndex != AutoCSer.BinarySerializer.NullValue) return writeIndex(apppendType(parameterTypes[-typeIndex]));
            var type = readRemoteType();
            return type != null && writeIndex(apppendType(type));
        }
        /// <summary>
        /// 格式化属性信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool writeProperty(uint header)
        {
            if ((header & (int)NodeHeaderEnum.Property) != 0)
            {
                if (*(int*)read == AutoCSer.BinarySerializer.NullValue)
                {
                    read += sizeof(int);
                    KeyValue<Type, int> type = readType();
                    if (type.Value != 0)
                    {
                        var propertyName = readString();
                        if (propertyName != null)
                        {
                            BindingFlags bindingFlags = (BindingFlags)(*(int*)read);
                            var property = type.Key.GetProperty(propertyName, bindingFlags);
                            if (property != null)
                            {
                                read += sizeof(int);
                                return writeIndex(metadata.Append(property, type.Value, ref NewProperties));
                            }
                            state = RemoteExpressionSerializeStateEnum.NotFoundProperty;
                        }
                    }
                }
                else state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
                return false;
            }
            return (header & (int)NodeHeaderEnum.PropertyIndex) == 0 || writeIndex();
        }
        /// <summary>
        /// 格式化字段信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool writeField(uint header)
        {
            if ((header & (int)NodeHeaderEnum.Field) != 0)
            {
                if (*(int*)read == AutoCSer.BinarySerializer.NullValue)
                {
                    read += sizeof(int);
                    KeyValue<Type, int> type = readType();
                    if (type.Value != 0)
                    {
                        var fieldName = readString();
                        if (fieldName != null)
                        {
                            BindingFlags bindingFlags = (BindingFlags)(*(int*)read);
                            var field = type.Key.GetField(fieldName, bindingFlags);
                            if (field != null)
                            {
                                read += sizeof(int);
                                return writeIndex(metadata.Append(field, type.Value, bindingFlags, ref NewFields));
                            }
                            state = RemoteExpressionSerializeStateEnum.NotFoundField;
                        }
                    }
                }
                else state = RemoteExpressionSerializeStateEnum.DeserializeFailed;
                return false;
            }
            return (header & (int)NodeHeaderEnum.FieldIndex) == 0 || writeIndex();
        }
        /// <summary>
        /// 写入 object 表达式数据
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool writeValue(uint header)
        {
            return (header & (int)NodeHeaderEnum.NullValue) != 0 || writeNode();
        }
        /// <summary>
        /// 写入参数集合信息
        /// </summary>
        /// <returns></returns>
        private bool writeArguments()
        {
            int count = *(int*)read;
            if (writeIndex(count))
            {
                read += sizeof(int);
                while(count > 0)
                {
                    if (writeNode()) --count;
                    else return false;
                }
                if (count == 0) return true;
                state = RemoteExpressionSerializeStateEnum.UnknownSerializeInfo;
            }
            return false;
        }
        /// <summary>
        /// 设置表达式反序列化位置
        /// </summary>
        /// <param name="data"></param>
        /// <param name="constantParameterFields"></param>
        internal void SetExpression(byte* data, FieldInfo[] constantParameterFields)
        {
            read = data + sizeof(int);
            this.constantParameterFields = constantParameterFields;
            int index = 0;
            foreach(FieldInfo field in constantParameterFields)
            {
                if (int.Parse(field.Name) != index)
                {
                    this.constantParameterFields = new FieldInfo[constantParameterFields.Length];
                    foreach (FieldInfo constantParameterField in constantParameterFields)
                    {
                        this.constantParameterFields[int.Parse(constantParameterField.Name)] = constantParameterField;
                    }
                    return;
                }
                ++index;
            }
        }
        /// <summary>
        /// 创建表达式
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterFields"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Expression CreateExpression(ParameterExpression? parameter, FieldInfo[] parameterFields)
#else
        internal Expression CreateExpression(ParameterExpression parameter, FieldInfo[] parameterFields)
#endif
        {
            this.parameter = parameter;
            this.parameterFields = parameterFields;
            return createNode();
        }
        /// <summary>
        /// 创建表达式
        /// </summary>
        /// <returns></returns>
        private Expression createNode()
        {
            uint header = *(uint*)read;
            read += sizeof(uint);
            switch ((byte)header)
            {
                case (int)ExpressionType.Add: return Expression.Add(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.AddChecked: return Expression.AddChecked(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.And: return Expression.And(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.AndAlso: return Expression.AndAlso(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.ArrayIndex: return Expression.ArrayIndex(createNode(), createNode());
                case (int)ExpressionType.Divide: return Expression.Divide(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.Equal: return Expression.Equal(createNode(), createNode(), false, readMethod(header));
                case (int)ExpressionType.ExclusiveOr: return Expression.ExclusiveOr(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.GreaterThan: return Expression.GreaterThan(createNode(), createNode(), false, readMethod(header));
                case (int)ExpressionType.GreaterThanOrEqual: return Expression.GreaterThanOrEqual(createNode(), createNode(), false, readMethod(header));
                case (int)ExpressionType.LeftShift: return Expression.LeftShift(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.LessThan: return Expression.LessThan(createNode(), createNode(), false, readMethod(header));
                case (int)ExpressionType.LessThanOrEqual: return Expression.LessThanOrEqual(createNode(), createNode(), false, readMethod(header));
                case (int)ExpressionType.Modulo: return Expression.Modulo(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.Multiply: return Expression.Multiply(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.MultiplyChecked: return Expression.MultiplyChecked(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.NotEqual: return Expression.NotEqual(createNode(), createNode(), false, readMethod(header));
                case (int)ExpressionType.Or: return Expression.Or(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.OrElse: return Expression.OrElse(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.Power: return Expression.Power(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.RightShift: return Expression.RightShift(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.Subtract: return Expression.Subtract(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.SubtractChecked: return Expression.SubtractChecked(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.Assign: return Expression.Assign(createNode(), createNode());
                case (int)ExpressionType.Coalesce: return Expression.Coalesce(createNode(), createNode());
                case (int)ExpressionType.AddAssign: return Expression.AddAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.AndAssign: return Expression.AndAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.DivideAssign: return Expression.DivideAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.ExclusiveOrAssign: return Expression.ExclusiveOrAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.LeftShiftAssign: return Expression.LeftShiftAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.ModuloAssign: return Expression.ModuloAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.MultiplyAssign: return Expression.MultiplyAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.OrAssign: return Expression.OrAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.PowerAssign: return Expression.PowerAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.RightShiftAssign: return Expression.RightShiftAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.SubtractAssign: return Expression.SubtractAssign(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.AddAssignChecked: return Expression.AddAssignChecked(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.MultiplyAssignChecked: return Expression.MultiplyAssignChecked(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.SubtractAssignChecked: return Expression.SubtractAssignChecked(createNode(), createNode(), readMethod(header));
                case (int)ExpressionType.TypeIs: return Expression.TypeIs(createNode(), readType(header).notNull());
                case (int)ExpressionType.TypeEqual: return Expression.TypeEqual(createNode(), readType(header).notNull());
                case (int)ExpressionType.Negate: return Expression.Negate(createNode(), readMethod(header));
                case (int)ExpressionType.UnaryPlus: return Expression.UnaryPlus(createNode(), readMethod(header));
                case (int)ExpressionType.NegateChecked: return Expression.NegateChecked(createNode(), readMethod(header));
                case (int)ExpressionType.Not: return Expression.Not(createNode(), readMethod(header));
                case (int)ExpressionType.Decrement: return Expression.Decrement(createNode(), readMethod(header));
                case (int)ExpressionType.Increment: return Expression.Increment(createNode(), readMethod(header));
                case (int)ExpressionType.PreIncrementAssign: return Expression.PreIncrementAssign(createNode(), readMethod(header));
                case (int)ExpressionType.PreDecrementAssign: return Expression.PreDecrementAssign(createNode(), readMethod(header));
                case (int)ExpressionType.PostIncrementAssign: return Expression.PostIncrementAssign(createNode(), readMethod(header));
                case (int)ExpressionType.PostDecrementAssign: return Expression.PostDecrementAssign(createNode(), readMethod(header));
                case (int)ExpressionType.OnesComplement: return Expression.OnesComplement(createNode(), readMethod(header));
                case (int)ExpressionType.IsTrue: return Expression.IsTrue(createNode(), readMethod(header));
                case (int)ExpressionType.IsFalse: return Expression.IsFalse(createNode(), readMethod(header));
                case (int)ExpressionType.ArrayLength: return Expression.ArrayLength(createNode());
                case (int)ExpressionType.Quote: return Expression.Quote(createNode());
                case (int)ExpressionType.Convert: return Expression.Convert(createNode(), readType(header).notNull(), readMethod(header));
                case (int)ExpressionType.ConvertChecked: return Expression.ConvertChecked(createNode(), readType(header).notNull(), readMethod(header));
                case (int)ExpressionType.TypeAs: return Expression.TypeAs(createNode(), readType(header).notNull());
                case (int)ExpressionType.Unbox: return Expression.Unbox(createNode(), readType(header).notNull());
                case (int)ExpressionType.Call: return Expression.Call(readValue(header), readMethod(header).notNull(), readArguments());
                case (int)ExpressionType.Index: return Expression.MakeIndex(readValue(header).notNull(), readProperty(header), readArguments());
                case (int)ExpressionType.MemberAccess: return Expression.MakeMemberAccess(readValue(header), readMember(header));
                case (int)ExpressionType.Conditional: return Expression.Condition(createNode(), createNode(), createNode());
                case (int)ExpressionType.Invoke: return Expression.Invoke(createNode(), readArguments());
                case (int)ExpressionType.Default: return Expression.Default(readType(header).notNull());
                case (int)ExpressionType.Constant:
                    return Expression.MakeMemberAccess(Expression.MakeMemberAccess(parameter, parameterFields[parameterFields.Length - 1]), constantParameterFields[header >> 8]);
                case (int)ExpressionType.Parameter: return Expression.MakeMemberAccess(parameter, parameterFields[header >> 8]);
                default: throw new NotSupportedException(((ExpressionType)(byte)header).ToString());
            }
        }
        /// <summary>
        /// 读取参数集合
        /// </summary>
        /// <returns></returns>
        private Expression[] readArguments()
        {
            int count = *(int*)read;
            read += sizeof(int);
            if (count != 0)
            {
                Expression[] expressions = new Expression[count];
                for (int index = 0; index != count; expressions[index++] = createNode()) ;
                return expressions;
            }
            return EmptyArray<Expression>.Array;
        }
        /// <summary>
        /// 读取 object 表达式
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private Expression? readValue(uint header)
#else
        private Expression readValue(uint header)
#endif
        {
            return (header & (int)NodeHeaderEnum.NullValue) != 0 ? null: createNode();
        }
        /// <summary>
        /// 读取类型信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private Type? readType(uint header)
#else
        private Type readType(uint header)
#endif
        {
            if ((header & (int)NodeHeaderEnum.TypeIndex) != 0)
            {
                int index = *(int*)read - 1;
                read += sizeof(int);
                return metadata.Types.Array[index];
            }
            return null;
        }
        /// <summary>
        /// 读取方法信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private MethodInfo? readMethod(uint header)
#else
        private MethodInfo readMethod(uint header)
#endif
        {
            if ((header & (int)NodeHeaderEnum.MethodIndex) != 0)
            {
                int index = *(int*)read;
                read += sizeof(int);
                return metadata.Methods.Array[index].Key;
            }
            return null;
        }
        /// <summary>
        /// 读取属性信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
#if NetStandard21
        private PropertyInfo? readProperty(uint header)
#else
        private PropertyInfo readProperty(uint header)
#endif
        {
            if ((header & (int)NodeHeaderEnum.PropertyIndex) != 0)
            {
                int index = *(int*)read;
                read += sizeof(int);
                return metadata.Properties.Array[index].Key;
            }
            return null;
        }
        /// <summary>
        /// 读取成员信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private MemberInfo readMember(uint header)
        {
            int index = *(int*)read;
            read += sizeof(int);
            if ((header & (int)NodeHeaderEnum.PropertyIndex) != 0) return metadata.Properties.Array[index].Key;
            return metadata.Fields.Array[index].Key;
        }
    }
}
