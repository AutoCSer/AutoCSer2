using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer.RemoteExpression;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 远程元数据构造函数编号信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    internal struct RemoteMetadataConstructorIndex
    {
        /// <summary>
        /// 构造函数编号
        /// </summary>
        public int Index;
        /// <summary>
        /// 类型编号
        /// </summary>
        public int TypeIndex;
        /// <summary>
        /// 参数类型集合
        /// </summary>
        public int[] ParameterTypes;
        /// <summary>
        /// 远程元数据构造函数编号信息
        /// </summary>
        /// <param name="typeIndex"></param>
        /// <param name="parameterTypes"></param>
        internal RemoteMetadataConstructorIndex(int typeIndex, int[] parameterTypes)
        {
            Index = 0;
            TypeIndex = typeIndex;
            ParameterTypes = parameterTypes;
        }
        /// <summary>
        /// 获取构造函数信息
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="types"></param>
        /// <returns></returns>
#if NetStandard21
        internal ConstructorInfo? GetConstructor(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#else
        internal ConstructorInfo GetConstructor(ClientMetadata metadata, ref LeftArray<KeyValue<Type, int>> types)
#endif
        {
            var type = metadata.GetType(TypeIndex, ref types);
            if (type != null)
            {
                var parameterTypes = metadata.GetTypeArray(ParameterTypes, ref types);
                if (parameterTypes != null) return type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, parameterTypes, null);
            }
            return null;
        }
    }
}
