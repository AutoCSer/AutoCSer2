using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Expression
{
    /// <summary>
    /// 创建表达式
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct Builder
    {
        /// <summary>
        /// 默认最大解析深度
        /// </summary>
        internal const int DefaultCheckDepth = 256;

        /// <summary>
        /// 字符状态位
        /// </summary>
        private readonly byte* bits;
        /// <summary>
        /// 表达式所在字符串
        /// </summary>
        private readonly string expressionString;
        /// <summary>
        /// 表达式所在字符串的起始位置
        /// </summary>
        private readonly char* expressionFixed;
        /// <summary>
        /// 解析字符串开始位置
        /// </summary>
        private readonly char* start;
        /// <summary>
        /// 解析字符结束位置
        /// </summary>
        private readonly char* end;
        /// <summary>
        /// 当前解析内容开始位置
        /// </summary>
        private char* currentStart;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        private char* current;
        /// <summary>
        /// 检查解析深度
        /// </summary>
        private int depth;
        /// <summary>
        /// 是否支持客户端成员 # / $ 索引位置
        /// </summary>
        private bool isClient;
        /// <summary>
        /// 表达式，失败为 null
        /// </summary>
#if NetStandard21
        internal ValueNode? Expression;
#else
        internal ValueNode Expression;
#endif
        /// <summary>
        /// 当前解析位置
        /// </summary>
        internal int Index { get { return (int)(current - start); } }
        /// <summary>
        /// 创建表达式
        /// </summary>
        /// <param name="expression">表达式字符串</param>
        /// <param name="isClient">是否支持客户端成员 # / $ 索引位置</param>
        internal Builder(ref SubString expression, bool isClient = false)
        {
            Expression = null;
            currentStart = null;
            depth = DefaultCheckDepth;
            bits = Bits.Byte;
            this.isClient = isClient;

            expressionString = expression.GetFixedBuffer();
            if (expression.Length != 0)
            {
                fixed (char* expressionFixed = expressionString)
                {
                    this.expressionFixed = expressionFixed;
                    current = start = expressionFixed + expression.Start;
                    end = start + expression.Length;

                    var value = this.value();
                    if (value != null)
                    {
                        do
                        {
                            if (current == end)
                            {
                                Expression = value;
                                return;
                            }
                        }
                        while (*++current == ' ');
                    }
                    return;
                }
            }
            end = current = start = expressionFixed = null;
        }
        /// <summary>
        /// 取值解析
        /// </summary>
        /// <returns>null 表示失败</returns>
#if NetStandard21
        private ValueNode? value()
#else
        private ValueNode value()
#endif
        {
            if (--depth != 0)
            {
                do
                {
                    char code = *current;
                    switch (code & 7)
                    {
                        case '!' & 7://0x21
                            if (code == '!')
                            {
                                if (++current != end)
                                {
                                    var notValue = new ValueNode(ValueTypeEnum.Not, this.value()).CheckNext();
                                    ++depth;
                                    return notValue;
                                }
                                return null;
                            }
                            break;
                        case '"' & 7://0x22
                            if (code == '"')
                            {
                                currentStart = current;
                                if (++current != end)
                                {
                                    bool isTransfer = false;
                                    do
                                    {
                                        if (!isTransfer)
                                        {
                                            if (*current == '"')
                                            {
                                                if (++current == end)
                                                {
                                                    ++depth;
                                                    return new ContentNode(ValueTypeEnum.String, new SubString(currentStart - expressionFixed, current - currentStart, expressionString));
                                                }
                                                ValueNode value = new ContentNode(ValueTypeEnum.String, new SubString(currentStart - expressionFixed, current - currentStart, expressionString), memberNext(ConstantTypeEnum.String));
                                                if (value.CheckNextNull())
                                                {
                                                    ++depth;
                                                    return value;
                                                }
                                                return null;
                                            }
                                            if (*current == '\\') isTransfer = true;
                                        }
                                        else isTransfer = false;
                                    }
                                    while (++current != end);
                                }
                                return null;
                            }
                            break;
                        case '(' & 7://0x28
                            if (code == '(')
                            {
                                if (++current != end)
                                {
                                    var value = this.value();
                                    if (value != null && ++current != end)
                                    {
                                        do
                                        {
                                            if (*current == ')')
                                            {
                                                if (++current == end)
                                                {
                                                    ++depth;
                                                    return new ParenthesisNode(value);
                                                }
                                                ParenthesisNode parenthesis = new ParenthesisNode(value, memberNext(ConstantTypeEnum.None));
                                                if (parenthesis.CheckNextNull())
                                                {
                                                    ++depth;
                                                    return parenthesis;
                                                }
                                                return null;
                                            }
                                            if (*current == ' ')
                                            {
                                                do
                                                {
                                                    if (++current == end) return null;
                                                }
                                                while (*current == ' ');
                                            }
                                            else return null;
                                        }
                                        while (true);
                                    }
                                }
                                return null;
                            }
                            break;
                        case '#' & 7://0x23
                                     //case '+' & 7://0x2b
                            if (code == '#')
                            {
                                if (isClient)
                                {
                                    if (++current == end)
                                    {
                                        ++depth;
                                        return new ValueNode(ValueTypeEnum.Client, null);
                                    }
                                    if (((bits[*(byte*)current] & memberNameStartBit) | *((byte*)current + 1)) == 0)
                                    {
                                        currentStart = current;
                                        var hashNode = new ValueNode(ValueTypeEnum.Client, member(-1)).CheckNext();
                                        ++depth;
                                        return hashNode;
                                    }
                                }
                                return null;
                            }
                            if (code == '+') return signed();
                            break;
                        case '$' & 7://0x24
                            if (code == '$')
                            {
                                if (isClient && ++current != end && ((bits[*(byte*)current] & memberNameStartBit) | *((byte*)current + 1)) == 0)
                                {
                                    currentStart = current;
                                    var clientNode = new ValueNode(ValueTypeEnum.Client, member(-1)).CheckNext();
                                    ++depth;
                                    return clientNode;
                                }
                                return null;
                            }
                            break;
                        case '-' & 7://0x2d
                            if (code == '-') return signed();
                            break;
                        case '.' & 7://0x2e
                            if (code == '.')
                            {
                                currentStart = current;
                                if (++current != end)
                                {
                                    if ((uint)(*current - '0') >= 10)
                                    {
                                        while (*current == '.')
                                        {
                                            if (++current == end) return null;
                                        }
                                        int memberDepth = (int)(current - currentStart);
                                        if (memberDepth <= byte.MaxValue && ((bits[*(byte*)current] & memberNameStartBit) | *((byte*)current + 1)) == 0)
                                        {
                                            currentStart = current;
                                            var value = member(memberDepth);
                                            ++depth;
                                            return value;
                                        }
                                        return null;
                                    }
                                    return getFloat(ValueTypeEnum.Decimal);
                                }
                                return null;
                            }
                            break;
                    }
                    if (((bits[(byte)code] & memberNameBit) | (code >> 8)) == 0)
                    {
                        currentStart = current;
                        var value = (uint)(code - '0') >= 10 ? member(0) : number(code, false);
                        ++depth;
                        return value;
                    }
                    if (code != ' ') return null;
                    do
                    {
                        if (++current == end) return null;
                    }
                    while (*current == ' ');
                }
                while (true);
            }
            return null;
        }
        /// <summary>
        /// 成员解析
        /// </summary>
        /// <param name="memberDepth">成员回溯深度</param>
        /// <returns>null 表示失败</returns>
#if NetStandard21
        private ValueNode? member(int memberDepth)
#else
        private ValueNode member(int memberDepth)
#endif
        {
            while (++current != end)
            {
                if (((bits[*(byte*)current] & memberNameBit) | *((byte*)current + 1)) != 0)
                {
                    if (memberDepth == 0)
                    {
                        switch ((int)(current - currentStart) - 4)
                        {
                            case 0://true
                                if (*(ulong*)currentStart == 't' + ('r' << 16) + ((ulong)'u' << 32) + ((ulong)'e' << 48)) return new ValueNode(ValueTypeEnum.True);
                                break;
                            case 1://false
                                if (*currentStart == 'f' && *(ulong*)(currentStart + 1) == 'a' + ('l' << 16) + ((ulong)'s' << 32) + ((ulong)'e' << 48)) return new ValueNode(ValueTypeEnum.False);
                                break;
                        }
                    }
                    if (--depth != 0)
                    {
                        ValueNode value = new ContentNode(ValueTypeEnum.Member, new SubString(currentStart - expressionFixed, current - currentStart, expressionString), memberNext(ConstantTypeEnum.None), (byte)Math.Max(memberDepth, 0));
                        if (value.CheckNextNull()) return value;
                    }
                    return null;
                }
            }
            return new ContentNode(ValueTypeEnum.Member, new SubString(currentStart - expressionFixed, current - currentStart, expressionString), null, (byte)Math.Max(memberDepth, 0));
        }
        /// <summary>
        /// 成员后续解析，调用者需要检查解析深度
        /// </summary>
        /// <param name="constantType"></param>
        /// <returns>null 表示失败，ValueExpression.Null 表示返回 null</returns>
#if NetStandard21
        private ValueNode? memberNext(ConstantTypeEnum constantType)
#else
        private ValueNode memberNext(ConstantTypeEnum constantType)
#endif
        {
            do
            {
                char code = *current;
                switch (code & 0xf)
                {
                    case ' ' & 0xf://0x20
                        if (code == ' ')
                        {
                            do
                            {
                                if (++current == end) return ValueNode.Null;
                            }
                            while (*current == ' ');
                            break;
                        }
                        return ValueNode.Null;
                    case '!' & 0xf://0x21
                        if (code == '!')
                        {
                            if (++current != end && *current == '=' && ++current != end) return new ValueNode(ValueTypeEnum.NotEqual, value()).CheckNext();
                            return null;
                        }
                        return ValueNode.Null;
                    case '$' & 0xf://0x24
                        if (code == '$')
                        {
                            if (isClient && ++current != end && constantType == ConstantTypeEnum.None)
                            {
                                if (((bits[*(byte*)current] & memberNameStartBit) | *((byte*)current + 1)) == 0)
                                {
                                    currentStart = current;
                                    var node = new ValueNode(ValueTypeEnum.NextMember, member(-1)).CheckNext();
                                    if (node != null) return new ValueNode(ValueTypeEnum.Client, node).CheckNext();
                                }
                                else if (*current == '&')
                                {
                                    ++current;
                                    return new ValueNode(ValueTypeEnum.ClientEncode);
                                }
                            }
                            return null;
                        }
                        return ValueNode.Null;
                    case '%' & 0xf://0x25
                        if (code == '%')
                        {
                            return ++current != end && constantType != ConstantTypeEnum.String ? new ValueNode(ValueTypeEnum.Mod, value()).CheckNext() : null;
                        }
                        return ValueNode.Null;
                    case '&' & 0xf://0x26
                        if (code == '&')
                        {
                            if (++current != end)
                            {
                                if (*current == '&') return ++current != end ? new ValueNode(ValueTypeEnum.And, value()).CheckNext() : null;
                                if (constantType != ConstantTypeEnum.String) return new ValueNode(ValueTypeEnum.BitAnd, value()).CheckNext();
                            }
                            return null;
                        }
                        return ValueNode.Null;
                    case '(' & 0xf://0x28
                        if (code == '(')
                        {
                            if (++current != end && constantType == ConstantTypeEnum.None)
                            {
                                var parameter = this.value();
                                if (current != end)
                                {
                                    LeftArray<ValueNode> parameters = new LeftArray<ValueNode>(0);
                                    if (parameter != null)
                                    {
                                        parameters.Add(parameter);
                                        do
                                        {
                                            if ((code = *current) == ')')
                                            {
                                                CallNode call = new CallNode(ValueTypeEnum.Call, ref parameters);
                                                return ++current == end || callNext(call) ? call : null;
                                            }
                                            if (code == ',')
                                            {
                                                if (++current != end)
                                                {
                                                    parameter = this.value();
                                                    if (parameter != null && current != end) parameters.Add(parameter);
                                                    else return null;
                                                }
                                                else return null;
                                            }
                                            else if (code == ' ')
                                            {
                                                do
                                                {
                                                    if (++current == end) return null;
                                                }
                                                while (*current == ' ');
                                            }
                                            else return null;
                                        }
                                        while (true);
                                    }
                                    if (*current == ')')
                                    {
                                        CallNode call = new CallNode(ValueTypeEnum.Call, ref parameters);
                                        if(++current == end || callNext(call)) return call;
                                    }
                                }
                            }
                            return null;
                        }
                        return ValueNode.Null;
                    case '*' & 0xf://0x2a
                        if (code == '*')
                        {
                            return ++current != end && constantType != ConstantTypeEnum.String ? new ValueNode(ValueTypeEnum.Multiply, value()).CheckNext() : null;
                        }
                        return ValueNode.Null;
                    case '+' & 0xf://0x2b
                                   //case '[' & 0xf://0x5b
                        if (code == '+') return ++current != end ? new ValueNode(ValueTypeEnum.Add, value()).CheckNext() : null;
                        if (code == '[') return ++current != end && constantType == ConstantTypeEnum.None ? index(ValueTypeEnum.Index) : null;
                        return ValueNode.Null;
                    case '<' & 0xf://0x3c
                                   //case '|' & 0xf://0x7c
                        if (code == '<')
                        {
                            if (++current != end)
                            {
                                switch (*current - 0x3c)
                                {
                                    case '<' - 0x3c://0x3c
                                        if (++current != end && constantType != ConstantTypeEnum.String) return new ValueNode(ValueTypeEnum.LeftShift, value()).CheckNext();
                                        break;
                                    case '=' - 0x3c://0x3d
                                        if (++current != end) return new ValueNode(ValueTypeEnum.LessOrEqual, value()).CheckNext();
                                        break;
                                    case '>' - 0x3c://0x3e
                                        if (++current != end) return new ValueNode(ValueTypeEnum.NotEqual, value()).CheckNext();
                                        break;
                                    default: return new ValueNode(ValueTypeEnum.Less, value()).CheckNext();
                                }
                            }
                            return null;
                        }
                        if (code == '|')
                        {
                            if (++current != end)
                            {
                                if (*current == '|') return ++current != end ? new ValueNode(ValueTypeEnum.Or, value()).CheckNext() : null;
                                if (constantType != ConstantTypeEnum.String) return new ValueNode(ValueTypeEnum.BitOr, value()).CheckNext();
                            }
                            return null;
                        }
                        return ValueNode.Null;
                    case '=' & 0xf://0x3d
                                   //case '-' & 0xf://0x2d
                        if (code == '=')
                        {
                            if (++current != end && *current == '=' && ++current != end) return new ValueNode(ValueTypeEnum.Equal, value()).CheckNext();
                            return null;
                        }
                        if (code == '-') return ++current != end && constantType != ConstantTypeEnum.String ? new ValueNode(ValueTypeEnum.Subtract, value()).CheckNext() : null;
                        return ValueNode.Null;
                    case '.' & 0xf://0x2e
                                   //case '>' & 0xf://0x3e
                                   //case '^' & 0xf://0x5e
                        if (code == '.')
                        {
                            if (++current != end && constantType == ConstantTypeEnum.None && ((bits[*(byte*)current] & memberNameStartBit) | *((byte*)current + 1)) == 0)
                            {
                                currentStart = current;
                                return new ValueNode(ValueTypeEnum.NextMember, member(-1)).CheckNext();
                            }
                            return null;
                        }
                        if (code == '>')
                        {
                            if (++current != end)
                            {
                                switch (*current - 0x3d)
                                {
                                    case '=' - 0x3d://0x3d
                                        if (++current != end) return new ValueNode(ValueTypeEnum.GreaterOrEqual, value()).CheckNext();
                                        break;
                                    case '>' - 0x3d://0x3e
                                        if (++current != end && constantType != ConstantTypeEnum.String) return new ValueNode(ValueTypeEnum.RightShift, value()).CheckNext();
                                        break;
                                    default: return new ValueNode(ValueTypeEnum.Greater, value()).CheckNext();
                                }
                            }
                            return null;
                        }
                        if (code == '^') return ++current != end && constantType != ConstantTypeEnum.String ? new ValueNode(ValueTypeEnum.Xor, value()).CheckNext() : null;
                        return ValueNode.Null;
                    case '/' & 0xf://0x2f
                                   //case '?' & 0xf://0x3f
                        if (code == '/') return ++current != end && constantType != ConstantTypeEnum.String ? new ValueNode(ValueTypeEnum.Divide, value()).CheckNext() : null;
                        if (code == '?')
                        {
                            if (++current != end)
                            {
                                if ((code = *current) == '.')
                                {
                                    if (constantType == ConstantTypeEnum.None && ++current != end && ((bits[*(byte*)current] & memberNameStartBit) | *((byte*)current + 1)) == 0)
                                    {
                                        currentStart = current;
                                        return new ValueNode(ValueTypeEnum.IfNotNullMember, member(-1)).CheckNext();
                                    }
                                    return null;
                                }
                                if (code == '?') return ++current != end ? new ValueNode(ValueTypeEnum.IfNullThen, value()).CheckNext() : null;
                                if (code == '[') return constantType == ConstantTypeEnum.None && ++current != end ? index(ValueTypeEnum.IfNotNullIndex) : null;
                                var ifValue = value();
                                if (ifValue != null && current != end)
                                {
                                    do
                                    {
                                        if ((code = *current) == ':')
                                        {
                                            if(++current != end)
                                            {
                                                var elseValue = value();
                                                if (elseValue != null) return new IfElseNode(ifValue, elseValue);
                                            }
                                            return null;
                                        }
                                        if (code == ' ')
                                        {
                                            do
                                            {
                                                if (++current == end) return null;
                                            }
                                            while (*current == ' ');
                                        }
                                        else return null;
                                    }
                                    while (true);
                                }
                            }
                            return null;
                        }
                        return ValueNode.Null;
                    default: return ValueNode.Null;
                }
            }
            while (true);
        }
        /// <summary>
        /// 方法调用后续解析
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool callNext(CallNode call)
        {
            if (--depth != 0 && call.SetNext(memberNext(ConstantTypeEnum.None)))
            {
                ++depth;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 索引调用解析
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns>null 表示失败</returns>
#if NetStandard21
        private CallNode? index(ValueTypeEnum valueType)
#else
        private CallNode index(ValueTypeEnum valueType)
#endif
        {
            LeftArray<ValueNode> parameters = new LeftArray<ValueNode>(0);
            var parameter = value();
            if (current != end)
            {
                if (parameter != null)
                {
                    parameters.Add(parameter);
                    do
                    {
                        char code = *current;
                        if (code == ']')
                        {
                            CallNode call = new CallNode(valueType, ref parameters);
                            return ++current == end || callNext(call) ? call : null;
                        }
                        if (code == ',')
                        {
                            if (++current != end)
                            {
                                parameter = value();
                                if (parameter != null && current != end) parameters.Add(parameter);
                                else return null;
                            }
                            else return null;
                        }
                        else if (code == ' ')
                        {
                            do
                            {
                                if (++current == end) return null;
                            }
                            while (*current == ' ');
                        }
                        else return null;
                    }
                    while (true);
                }
                if (*current == ']')
                {
                    CallNode call = new CallNode(valueType, ref parameters);
                    if (++current == end || callNext(call)) return call;
                }
            }
            return null;
        }
        /// <summary>
        /// 带符号数解析
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        private ValueNode? signed()
#else
        private ValueNode signed()
#endif
        {
            currentStart = current;
            if (++current != end)
            {
                char numberCode = *current;
                if ((uint)(numberCode - '0') < 10)
                {
                    var value = number(numberCode, true);
                    ++depth;
                    return value;
                }
                if (numberCode == '.' && ++current != end && (uint)(*current - '0') < 10) return getFloat(ValueTypeEnum.SignedDecimal);
            }
            return null;
        }
        /// <summary>
        /// 小数点开头的浮点数解析
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
#if NetStandard21
        private ValueNode? getFloat(ValueTypeEnum valueType)
#else
        private ValueNode getFloat(ValueTypeEnum valueType)
#endif
        {
            do
            {
                if (++current == end)
                {
                    ++depth;
                    return new ContentNode(valueType, new SubString(currentStart - expressionFixed, current - currentStart, expressionString));
                }
            }
            while ((uint)(*current - '0') < 10);
            var value = numberNext(valueType);
            ++depth;
            return value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isSigned"></param>
        /// <returns>null 表示失败</returns>
#if NetStandard21
        private ValueNode? number(char code, bool isSigned)
#else
        private ValueNode number(char code, bool isSigned)
#endif
        {
            ValueTypeEnum valueType = isSigned ? ValueTypeEnum.SignedDecimalism : ValueTypeEnum.Decimalism;
            if (++current != end)
            {
                if (code != '0')
                {
                    while ((uint)(*current - '0') < 10)
                    {
                        if (++current == end) return new ContentNode(valueType, new SubString(currentStart - expressionFixed, current - currentStart, expressionString));
                    }
                    if (*current == '.')
                    {
                        do
                        {
                            if (++current == end) return new ContentNode(valueType, new SubString(currentStart - expressionFixed, current - currentStart, expressionString));
                        }
                        while ((uint)(*current - '0') < 10);
                    }
                    return numberNext(valueType);
                }
                if ((*current | 0x20) == 'x')
                {
                    if (++current != end && ((bits[*(byte*)current] & hexBit) | *((byte*)current + 1)) == 0)
                    {
                        valueType = isSigned ? ValueTypeEnum.SignedHex : ValueTypeEnum.Hex;
                        do
                        {
                            if (++current == end) return new ContentNode(valueType, new SubString(currentStart - expressionFixed, current - currentStart, expressionString));
                        }
                        while (((bits[*(byte*)current] & hexBit) | *((byte*)current + 1)) == 0);
                        return numberNext(valueType);
                    }
                    return null;
                }
                return numberNext(valueType);
            }
            return new ContentNode(valueType, new SubString(currentStart - expressionFixed, current - currentStart, expressionString));
        }
        /// <summary>
        /// 数字后续解析
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns>null 表示失败</returns>
#if NetStandard21
        private ValueNode? numberNext(ValueTypeEnum valueType)
#else
        private ValueNode numberNext(ValueTypeEnum valueType)
#endif
        {
            if (--depth != 0)
            {
                ValueNode value = new ContentNode(valueType, new SubString(currentStart - expressionFixed, current - currentStart, expressionString), memberNext(ConstantTypeEnum.Number));
                if (value.CheckNextNull())
                {
                    ++depth;
                    return value;
                }
            }
            return null;
        }

        /// <summary>
        /// 成员名称开始符号
        /// </summary>
        private const byte memberNameStartBit = 1;
        /// <summary>
        /// 成员名称符号
        /// </summary>
        private const byte memberNameBit = 2;
        /// <summary>
        /// 十六进制字符
        /// </summary>
        private const byte hexBit = 4;
        /// <summary>
        /// 字符状态位
        /// </summary>
        internal static Pointer Bits;
        unsafe static Builder()
        {
            Bits = AutoCSer.Extensions.Memory.Unmanaged.GetRawExpressionBits();
            byte* bits = Bits.Byte;
            AutoCSer.Common.Fill(Bits.ULong, 256 >> 3, ulong.MaxValue);
            bits['_'] &= (memberNameStartBit | memberNameBit) ^ 255;
            for (int value = '0'; value <= '9'; ++value) bits[value] &= (memberNameBit | hexBit) ^ 255;
            for (int value = 'A'; value <= 'F'; ++value)
            {
                bits[value] &= (memberNameStartBit | memberNameBit | hexBit) ^ 255;
                bits[value | 0x20] &= (memberNameStartBit | memberNameBit | hexBit) ^ 255;
            }
            for (int value = 'G'; value <= 'Z'; ++value)
            {
                bits[value] &= (memberNameStartBit | memberNameBit) ^ 255;
                bits[value | 0x20] &= (memberNameStartBit | memberNameBit) ^ 255;
            }
        }
    }
}
