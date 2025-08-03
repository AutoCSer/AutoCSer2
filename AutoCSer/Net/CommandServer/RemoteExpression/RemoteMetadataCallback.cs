using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Reflection;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据信息编号回调
    /// </summary>
    internal sealed class RemoteMetadataCallback : AutoCSer.Threading.Link<RemoteMetadataCallback>
    {
        /// <summary>
        /// 远程类型编号集合
        /// </summary>
        internal readonly KeyValue<Type, int>[] Types;
        /// <summary>
        /// 远程方法编号集合
        /// </summary>
        internal readonly KeyValue<MethodInfo, int>[] Methods;
        /// <summary>
        /// 远程属性编号集合
        /// </summary>
        internal readonly KeyValue<PropertyInfo, int>[] Properties;
        /// <summary>
        /// 远程字段编号集合
        /// </summary>
        internal readonly KeyValue<FieldInfo, int>[] Fields;
        /// <summary>
        /// 远程元数据信息编号回调
        /// </summary>
        /// <param name="types"></param>
        /// <param name="methods"></param>
        /// <param name="properties"></param>
        /// <param name="fields"></param>
        internal RemoteMetadataCallback(KeyValue<Type, int>[] types, KeyValue<MethodInfo, int>[] methods, KeyValue<PropertyInfo, int>[] properties, KeyValue<FieldInfo, int>[] fields)
        {
            this.Types = types;
            this.Methods = methods;
            this.Properties = properties;
            this.Fields = fields;
        }
        /// <summary>
        /// 远程元数据信息编号回调
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
#if NetStandard21
        internal RemoteMetadataCallback? Callback(ClientMetadata metadata)
#else
        internal RemoteMetadataCallback Callback(ClientMetadata metadata)
#endif
        {
            metadata.Callback(this);
            return LinkNext;
        }
    }
}
