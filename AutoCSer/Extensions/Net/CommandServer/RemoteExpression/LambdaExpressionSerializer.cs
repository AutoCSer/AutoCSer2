using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer.RemoteExpression
{
    /// <summary>
    /// 远程 Lambda 表达式序列化
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct LambdaExpressionSerializer
    {
        /// <summary>
        /// Output buffer stream
        /// 序列化输出缓冲区流
        /// </summary>
        private readonly UnmanagedStream stream;
        /// <summary>
        /// Binary data serialization
        /// 二进制数据序列化
        /// </summary>
        private readonly BinarySerializer serializer;
        /// <summary>
        /// 表达式参数集合
        /// </summary>
        private readonly ReadOnlyCollection<ParameterExpression> parameters;
        /// <summary>
        /// Remote expression serialization status
        /// 远程表达式序列化状态
        /// </summary>
        private RemoteExpressionSerializeStateEnum state;
        /// <summary>
        /// 是否需要检查常量表达式
        /// </summary>
        private bool isCheckConstant;
        /// <summary>
        /// 远程 Lambda 表达式序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="parameters"></param>
        internal LambdaExpressionSerializer(BinarySerializer serializer, ReadOnlyCollection<ParameterExpression> parameters)
        {
            this.serializer = serializer;
            stream = serializer.Stream;
            this.parameters = parameters;
            state = RemoteExpressionSerializeStateEnum.Success;
            isCheckConstant = true;
        }
        /// <summary>
        /// 序列化 Lambda 表达式
        /// </summary>
        /// <param name="expression"></param>
        internal void Serialize(System.Linq.Expressions.Expression expression)
        {
            int startIndex = stream.GetIndexBeforeMove(sizeof(int) * 2, (byte)RemoteExpressionSerializeStateEnum.Success);
            if (startIndex >= 0)
            {
                if (serializeParameters() && serializeNode(expression))
                {
                    if (!stream.IsResizeError) stream.Data.Pointer.WriteSizeData(startIndex + sizeof(int) * 2);
                }
                else if (!stream.IsResizeError) stream.Data.Pointer.SetCurrentIndex(startIndex, (byte)state);
            }
        }
        /// <summary>
        /// 写入表达式序列化数据的哈希值
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool writeHashCode(int startIndex)
        {
            ulong hashCode64 = stream.Data.Pointer.WriteSizeGetHashCode64(startIndex);
            return stream.Write((uint)(hashCode64 ^ (hashCode64 >> 32)));
        }
        /// <summary>
        /// 序列化类型信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool serializeRemoteType(AutoCSer.Reflection.RemoteType type)
        {
            bool isArray;
            int typeIndex = type.GetTypeIndex(out isArray);
            if (typeIndex >= 0)
            {
                return stream.Write(typeIndex | (isArray ? (int)(NodeHeaderEnum.TypeIndex | NodeHeaderEnum.IsArray) : (int)NodeHeaderEnum.TypeIndex));
            }
            int startIndex = stream.GetIndexBeforeMove(sizeof(int) * 2, (int)NodeHeaderEnum.Type);
            return startIndex >= 0 && serialize(type.AssemblyName) && serialize(type.Name) && writeHashCode(startIndex + sizeof(int));
        }
        /// <summary>
        /// 序列化类型信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private bool serialize(Type? type)
#else
        private bool serialize(Type type)
#endif
        {
            return type == null || serializeRemoteType(new AutoCSer.Reflection.RemoteType(type));
        }
        /// <summary>
        /// 序列化方法信息
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
#if NetStandard21
        private bool serialize(MethodInfo? method)
#else
        private bool serialize(MethodInfo method)
#endif
        {
            if (method == null) return true;
            BindingFlags bindingFlags = method.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
            int startIndex = stream.GetIndexBeforeMove(sizeof(int));
            bindingFlags |= method.IsPublic ? BindingFlags.Public : (BindingFlags.Public | BindingFlags.NonPublic);
            if (startIndex >= 0 && serializeRemoteType(method.ReflectedType.notNull()) && serialize(method.Name) && stream.Write((int)bindingFlags))
            {
                Type[] types = method.IsGenericMethod ? method.GetGenericArguments() : EmptyArray<Type>.Array;
                if (types.Length < 256)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length < 256)
                    {
                        if (stream.Write(types.Length | (parameters.Length << 8)))
                        {
                            foreach (Type type in types)
                            {
                                if (!serializeRemoteType(type)) return false;
                            }
                            foreach (ParameterInfo parameter in parameters)
                            {
                                if (!serializeRemoteType(parameter.ParameterType)) return false;
                            }
                            return writeHashCode(startIndex);
                        }
                    }
                    state = RemoteExpressionSerializeStateEnum.TooManyParameters;
                }
                else state = RemoteExpressionSerializeStateEnum.TooManyGenericArguments;
            }
            return false;
        }
        /// <summary>
        /// 序列化属性信息
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
#if NetStandard21
        private bool serialize(PropertyInfo? property)
