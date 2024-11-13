using AutoCSer.Extensions;
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
#if NetStandard21
        internal Dictionary<HashSubString, JsonApiRouteNode>? Nodes;
#else
        internal Dictionary<HashSubString, JsonApiRouteNode> Nodes;
#endif
        /// <summary>
        /// JSON API 请求实例
        /// </summary>
#if NetStandard21
        internal JsonApiRequest? Request;
#else
        internal JsonApiRequest Request;
#endif
        /// <summary>
        /// JSON API 路由节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="pathIndex">模板解析开始位置</param>
#if NetStandard21
        internal JsonApiRouteNode(JsonApiRouteNode? parent, int pathIndex)
#else
        internal JsonApiRouteNode(JsonApiRouteNode parent, int pathIndex)
#endif
        {
            this.parent = parent ?? this;
            this.pathIndex = pathIndex;
        }
        /// <summary>
        /// 尝试添加子节点
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal JsonApiRouteNode AppendNode(SubString path)
        {
            var node = default(JsonApiRouteNode);
            if (Nodes != null)
            {
                if (Nodes.TryGetValue(path, out node)) return node;
            }
            else Nodes = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<JsonApiRouteNode>();
            Nodes.Add(path.ToString().notNull(), node = new JsonApiRouteNode(this, path.Length + pathIndex + 1));
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
            return Request.notNull().Request(httpContext, requestPath, pathIndex);
        }
    }
}
