using System;
/*DateTime;float;double;decimal;TimeSpan;Guid*/

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
        internal unsafe bool Get(string name, ref DateTime value)
        {
            if (!isEnd && startIndex != requestPath.Length)
            {
                int nextIndex = requestPath.IndexOf('/', startIndex);
                if (nextIndex < 0)
                {
                    if (DateTime.TryParse(new SubString(startIndex, requestPath.Length - startIndex, requestPath), out value))
                    {
                        startIndex = requestPath.Length;
                        return isEnd = true;
                    }
                }
                else if (DateTime.TryParse(new SubString(startIndex, nextIndex - startIndex, requestPath), out value))
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