#else
        private bool serialize(PropertyInfo property)
#endif
        {
            if (property == null) return true;
            int startIndex = stream.GetIndexBeforeMove(sizeof(int));
            return startIndex >= 0 && serializeRemoteType(property.ReflectedType.notNull()) && serialize(property.Name) && stream.Write((int)(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) && writeHashCode(startIndex);
        }
        /// <summary>
        /// 序列化字段信息
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool serialize(FieldInfo field)
        {
            BindingFlags bindingFlags = field.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
            int startIndex = stream.GetIndexBeforeMove(sizeof(int));
            bindingFlags |= field.IsPublic ? BindingFlags.Public : (BindingFlags.Public | BindingFlags.NonPublic);
            return startIndex >= 0 && serializeRemoteType(field.ReflectedType.notNull()) && serialize(field.Name) && stream.Write((int)bindingFlags) && writeHashCode(startIndex);
        }
        /// <summary>
        /// 序列化构造函数信息
        /// </summary>
        /// <param name="constructor"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool serialize(ConstructorInfo constructor, Type type)
        {
            int startIndex = stream.GetIndexBeforeMove(sizeof(int));
            if (startIndex >= 0 && serializeRemoteType(type))
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                if (parameters.Length < 256)
                {
                    if (stream.Write(parameters.Length))
                    {
                        foreach (ParameterInfo parameter in parameters)
                        {
                            if (!serializeRemoteType(parameter.ParameterType)) return false;
                        }
                        return writeHashCode(startIndex);
                    }
                }
                state = RemoteExpressionSerializeStateEnum.TooManyParameters;
            }
            return false;
        }
        /// <summary>
        /// new 调用成员集合序列化
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
#if NetStandard21
        private bool serialize(ReadOnlyCollection<MemberInfo>? members)
#else
        private bool serialize(ReadOnlyCollection<MemberInfo> members)
