//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 路由参数解析
    /// </summary>
    public partial struct RouteParameter
    {
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref float value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0)
                {
                    if (float.TryParse(new SubString(startIndex, requestPath.Length - startIndex, requestPath), out value))
                    {
                        startIndex = requestPath.Length;
                        return isEnd = true;
                    }
                }
                else if (float.TryParse(new SubString(startIndex, nextIndex - startIndex, requestPath), out value))
                {
                    startIndex = nextIndex + 1;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            else message = $"参数 {name} 缺少数据";
            return false;
        }
    }
}

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 路由参数解析
    /// </summary>
    public partial struct RouteParameter
    {
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref double value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0)
                {
                    if (double.TryParse(new SubString(startIndex, requestPath.Length - startIndex, requestPath), out value))
                    {
                        startIndex = requestPath.Length;
                        return isEnd = true;
                    }
                }
                else if (double.TryParse(new SubString(startIndex, nextIndex - startIndex, requestPath), out value))
                {
                    startIndex = nextIndex + 1;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            else message = $"参数 {name} 缺少数据";
            return false;
        }
    }
}

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 路由参数解析
    /// </summary>
    public partial struct RouteParameter
    {
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref decimal value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0)
                {
                    if (decimal.TryParse(new SubString(startIndex, requestPath.Length - startIndex, requestPath), out value))
                    {
                        startIndex = requestPath.Length;
                        return isEnd = true;
                    }
                }
                else if (decimal.TryParse(new SubString(startIndex, nextIndex - startIndex, requestPath), out value))
                {
                    startIndex = nextIndex + 1;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            else message = $"参数 {name} 缺少数据";
            return false;
        }
    }
}

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 路由参数解析
    /// </summary>
    public partial struct RouteParameter
    {
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref TimeSpan value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0)
                {
                    if (TimeSpan.TryParse(new SubString(startIndex, requestPath.Length - startIndex, requestPath), out value))
                    {
                        startIndex = requestPath.Length;
                        return isEnd = true;
                    }
                }
                else if (TimeSpan.TryParse(new SubString(startIndex, nextIndex - startIndex, requestPath), out value))
                {
                    startIndex = nextIndex + 1;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            else message = $"参数 {name} 缺少数据";
            return false;
        }
    }
}

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 路由参数解析
    /// </summary>
    public partial struct RouteParameter
    {
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal unsafe bool Get(string name, ref Guid value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0)
                {
                    if (Guid.TryParse(new SubString(startIndex, requestPath.Length - startIndex, requestPath), out value))
                    {
                        startIndex = requestPath.Length;
                        return isEnd = true;
                    }
                }
                else if (Guid.TryParse(new SubString(startIndex, nextIndex - startIndex, requestPath), out value))
                {
                    startIndex = nextIndex + 1;
                    return true;
                }
                message = $"参数 {name} 解析失败";
            }
            else message = $"参数 {name} 缺少数据";
            return false;
        }
    }
}

#endif