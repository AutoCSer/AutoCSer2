using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 路由参数解析
    /// </summary>
    public partial struct RouteParameter
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        private readonly string requestPath;
        /// <summary>
        /// 开始解析位置
        /// </summary>
        private int startIndex;
        /// <summary>
        /// 是否解析结束
        /// </summary>
        private bool isEnd;
        /// <summary>
        /// 路由参数解析错误信息
        /// </summary>
#if NetStandard21
        private string? message;
#else
        private string message;
#endif
        /// <summary>
        /// 路由参数解析
        /// </summary>
        /// <param name="requestPath">请求路径</param>
        /// <param name="startIndex">开始解析位置</param>
        internal RouteParameter(string requestPath, int startIndex)
        {
            this.requestPath = requestPath;
            this.startIndex = startIndex;
            isEnd = false;
            message = null;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref bool value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0) isEnd = true;
                switch ((isEnd ? requestPath.Length : nextIndex) - startIndex - 4)
                {
                    case 0:
                        fixed (char* pathFixed = requestPath)
                        {
                            if ((*(ulong*)(pathFixed + startIndex) | 0x20002000200020UL) == 't' + ((ulong)'r' << 16) + ((ulong)'u' << 32) + ((ulong)'e' << 48))
                            {//true
                                startIndex = isEnd ? requestPath.Length : (nextIndex + 1);
                                return true;
                            }
                        }
                        break;
                    case 1:
                        fixed (char* pathFixed = requestPath)
                        {
                            char* start = pathFixed + startIndex;
                            if ((*start | 0x20) == 'f' && (*(ulong*)(start + 1) | 0x20002000200020UL) == 'a' + ((ulong)'l' << 16) + ((ulong)'s' << 32) + ((ulong)'e' << 48))
                            {//false
                                startIndex = isEnd ? requestPath.Length : (nextIndex + 1);
                                return true;
                            }
                        }
                        break;
                }
                message = $"参数 {name} 解析失败";
            }
            else message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref sbyte value)
        {
            int intValue = 0;
            if (Get(name, ref intValue))
            {
                if (intValue >= sbyte.MinValue && intValue <= sbyte.MaxValue)
                {
                    value = (sbyte)intValue;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref byte value)
        {
            uint intValue = 0;
            if (Get(name, ref intValue))
            {
                if (intValue >= byte.MinValue && intValue <= byte.MaxValue)
                {
                    value = (byte)intValue;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref short value)
        {
            int intValue = 0;
            if (Get(name, ref intValue))
            {
                if (intValue >= short.MinValue && intValue <= short.MaxValue)
                {
                    value = (short)intValue;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref ushort value)
        {
            uint intValue = 0;
            if (Get(name, ref intValue))
            {
                if (intValue >= ushort.MinValue && intValue <= ushort.MaxValue)
                {
                    value = (ushort)intValue;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref int value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                fixed (char* pathFixed = requestPath)
                {
                    bool isSinged = false;
                    char* start = pathFixed + startIndex, end = pathFixed + requestPath.Length;
                    if (*start == '-')
                    {
                        if (++start == end)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        if ((uint)(value = *start - '0') > 9)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        isSinged = true;
                    }
                    else if ((uint)(value = *start - '0') > 9)
                    {
                        message = $"参数 {name} 解析失败";
                        return false;
                    }
                    while (++start != end)
                    {
                        int code = *start - '0';
                        if ((uint)code > 9)
                        {
                            if (*start == '/')
                            {
                                if (isSinged) value = -value;
                                startIndex = (int)(start - pathFixed) + 1;
                                return true;
                            }
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        if ((code += value * 10) < value)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        value = code;
                    }
                    if (isSinged) value = -value;
                    startIndex = requestPath.Length;
                    return isEnd = true;
                }
            }
            message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref uint value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                fixed (char* pathFixed = requestPath)
                {
                    char* start = pathFixed + startIndex, end = pathFixed + requestPath.Length;
                    if ((value = (uint)(*start - '0')) > 9)
                    {
                        message = $"参数 {name} 解析失败";
                        return false;
                    }
                    while (++start != end)
                    {
                        uint code = (uint)(*start - '0');
                        if (code > 9)
                        {
                            if (*start == '/')
                            {
                                startIndex = (int)(start - pathFixed) + 1;
                                return true;
                            }
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        if ((code += value * 10) < value)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        value = code;
                    }
                    startIndex = requestPath.Length;
                    return isEnd = true;
                }
            }
            message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref long value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                fixed (char* pathFixed = requestPath)
                {
                    bool isSinged = false;
                    char* start = pathFixed + startIndex, end = pathFixed + requestPath.Length;
                    if (*start == '-')
                    {
                        if (++start == end)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        if ((ulong)(value = *start - '0') > 9)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        isSinged = true;
                    }
                    else if ((ulong)(value = *start - '0') > 9)
                    {
                        message = $"参数 {name} 解析失败";
                        return false;
                    }
                    while (++start != end)
                    {
                        long code = *start - '0';
                        if ((ulong)code > 9)
                        {
                            if (*start == '/')
                            {
                                if (isSinged) value = -value;
                                startIndex = (int)(start - pathFixed) + 1;
                                return true;
                            }
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        if ((code += value * 10) < value)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        value = code;
                    }
                    if (isSinged) value = -value;
                    startIndex = requestPath.Length;
                    return isEnd = true;
                }
            }
            message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref ulong value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                fixed (char* pathFixed = requestPath)
                {
                    char* start = pathFixed + startIndex, end = pathFixed + requestPath.Length;
                    if ((value = (ulong)(*start - '0')) > 9)
                    {
                        message = $"参数 {name} 解析失败";
                        return false;
                    }
                    while (++start != end)
                    {
                        ulong code = (ulong)(*start - '0');
                        if (code > 9)
                        {
                            if (*start == '/')
                            {
                                startIndex = (int)(start - pathFixed) + 1;
                                return true;
                            }
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        if ((code += value * 10) < value)
                        {
                            message = $"参数 {name} 解析失败";
                            return false;
                        }
                        value = code;
                    }
                    startIndex = requestPath.Length;
                    return isEnd = true;
                }
            }
            message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref SubString value)
        {
            if (!isEnd)
            {
                if (startIndex != requestPath.Length)
                {
                    int nextIndex = requestPath.IndexOf('/', startIndex);
                    if (nextIndex < 0)
                    {
                        value.Set(requestPath, startIndex, requestPath.Length - startIndex);
                        startIndex = requestPath.Length;
                        isEnd = true;
                    }
                    else
                    {
                        value.Set(requestPath, startIndex, nextIndex - startIndex);
                        startIndex = nextIndex + 1;
                    }
                }
                else
                {
                    value.SetEmpty();
                    isEnd = true;
                }
                return true;
            }
            message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref string value)
        {
            if (!isEnd)
            {
                if (startIndex != requestPath.Length)
                {
                    int nextIndex = requestPath.IndexOf('/', startIndex);
                    if (nextIndex < 0)
                    {
                        value = requestPath.Substring(startIndex);
                        startIndex = requestPath.Length;
                        isEnd = true;
                    }
                    else
                    {
                        value = requestPath.Substring(startIndex, nextIndex - startIndex);
                        startIndex = nextIndex + 1;
                    }
                }
                else
                {
                    value = string.Empty;
                    isEnd = true;
                }
                return true;
            }
            message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// JSON 参数解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool Get<T>(string name, ref T? value)
#else
        internal bool Get<T>(string name, ref T value)
#endif
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0)
                {
                    get(name, ref value, requestPath.Length);
                    startIndex = requestPath.Length;
                    isEnd = true;
                }
                else
                {
                    get(name, ref value, nextIndex);
                    startIndex = nextIndex + 1;
                }
                return message == null;
            }
            message = $"参数 {name} 缺少数据";
            return false;
        }
        /// <summary>
        /// 参数 JSON 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nextIndex"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
#if NetStandard21
        private void get<T>(string name, ref T? value, int nextIndex)
#else
        private void get<T>(string name, ref T value, int nextIndex)
#endif
        {
            AutoCSer.Json.DeserializeResult result = AutoCSer.JsonDeserializer.Deserialize(new SubString(startIndex, nextIndex - startIndex, requestPath), ref value);
            if (result.State != AutoCSer.Json.DeserializeStateEnum.Success) message = $"参数 {name} JSON 反序列化位置 {result.Index.toString()} 失败 {result.State}";
        }
        /// <summary>
        /// 解析结束检查
        /// </summary>
        /// <returns></returns>
        internal ResponseResult End()
        {
            if (message == null)
            {
                if (startIndex == requestPath.Length) return ResponseStateEnum.Success;
                message = $"路由参数解析错误位置 {startIndex.toString()}";
            }
            return new ResponseResult(ResponseStateEnum.RouteParameterFail, message);
        }
    }
}