#endif
        {
            if (members == null) return stream.Write(AutoCSer.BinarySerializer.NullValue);
            if (stream.Write(members.Count))
            {
                foreach (MemberInfo member in members)
                {
                    if (!serialize(member)) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private bool serialize(MemberInfo member)
        {
            var property = member as PropertyInfo;
            if (property != null) return stream.Write(new PropertyIndex(property).NodeHeader) && serialize(property);
            FieldInfo field = (FieldInfo)member;
            return stream.Write(new FieldIndex(field).NodeHeader) && serialize(field);
        }
        /// <summary>
        /// new 操作调用成员初始化序列化
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        private bool serialize(ReadOnlyCollection<ElementInit> members)
        {
            if (stream.Write(members.Count))
            {
                foreach (ElementInit member in members)
                {
                    if (!serialize(member)) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// new 操作调用成员初始化序列化
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private bool serialize(ElementInit member)
        {
            MethodInfo method = member.AddMethod;
            return stream.Write(new MethodIndex(method).NodeHeader) && serialize(method) && serialize(member.Arguments);
        }
        /// <summary>
        /// new 操作调用成员初始化序列化
        /// </summary>
        /// <param name="bindings"></param>
        /// <returns></returns>
        private bool serialize(ReadOnlyCollection<MemberBinding> bindings)
        {
            if (stream.Write(bindings.Count))
            {
                foreach (MemberBinding member in bindings)
                {
                    if (!serialize(member)) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// new 操作调用成员初始化序列化
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private bool serialize(MemberBinding member)
        {
            if (member.BindingType == MemberBindingType.Assignment) return serialize(member.Member) && serializeNode(((MemberAssignment)member).Expression);
            state = RemoteExpressionSerializeStateEnum.NotSupportMemberInitBindingType;
            return false;
        }
        /// <summary>
        /// 序列化参数集合
        /// </summary>
        /// <returns></returns>
        private bool serializeParameters()
        {
            int count = parameters.Count;
            if (stream.Write(count))
            {
                foreach (ParameterExpression parameter in parameters)
                {
                    if (!serializeRemoteType(parameter.Type) || !serialize(parameter.Name)) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 方法调用参数序列化节点
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private bool serialize(ReadOnlyCollection<System.Linq.Expressions.Expression> arguments)
        {
            if (stream.Write(arguments.Count))
            {
                foreach (System.Linq.Expressions.Expression expression in arguments)
                {
                    if (!serializeNode(expression)) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private bool serialize(string? value)
#else
        private bool serialize(string value)
#endif
        {
            if (value != null)
            {
                switch (value.Length)
                {
                    case 0: return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.EmptyString << 8));
                    case 1: return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.CharString << 8) | ((uint)value[0] << 16));
                    default:
                        if (stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.String << 8)))
                        {
                            fixed (char* valueFixed = value) stream.Serialize(valueFixed, value.Length);
                            return !stream.IsResizeError;
                        }
                        return false;
                }
            }
            return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullString << 8));
        }
        /// <summary>
        /// 序列化表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serializeNode(System.Linq.Expressions.Expression expression)
        {
            ExpressionType nodeType = expression.NodeType;
            if (nodeType != ExpressionType.MemberAccess) isCheckConstant = true;
            switch (nodeType)
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
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return serialize((NewArrayExpression)expression);
                case ExpressionType.New:
                    return serialize((NewExpression)expression);
                case ExpressionType.ListInit:
                    return serialize((ListInitExpression)expression);
                case ExpressionType.MemberInit:
                    return serialize((MemberInitExpression)expression);
                case ExpressionType.Lambda:
                case ExpressionType.RuntimeVariables:
                    state = RemoteExpressionSerializeStateEnum.NotSupportLambda;
                    return false;
                case ExpressionType.Block:
                    state = RemoteExpressionSerializeStateEnum.NotSupportBlock;
                    return false;
                case ExpressionType.DebugInfo:
                    state = RemoteExpressionSerializeStateEnum.NotSupportDebugInfo;
                    return false;
                case ExpressionType.Dynamic:
                    state = RemoteExpressionSerializeStateEnum.NotSupportDynamic;
                    return false;
                case ExpressionType.Extension:
                    state = RemoteExpressionSerializeStateEnum.NotSupportExtension;
                    return false;
                case ExpressionType.Label:
                case ExpressionType.Goto:
                case ExpressionType.Loop:
                case ExpressionType.Switch:
                case ExpressionType.Try:
                case ExpressionType.Throw:
                    state = RemoteExpressionSerializeStateEnum.NotSupportLabel;
                    return false;
                default:
                    state = RemoteExpressionSerializeStateEnum.UnknownNodeType;
                    return false;
            }
        }
        /// <summary>
        /// 序列化二元表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(BinaryExpression expression)
        {
            MethodIndex methodIndex = new MethodIndex(expression.Method);
            return stream.Write((int)expression.NodeType | methodIndex.NodeHeader) && serializeNode(expression.Left) && serializeNode(expression.Right) && serialize(methodIndex.Method);
        }
        /// <summary>
        /// 序列化二元表达式节点（检查 Lambda 转换参数）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serializeCheckConversion(BinaryExpression expression)
        {
            if (expression.Conversion == null) return serialize(expression);
            state = RemoteExpressionSerializeStateEnum.NotSupportLambda;
            return false;
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
            state = RemoteExpressionSerializeStateEnum.NotSupportArrayIndex;
            return false;
        }
        /// <summary>
        /// 序列化类型判断表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(TypeBinaryExpression expression)
        {
            TypeIndex typeIndex = new TypeIndex(expression.TypeOperand);
            return stream.Write((int)expression.NodeType | typeIndex.NodeHeader) && serializeNode(expression.Expression) && serialize(typeIndex.Type);
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
            TypeIndex typeIndex = new TypeIndex(isType ? expression.Type : null);
            MethodIndex methodIndex = new MethodIndex(isMethod ? expression.Method : null);
            return stream.Write((int)expression.NodeType | typeIndex.NodeHeader | methodIndex.NodeHeader) && serializeNode(expression.Operand) && serialize(typeIndex.Type) && serialize(methodIndex.Method);
        }
        /// <summary>
        /// 序列化方法调用表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(MethodCallExpression expression)
        {
            var value = expression.Object;
            MethodIndex methodIndex = new MethodIndex(expression.Method);
            if (value != null)
            {
                return stream.Write((int)expression.NodeType | methodIndex.NodeHeader) && serializeNode(value) && serialize(methodIndex.Method) && serialize(expression.Arguments);
            }
            return stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.NullValue | methodIndex.NodeHeader) && serialize(methodIndex.Method) && serialize(expression.Arguments);
        }
        /// <summary>
        /// 序列化索引调用节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(IndexExpression expression)
        {
            var value = expression.Object;
            PropertyIndex propertyIndex = new PropertyIndex(expression.Indexer);
            if (value != null)
            {
                return stream.Write((int)expression.NodeType | propertyIndex.NodeHeader) && serializeNode(value) && serialize(propertyIndex.Property) && serialize(expression.Arguments);
            }
            return stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.NullValue | propertyIndex.NodeHeader) && serialize(propertyIndex.Property) && serialize(expression.Arguments);
        }
        /// <summary>
        /// 序列化 Invoke 节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(InvocationExpression expression)
        {
            return stream.Write((int)expression.NodeType) && serializeNode(expression.Expression) && serialize(expression.Arguments);
        }
        /// <summary>
        /// 序列化类型默认值表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(DefaultExpression expression)
        {
            TypeIndex typeIndex = new TypeIndex(expression.Type);
            return stream.Write((int)expression.NodeType | typeIndex.NodeHeader) && serialize(typeIndex.Type);
        }
        /// <summary>
        /// 序列化条件表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(ConditionalExpression expression)
        {
            return stream.Write((int)expression.NodeType) && serializeNode(expression.Test) && serializeNode(expression.IfTrue) && serializeNode(expression.IfFalse);
        }
        /// <summary>
        /// 序列化 new T[] 表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(NewArrayExpression expression)
        {
            TypeIndex typeIndex = new TypeIndex(expression.Type.GetElementType());
            return stream.Write((int)expression.NodeType | typeIndex.NodeHeader) && serialize(typeIndex.Type) && serialize(expression.Expressions);
        }
        /// <summary>
        /// 序列化 new 表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(NewExpression expression)
        {
            Type type = expression.Type;
            if (type.Name[0] != '<')
            {
                var constructor = expression.Constructor;
                isCheckConstant = true;
                if (constructor != null)
                {
                    return stream.Write((int)expression.NodeType | new ConstructorIndex(constructor).NodeHeader) && serialize(constructor, type) && serialize(expression.Arguments) && serialize(expression.Members);
                }
                TypeIndex typeIndex = new TypeIndex(type);
                return stream.Write((int)expression.NodeType | typeIndex.NodeHeader | (int)NodeHeaderEnum.NewType) && serialize(typeIndex.Type);
            }
            state = RemoteExpressionSerializeStateEnum.NotSupportNewAnonymousType;
            return false;

        }
        /// <summary>
        /// 序列化 new List 表达式节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(ListInitExpression expression)
        {
            return stream.Write((int)expression.NodeType) && serialize(expression.NewExpression) && serialize(expression.Initializers);
        }
        /// <summary>
        /// 序列化 new 操作成员初始化节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serialize(MemberInitExpression expression)
        {
            return stream.Write((int)expression.NodeType) && serialize(expression.NewExpression) && serialize(expression.Bindings);
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
                if (state == RemoteExpressionSerializeStateEnum.Success)
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
            if (target != null) return ClientMetadata.GetConstantParameter(member, target);
            state = RemoteExpressionSerializeStateEnum.ConstantNullReference;
            return default(ConstantParameter);
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
                if (value != null)
                {
                    return stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.Property) && serializeNode(value) && serialize(property);
                }
                return stream.Write((int)expression.NodeType | (int)(NodeHeaderEnum.NullValue | NodeHeaderEnum.Property)) && serialize(property);
            }
            FieldInfo field = (FieldInfo)member;
            if (value != null)
            {
                return stream.Write((int)expression.NodeType | (int)NodeHeaderEnum.Field) && serializeNode(value) && serialize(field);
            }
            return stream.Write((int)expression.NodeType | (int)(NodeHeaderEnum.NullValue | NodeHeaderEnum.Field)) && serialize(field);
        }
        /// <summary>
        /// 序列化参数节点
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool serializeParameter(System.Linq.Expressions.Expression expression)
        {
            int index = 0;
            foreach (ParameterExpression parameterExpression in parameters)
            {
                if (object.ReferenceEquals(parameterExpression, expression)) return stream.Write((int)expression.NodeType | (index << 8));
                ++index;
            }
            state = RemoteExpressionSerializeStateEnum.NotFoundParameter;
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
            ConstantTypeEnum constantType;
            if (constantTypes.TryGetValue(type, out constantType))
            {
                switch (constantType)
                {
                    case ConstantTypeEnum.Bool:
                        if (value.castValue<bool>()) return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.True << 8));
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Bool << 8));
                    case ConstantTypeEnum.Byte:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Byte << 8) | ((int)value.castValue<byte>() << 16));
                    case ConstantTypeEnum.SByte:
                        return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.SByte << 8) | ((uint)value.castValue<sbyte>() << 16));
                    case ConstantTypeEnum.Short:
                        return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.Short << 8) | ((uint)value.castValue<short>() << 16));
                    case ConstantTypeEnum.UShort:
                        return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.UShort << 8) | ((uint)value.castValue<ushort>() << 16));
                    case ConstantTypeEnum.Int:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Int << 8)) && stream.Write(value.castValue<int>());
                    case ConstantTypeEnum.UInt:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.UInt << 8)) && stream.Write(value.castValue<uint>());
                    case ConstantTypeEnum.Long:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Long << 8)) && stream.Write(value.castValue<long>());
                    case ConstantTypeEnum.ULong:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.ULong << 8)) && stream.Write(value.castValue<ulong>());
                    case ConstantTypeEnum.Float:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Float << 8)) && stream.Write(value.castValue<float>());
                    case ConstantTypeEnum.Double:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Double << 8)) && stream.Write(value.castValue<double>());
                    case ConstantTypeEnum.Decimal:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Decimal << 8)) && stream.Write(value.castValue<decimal>());
                    case ConstantTypeEnum.Char:
                        return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.Char << 8) | ((uint)value.castValue<char>() << 16));
                    case ConstantTypeEnum.DateTime:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.DateTime << 8)) && stream.Write(value.castValue<DateTime>());
                    case ConstantTypeEnum.TimeSpan:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.TimeSpan << 8)) && stream.Write(value.castValue<TimeSpan>());
                    case ConstantTypeEnum.Guid:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Guid << 8)) && stream.Write(value.castValue<Guid>());
                    case ConstantTypeEnum.NullableBool:
                        if (value != null)
                        {
                            bool? nullableValue = (bool?)value;
                            if (nullableValue.HasValue)
                            {
                                if (nullableValue.Value) return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableBoolTrue << 8));
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableBool << 8));
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullBool << 8));
                    case ConstantTypeEnum.NullableByte:
                        if (value != null)
                        {
                            byte? nullableValue = (byte?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableByte << 8) | ((int)nullableValue.Value << 16));
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullByte << 8));
                    case ConstantTypeEnum.NullableSByte:
                        if (value != null)
                        {
                            sbyte? nullableValue = (sbyte?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.NullableSByte << 8) | ((uint)nullableValue.Value << 16));
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullSByte << 8));
                    case ConstantTypeEnum.NullableShort:
                        if (value != null)
                        {
                            short? nullableValue = (short?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.NullableShort << 8) | ((uint)nullableValue.Value << 16));
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullShort << 8));
                    case ConstantTypeEnum.NullableUShort:
                        if (value != null)
                        {
                            ushort? nullableValue = (ushort?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.NullableUShort << 8) | ((uint)nullableValue.Value << 16));
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullUShort << 8));
                    case ConstantTypeEnum.NullableInt:
                        if (value != null)
                        {
                            int? nullableValue = (int?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableInt << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullInt << 8));
                    case ConstantTypeEnum.NullableUInt:
                        if (value != null)
                        {
                            uint? nullableValue = (uint?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableUInt << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullUInt << 8));
                    case ConstantTypeEnum.NullableLong:
                        if (value != null)
                        {
                            long? nullableValue = (long?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableLong << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullLong << 8));
                    case ConstantTypeEnum.NullableULong:
                        if (value != null)
                        {
                            ulong? nullableValue = (ulong?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableULong << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullULong << 8));
                    case ConstantTypeEnum.NullableFloat:
                        if (value != null)
                        {
                            float? nullableValue = (float?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableFloat << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullFloat << 8));
                    case ConstantTypeEnum.NullableDouble:
                        if (value != null)
                        {
                            double? nullableValue = (double?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableDouble << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullDouble << 8));
                    case ConstantTypeEnum.NullableDecimal:
                        if (value != null)
                        {
                            decimal? nullableValue = (decimal?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableDecimal << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullDecimal << 8));
                    case ConstantTypeEnum.NullableChar:
                        if (value != null)
                        {
                            char? nullableValue = (char?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.NullableChar << 8) | ((uint)nullableValue.Value << 16));
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullChar << 8));
                    case ConstantTypeEnum.NullableDateTime:
                        if (value != null)
                        {
                            DateTime? nullableValue = (DateTime?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableDateTime << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullDateTime << 8));
                    case ConstantTypeEnum.NullableTimeSpan:
                        if (value != null)
                        {
                            TimeSpan? nullableValue = (TimeSpan?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableTimeSpan << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullTimeSpan << 8));
                    case ConstantTypeEnum.NullableGuid:
                        if (value != null)
                        {
                            Guid? nullableValue = (Guid?)value;
                            if (nullableValue.HasValue)
                            {
                                return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullableGuid << 8)) && stream.Write(nullableValue.Value);
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullGuid << 8));
                    case ConstantTypeEnum.Complex:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Complex << 8)) && stream.Write(value.castValue<System.Numerics.Complex>());
                    case ConstantTypeEnum.Plane:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Plane << 8)) && stream.Write(value.castValue<System.Numerics.Plane>());
                    case ConstantTypeEnum.Quaternion:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Quaternion << 8)) && stream.Write(value.castValue<System.Numerics.Quaternion>());
                    case ConstantTypeEnum.Matrix3x2:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Matrix3x2 << 8)) && stream.Write(value.castValue<System.Numerics.Matrix3x2>());
                    case ConstantTypeEnum.Matrix4x4:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Matrix4x4 << 8)) && stream.Write(value.castValue<System.Numerics.Matrix4x4>());
                    case ConstantTypeEnum.Vector2:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Vector2 << 8)) && stream.Write(value.castValue<System.Numerics.Vector2>());
                    case ConstantTypeEnum.Vector3:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Vector3 << 8)) && stream.Write(value.castValue<System.Numerics.Vector3>());
                    case ConstantTypeEnum.Vector4:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Vector4 << 8)) && stream.Write(value.castValue<System.Numerics.Vector4>());
                    case ConstantTypeEnum.Half:
                        Half half = value.castValue<Half>();
                        return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.Half << 8) | ((uint)*(ushort*)&half << 16));
                    case ConstantTypeEnum.Int128:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.Int128 << 8)) && stream.Write(value.castValue<Int128>());
                    case ConstantTypeEnum.UInt128:
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.UInt128 << 8)) && stream.Write(value.castValue<UInt128>());
                    case ConstantTypeEnum.ByteArray:
                        if (value != null)
                        {
                            var byteArray = value.notNullCastType<byte[]>();
                            switch (byteArray.Length)
                            {
                                case 0: return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.EmptyByteArray << 8));
                                case 1: return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.ByteArray1 << 8) | ((int)byteArray[0] << 16));
                                case 2: return stream.Write((uint)ExpressionType.Constant | ((uint)(byte)ConstantTypeEnum.ByteArray2 << 8) | ((uint)byteArray[0] << 16) | ((uint)byteArray[1] << 24));
                                default:
                                    if (stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.ByteArray << 8)))
                                    {
                                        fixed (byte* valueFixed = byteArray) stream.Serialize(valueFixed, byteArray.Length, byteArray.Length);
                                        return !stream.IsResizeError;
                                    }
                                    return false;
                            }
                        }
                        return stream.Write((int)ExpressionType.Constant | ((int)(byte)ConstantTypeEnum.NullByteArray << 8));
                    case ConstantTypeEnum.String: return serialize(value.castClass<string>());
                }
            }
            if (value != null)
            {
                if (stream.Write((int)ExpressionType.Constant) && serializeRemoteType(type))
                {
                    AutoCSer.Extensions.Metadata.GenericType.Get(type).BinarySerialize(serializer, value);
                    return !stream.IsResizeError;
                }
            }
            else
            {
                return stream.Write((int)ExpressionType.Constant | (int)NodeHeaderEnum.NullValue) && serializeRemoteType(type);
            }
            return false;
        }

        /// <summary>
        /// 常量类型集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, ConstantTypeEnum> constantTypes;
        static LambdaExpressionSerializer()
        {
            constantTypes = DictionaryCreator.CreateHashObject<Type, ConstantTypeEnum>((byte)ConstantTypeEnum.NullString);
            constantTypes.Add(typeof(string), ConstantTypeEnum.String);
            constantTypes.Add(typeof(bool), ConstantTypeEnum.Bool);
            constantTypes.Add(typeof(byte), ConstantTypeEnum.Byte);
            constantTypes.Add(typeof(sbyte), ConstantTypeEnum.SByte);
            constantTypes.Add(typeof(short), ConstantTypeEnum.Short);
            constantTypes.Add(typeof(ushort), ConstantTypeEnum.UShort);
            constantTypes.Add(typeof(int), ConstantTypeEnum.Int);
            constantTypes.Add(typeof(uint), ConstantTypeEnum.UInt);
            constantTypes.Add(typeof(long), ConstantTypeEnum.Long);
            constantTypes.Add(typeof(ulong), ConstantTypeEnum.ULong);
            constantTypes.Add(typeof(float), ConstantTypeEnum.Float);
            constantTypes.Add(typeof(double), ConstantTypeEnum.Double);
            constantTypes.Add(typeof(decimal), ConstantTypeEnum.Decimal);
            constantTypes.Add(typeof(char), ConstantTypeEnum.Char);
            constantTypes.Add(typeof(DateTime), ConstantTypeEnum.DateTime);
            constantTypes.Add(typeof(TimeSpan), ConstantTypeEnum.TimeSpan);
            constantTypes.Add(typeof(Guid), ConstantTypeEnum.Guid);
            constantTypes.Add(typeof(bool?), ConstantTypeEnum.NullableBool);
            constantTypes.Add(typeof(byte?), ConstantTypeEnum.NullableByte);
            constantTypes.Add(typeof(sbyte?), ConstantTypeEnum.NullableSByte);
            constantTypes.Add(typeof(short?), ConstantTypeEnum.NullableShort);
            constantTypes.Add(typeof(ushort?), ConstantTypeEnum.NullableUShort);
            constantTypes.Add(typeof(int?), ConstantTypeEnum.NullableInt);
            constantTypes.Add(typeof(uint?), ConstantTypeEnum.NullableUInt);
            constantTypes.Add(typeof(long?), ConstantTypeEnum.NullableLong);
            constantTypes.Add(typeof(ulong?), ConstantTypeEnum.NullableULong);
            constantTypes.Add(typeof(float?), ConstantTypeEnum.NullableFloat);
            constantTypes.Add(typeof(double?), ConstantTypeEnum.NullableDouble);
            constantTypes.Add(typeof(decimal?), ConstantTypeEnum.NullableDecimal);
            constantTypes.Add(typeof(char?), ConstantTypeEnum.NullableChar);
            constantTypes.Add(typeof(DateTime?), ConstantTypeEnum.NullableDateTime);
            constantTypes.Add(typeof(TimeSpan?), ConstantTypeEnum.NullableTimeSpan);
            constantTypes.Add(typeof(Guid?), ConstantTypeEnum.NullableGuid);
            constantTypes.Add(typeof(System.Numerics.Complex), ConstantTypeEnum.Complex);
            constantTypes.Add(typeof(System.Numerics.Plane), ConstantTypeEnum.Plane);
            constantTypes.Add(typeof(System.Numerics.Quaternion), ConstantTypeEnum.Quaternion);
            constantTypes.Add(typeof(System.Numerics.Matrix3x2), ConstantTypeEnum.Matrix3x2);
            constantTypes.Add(typeof(System.Numerics.Matrix4x4), ConstantTypeEnum.Matrix4x4);
            constantTypes.Add(typeof(System.Numerics.Vector2), ConstantTypeEnum.Vector2);
            constantTypes.Add(typeof(System.Numerics.Vector3), ConstantTypeEnum.Vector3);
            constantTypes.Add(typeof(System.Numerics.Vector4), ConstantTypeEnum.Vector4);
            constantTypes.Add(typeof(Half), ConstantTypeEnum.Half);
            constantTypes.Add(typeof(Int128), ConstantTypeEnum.Int128);
            constantTypes.Add(typeof(UInt128), ConstantTypeEnum.UInt128);
            constantTypes.Add(typeof(byte[]), ConstantTypeEnum.ByteArray);
        }
    }
}
