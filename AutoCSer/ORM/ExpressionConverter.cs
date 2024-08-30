using System.Linq.Expressions;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.ORM.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 表达式转换
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ExpressionConverter
    {
        /// <summary>
        /// SQL 字符串流
        /// </summary>
        internal CharStream CharStream;
        /// <summary>
        /// 数据库表格持久化写入
        /// </summary>
        internal TableWriter TableWriter;
        /// <summary>
        /// 条件表达式重组
        /// </summary>
        private ConditionExpressionConverter conditionConverter;
        /// <summary>
        /// 最后转换表达式的 DateTime 成员
        /// </summary>
        private Member dateTimeMember;
        /// <summary>
        /// 表达式转换
        /// </summary>
        /// <param name="tableWriter"></param>
        /// <param name="charStream"></param>
        internal ExpressionConverter(TableWriter tableWriter, CharStream charStream)
        {
            TableWriter = tableWriter;
            CharStream = charStream;
            conditionConverter = default(ConditionExpressionConverter);
            dateTimeMember = null;
        }
        /// <summary>
        /// 表达式转换
        /// </summary>
        /// <param name="tableWriter"></param>
        internal ExpressionConverter(TableWriter tableWriter)
        {
            TableWriter = tableWriter;
            CharStream = tableWriter.ConnectionPool.Creator.GetCharStreamCache();
            conditionConverter = default(ConditionExpressionConverter);
            dateTimeMember = null;
        }
        /// <summary>
        /// 释放 SQL 字符流
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeCharStream()
        {
            TableWriter.ConnectionPool.Creator.FreeCharStreamCache(CharStream);
        }
        /// <summary>
        /// AND 条件子项转换
        /// </summary>
        /// <param name="expression"></param>
        internal void ConvertAnd(System.Linq.Expressions.Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.OrElse:
                    CharStream.Write('(');
                    Convert(expression);
                    CharStream.Write(')');
                    return;
                default: Convert(expression); return;
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        internal void Convert(System.Linq.Expressions.Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:
                    convertConcatLogic((BinaryExpression)expression);
                    return;
                case ExpressionType.Equal: convertEqual((BinaryExpression)expression); return;
                case ExpressionType.NotEqual: convertNotEqual((BinaryExpression)expression); return;
                case ExpressionType.GreaterThanOrEqual: convertBinaryExpression((BinaryExpression)expression, '>', '='); return;
                case ExpressionType.GreaterThan: convertBinaryExpression((BinaryExpression)expression, '>'); return;
                case ExpressionType.LessThan: convertBinaryExpression((BinaryExpression)expression, '<'); return;
                case ExpressionType.LessThanOrEqual: convertBinaryExpression((BinaryExpression)expression, '<', '='); return;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    convertAdd((BinaryExpression)expression, '+');
                    return;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    convertAdd((BinaryExpression)expression, '-');
                    return;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked: convertBinaryExpression((BinaryExpression)expression, '*'); return;
                case ExpressionType.Divide: convertBinaryExpression((BinaryExpression)expression, '/'); return;
                case ExpressionType.Modulo: convertBinaryExpression((BinaryExpression)expression, '%'); return;
                case ExpressionType.Or: convertBinaryExpression((BinaryExpression)expression, '|'); return;
                case ExpressionType.And: convertBinaryExpression((BinaryExpression)expression, '&'); return;
                case ExpressionType.ExclusiveOr: convertBinaryExpression((BinaryExpression)expression, '^'); return;
                case ExpressionType.LeftShift: convertBinaryExpression((BinaryExpression)expression, '<', '<'); return;
                case ExpressionType.RightShift: convertBinaryExpression((BinaryExpression)expression, '>', '>'); return;
                case ExpressionType.MemberAccess: convertMemberAccess((MemberExpression)expression); return;
                case ExpressionType.Coalesce: convertCoalesce((BinaryExpression)expression); return;
                case ExpressionType.Unbox:
                case ExpressionType.UnaryPlus:
                    convertIsSimple(((UnaryExpression)expression).Operand); return;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked: convertNegate((UnaryExpression)expression); return;
                case ExpressionType.IsTrue: convertIsTrue((UnaryExpression)expression); return;
                case ExpressionType.Not:
                case ExpressionType.IsFalse:
                    convertIsFalse((UnaryExpression)expression);
                    return;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked: convertConvert((UnaryExpression)expression); return;
                case ExpressionType.Conditional: convertConditional((ConditionalExpression)expression); return;
                case ExpressionType.Call: convertCall((MethodCallExpression)expression); return;
                case ExpressionType.Constant: convertConstant(expression.getConstantValue()); return;
                default: throw new InvalidCastException("未知表达式类型 " + expression.NodeType.ToString());
            }
        }
        /// <summary>
        /// 简单表达式转换
        /// </summary>
        /// <param name="expression"></param>
        private void convertIsSimple(System.Linq.Expressions.Expression expression)
        {
            if (expression.isSimple()) Convert(expression);
            else
            {
                CharStream.Write('(');
                Convert(expression);
                CharStream.Write(')');
            }
        }
        /// <summary>
        /// 连接逻辑表达式
        /// </summary>
        /// <param name="binaryExpression">表达式</param>
        private void convertConcatLogic(BinaryExpression binaryExpression)
        {
            convertConcatLogic(binaryExpression.Left, binaryExpression.NodeType);
            switch (binaryExpression.NodeType)
            {
                case ExpressionType.OrElse: CharStream.SimpleWrite(" or "); break;
                case ExpressionType.AndAlso: CharStream.SimpleWrite(" and "); break;
            }
            convertConcatLogic(binaryExpression.Right, binaryExpression.NodeType);
        }
        /// <summary>
        /// 连接逻辑表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="logicType"></param>
        private void convertConcatLogic(System.Linq.Expressions.Expression expression, ExpressionType logicType)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:
                    if (expression.NodeType == logicType) convertConcatLogic((BinaryExpression)expression);
                    else
                    {
                        CharStream.Write('(');
                        convertConcatLogic((BinaryExpression)expression);
                        CharStream.Write(')');
                    }
                    return;
                default: Convert(expression); return;
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertEqual(BinaryExpression expression)
        {
            System.Linq.Expressions.Expression left = expression.Left, right = expression.Right;
            object value = getCustomColumnMemberAccessConstantValue(left, right);
            if (value == null)
            {
                if (left.isConstantNull())
                {
                    convertIsSimple(expression.Right);
                    CharStream.SimpleWrite(" is null");
                    return;
                }
                convertIsSimple(left);
                if (right.isConstantNull()) CharStream.SimpleWrite(" is null");
                else
                {
                    CharStream.Write('=');
                    convertIsSimple(right);
                }
                return;
            }
            bool isFrist = true;
            CharStream.Write('(');
            foreach (KeyValue<CustomColumnName, object> nameValue in getCustomColumnMemberAccessValues((MemberExpression)left, value))
            {
                if (isFrist) isFrist = false;
                else CharStream.SimpleWrite(" and ");
                CharStream.SimpleWrite(nameValue.Key.Name);
                object memberValue = nameValue.Value;
                if (memberValue == null) CharStream.SimpleWrite(" is null");
                else
                {
                    CharStream.Write('=');
                    Member member = nameValue.Key.Member;
                    if (member.ReaderDataType == ReaderDataTypeEnum.DateTime) dateTimeMember = member;
                    convertConstant(memberValue);
                }
            }
            CharStream.Write(')');
            if (isFrist) throw new InvalidCastException($"{value.GetType().fullName()} 缺少查询成员");
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertNotEqual(BinaryExpression expression)
        {
            System.Linq.Expressions.Expression left = expression.Left, right = expression.Right;
            object value = getCustomColumnMemberAccessConstantValue(left, right);
            if (value == null)
            {
                if (left.isConstantNull())
                {
                    convertIsSimple(expression.Right);
                    CharStream.SimpleWrite(" is not null");
                    return;
                }
                convertIsSimple(left);
                if (right.isConstantNull()) CharStream.SimpleWrite(" is not null");
                else
                {
                    CharStream.Write('<');
                    CharStream.Write('>');
                    convertIsSimple(right);
                }
                return;
            }
            bool isFrist = true;
            CharStream.Write('(');
            foreach (KeyValue<CustomColumnName, object> nameValue in getCustomColumnMemberAccessValues((MemberExpression)left, value))
            {
                if (isFrist) isFrist = false;
                else CharStream.SimpleWrite(" or ");
                CharStream.SimpleWrite(nameValue.Key.Name);
                object memberValue = nameValue.Value;
                if (memberValue == null) CharStream.SimpleWrite(" is not null");
                else
                {
                    CharStream.Write('<');
                    CharStream.Write('>');
                    Member member = nameValue.Key.Member;
                    if (member.ReaderDataType == ReaderDataTypeEnum.DateTime) dateTimeMember = member;
                    convertConstant(memberValue);
                }
            }
            CharStream.Write(')');
            if (isFrist) throw new InvalidCastException($"{value.GetType().fullName()} 缺少查询成员");
        }
        /// <summary>
        /// 检查自定义列成员
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private object getCustomColumnMemberAccessConstantValue(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
        {
            if (left.NodeType == ExpressionType.MemberAccess && right.NodeType == ExpressionType.Constant)
            {
                object value = right.getConstantValue();
                if (value != null)
                {
                    Type type = value.GetType();
                    if (type.IsValueType && !type.IsEnum && !type.isNullable() && AutoCSer.ORM.Metadata.StructGenericType.Get(type).CustomColumnAttribute != null)
                    {
                        MemberExpression memberExpression = (MemberExpression)left;
                        FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
                        if (fieldInfo != null && value.GetType() == fieldInfo.FieldType) return value;
                        PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
                        if (propertyInfo != null && value.GetType() == propertyInfo.PropertyType) return value;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取自定义列信息与数值
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private IEnumerable<KeyValue<CustomColumnName, object>> getCustomColumnMemberAccessValues(MemberExpression memberExpression, object value)
        {
            LeftArray<MemberExpression> memberExpressions;
            Member member = getMember(ref memberExpression, out memberExpressions);
            if (member.ReaderDataType == ReaderDataTypeEnum.CustomColumn)
            {
                foreach (KeyValue<CustomColumnName, object> memberValue in member.GetCustomColumnMemberNameValues(memberExpression, ref memberExpressions, value)) yield return memberValue;
            }
            else yield return new KeyValue<CustomColumnName, object>(new CustomColumnName(member, member.MemberIndex.Member.Name), value);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="binaryExpression">表达式</param>
        /// <param name="char1">操作字符1</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convertAdd(BinaryExpression binaryExpression, char char1)
        {
            convertIsSimple(binaryExpression.Left);
            CharStream.Write(char1);
            System.Linq.Expressions.Expression right = binaryExpression.Right;
            if (right.isSimple(true)) Convert(right);
            else
            {
                CharStream.Write('(');
                Convert(right);
                CharStream.Write(')');
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="binaryExpression">表达式</param>
        /// <param name="char1">操作字符1</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convertBinaryExpression(BinaryExpression binaryExpression, char char1)
        {
            convertIsSimple(binaryExpression.Left);
            CharStream.Write(char1);
            convertIsSimple(binaryExpression.Right);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="binaryExpression">表达式</param>
        /// <param name="char1">操作字符1</param>
        /// <param name="char2">操作字符2</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convertBinaryExpression(BinaryExpression binaryExpression, char char1, char char2)
        {
            convertIsSimple(binaryExpression.Left);
            CharStream.Write(char1);
            CharStream.Write(char2);
            convertIsSimple(binaryExpression.Right);
        }
        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <param name="memberExpressions"></param>
        /// <returns></returns>
        private Member getMember(ref MemberExpression memberExpression, out LeftArray<MemberExpression> memberExpressions)
        {
            memberExpressions = new LeftArray<MemberExpression>(0);
            for (System.Linq.Expressions.Expression expression = memberExpression.Expression; expression.NodeType == ExpressionType.MemberAccess; expression = ((MemberExpression)expression).Expression)
            {
                memberExpressions.Add((MemberExpression)expression);
            }
            MemberExpression startExpression;
            if (!memberExpressions.TryPop(out startExpression))
            {
                startExpression = memberExpression;
                memberExpression = null;
            }
            Member member = TableWriter.GetMember(startExpression.Member);
            if (member != null) return member;
            throw new MemberAccessException($"{TableWriter.TableName} 没有找到成员定义 {startExpression.Member.Name}");
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="memberExpression"></param>
        private void convertMemberAccess(MemberExpression memberExpression)
        {
            LeftArray<MemberExpression> memberExpressions;
            Member member = getMember(ref memberExpression, out memberExpressions);
            if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
            {
                if (member.ReaderDataType == ReaderDataTypeEnum.DateTime) dateTimeMember = member;
                TableWriter.ConnectionPool.Creator.FormatName(CharStream, member.MemberIndex.Member.Name);
                return;
            }
            CustomColumnName name = member.CustomColumnNames.Length == 1 ? member.CustomColumnNames[0] : member.GetCustomColumnMemberName(memberExpression, ref memberExpressions);
            if (name.Member.ReaderDataType == ReaderDataTypeEnum.DateTime) dateTimeMember = name.Member;
            TableWriter.ConnectionPool.Creator.FormatName(CharStream, name.Name);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="binaryExpression"></param>
        private void convertCoalesce(BinaryExpression binaryExpression)
        {
            CharStream.SimpleWrite("isnull(");
            Convert(binaryExpression.Left);
            CharStream.Write(',');
            Convert(binaryExpression.Right);
            CharStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convertNegate(UnaryExpression unaryExpression)
        {
            CharStream.SimpleWrite("-(");
            Convert(unaryExpression.Operand);
            CharStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertIsTrue(UnaryExpression unaryExpression)
        {
            convertIsSimple(unaryExpression.Operand);
            CharStream.Write('=');
            CharStream.Write('1');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertIsFalse(UnaryExpression unaryExpression)
        {
            convertIsSimple(unaryExpression.Operand);
            CharStream.Write('=');
            CharStream.Write('0');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertConvert(UnaryExpression expression)
        {
            System.Linq.Expressions.Expression operandExpression = expression.Operand;
            if (operandExpression.NodeType == ExpressionType.MemberAccess)
            {
                Convert(operandExpression);
                return;
            }
            if (expression.Type == typeof(int))
            {
                Type operandType = operandExpression.Type;
                if (operandType.IsEnum) operandType = System.Enum.GetUnderlyingType(operandType);
                if (operandType == typeof(byte) || operandType == typeof(sbyte) || operandType == typeof(short) || operandType == typeof(ushort))
                {
                    Convert(operandExpression);
                    return;
                }

            }
            CharStream.SimpleWrite("cast(");
            convertIsSimple(operandExpression);
            CharStream.SimpleWrite(" as ");
            CharStream.SimpleWrite(expression.Type.getDbType().ToString());
            CharStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertConditional(ConditionalExpression expression)
        {
            System.Linq.Expressions.Expression test = expression.Test;
            CharStream.SimpleWrite("case when ");
            if (test.isSimpleNotLogic())
            {
                convertIsSimple(test);
                CharStream.Write('=');
                CharStream.Write('1');
            }
            else convertIsSimple(test);
            CharStream.SimpleWrite(" then ");
            convertIsSimple(expression.IfTrue);
            CharStream.SimpleWrite(" else ");
            convertIsSimple(expression.IfFalse);
            CharStream.SimpleWrite(" end");
        }
        /// <summary>
        /// 表达式重组以后再转换
        /// </summary>
        /// <param name="expression"></param>
        internal void ConditionConvert(System.Linq.Expressions.Expression expression)
        {
            conditionConverter.Convert(expression);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport)
            {
                throw new InvalidCastException($"未知表达式类型 {conditionConverter.NotSupportType}.{conditionConverter.ExceptionType}");
            }
            Convert(conditionConverter.Expression);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertCall(MethodCallExpression expression)
        {
            MethodInfo method = expression.Method;
            switch (method.Name.Length)
            {
                case 2:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.In): convertCallIn(expression, true); break;
                    }
                    break;
                case 3:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.Sum):
                            CharStream.SimpleWrite("sum(");
                            ConditionConvert(expression.Arguments[0]);
                            CharStream.Write(')');
                            break;
                        case nameof(SQLExpression.Max):
                            CharStream.SimpleWrite("max(");
                            ConditionConvert(expression.Arguments[0]);
                            CharStream.Write(')');
                            break;
                        case nameof(SQLExpression.Min):
                            CharStream.SimpleWrite("min(");
                            ConditionConvert(expression.Arguments[0]);
                            CharStream.Write(')');
                            break;
                        case nameof(SQLExpression.Len):
                            CharStream.SimpleWrite("len(");
                            ConditionConvert(expression.Arguments[0]);
                            CharStream.Write(')');
                            break;
                    }
                    break;
                case 4:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.Like): convertCallLike(expression, false, true, true); break;
                        case nameof(SQLExpression.Case): convertCallCase(expression); break;
                        case nameof(SQLExpression.Call):
                            int parameterIndex = 0;
                            foreach (System.Linq.Expressions.Expression argumentExpression in expression.Arguments)
                            {
                                switch (parameterIndex)
                                {
                                    case 0:
                                        conditionConverter.Convert(argumentExpression);
                                        if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
                                        {
                                            throw new InvalidCastException($"未知 SQL 函数名称 {conditionConverter.Expression.NodeType}");
                                        }
                                        string functionName = (string)conditionConverter.Expression.getConstantValue();
                                        if (string.IsNullOrEmpty(functionName)) throw new InvalidCastException($"SQL 函数名称不能为空 {conditionConverter.Expression.NodeType}");
                                        CharStream.SimpleWrite(functionName);
                                        CharStream.Write('(');
                                        break;
                                    case 1: ConditionConvert(argumentExpression); break;
                                    default:
                                        CharStream.Write(',');
                                        ConditionConvert(argumentExpression);
                                        break;
                                }
                                ++parameterIndex;
                            }
                            CharStream.Write(')');
                            break;
                    }
                    break;
                case 5:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.NotIn): convertCallIn(expression, false); break;
                        case nameof(SQLExpression.Count):
                            if (expression.Arguments.Count == 0) CharStream.SimpleWrite("count(*)");
                            else
                            {
                                CharStream.SimpleWrite("count(");
                                ConditionConvert(expression.Arguments[0]);
                                CharStream.Write(')');
                            }
                            break;
                    }
                    break;
                case 6:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.IsNull):
                            CharStream.SimpleWrite("isnull(");
                            ConditionConvert(expression.Arguments[0]);
                            CharStream.Write(',');
                            ConditionConvert(expression.Arguments[1]);
                            CharStream.Write(')');
                            break;
                        case nameof(SQLExpression.Exists): convertCallExists(expression, false); break;
                        case nameof(SQLExpression.LikeOr): convertCallLikeOr(expression); break;
                    }
                    break;
                case 7:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.NotLike): convertCallLike(expression, true, true, true); break;
                        case nameof(SQLExpression.GetDate): CharStream.SimpleWrite("getdate()"); break;
                        case nameof(SQLExpression.Replace): convertCallReplace(expression); break;
                    }
                    break;
                case 8:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.Distinct):
                            CharStream.SimpleWrite("distinct(");
                            ConditionConvert(expression.Arguments[0]);
                            CharStream.Write(')');
                            break;
                        case nameof(SQLExpression.EndsWith): convertCallLike(expression, false, true, false); break;
                        case nameof(SQLExpression.Contains): convertCallContains(expression, false); break;
                        case nameof(SQLExpression.DateDiff): convertCallDateDiff(expression); break;
                    }
                    break;
                case 9:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.NotExists): convertCallExists(expression, true); break;
                        case nameof(SQLExpression.CompareTo): convertCallCompareTo(expression); break;
                    }
                    break;
                case 10:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.StartsWith): convertCallLike(expression, false, false, true); break;
                        case nameof(SQLExpression.DataLength):
                            CharStream.SimpleWrite("datalength(");
                            ConditionConvert(expression.Arguments[0]);
                            CharStream.Write(')');
                            break;
                    }
                    break;
                case 11:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.NotEndsWith): convertCallLike(expression, true, true, false); break;
                        case nameof(SQLExpression.NotContains): convertCallContains(expression, true); break;
                        case nameof(SQLExpression.SysDateTime): CharStream.SimpleWrite("sysdatetime()"); break;
                    }
                    break;
                case 13:
                    switch (method.Name)
                    {
                        case nameof(SQLExpression.NotStartsWith): convertCallLike(expression, true, false, true); break;
                    }
                    break;
            }
        }
        /// <summary>
        /// IN 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="isIn"></param>
        private void convertCallIn(MethodCallExpression expression, bool isIn)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            conditionConverter.Convert(arguments[1]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
            {
                throw new InvalidCastException($"未知函数表达式参数值 {expression.Method.Name}");
            }
            object argument = conditionConverter.Expression.getConstantValue();
            System.Collections.IEnumerable values = argument as System.Collections.IEnumerable;
            if (values != null)
            {
                LeftArray<object> array = new LeftArray<object>(0);
                foreach (object value in values) array.Add(value);
                switch (array.Length)
                {
                    case 0: break;
                    case 1:
                        convertIsSimple(arguments[0]);
                        if (array[0] == null) CharStream.SimpleWrite(isIn ? " is null" : " is not null");
                        else
                        {
                            if (isIn) CharStream.Write('=');
                            else
                            {
                                CharStream.Write('<');
                                CharStream.Write('>');
                            }
                            convertConstant(array[0]);
                        }
                        return;
                    default:
                        ConditionConvert(arguments[0]);
                        CharStream.SimpleWrite(isIn ? " in(" : " not in(");
                        Action<CharStream, object> toString = TableWriter.ConnectionPool.Creator.GetConstantConverter(array[0].GetType(), dateTimeMember);
                        int index = 0;
                        foreach (object value in array)
                        {
                            if (index == 0) index = 1;
                            else CharStream.Write(',');
                            toString(CharStream, value);
                        }
                        CharStream.Write(')');
                        return;
                }
            }
            else
            {
                IQueryBuilder query = argument as IQueryBuilder;
                if (query != null && query.IsQuery)
                {
                    ConditionConvert(arguments[0]);
                    CharStream.SimpleWrite(isIn ? " in" : " not in");
                    convertQueryBuilder(query);
                    return;
                }
            }
            CharStream.SimpleWrite(isIn ? "(1=0)" : "(1=1)");
        }
        /// <summary>
        /// 转换 LIKE 表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="isNot"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        private void convertCallLike(MethodCallExpression expression, bool isNot, bool isStart, bool isEnd)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            conditionConverter.Convert(arguments[1]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
            {
                throw new InvalidCastException($"未知函数表达式参数值 {expression.Method.Name}");
            }
            System.Linq.Expressions.Expression like = conditionConverter.Expression;
            convertIsSimple(arguments[0]);
            CharStream.SimpleWrite(isNot ? " not like " : " like ");
            TableWriter.ConnectionPool.Creator.ConvertLike(CharStream, like.getConstantValue()?.ToString(), isStart, isEnd);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertCallLikeOr(MethodCallExpression expression)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            conditionConverter.Convert(arguments[1]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
            {
                throw new InvalidCastException($"未知函数表达式参数值 {expression.Method.Name}");
            }
            string[] array = (string[])conditionConverter.Expression.getConstantValue();
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: break;
                    case 1:
                        convertIsSimple(arguments[0]);
                        CharStream.SimpleWrite(" like ");
                        TableWriter.ConnectionPool.Creator.ConvertLike(CharStream, array[0], true, true);
                        return;
                    default:
                        int index = 0;
                        System.Linq.Expressions.Expression memberExpression = arguments[0];
                        CharStream.Write('(');
                        foreach (string value in array)
                        {
                            if (index == 0) index = 1;
                            else CharStream.SimpleWrite(" or ");
                            convertIsSimple(memberExpression);
                            CharStream.SimpleWrite(" like ");
                            TableWriter.ConnectionPool.Creator.ConvertLike(CharStream, value, true, true);
                        }
                        CharStream.Write(')');
                        return;
                }
            }
            CharStream.SimpleWrite("(1=0)");
        }
        /// <summary>
        /// CONTAINS 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isNot"></param>
        private void convertCallContains(MethodCallExpression expression, bool isNot)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            conditionConverter.Convert(arguments[1]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
            {
                throw new InvalidCastException($"未知函数表达式参数值 {expression.Method.Name}");
            }
            string value = (string)conditionConverter.Expression.getConstantValue();
            CharStream.SimpleWrite(isNot ? "not contains(" : " contains(");
            convertIsSimple(arguments[0]);
            CharStream.Write(',');
            TableWriter.ConnectionPool.Creator.Convert(CharStream, value);
            CharStream.Write(')');
        }
        /// <summary>
        /// 转换 REPLACE 表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertCallReplace(MethodCallExpression expression)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            CharStream.SimpleWrite("replace(");
            ConditionConvert(arguments[0]);
            CharStream.Write(',');
            ConditionConvert(arguments[1]);
            CharStream.Write(',');
            ConditionConvert(arguments[2]);
            CharStream.Write(')');
        }
        /// <summary>
        /// EXISTS 表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isNot"></param>
        private void convertCallExists(MethodCallExpression expression, bool isNot)
        {
            conditionConverter.Convert(expression.Arguments[0]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
            {
                throw new InvalidCastException($"未知函数表达式参数值 {expression.Method.Name}");
            }
            IQueryBuilder query = (IQueryBuilder)conditionConverter.Expression.getConstantValue();
            if (query != null && query.IsQuery)
            {
                CharStream.SimpleWrite(isNot ? " not exists" : " exists");
                convertQueryBuilder(query);
            }
            else CharStream.SimpleWrite(isNot ? "(1=1)" : "(1=0)");
        }
        /// <summary>
        /// 转换 DATEDIFF 表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertCallDateDiff(MethodCallExpression expression)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            conditionConverter.Convert(arguments[0]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
            {
                throw new InvalidCastException($"未知函数表达式参数值 {expression.Method.Name}");
            }
            ExpressionCallDateDiffType type = (ExpressionCallDateDiffType)conditionConverter.Expression.getConstantValue();
            CharStream.SimpleWrite("datediff(");
            CharStream.SimpleWrite(type.ToString());
            CharStream.Write(',');
            ConditionConvert(arguments[1]);
            CharStream.Write(',');
            ConditionConvert(arguments[2]);
            CharStream.Write(')');
        }
        /// <summary>
        /// 转换 CASE 表达式
        /// </summary>
        /// <param name="expression"></param>
        private void convertCallCase(MethodCallExpression expression)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            CharStream.SimpleWrite("CASE WHEN ");
            ConditionConvert(arguments[0]);
            CharStream.SimpleWrite(" THEN ");
            ConditionConvert(arguments[1]);
            CharStream.SimpleWrite(" ELSE ");
            ConditionConvert(arguments[2]);
            CharStream.SimpleWrite(" END");
        }
        /// <summary>
        /// 转换比较操作
        /// </summary>
        /// <param name="expression"></param>
        private void convertCallCompareTo(MethodCallExpression expression)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            conditionConverter.Convert(arguments[2]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport || conditionConverter.Expression.NodeType != ExpressionType.Constant)
            {
                throw new InvalidCastException($"未知函数表达式参数值 {expression.Method.Name}");
            }
            ExpressionType type = (ExpressionType)conditionConverter.Expression.getConstantValue();
            switch (type)
            {
                case ExpressionType.GreaterThan:
                    ConditionConvertIsSimple(arguments[0]);
                    CharStream.Write('>');
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    ConditionConvertIsSimple(arguments[0]);
                    CharStream.Write('>');
                    CharStream.Write('=');
                    break;
                case ExpressionType.LessThan:
                    ConditionConvertIsSimple(arguments[0]);
                    CharStream.Write('<');
                    break;
                case ExpressionType.LessThanOrEqual:
                    ConditionConvertIsSimple(arguments[0]);
                    CharStream.Write('<');
                    CharStream.Write('=');
                    break;
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    break;
                default: throw new InvalidCastException($"不支持的比较操作类型 {expression.Method.Name} {type}");
            }
            conditionConverter.Convert(arguments[1]);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport)
            {
                throw new InvalidCastException($"未知表达式类型 {conditionConverter.NotSupportType}.{conditionConverter.ExceptionType}");
            }
            System.Linq.Expressions.Expression right = conditionConverter.Expression;
            if (right.NodeType == ExpressionType.Constant)
            {
                object value = right.getConstantValue();
                IQueryBuilder query = value as IQueryBuilder;
                if (query == null)
                {
                    switch (type)
                    {
                        case ExpressionType.GreaterThan:
                        case ExpressionType.GreaterThanOrEqual:
                        case ExpressionType.LessThan:
                        case ExpressionType.LessThanOrEqual:
                            convertConstant(value);
                            break;
                        default: throw new InvalidCastException($"不支持的比较操作类型 {expression.Method.Name} {type}");
                    }
                }
                else
                {
                    if(!query.IsQuery) throw new InvalidCastException($"不支持的查询条件 {expression.Method.Name} {type}");
                    switch (type)
                    {
                        case ExpressionType.GreaterThan:
                        case ExpressionType.GreaterThanOrEqual:
                        case ExpressionType.LessThan:
                        case ExpressionType.LessThanOrEqual:
                            convertQueryBuilder(query);
                            break;
                        default:
                            conditionConverter.Convert(arguments[0]);
                            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport)
                            {
                                throw new InvalidCastException($"未知表达式类型 {conditionConverter.NotSupportType}.{conditionConverter.ExceptionType}");
                            }
                            System.Linq.Expressions.Expression left = conditionConverter.Expression;
                            if (left.isConstantNull())
                            {
                                convertQueryBuilder(query);
                                switch (type)
                                {
                                    case ExpressionType.Equal: CharStream.SimpleWrite("is null"); break;
                                    case ExpressionType.NotEqual: CharStream.SimpleWrite("is not null"); break;
                                }
                            }
                            else
                            {
                                convertIsSimple(left);
                                switch (type)
                                {
                                    case ExpressionType.Equal: CharStream.Write('='); break;
                                    case ExpressionType.NotEqual:
                                        CharStream.Write('<');
                                        CharStream.Write('>');
                                        break;
                                }
                                convertQueryBuilder(query);
                            }
                            break;
                    }
                }
            }
            else
            {
                switch (type)
                {
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                        convertIsSimple(right);
                        break;
                    default: throw new InvalidCastException($"不支持的比较操作类型 {expression.Method.Name} {type}");
                }
            }
        }
        /// <summary>
        /// 表达式重组以后再转换
        /// </summary>
        /// <param name="expression"></param>
        internal void ConditionConvertIsSimple(System.Linq.Expressions.Expression expression)
        {
            conditionConverter.Convert(expression);
            if (conditionConverter.Type == ConditionExpression.ConvertTypeEnum.NotSupport)
            {
                throw new InvalidCastException($"未知表达式类型 {conditionConverter.NotSupportType}.{conditionConverter.ExceptionType}");
            }
            convertIsSimple(conditionConverter.Expression);
        }
        /// <summary>
        /// 子查询转换
        /// </summary>
        /// <param name="query"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void convertQueryBuilder(IQueryBuilder query)
        {
            CharStream.Write('(');
            query.GetStatement(CharStream);
            CharStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="value"></param>
        private void convertConstant(object value)
        {
            if (value != null) (TableWriter.ConnectionPool.Creator.GetConstantConverter(value.GetType(), dateTimeMember))(CharStream, value);
            else CharStream.WriteJsonNull();
        }
    }
}
