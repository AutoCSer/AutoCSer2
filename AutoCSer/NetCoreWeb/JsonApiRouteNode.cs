using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 路由节点
    /// </summary>
    internal sealed class JsonApiRouteNode
    {
        /// <summary>
        /// 父节点
        /// </summary>
        private readonly JsonApiRouteNode parent;
        /// <summary>
        /// 模板解析开始位置
        /// </summary>
        private readonly int pathIndex;
        /// <summary>
        /// 子节点集合
        /// </summary>
        internal Dictionary<HashSubString, JsonApiRouteNode> Nodes;
        /// <summary>
        /// JSON API 请求实例
        /// </summary>
        internal JsonApiRequest Request;
        /// <summary>
        /// JSON API 路由节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="pathIndex">模板解析开始位置</param>
        internal JsonApiRouteNode(JsonApiRouteNode parent, int pathIndex)
        {
            this.parent = parent;
            this.pathIndex = pathIndex;
        }
        /// <summary>
        /// 尝试添加子节点
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal JsonApiRouteNode AppendNode(SubString path)
        {
            JsonApiRouteNode node;
            if (Nodes != null)
            {
                if (Nodes.TryGetValue(path, out node)) return node;
            }
            else Nodes = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<JsonApiRouteNode>();
            Nodes.Add((string)path, node = new JsonApiRouteNode(this, path.Length + pathIndex + 1));
            return node;
        }
        /// <summary>
        /// JSON API 请求
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="requestPath">请求地址</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task LoadRequest(HttpContext httpContext, string requestPath)
        {
            return Request.Request(httpContext, requestPath, pathIndex);
        }
    }
}
