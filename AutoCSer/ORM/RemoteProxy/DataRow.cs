using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据行
    /// </summary>
    [AutoCSer.BinarySerialize(CustomReferenceTypes = new Type[0])]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DataRow : AutoCSer.BinarySerialize.ICustomSerialize<DataRow>
    {
        /// <summary>
        /// 数据列描述集合
        /// </summary>
        internal Column[] Columns;
        /// <summary>
        /// 行数据
        /// </summary>
        internal DataValue[] Row;

        /// <summary>
        /// 数据行
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columns"></param>
        internal DataRow(DbDataReader reader, Column[] columns)
        {
            if (columns == null)
            {
                Columns = new Column[reader.FieldCount];
                for (int index = Columns.Length; index != 0; Columns[index].Set(reader, index)) --index;
                columns = Columns;
            }
            else Columns = null;
            Row = new DataValue[columns.Length];
            for (int index = 0; index != Row.Length; ++index) Row[index].Set(reader, index, columns[index].DataType);
        }

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="serializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<DataRow>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            if (Row != null)
            {
                int dataLength = Row.Length;
                UnmanagedStream stream = serializer.Stream;
                stream.Write(Columns == null ? dataLength : -dataLength);
                if (Columns != null)
                {
                    foreach (Column column in Columns)
                    {
                        fixed (char* stringFixed = column.Name) stream.Serialize(stringFixed, column.Name.Length);
                        if (stream.IsResizeError) return;
                    }
                }
                byte* type = stream.GetBeforeMove((dataLength + 3) & 0x7ffffffc);
                if (type != null)
                {
                    foreach (DataValue value in Row)
                    {
                        *type++ = (byte)value.Serialize(stream);
                        if (stream.IsResizeError) return;
                    }
                }
            }
            else serializer.Stream.Write(0);
        }
        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<DataRow>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            int dataLength;
            if (!deserializer.Read(out dataLength)) return;
            if (dataLength != 0)
            {
                if (dataLength < 0)
                {
                    Columns = new Column[dataLength = -dataLength];
                    for (int index = 0; index != dataLength; ++index) deserializer.Deserialize(ref Columns[index].Name);
                }
                int typeSize = (dataLength + 3) & 0x7ffffffc;
                byte* type = deserializer.GetBeforeMove(typeSize);
                if (type != null)
                {
                    Row = new DataValue[dataLength];
                    for (int index = 0; index != dataLength; ++index)
                    {
                        if (!Row[index].Deserialize(*type++, deserializer)) return;
                    }
                }
            }
        }

        /// <summary>
        /// 客户端获取远程代理访问数据库结果 查询第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SingleOrDefault<T>() where T : class
        {
            if (Columns == null) return null;
            T value = DefaultConstructor<T>.Constructor();
            int[] columnIndexs = ModelReader<T>.GetColumnIndexCache(Columns);
            try
            {
                if (columnIndexs.Length != 0) ModelReader<T>.Reader(Row, value, columnIndexs);
            }
            finally { ModelReader<T>.FreeColumnIndexCache(columnIndexs); }
            return value;
        }

        /// <summary>
        /// 客户端获取远程代理访问数据库结果 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeratorCommand">RPC 远程代理访问命令</param>
        /// <param name="capacity">数组容器初始化大小</param>
        /// <returns></returns>
        public static async Task<AutoCSer.Net.CommandClientReturnValue<LeftArray<T>>> Query<T>(AutoCSer.Net.EnumeratorCommand<DataRow> enumeratorCommand, int capacity = 0) where T : class
        {
            int[] columnIndexs = null;
            LeftArray<T> array = new LeftArray<T>(0);
            try
            {
                while (await enumeratorCommand.MoveNext())
                {
                    DataRow dataRow = enumeratorCommand.Current;
                    T value = DefaultConstructor<T>.Constructor();
                    if (columnIndexs == null) columnIndexs = ModelReader<T>.GetColumnIndexCache(dataRow.Columns);
                    if (columnIndexs.Length != 0) ModelReader<T>.Reader(dataRow.Row, value, columnIndexs);
                    if (array.Array.Length == 0 && capacity != 0) array = new LeftArray<T>(capacity);
                    array.Add(value);
                }
            }
            finally
            {
                if (columnIndexs != null) ModelReader<T>.FreeColumnIndexCache(columnIndexs);
            }
            if(enumeratorCommand.ReturnType == AutoCSer.Net.CommandClientReturnTypeEnum.Success) return array;
            return new AutoCSer.Net.CommandClientReturnValue<LeftArray<T>> { ReturnType = enumeratorCommand.ReturnType };
        }
#if DotNet45 || NetStandard2
        /// <summary>
        /// 客户端获取远程代理访问数据库结果 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeratorCommand">RPC 远程代理访问命令</param>
        /// <returns></returns>
        public static Task<ModelSelectEnumerator<T>> Select<T>(AutoCSer.Net.EnumeratorCommand<DataRow> enumeratorCommand) where T : class
        {
            return Task<ModelSelectEnumerator<T>>.FromResult(new ModelSelectEnumerator<T>(enumeratorCommand));
        }
#else
        /// <summary>
        /// 客户端获取远程代理访问数据库结果 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeratorCommand">RPC 远程代理访问命令</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<T> Select<T>(AutoCSer.Net.EnumeratorCommand<DataRow> enumeratorCommand) where T : class
        {
            int[] columnIndexs = null;
            try
            {
                while (await enumeratorCommand.MoveNext())
                {
                    DataRow dataRow = enumeratorCommand.Current;
                    T value = DefaultConstructor<T>.Constructor();
                    if (columnIndexs == null) columnIndexs = ModelReader<T>.GetColumnIndexCache(dataRow.Columns);
                    if (columnIndexs.Length != 0) ModelReader<T>.Reader(dataRow.Row, value, columnIndexs);
                    yield return value;
                }
            }
            finally
            {
                if (columnIndexs != null) ModelReader<T>.FreeColumnIndexCache(columnIndexs);
            }
        }
#endif
    }
}
