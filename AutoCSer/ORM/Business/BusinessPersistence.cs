using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 业务表格持久化
    /// </summary>
    public static class BusinessPersistence
    {
        /// <summary>
        /// 获取持久化表格模型类型构造函数
        /// </summary>
        /// <param name="constructors">构造函数集合</param>
        /// <param name="persistenceTypes">持久化数据信息类型集合</param>
        public static void GetConstructors(Dictionary<HashObject<System.Type>, KeyValue<Delegate, Type>> constructors, IEnumerable<Type> persistenceTypes)
        {
            KeyValue<Delegate, Type> constructor;
            foreach (Type persistenceType in persistenceTypes)
            {
                foreach (FieldInfo field in persistenceType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    Type type = field.FieldType;
                    if (type.IsGenericType)
                    {
                        Type genericType = type.GetGenericTypeDefinition();
                        if (genericType == typeof(BusinessPersistence<,,>))
                        {
                            Type[] types = type.GetGenericArguments();
                            HashObject<System.Type> baseType = types[1];
                            if (constructors.TryGetValue(baseType, out constructor))
                            {
                                if (constructor.Value != types[0] && !types[0].IsAssignableFrom(constructor.Value))
                                {
                                    LogHelper.ErrorIgnoreException($"数据库表格模型 {types[1].fullName()} 映射业务模型 {types[0].fullName()} 与 {constructor.Value.fullName()} 冲突");
                                }
                            }
                            else constructors.Add(baseType, new KeyValue<Delegate, Type>(AutoCSer.ORM.Metadata.BaseGenericType.Get(types[0], types[1]).BusinessConstructorDelegate, types[0]));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 持久化表格模型类型构造函数
        /// </summary>
        /// <typeparam name="BT">业务表格模型类型</typeparam>
        /// <typeparam name="T">持久化表格模型类型</typeparam>
        /// <returns></returns>
        internal static T BusinessConstructor<BT, T>()
            where BT : class, T
            where T : class
        {
            return AutoCSer.Metadata.DefaultConstructor<BT>.Constructor();
        }
    }
    /// <summary>
    /// 业务表格持久化
    /// </summary>
    /// <typeparam name="BT">业务表格模型类型</typeparam>
    /// <typeparam name="T">持久化表格模型类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct BusinessPersistence<BT, T, KT>
        where BT : class, T
        where T : class
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 业务表格模型持久化查询
        /// </summary>
        public readonly BusinessQuery<BT, T, KT> Query;
        /// <summary>
        /// 业务表格模型持久化写入
        /// </summary>
        public readonly BusinessWriter<BT, T, KT> Writer;
        /// <summary>
        /// 数据库表格持久化
        /// </summary>
        /// <param name="query"></param>
        internal BusinessPersistence(TableQuery<T, KT> query)
        {
            Query = new BusinessQuery<BT, T, KT>(query);
            Writer = new BusinessWriter<BT, T, KT>(query.Writer);
        }
    }
}
