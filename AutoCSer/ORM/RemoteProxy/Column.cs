using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据列描述
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Column
    {
        /// <summary>
        /// 数据列名称
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal string Name;
        /// <summary>
        /// 数据类型
        /// </summary>
        internal DataType DataType;

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        internal void Set(DbDataReader reader, int index)
        {
            Name = reader.GetName(index);
            dataTypes.TryGetValue(reader.GetFieldType(index), out DataType);
        }

        /// <summary>
        /// 数据类型转换集合
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, DataType> dataTypes;
        static Column()
        {
            dataTypes = DictionaryCreator.CreateHashObject<System.Type, DataType>(11);
            dataTypes.Add(typeof(string), DataType.String);
            dataTypes.Add(typeof(int), DataType.Int);
            dataTypes.Add(typeof(bool), DataType.Bool);
            dataTypes.Add(typeof(DateTime), DataType.DateTime);
            dataTypes.Add(typeof(TimeSpan), DataType.TimeSpan);
            dataTypes.Add(typeof(long), DataType.Long);
            dataTypes.Add(typeof(decimal), DataType.Decimal);
            dataTypes.Add(typeof(Guid), DataType.Guid);
            dataTypes.Add(typeof(byte), DataType.Byte);
            dataTypes.Add(typeof(short), DataType.Short);
            dataTypes.Add(typeof(float), DataType.Float);
            dataTypes.Add(typeof(double), DataType.Double);
        }
    }
}
