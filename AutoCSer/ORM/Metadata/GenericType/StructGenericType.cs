using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class StructGenericType : AutoCSer.Metadata.GenericTypeCache<StructGenericType>
    {
        /// <summary>
        /// 自定义数据列配置
        /// </summary>
        internal abstract CustomColumnAttribute CustomColumnAttribute { get; }
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        internal abstract Delegate CustomColumnTableReadDelegate { get; }
        /// <summary>
        /// 写入表格模型数据
        /// </summary>
        internal abstract Delegate CustomColumnTableInsertDelegate { get; }
        /// <summary>
        /// 写入更新数据
        /// </summary>
        internal abstract Delegate CustomColumnTableUpdateDelegate { get; }
        /// <summary>
        /// 写入条件数据
        /// </summary>
        internal abstract Delegate CustomColumnTableConcatConditionDelegate { get; }
        /// <summary>
        /// 自定义数据列表格字段数量
        /// </summary>
        internal abstract int CustomColumnMemberCount { get; }
        /// <summary>
        /// 递归获取自定义数据列所有表格列名称
        /// </summary>
        internal abstract Func<string, string, IEnumerable<CustomColumnName>> GetCustomColumnMemberNames { get; }
        /// <summary>
        /// 递归匹配自定义数据列名称
        /// </summary>
        internal abstract Func<MemberExpression, LeftArray<MemberExpression>, string, string, CustomColumnName> GetCustomColumnMemberName { get; }
        /// <summary>
        /// 递归匹配自定义数据列获取数值
        /// </summary>
        internal abstract Func<MemberExpression, LeftArray<MemberExpression>, object, string, string, IEnumerable<KeyValue<CustomColumnName, object>>> GetCustomColumnMemberNameValues { get; }
        ///// <summary>
        ///// 根据名称获取成员
        ///// </summary>
        //internal abstract Func<string, Member> GetCustomColumnMember { get; }
        /// <summary>
        /// 自定义数据列值转数组
        /// </summary>
        internal abstract Delegate CustomColumnToArrayDelegate { get; }
        /// <summary>
        /// 自定义数据列值转数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array"></param>
        /// <param name="index"></param>
        internal abstract void CustomColumnToArray(object value, object[] array, ref int index);
        /// <summary>
        /// 判断可空类型是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal abstract bool IsNullableHasValue(object value);
        /// <summary>
        /// 获取可空类型数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal abstract object GetNullableValue(object value);
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        internal abstract Delegate CustomColumnModelReaderDelegate { get; }
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        internal abstract Delegate CustomColumnModelRemoteProxyReaderDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static StructGenericType create<T>() where T : struct
        {
            return new StructGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static StructGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StructGenericType Get(Type type)
        {
            StructGenericType value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class StructGenericType<T> : StructGenericType
        where T : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 自定义数据列配置
        /// </summary>
        internal override CustomColumnAttribute CustomColumnAttribute { get { return CustomColumn.ModelMetadata<T>.Attribute; } }
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        internal override Delegate CustomColumnTableReadDelegate { get { return (CustomColumn.TableModel<T>.ReaderDelegate)CustomColumn.TableModel<T>.Read; } }
        /// <summary>
        /// 写入表格模型数据
        /// </summary>
        internal override Delegate CustomColumnTableInsertDelegate { get { return (Func<CharStream, T, TableWriter, int, int>)CustomColumn.TableModel<T>.Insert; } }
        /// <summary>
        /// 写入更新数据
        /// </summary>
        internal override Delegate CustomColumnTableUpdateDelegate { get { return (Func<CharStream, T, TableWriter, int, bool, int>)CustomColumn.TableModel<T>.Update; } }
        /// <summary>
        /// 写入条件数据
        /// </summary>
        internal override Delegate CustomColumnTableConcatConditionDelegate { get { return (Func<CharStream, T, TableWriter, int, bool, int>)CustomColumn.TableModel<T>.ConcatCondition; } }
        /// <summary>
        /// 自定义数据列表格字段数量
        /// </summary>
        internal override int CustomColumnMemberCount { get { return CustomColumn.ModelMetadata<T>.MemberCount; } }
        /// <summary>
        /// 递归获取自定义数据列所有表格列名称
        /// </summary>
        internal override Func<string, string, IEnumerable<CustomColumnName>> GetCustomColumnMemberNames { get { return CustomColumn.ModelMetadata<T>.GetMemberNames; } }
        /// <summary>
        /// 递归匹配自定义数据列名称
        /// </summary>
        internal override Func<MemberExpression, LeftArray<MemberExpression>, string, string, CustomColumnName> GetCustomColumnMemberName { get { return CustomColumn.ModelMetadata<T>.GetMemberName; } }
        /// <summary>
        /// 递归匹配自定义数据列获取数值
        /// </summary>
        internal override Func<MemberExpression, LeftArray<MemberExpression>, object, string, string, IEnumerable<KeyValue<CustomColumnName, object>>> GetCustomColumnMemberNameValues { get { return CustomColumn.ModelMetadata<T>.GetMemberNameValues; } }
        ///// <summary>
        ///// 根据名称获取成员
        ///// </summary>
        //internal override Func<string, Member> GetCustomColumnMember { get { return CustomColumn.ModelMetadata<T>.GetMember; } }
        /// <summary>
        /// 自定义数据列值转数组
        /// </summary>
        internal override Delegate CustomColumnToArrayDelegate { get { return (CustomColumn.ToArray<T>.Writer)CustomColumn.ToArray<T>.Write; } }
        /// <summary>
        /// 自定义数据列值转数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="array"></param>
        /// <param name="index"></param>
        internal override void CustomColumnToArray(object value, object[] array, ref int index) { CustomColumn.ToArray<T>.Write((T)value, array, ref index); }
        /// <summary>
        /// 判断可空类型是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal override bool IsNullableHasValue(object value)
        {
            return ((T?)value).HasValue;
        }
        /// <summary>
        /// 获取可空类型数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal override object GetNullableValue(object value)
        {
            return ((T?)value).Value;
        }
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        internal override Delegate CustomColumnModelReaderDelegate { get { return (CustomColumn.ModelReader<T>.ReaderDelegate)CustomColumn.ModelReader<T>.Read; } }
        /// <summary>
        /// 读取自定义数据列
        /// </summary>
        internal override Delegate CustomColumnModelRemoteProxyReaderDelegate { get { return (AutoCSer.ORM.RemoteProxy.CustomColumnReader<T>.ReaderDelegate)AutoCSer.ORM.RemoteProxy.CustomColumnReader<T>.Read; } }
    }
}
