using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// Remote expression client metadata information
    /// 远程表达式客户端元数据信息
    /// </summary>
    internal sealed partial class ClientMetadata
    {
        /// <summary>
        /// Output buffer stream
        /// 序列化输出缓冲区流
        /// </summary>
        internal UnmanagedStream Stream;
        /// <summary>
        /// Binary data serialization
        /// 二进制数据序列化
        /// </summary>
        private BinarySerializer serializer;
        /// <summary>
        /// 委托泛型参数类型集合
        /// </summary>
        private Type[] parameterTypes = EmptyArray<Type>.Array;
        /// <summary>
        /// 表达式参数集合
        /// </summary>
        private ReadOnlyCollection<ParameterExpression> parameters;
        /// <summary>
        /// 未知类型编号集合
        /// </summary>
        private readonly ReusableDictionary<HashObject<Type>, int> typeIndexs;
        /// <summary>
        /// 常量参数编号集合
        /// </summary>
        private readonly ReusableDictionary<ConstantParameter, int> constantParameterIndexs;
        /// <summary>
        /// 常量参数集合
        /// </summary>
        private LeftArray<ConstantParameter> constantParameters;
        /// <summary>
        /// Remote expression serialization status
        /// 远程表达式序列化状态
        /// </summary>
        internal RemoteExpressionSerializeStateEnum State;
        /// <summary>
        /// 是否所有元数据都是编号方式
        /// </summary>
        private bool isMetadataIndex;
        /// <summary>
        /// 是否需要检查常量表达式
        /// </summary>
        private bool isCheckConstant;
        /// <summary>
        /// 是否已经发送获取远程元数据命令
        /// </summary>
        private bool isCommand;

        /// <summary>
        /// 远程表达式元数据信息编号回调委托集合
        /// </summary>
        private AutoCSer.Threading.LinkStack<RemoteMetadataCallback> callbacks;
        /// <summary>
        /// 远程类型编号集合
        /// </summary>
        private readonly Dictionary<HashObject<Type>, int> types;
        /// <summary>
        /// 远程类型集合
        /// </summary>
        private LeftArray<Type> typeArray;
        /// <summary>
        /// 远程方法编号集合
        /// </summary>
        private readonly Dictionary<HashObject<MethodInfo>, int> methods;
        /// <summary>
        /// 远程方法集合
        /// </summary>
        private LeftArray<MethodInfo> methodArray;
        /// <summary>
        /// 远程属性编号集合
        /// </summary>
        private readonly Dictionary<HashObject<PropertyInfo>, int> properties;
        /// <summary>
        /// 远程属性集合
        /// </summary>
        private LeftArray<PropertyInfo> propertyArray;
        /// <summary>
        /// 远程字段编号集合
        /// </summary>
        private readonly Dictionary<HashObject<FieldInfo>, int> fields;
        /// <summary>
        /// 远程字段集合
        /// </summary>
        private LeftArray<FieldInfo> fieldArray;
        /// <summary>
        /// Remote expression client metadata information
        /// 远程表达式客户端元数据信息
        /// </summary>
        /// <param name="isNull"></param>
        internal ClientMetadata(bool isNull)
        {
            Stream = UnmanagedStream.Null;
            serializer = CommandServerConfigBase.NullBinarySerializer;
            types = DictionaryCreator.CreateHashObject<Type, int>();
            methods = DictionaryCreator.CreateHashObject<MethodInfo, int>();
            properties = DictionaryCreator.CreateHashObject<PropertyInfo, int>();
            fields = DictionaryCreator.CreateHashObject<FieldInfo, int>();
            typeArray = new LeftArray<Type>(0);
            methodArray = new LeftArray<MethodInfo>(0);
            propertyArray = new LeftArray<PropertyInfo>(0);
            fieldArray = new LeftArray<FieldInfo>(0);
            typeIndexs = new ReusableDictionary<HashObject<Type>, int>(isNull ? -1 : 0);
            constantParameterIndexs = new ReusableDictionary<ConstantParameter, int>(isNull ? -1 : 0);
            constantParameters = new LeftArray<ConstantParameter>(0);
            this.parameters = EmptyArray<ParameterExpression>.Array.AsReadOnly();
        }
        /// <summary>
        /// 是否发送获取远程元数据命令
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool GetIsCommand()
        {
            if (isCommand) return false;
            return isCommand = true;
        }
        /// <summary>
        /// Remote metadata callback
        /// 远程元数据回调
        /// </summary>
        /// <param name="remoteMetadataOutputData"></param>
        internal void Callback(ref RemoteMetadataOutputData remoteMetadataOutputData)
        {
            LeftArray<KeyValue<Type, int>> types = new LeftArray<KeyValue<Type, int>>(0);
            LeftArray<KeyValue<MethodInfo, int>> methods = new LeftArray<KeyValue<MethodInfo, int>>(0);
            LeftArray<KeyValue<PropertyInfo, int>> properties = new LeftArray<KeyValue<PropertyInfo, int>>(0);
            LeftArray<KeyValue<FieldInfo, int>> fields = new LeftArray<KeyValue<FieldInfo, int>>(0);
            try
            {
                if (remoteMetadataOutputData.TypeIndexs.Length != 0)
                {
                    types.PrepLength(remoteMetadataOutputData.TypeIndexs.Length);
                    foreach (RemoteMetadataTypeIndex typeIndex in remoteMetadataOutputData.TypeIndexs)
                    {
                        var type = typeIndex.GetType();
                        if (type != null) types.Add(new KeyValue<Type, int>(type, typeIndex.Index));
                    }
                }
                if (remoteMetadataOutputData.MethodIndexs.Length != 0)
                {
                    methods.PrepLength(remoteMetadataOutputData.MethodIndexs.Length);
                    foreach (RemoteMetadataMethodIndex methodIndex in remoteMetadataOutputData.MethodIndexs)
                    {
                        var method = methodIndex.GetMethod(this, ref types);
                        if(method != null) methods.Add(new KeyValue<MethodInfo, int>(method, methodIndex.Index));
                    }
                }
                if (remoteMetadataOutputData.PropertyIndexs.Length != 0)
                {
                    properties.PrepLength(remoteMetadataOutputData.PropertyIndexs.Length);
                    foreach (RemoteMetadataMemberIndex memberIndex in remoteMetadataOutputData.PropertyIndexs)
                    {
                        var property = memberIndex.GetProperty(this, ref types);
                        if (property != null) properties.Add(new KeyValue<PropertyInfo, int>(property, memberIndex.Index));
                    }
                }
                if (remoteMetadataOutputData.FieldIndexs.Length != 0)
                {
                    fields.PrepLength(remoteMetadataOutputData.FieldIndexs.Length);
                    foreach (RemoteMetadataMemberIndex memberIndex in remoteMetadataOutputData.FieldIndexs)
                    {
                        var field = memberIndex.GetField(this, ref types);
                        if (field != null) fields.Add(new KeyValue<FieldInfo, int>(field, memberIndex.Index));
                    }
                }
            }
            finally
            {
                callbacks.Push(new RemoteMetadataCallback(types.ToArray(), methods.ToArray(), properties.ToArray(), fields.ToArray()));
            }
        }
        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="typeIndex"></param>
        /// <param name="types"></param>
        /// <returns></returns>
#if NetStandard21
        internal Type? GetType(int typeIndex, ref LeftArray<KeyValue<Type, int>> types)
#else
        internal Type GetType(int typeIndex, ref LeftArray<KeyValue<Type, int>> types)
#endif
        {
            if (typeIndex < typeArray.Length) return typeArray.Array[typeIndex];
            int count = types.Length;
            if (count != 0)
            {
                foreach(KeyValue<Type, int> type in types)
                {
                    if (type.Value >= typeIndex) return type.Value == typeIndex ? type.Key : null;
                    if (--count == 0) return null;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取类型数组
        /// </summary>
        /// <param name="typeIndexs"></param>
        /// <param name="types"></param>
        /// <returns></returns>
#if NetStandard21
        internal Type[]? GetTypeArray(int[] typeIndexs, ref LeftArray<KeyValue<Type, int>> types)
#else
        internal Type[] GetTypeArray(int[] typeIndexs, ref LeftArray<KeyValue<Type, int>> types)
#endif
        {
            if (typeIndexs.Length == 0) return EmptyArray<Type>.Array;
            int index = 0;
            Type[] typeArray = new Type[typeIndexs.Length];
            foreach (int typeIndex in typeIndexs)
            {
                var type = GetType(typeIndex, ref types);
                if (type != null) typeArray[index++] = type;
                else return null;
            }
            return typeArray;
        }
        /// <summary>
        /// Remote metadata callback
        /// 远程元数据回调
        /// </summary>
        /// <param name="callback"></param>
        internal void Callback(RemoteMetadataCallback callback)
        {
            foreach (KeyValue<Type, int> type in callback.Types)
            {
                types[type.Key] = type.Value;
                typeArray.Set(type.Value, type.Key);
            }
            foreach (KeyValue<MethodInfo, int> method in callback.Methods)
            {
                methods[method.Key] = method.Value;
                methodArray.Set(method.Value, method.Key);
            }
            foreach (KeyValue<PropertyInfo, int> property in callback.Properties)
            {
                properties[property.Key] = property.Value;
                propertyArray.Set(property.Value, property.Key);
            }
            foreach (KeyValue<FieldInfo, int> field in callback.Fields)
            {
                fields[field.Key] = field.Value;
                fieldArray.Set(field.Value, field.Key);
            }
        }
        /// <summary>
        /// 执行远程表达式元数据信息编号回调委托
        /// </summary>
        private void callback()
        {
            var callback = callbacks.Get().notNull();
            do
            {
                callback = callback.Callback(this);
            }
            while (callback != null);
        }
        /// <summary>
        /// 获取远程类型编号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        private TypeIndex getTypeIndex(Type? type)
#else
        private TypeIndex getTypeIndex(Type type)
#endif
        {
            if (type != null)
            {
                if (!callbacks.IsEmpty) callback();
                int index;
                if (types.TryGetValue(type, out index)) return new TypeIndex(index);
                isMetadataIndex = false;
                index = 0;
                foreach (Type parameterType in parameterTypes)
                {
                    if (type == parameterType) return new TypeIndex(index);
                    --index;
                }
            }
            return new TypeIndex(type);
        }
        /// <summary>
        /// 添加未知类型编号
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendTypeIndex(Type type)
        {
            typeIndexs[type] = -parameterTypes.Length - typeIndexs.Count;
        }
        /// <summary>
        /// 序列化类型信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Serialize(Type type)
        {
            return getTypeIndex(type).Serialize(this);
        }
        /// <summary>
        /// 获取远程方法编号
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
#if NetStandard21
        private MethodIndex getMethodIndex(MethodInfo? method)
#else
        private MethodIndex getMethodIndex(MethodInfo method)
#endif
        {
            if (method != null)
            {
                if (!callbacks.IsEmpty) callback();
                int index;
                if (methods.TryGetValue(method, out index)) return new MethodIndex(index);
                isMetadataIndex = false;
            }
            return new MethodIndex(method);
        }
        /// <summary>
        /// 获取远程属性编号
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
#if NetStandard21
        private PropertyIndex getPropertyIndex(PropertyInfo? property)
#else
        private PropertyIndex getPropertyIndex(PropertyInfo property)
#endif
        {
            if (property != null)
            {
                if (!callbacks.IsEmpty) callback();
                int index;
                if (properties.TryGetValue(property, out index)) return new PropertyIndex(index);
                isMetadataIndex = false;
            }
            return new PropertyIndex(property);
        }
        /// <summary>
        /// 获取远程字段编号
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private FieldIndex getFieldIndex(FieldInfo field)
        {
            if (!callbacks.IsEmpty) callback();
            int index;
            if (fields.TryGetValue(field, out index)) return new FieldIndex(index);
            isMetadataIndex = false;
            return new FieldIndex(field);
        }
        /// <summary>
        /// 序列化表达式
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="expression"></param>
        /// <param name="parameters"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        internal unsafe bool Serialize(BinarySerializer serializer, Type[] parameterTypes, ReadOnlyCollection<ParameterExpression> parameters, Expression expression)
        {
            UnmanagedStream stream = serializer.Stream;
            this.serializer = serializer;
            this.parameterTypes = parameterTypes;
            this.parameters = parameters;
            typeIndexs.ClearCount();
            constantParameterIndexs.ClearCount();
            constantParameters.Length = 0;
            State = RemoteExpressionSerializeStateEnum.Success;
            isCheckConstant = isMetadataIndex = true;
            Stream = stream;
            int startIndex = stream.Data.Pointer.CurrentIndex;
            if (serializeNode(expression))
            {
                stream.Data.Pointer.WriteSizeData(startIndex);

                int constantCount = constantParameters.Length;
                if (constantCount != 0)
                {
                    int constantTypeStartIndex = stream.GetMoveSize(sizeof(int) * 2);
                    if (constantTypeStartIndex != 0)
                    {
                        *(int*)(stream.Data.Pointer.Byte + (constantTypeStartIndex - sizeof(int) * 2)) = constantCount;
                        foreach (ConstantParameter constantParameter in constantParameters.Array)
                        {
                            if (Serialize(constantParameter.Type))
                            {
                                if (--constantCount == 0)
                                {
                                    stream.Data.Pointer.WriteSizeData(constantTypeStartIndex);
                                    if (writeHashCode(startIndex))
                                    {
                                        ConstantParameterSerializer.Get(new ConstantParameterKey(ref constantParameters)).Serializer(this);
                                        return !stream.IsResizeError;
                                    }
                                    return false;
                                }
                            }
                            else return false;
                        }
                    }
                }
                else return stream.Write(0) && writeHashCode(startIndex);
            }
            return false;
        }
        /// <summary>
        /// 写入表达式序列化数据的哈希值
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private unsafe bool writeHashCode(int startIndex)
        {
            if (isMetadataIndex)
            {
                ulong hashCode64 = Stream.Data.Pointer.GetHashCode64(startIndex - sizeof(int));
                uint hashCode = (uint)(hashCode64 ^ (hashCode64 >> 32));
                return Stream.Write(hashCode | hashCode.logicalInversion());
            }
            return Stream.Write(0);
        }
        /// <summary>
        /// 序列化表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serializeNode(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Assign:
                    return serialize((BinaryExpression)expression);
                case ExpressionType.Coalesce:
                case ExpressionType.AddAssign:
                case ExpressionType.AndAssign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.OrAssign:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.SubtractAssignChecked:
                    return serializeCheckConversion((BinaryExpression)expression);
                case ExpressionType.ArrayIndex:
                    return serializeArrayIndex(expression as BinaryExpression);
                case ExpressionType.TypeIs:
                case ExpressionType.TypeEqual:
                    return serialize((TypeBinaryExpression)expression);
                case ExpressionType.Negate:
                case ExpressionType.UnaryPlus:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.OnesComplement:
                case ExpressionType.IsTrue:
                case ExpressionType.IsFalse:
                    return serialize((UnaryExpression)expression);
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                    return serialize((UnaryExpression)expression, false, false);
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return serialize((UnaryExpression)expression, true, true);
                case ExpressionType.TypeAs:
                case ExpressionType.Unbox:
                    return serialize((UnaryExpression)expression, false, true);
                case ExpressionType.Call:
                    return serialize((MethodCallExpression)expression);
                case ExpressionType.Index:
                    return serialize((IndexExpression)expression);
                case ExpressionType.Constant:
                    return serialize((ConstantExpression)expression);
                case ExpressionType.MemberAccess:
                    return serialize((MemberExpression)expression);
                case ExpressionType.Conditional:
                    return serialize((ConditionalExpression)expression);
                case ExpressionType.Invoke:
                    return serialize((InvocationExpression)expression);
                case ExpressionType.Default:
                    return serialize((DefaultExpression)expression);
                case ExpressionType.Parameter:
                    return serializeParameter(expression);
                case ExpressionType.New:
                case ExpressionType.NewArrayInit:
                case ExpressionType.ListInit:
                case ExpressionType.MemberInit:
                case ExpressionType.NewArrayBounds:
                    State = RemoteExpressionSerializeStateEnum.NotSupportNew;
                    return false;
                case ExpressionType.Lambda:
                case ExpressionType.RuntimeVariables:
                    State = RemoteExpressionSerializeStateEnum.NotSupportLambda;
                    return false;
                case ExpressionType.Block:
                    State = RemoteExpressionSerializeStateEnum.NotSupportBlock;
                    return false;
                case ExpressionType.DebugInfo:
                    State = RemoteExpressionSerializeStateEnum.NotSupportDebugInfo;
                    return false;
                case ExpressionType.Dynamic:
                    State = RemoteExpressionSerializeStateEnum.NotSupportDynamic;
                    return false;
                case ExpressionType.Extension:
                    State = RemoteExpressionSerializeStateEnum.NotSupportExtension;
                    return false;
                case ExpressionType.Label:
                case ExpressionType.Goto:
                case ExpressionType.Loop:
                case ExpressionType.Switch:
                case ExpressionType.Try:
                case ExpressionType.Throw:
                    State = RemoteExpressionSerializeStateEnum.NotSupportLabel;
                    return false;
                default:
                    State = RemoteExpressionSerializeStateEnum.UnknownNodeType;
                    return false;
            }
        }
        /// <summary>
        /// 方法调用参数序列化节点
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private bool serialize(ReadOnlyCollection<Expression> arguments)
        {
            if (Stream.Write(arguments.Count))
            {
                foreach (Expression expression in arguments)
                {
                    if (!serializeNode(expression)) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 序列化常量表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool serialize(ConstantExpression expression)
        {
            return serialize(expression.Type, expression.Value);
        }
        /// <summary>
        /// 序列化常量表达式节点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private bool serialize(Type type, object? value)
#else
        private bool serialize(Type type, object value)
#endif
        {
            int index = constantParameters.Length;
            if (value != null)
            {
                ConstantParameter constantParameter = new ConstantParameter(type, value);
                if ((index & 0x7fff0000) == 0)
                {
                    if (constantParameterIndexs.TryAdd(constantParameter, index)) constantParameters.Add(constantParameter);
                    else index = constantParameterIndexs[constantParameter];
                    return Stream.Write((int)ExpressionType.Constant | (index << 8));
                }
                if (constantParameterIndexs.TryGetValue(constantParameter, out index)) return Stream.Write((int)ExpressionType.Constant | (index << 8));
            }
            else if ((index & 0x7fff0000) == 0)
            {
                constantParameters.Add(new ConstantParameter(type));
                return Stream.Write((int)ExpressionType.Constant | (index << 8));
            }
            State = RemoteExpressionSerializeStateEnum.TooManyConstant;
            return false;
        }
        /// <summary>
        /// 序列化二元表达式节点（检查 Lambda 转换参数）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serializeCheckConversion(BinaryExpression expression)
        {
            if (expression.Conversion == null) return serialize(expression);
            State = RemoteExpressionSerializeStateEnum.NotSupportLambda;
            return false;
        }
        /// <summary>
        /// 序列化二元表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(BinaryExpression expression)
        {
            MethodIndex methodIndex = getMethodIndex(expression.Method);
            return Stream.Write((int)expression.NodeType | methodIndex.NodeHeader) && serializeNode(expression.Left) && serializeNode(expression.Right) && methodIndex.Serialize(this);
        }
        /// <summary>
        /// 序列化数组索引调用节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
#if NetStandard21
        private bool serializeArrayIndex(BinaryExpression? expression)
#else
        private bool serializeArrayIndex(BinaryExpression expression)
#endif
        {
            if (expression != null) return serialize(expression);
            State = RemoteExpressionSerializeStateEnum.NotSupportArrayIndex;
            return false;
        }
        /// <summary>
        /// 序列化一元表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isMethod"></param>
        /// <param name="isType"></param>
        /// <returns></returns>
        private bool serialize(UnaryExpression expression, bool isMethod = true, bool isType = false)
        {
            TypeIndex typeIndex = getTypeIndex(isType ? expression.Type : null);
            MethodIndex methodIndex = getMethodIndex(isMethod ? expression.Method : null);
            return Stream.Write((int)expression.NodeType | typeIndex.NodeHeader | methodIndex.NodeHeader) && serializeNode(expression.Operand) && typeIndex.Serialize(this) && methodIndex.Serialize(this);
        }
        /// <summary>
        /// 序列化类型判断表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(TypeBinaryExpression expression)
        {
            TypeIndex typeIndex = getTypeIndex(expression.TypeOperand);
            return Stream.Write((int)expression.NodeType | typeIndex.NodeHeader) && serializeNode(expression.Expression) && typeIndex.Serialize(this);
        }
        /// <summary>
        /// 序列化方法调用表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(MethodCallExpression expression)
        {
            var value = expression.Object;
            MethodIndex methodIndex = getMethodIndex(expression.Method);
            if (value != null)
            {
                return Stream.Write((int)expression.NodeType | methodIndex.NodeHeader) && serializeNode(value) && methodIndex.Serialize(this) && serialize(expression.Arguments);
            }
            return Stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.NullValue | methodIndex.NodeHeader) && methodIndex.Serialize(this) && serialize(expression.Arguments);
        }
        /// <summary>
        /// 序列化索引调用节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(IndexExpression expression)
        {
            var value = expression.Object;
            PropertyIndex propertyIndex = getPropertyIndex(expression.Indexer);
            if (value != null)
            {
                return Stream.Write((int)expression.NodeType | propertyIndex.NodeHeader) && serializeNode(value) && propertyIndex.Serialize(this) && serialize(expression.Arguments);
            }
            return Stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.NullValue | propertyIndex.NodeHeader) && propertyIndex.Serialize(this) && serialize(expression.Arguments);
        }
        /// <summary>
        /// 序列化 Invoke 节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(InvocationExpression expression)
        {
            return Stream.Write((int)expression.NodeType) && serializeNode(expression.Expression) && serialize(expression.Arguments);
        }
        /// <summary>
        /// 序列化类型默认值表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(DefaultExpression expression)
        {
            TypeIndex typeIndex = getTypeIndex(expression.Type);
            return Stream.Write((int)expression.NodeType | typeIndex.NodeHeader) && typeIndex.Serialize(this);
        }
        /// <summary>
        /// 序列化条件表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(ConditionalExpression expression)
        {
            return Stream.Write((int)expression.NodeType) && serializeNode(expression.Test) && serializeNode(expression.IfTrue) && serializeNode(expression.IfFalse);
        }
        /// <summary>
        /// 序列化成员表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(MemberExpression expression)
        {
            if (isCheckConstant)
            {
                var value = getConstantParameter(expression);
                if (value.Type != null) return serialize(value.Type, value.Value);
                if (State == RemoteExpressionSerializeStateEnum.Success)
                {
                    isCheckConstant = false;
                    bool isNode = write(expression);
                    isCheckConstant = true;
                    return isNode;
                }
                return false;
            }
            return write(expression);
        }
        /// <summary>
        /// 获取常量参数值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private ConstantParameter getConstantParameter(MemberExpression expression)
        {
            var targetExpression = expression.Expression;
            if (targetExpression != null)
            {
                switch (targetExpression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        var value = getConstantParameter((MemberExpression)targetExpression);
                        return value.Type != null ? getConstantParameter(expression.Member, value.Value) : value;
                    case ExpressionType.Constant:
                        return getConstantParameter(expression.Member, ((ConstantExpression)targetExpression).Value);
                    default: return default(ConstantParameter);
                }
            }
            return default(ConstantParameter);
            //MemberInfo member = expression.Member;
            //var property = member as PropertyInfo;
            //if (property != null)
            //{
            //    Type type = property.PropertyType;
            //    if (type == typeof(DateTime))
            //    {
            //        if (property.ReflectedType == typeof(DateTime))
            //        {
            //            string name = property.Name;
            //            switch (name.Length - 3)
            //            {
            //                case 3 - 3:
            //                    if (name == nameof(DateTime.Now)) return default(ConstantParameter);
            //                    break;
            //                case 6 - 3:
            //                    if (name == nameof(DateTime.UtcNow)) return default(ConstantParameter);
            //                    break;
            //            }
            //        }
            //    }
            //    return new ConstantParameter(type, property.GetValue(null));
            //}
            //FieldInfo field = (FieldInfo)member;
            //return new ConstantParameter(field.FieldType, field.GetValue(null));
        }
        /// <summary>
        /// 获取常量参数值
        /// </summary>
        /// <param name="member"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private ConstantParameter getConstantParameter(MemberInfo member, object? target)
#else
        private ConstantParameter getConstantParameter(MemberInfo member, object target)
#endif
        {
            if (target != null) return GetConstantParameter(member, target);
            State = RemoteExpressionSerializeStateEnum.ConstantNullReference;
            return default(ConstantParameter);
        }
        /// <summary>
        /// 获取常量参数
        /// </summary>
        /// <param name="member"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static ConstantParameter GetConstantParameter(MemberInfo member, object target)
        {
            var property = member as PropertyInfo;
            if (property != null) return new ConstantParameter(property.PropertyType, property.GetValue(target));
            FieldInfo field = (FieldInfo)member;
            return new ConstantParameter(field.FieldType, field.GetValue(target));
        }
        /// <summary>
        /// 序列化成员表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool write(MemberExpression expression)
        {
            MemberInfo member = expression.Member;
            var value = expression.Expression;
            var property = member as PropertyInfo;
            if (property != null)
            {
                PropertyIndex propertyIndex = getPropertyIndex(property);
                if (value != null)
                {
                    return Stream.Write((int)expression.NodeType | propertyIndex.NodeHeader) && serializeNode(value) && propertyIndex.Serialize(this);
                }
                return Stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.NullValue | propertyIndex.NodeHeader) && propertyIndex.Serialize(this);
            }
            FieldIndex fieldIndex = getFieldIndex((FieldInfo)member);
            if (value != null)
            {
                return Stream.Write((int)expression.NodeType | fieldIndex.NodeHeader) && serializeNode(value) && fieldIndex.Serialize(this);
            }
            return Stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.NullValue | fieldIndex.NodeHeader) && fieldIndex.Serialize(this);
        }
        /// <summary>
        /// 序列化参数节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serializeParameter(Expression expression)
        {
            int index = 0;
            foreach (ParameterExpression parameterExpression in parameters)
            {
                if (object.ReferenceEquals(parameterExpression, expression)) return Stream.Write((int)expression.NodeType | (index << 8));
                ++index;
            }
            State = RemoteExpressionSerializeStateEnum.NotFoundParameter;
            return false;
        }
        /// <summary>
        /// 常量序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Serialize(string value)
        {
            switch (value.Length)
            {
                case 0: return Stream.Write((int)(byte)ConstantTypeEnum.EmptyString);
                case 1: return Stream.Write(((int)value[0] << 8) | (byte)ConstantTypeEnum.CharString);
                default:
                    UnmanagedStream stream = Stream;
                    if (stream.Write((int)(byte)ConstantTypeEnum.String))
                    {
                        fixed (char* valueFixed = value) stream.Serialize(valueFixed, value.Length);
                        return !stream.IsResizeError;
                    }
                    return false;
            }
        }

        /// <summary>
        /// 获取常量参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T? GetConstantParameterValue<T>(ClientMetadata metadata, int index)
#else
        internal static T GetConstantParameterValue<T>(ClientMetadata metadata, int index)
#endif
        {
            return metadata.constantParameters.Array[index].Value.castType<T>();
        }
        /// <summary>
        /// 常量参数序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SimpleSerializeConstantParameter<T>(ClientMetadata metadata, ref T value) where T : struct
        {
            metadata.serializer.SimpleSerialize(ref value);
        }
        /// <summary>
        /// 常量参数序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SerializeConstantParameter<T>(ClientMetadata metadata, ref T value) where T : struct
        {
            metadata.serializer.InternalIndependentSerializeNotNull(ref value);
        }
        /// <summary>
        /// 序列化表达式
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="expression"></param>
        internal unsafe static void Serialize(BinarySerializer serializer, Type[] parameterTypes, LambdaExpression expression)
        {
            UnmanagedStream stream = serializer.Stream;
            if (expression != null)
            {
                int startIndex = stream.GetIndexBeforeMove(sizeof(int) * 2, (byte)RemoteExpressionSerializeStateEnum.Success);
                if (startIndex >= 0)
                {
                    var metadata = serializer.Context.castType<CommandClientSocket>().notNull().GetRemoteMetadata();
                    RemoteExpressionSerializeStateEnum state;
                    if (metadata != null)
                    {
                        if (metadata.Serialize(serializer, parameterTypes, expression.Parameters, expression.Body) || stream.IsResizeError) return;
                        state = metadata.State;
                    }
                    else state = RemoteExpressionSerializeStateEnum.NotSupportClient;
                    stream.Data.Pointer.SetCurrentIndex(startIndex, (byte)state);
                }
            }
            else stream.Write((int)(byte)RemoteExpressionSerializeStateEnum.NullExpression);
        }
    }
}
