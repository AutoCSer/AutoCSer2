using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 数据表格模型集合委托
    /// </summary>
    internal sealed class TableModel<T> where T : class
    {
        /// <summary>
        /// 读取数据表格模型对象委托
        /// </summary>
        public Action<DbDataReader, T, MemberMap<T>> Read;
        /// <summary>
        /// 写入表格模型数据委托
        /// </summary>
        public Action<CharStream, T, TableWriter> Insert;
        /// <summary>
        /// 写入更新数据委托
        /// </summary>
        public Action<CharStream, T, TableWriter, MemberMap<T>> Update;
        /// <summary>
        /// 写入条件数据委托
        /// </summary>
        public Action<CharStream, T, TableWriter, MemberMap<T>> ConcatCondition;
        /// <summary>
        /// 复制数据委托
        /// </summary>
        public Action<T, T, MemberMap<T>> Copy;
        /// <summary>
        /// 数据列值转数组
        /// </summary>
        public Action<T, object[]> ToArray;
        /// <summary>
        /// 数据库表格模型成员位图
        /// </summary>
        private MemberMapData<T> memberMap;

        /// <summary>
        /// 读取数据表格模型对象委托访问锁
        /// </summary>
        private static readonly object modelLock = new object();
        /// <summary>
        /// 读取数据表格模型对象委托
        /// </summary>
        private static TableModel<T> model;
        /// <summary>
        /// 读取数据表格模型对象委托集合
        /// </summary>
        private static LeftArray<TableModel<T>> models = new LeftArray<TableModel<T>>(0);
        /// <summary>
        /// 获取获取数据表格模型集合委托
        /// </summary>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        private static TableModel<T> get(ref MemberMapData<T> memberMap)
        {
            if (model != null)
            {
                if (model.memberMap.Equals(memberMap)) return model;
                foreach (TableModel<T> model in models)
                {
                    if (model.memberMap.Equals(memberMap)) return model;
                }
            }
            return null;
        }
        /// <summary>
        /// 写入表格模型数据验证接口调用
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="member"></param>
        private static void insertVerifyInterface(ILGenerator generator, Member member)
        {
            MethodInfo verifyInterfaceMethod = member.VerifyInterfaceMethod;
            generator.Emit(OpCodes.Ldarg_1);
            if (member.MemberIndex.IsField)
            {
                if (verifyInterfaceMethod == null || !member.MemberIndex.MemberSystemType.IsValueType) generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                else generator.Emit(OpCodes.Ldflda, (FieldInfo)member.MemberIndex.Member);
            }
            else
            {
                generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true));
                if (verifyInterfaceMethod != null && member.MemberIndex.MemberSystemType.IsValueType)
                {
                    LocalBuilder verifyLocal = generator.DeclareLocal(member.MemberIndex.MemberSystemType);
                    generator.Emit(OpCodes.Stloc, verifyLocal);
                    generator.Emit(OpCodes.Ldloca, verifyLocal);
                }
            }
            if (verifyInterfaceMethod != null) generator.call(verifyInterfaceMethod);
        }
        /// <summary>
        /// 写入列数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="member"></param>
        /// <param name="connectionCreatorLocal"></param>
        /// <param name="columnIndexLocal"></param>
        private static void write(ILGenerator generator, Member member, LocalBuilder connectionCreatorLocal, LocalBuilder columnIndexLocal)
        {
            #region ConnectionCreator.Convert(connectionCreator, charStream, value.NullField);
            bool isObjectToString;
            KeyValue<MethodInfo, bool> method = member.GetConstantConvertMethod(out isObjectToString);
            if (method.Value) generator.Emit(OpCodes.Ldloc_S, connectionCreatorLocal);
            generator.Emit(OpCodes.Ldarg_0);
            MethodInfo verifyMethod = member.VerifyMethod;
            if (verifyMethod != null)
            {
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
            }
            insertVerifyInterface(generator, member);
            if (member.ReaderDataType == ReaderDataTypeEnum.Json) generator.call(member.GenericType.JsonSerializeDelegate.Method);
            else if (isObjectToString && member.MemberIndex.MemberSystemType.IsValueType) generator.Emit(OpCodes.Box, member.MemberIndex.MemberSystemType);
            if (verifyMethod != null) generator.call(verifyMethod);
            generator.call(method.Key);
            #endregion
        }
        /// <summary>
        /// 获取数据表格模型集合委托
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        internal static TableModel<T> Get(TableWriter<T> writer)
        {
            Monitor.Enter(modelLock);
            try
            {
                TableModel<T> tableModel = get(ref writer.MemberMap.MemberMapData);
                if (tableModel != null) return tableModel;
                Type type = typeof(T);
                AutoCSer.Metadata.GenericType genericType = new AutoCSer.Metadata.GenericType<T>();
                string hashCode = writer.MemberMap.MemberMapData.GetHashCode64().toHex();

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelRead." + hashCode, null, new Type[] { typeof(DbDataReader), type, genericType.GetMemberMapType }, type, true);
                ILGenerator generator = dynamicMethod.GetILGenerator();
                #region int index = 0;
                LocalBuilder indexLocalBuilder = generator.DeclareLocal(typeof(int));
                generator.Emit(OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Stloc_0);
                #endregion
                foreach (Member member in writer.Members)
                {
                    Label notMemberLabel = generator.DefineLabel();
                    #region if (memberMap.IsMember(MemberIndex))
                    generator.memberMapObjectIsMember(OpCodes.Ldarg_2, member.MemberIndex.MemberIndex, genericType);
                    generator.Emit(OpCodes.Brfalse_S, notMemberLabel);
                    #endregion
                    if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                    {
                        #region value.Field = AutoCSer.ORM.Member.Read(reader, index++);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldloc_S, indexLocalBuilder);
                        generator.Emit(OpCodes.Dup);
                        generator.Emit(OpCodes.Ldc_I4_1);
                        generator.Emit(OpCodes.Add);
                        generator.Emit(OpCodes.Stloc_S, indexLocalBuilder);
                        generator.call(member.GetReadMethod());
                        if (member.MemberIndex.IsField) generator.Emit(OpCodes.Stfld, (FieldInfo)member.MemberIndex.Member);
                        else generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true));
                        #endregion
                    }
                    else if (member.MemberIndex.IsField)
                    {
                        #region AutoCSer.ORM.CustomColumn.TableReader<PropertyType>.Read(reader, ref value.CustomField, ref index);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Ldflda, (FieldInfo)member.MemberIndex.Member);
                        generator.Emit(OpCodes.Ldloca_S, indexLocalBuilder);
                        generator.call(member.StructGenericType.CustomColumnTableReadDelegate.Method);
                        #endregion
                    }
                    else
                    {
                        #region PropertyType property = default(PropertyType);
                        LocalBuilder propertyLocalBuilder = generator.DeclareLocal(member.MemberIndex.MemberSystemType);
                        generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                        generator.Emit(OpCodes.Initobj, member.MemberIndex.MemberSystemType);
                        #endregion
                        #region AutoCSer.ORM.CustomColumn.TableReader<PropertyType>.Read(reader, ref property, ref index);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldloca, propertyLocalBuilder);
                        generator.Emit(OpCodes.Ldloca_S, indexLocalBuilder);
                        generator.call(member.StructGenericType.CustomColumnTableReadDelegate.Method);
                        #endregion
                        #region value.CustomProperty = property;
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Ldloc, propertyLocalBuilder);
                        generator.call(((PropertyInfo)member.MemberIndex.Member).GetSetMethod(true));
                        #endregion
                    }
                    generator.MarkLabel(notMemberLabel);
                }
                generator.Emit(OpCodes.Ret);
                Action<DbDataReader, T, MemberMap<T>> read = (Action<DbDataReader, T, MemberMap<T>>)dynamicMethod.CreateDelegate(typeof(Action<DbDataReader, T, MemberMap<T>>));

                dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelInsert." + hashCode, null, new Type[] { typeof(CharStream), type, typeof(TableWriter) }, type, true);
                generator = dynamicMethod.GetILGenerator();
                #region ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);
                LocalBuilder connectionCreatorLocal = generator.DeclareLocal(typeof(ConnectionCreator));
                generator.Emit(OpCodes.Ldarg_2);
                generator.call(((Func<TableWriter, ConnectionCreator>)TableWriter.GetConnectionCreator).Method);
                generator.Emit(OpCodes.Stloc_S, connectionCreatorLocal);
                #endregion
                #region int columnIndex = 0;
                LocalBuilder columnIndexLocal = generator.DeclareLocal(typeof(int));
                generator.Emit(OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                #endregion
                bool isFirst = true;
                foreach (Member member in writer.Members)
                {
                    if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                    {
                        #region ConnectionCreator.WriteConcat(charStream);
                        if (!isFirst)
                        {
                            generator.Emit(OpCodes.Ldarg_0);
                            generator.call(((Action<CharStream>)ConnectionCreator.WriteConcat).Method);
                        }
                        #endregion
                        write(generator, member, connectionCreatorLocal, columnIndexLocal);
                        #region ++columnIndex;
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.Emit(OpCodes.Ldc_I4_1);
                        generator.Emit(OpCodes.Add);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                    }
                    else
                    {
                        #region columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataField, tableWriter, columnIndex);
                        generator.Emit(OpCodes.Ldarg_0);
                        insertVerifyInterface(generator, member);
                        generator.Emit(OpCodes.Ldarg_2);
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.call(member.StructGenericType.CustomColumnTableInsertDelegate.Method);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                    }
                    isFirst = false;
                }
                generator.Emit(OpCodes.Ret);
                Action<CharStream, T, TableWriter> insert = (Action<CharStream, T, TableWriter>)dynamicMethod.CreateDelegate(typeof(Action<CharStream, T, TableWriter>));
                
                dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelUpdate." + hashCode, null, new Type[] { typeof(CharStream), type, typeof(TableWriter), genericType.GetMemberMapType }, type, true);
                generator = dynamicMethod.GetILGenerator();
                #region int columnIndex = 0;
                columnIndexLocal = generator.DeclareLocal(typeof(int));
                generator.Emit(OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                #endregion
                #region bool isFirst = true;
                LocalBuilder isFirstLocal = generator.DeclareLocal(typeof(bool));
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Stloc_S, isFirstLocal);
                #endregion
                #region ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);
                connectionCreatorLocal = generator.DeclareLocal(typeof(ConnectionCreator));
                generator.Emit(OpCodes.Ldarg_2);
                generator.call(((Func<TableWriter, ConnectionCreator>)TableWriter.GetConnectionCreator).Method);
                generator.Emit(OpCodes.Stloc_S, connectionCreatorLocal);
                #endregion
                foreach (Member member in writer.Members)
                {
                    Label notMemberLabel = generator.DefineLabel();
                    #region if (memberMap.IsMember(MemberIndex))
                    generator.memberMapObjectIsMember(OpCodes.Ldarg_3, member.MemberIndex.MemberIndex, genericType);
                    generator.Emit(OpCodes.Brfalse, notMemberLabel);
                    #endregion
                    if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                    {
                        #region ConnectionCreator.WriteConcat(charStream, tableWriter, columnIndex, isFirst);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldarg_2);
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.Emit(OpCodes.Ldloc_S, isFirstLocal);
                        generator.call(((Action<CharStream, TableWriter, int, bool>)ConnectionCreator.WriteConcat).Method);
                        #endregion
                        write(generator, member, connectionCreatorLocal, columnIndexLocal);
                        #region isFirst = false;
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Stloc_S, isFirstLocal);
                        #endregion
                        generator.MarkLabel(notMemberLabel);
                        #region ++columnIndex;
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.Emit(OpCodes.Ldc_I4_1);
                        generator.Emit(OpCodes.Add);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                    }
                    else
                    {
                        #region columnIndex = AutoCSer.ORM.CustomColumn.TableModel<AutoCSer.ORM.CustomColumn.Date>.Update(charStream, value.CustomField, tableWriter, columnIndex, isFirst);
                        Label nextLabel = generator.DefineLabel();
                        generator.Emit(OpCodes.Ldarg_0);
                        insertVerifyInterface(generator, member);
                        generator.Emit(OpCodes.Ldarg_2);
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.Emit(OpCodes.Ldloc_S, isFirstLocal);
                        generator.call(member.StructGenericType.CustomColumnTableUpdateDelegate.Method);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                        #region isFirst = false;
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Stloc_S, isFirstLocal);
                        #endregion
                        generator.Emit(OpCodes.Br_S, nextLabel);
                        generator.MarkLabel(notMemberLabel);
                        #region columnIndex += member.CustomColumnNames.Length;
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.int32(member.CustomColumnNames.Length);
                        generator.Emit(OpCodes.Add);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                        generator.MarkLabel(nextLabel);
                    }
                }
                generator.Emit(OpCodes.Ret);
                Action<CharStream, T, TableWriter, MemberMap<T>> update = (Action<CharStream, T, TableWriter, MemberMap<T>>)dynamicMethod.CreateDelegate(typeof(Action<CharStream, T, TableWriter, MemberMap<T>>));

                dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelConcatCondition." + hashCode, null, new Type[] { typeof(CharStream), type, typeof(TableWriter), genericType.GetMemberMapType }, type, true);
                generator = dynamicMethod.GetILGenerator();
                #region int columnIndex = 0;
                columnIndexLocal = generator.DeclareLocal(typeof(int));
                generator.Emit(OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                #endregion
                #region bool isFirst = true;
                isFirstLocal = generator.DeclareLocal(typeof(bool));
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Stloc_S, isFirstLocal);
                #endregion
                #region ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);
                connectionCreatorLocal = generator.DeclareLocal(typeof(ConnectionCreator));
                generator.Emit(OpCodes.Ldarg_2);
                generator.call(((Func<TableWriter, ConnectionCreator>)TableWriter.GetConnectionCreator).Method);
                generator.Emit(OpCodes.Stloc_S, connectionCreatorLocal);
                #endregion
                bool isObjectToString;
                foreach (Member member in writer.Members)
                {
                    Label notMemberLabel = generator.DefineLabel();
                    #region if (memberMap.IsMember(MemberIndex))
                    generator.memberMapObjectIsMember(OpCodes.Ldarg_3, member.MemberIndex.MemberIndex, genericType);
                    generator.Emit(OpCodes.Brfalse, notMemberLabel);
                    #endregion
                    if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                    {
                        #region ConnectionCreator.WriteConcatCondition(charStream, tableWriter, columnIndex, isFirst);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldarg_2);
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.Emit(OpCodes.Ldloc_S, isFirstLocal);
                        generator.call(((Action<CharStream, TableWriter, int, bool>)ConnectionCreator.WriteConcatCondition).Method);
                        #endregion
                        #region ConnectionCreator.Convert(connectionCreator, charStream, value.NullField);
                        KeyValue<MethodInfo, bool> method = member.GetConstantConvertMethod(out isObjectToString);
                        if (method.Value) generator.Emit(OpCodes.Ldloc_S, connectionCreatorLocal);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldarg_1);
                        if (member.MemberIndex.IsField) generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                        else generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true));
                        if (member.ReaderDataType == ReaderDataTypeEnum.Json) generator.call(member.GenericType.JsonSerializeDelegate.Method);
                        else if (isObjectToString && member.MemberIndex.MemberSystemType.IsValueType) generator.Emit(OpCodes.Box, member.MemberIndex.MemberSystemType);
                        generator.call(method.Key);
                        #endregion
                        #region isFirst = false;
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Stloc_S, isFirstLocal);
                        #endregion
                        generator.MarkLabel(notMemberLabel);
                        #region ++columnIndex;
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.Emit(OpCodes.Ldc_I4_1);
                        generator.Emit(OpCodes.Add);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                    }
                    else
                    {
                        #region columnIndex = AutoCSer.ORM.CustomColumn.TableModel<AutoCSer.ORM.CustomColumn.Date>.ConcatCondition(charStream, value.CustomField, tableWriter, columnIndex, isFirst);
                        Label nextLabel = generator.DefineLabel();
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldarg_1);
                        if (member.MemberIndex.IsField) generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                        else generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true));
                        generator.Emit(OpCodes.Ldarg_2);
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.Emit(OpCodes.Ldloc_S, isFirstLocal);
                        generator.call(member.StructGenericType.CustomColumnTableConcatConditionDelegate.Method);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                        #region isFirst = false;
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Stloc_S, isFirstLocal);
                        #endregion
                        generator.Emit(OpCodes.Br_S, nextLabel);
                        generator.MarkLabel(notMemberLabel);
                        #region columnIndex += member.CustomColumnNames.Length;
                        generator.Emit(OpCodes.Ldloc_S, columnIndexLocal);
                        generator.int32(member.CustomColumnNames.Length);
                        generator.Emit(OpCodes.Add);
                        generator.Emit(OpCodes.Stloc_S, columnIndexLocal);
                        #endregion
                        generator.MarkLabel(nextLabel);
                    }
                }
                generator.Emit(OpCodes.Ret);
                Action<CharStream, T, TableWriter, MemberMap<T>> concatCondition = (Action<CharStream, T, TableWriter, MemberMap<T>>)dynamicMethod.CreateDelegate(typeof(Action<CharStream, T, TableWriter, MemberMap<T>>));

                tableModel = new TableModel<T> { Read = read, Insert = insert, Update = update, ConcatCondition = concatCondition, memberMap = writer.MemberMap.MemberMapData };
                if (model == null) model = tableModel;
                else models.Add(tableModel);
                return tableModel;
            }
            finally { Monitor.Exit(modelLock); }
        }
        /// <summary>
        /// 获取复制数据委托
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        internal static Action<T, T, MemberMap<T>> GetCopy(TableWriter<T> writer)
        {
            Monitor.Enter(modelLock);
            try
            {
                TableModel<T> tableModel = get(ref writer.MemberMap.MemberMapData);
                if (tableModel.Copy != null) return tableModel.Copy;
                Type type = typeof(T);
                AutoCSer.Metadata.GenericType genericType = new AutoCSer.Metadata.GenericType<T>();
                string hashCode = writer.MemberMap.MemberMapData.GetHashCode64().toHex();

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelCopy." + hashCode, null, new Type[] { type, type, genericType.GetMemberMapType }, type, true);
                ILGenerator generator = dynamicMethod.GetILGenerator();
                foreach (Member member in writer.Members)
                {
                    Label notMemberLabel = generator.DefineLabel();
                    #region if (memberMap.IsMember(MemberIndex))
                    generator.memberMapObjectIsMember(OpCodes.Ldarg_2, member.MemberIndex.MemberIndex, genericType);
                    generator.Emit(OpCodes.Brfalse_S, notMemberLabel);
                    #endregion
                    #region destination.Field = source.Field;
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    if (member.MemberIndex.IsField)
                    {
                        FieldInfo field = (FieldInfo)member.MemberIndex.Member;
                        generator.Emit(OpCodes.Ldfld, field);
                        generator.Emit(OpCodes.Stfld, field);
                    }
                    else
                    {
                        PropertyInfo property = (PropertyInfo)member.MemberIndex.Member;
                        generator.call(property.GetGetMethod(true));
                        generator.call(property.GetSetMethod(true));
                    }
                    #endregion
                    generator.MarkLabel(notMemberLabel);
                }
                generator.Emit(OpCodes.Ret);
                tableModel.Copy = (Action<T, T, MemberMap<T>>)dynamicMethod.CreateDelegate(typeof(Action<T, T, MemberMap<T>>));
                return tableModel.Copy;
            }
            finally { Monitor.Exit(modelLock); }
        }
        /// <summary>
        /// 获取数据列值转数组委托
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        internal static Action<T, object[]> GetToArray(TableWriter<T> writer)
        {
            Monitor.Enter(modelLock);
            try
            {
                TableModel<T> tableModel = get(ref writer.MemberMap.MemberMapData);
                if (tableModel.Copy != null) return tableModel.ToArray;
                Type type = typeof(T);
                string hashCode = writer.MemberMap.MemberMapData.GetHashCode64().toHex();

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "SQLModelToArray." + hashCode, null, new Type[] { type, typeof(object[]) }, type, true);
                ILGenerator generator = dynamicMethod.GetILGenerator();
                #region int index = 0;
                LocalBuilder indexLocalBuilder = generator.DeclareLocal(typeof(int));
                generator.Emit(OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Stloc_0);
                #endregion
                foreach (Member member in writer.Members)
                {
                    if (member.ReaderDataType != ReaderDataTypeEnum.CustomColumn)
                    {
                        #region array[index++] = value.NullField;
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Ldloc_S, indexLocalBuilder);
                        generator.Emit(OpCodes.Dup);
                        generator.Emit(OpCodes.Ldc_I4_1);
                        generator.Emit(OpCodes.Add);
                        generator.Emit(OpCodes.Stloc_0);
                        generator.Emit(OpCodes.Ldarg_0);
                        if (member.MemberIndex.IsField) generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                        else generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true));
                        if (member.ReaderDataType == ReaderDataTypeEnum.Json) generator.call(member.GenericType.JsonSerializeDelegate.Method);
                        else if (member.MemberIndex.MemberSystemType.IsValueType) generator.Emit(OpCodes.Box, member.MemberIndex.MemberSystemType);
                        generator.Emit(OpCodes.Stelem_Ref);
                        #endregion
                    }
                    else
                    {
                        #region AutoCSer.ORM.CustomColumn.ToArray<VerifyData>.Write(array, value.CustomVerifyDataField, ref index);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Ldarg_0);
                        if (member.MemberIndex.IsField) generator.Emit(OpCodes.Ldfld, (FieldInfo)member.MemberIndex.Member);
                        else generator.call(((PropertyInfo)member.MemberIndex.Member).GetGetMethod(true));
                        generator.Emit(OpCodes.Ldloca_S, indexLocalBuilder);
                        generator.call(member.StructGenericType.CustomColumnToArrayDelegate.Method);
                        #endregion
                    }
                }
                generator.Emit(OpCodes.Ret);
                tableModel.ToArray = (Action<T, object[]>)dynamicMethod.CreateDelegate(typeof(Action<T, object[]>));
                return tableModel.ToArray;
            }
            finally { Monitor.Exit(modelLock); }
        }
    }
#if DEBUG
#pragma warning disable
    internal class TableModelReaderIL
    {
        private decimal Field;
        private decimal Property { get; set; }

        private decimal? NullField;
        private decimal? NullProperty { get; set; }

        private VerifyData VerifyDataField;
        private VerifyData VerifyDataProperty { get; set; }

        private VerifyValue VerifyValueField;
        private VerifyValue VerifyValueProperty { get; set; }

        private VerifyData CustomVerifyDataField;
        private VerifyData CustomVerifyDataProperty { get; set; }

        private struct VerifyData
        {
            public VerifyData Verify() { return this; }
        }
        private sealed class VerifyValue
        {
            public VerifyValue Verify() { return this; }
        }
        public static void ToArray(TableModelReaderIL value, object[] array)
        {
            int index = 0;
            array[index++] = value.NullField;
            array[index++] = value.NullProperty;
            array[index++] = value.VerifyValueField;
            array[index++] = value.VerifyValueProperty;
            array[index++] = AutoCSer.ORM.Member.JsonSerialize(value.VerifyDataField);
            array[index++] = AutoCSer.ORM.Member.JsonSerialize(value.VerifyDataProperty);
            array[index++] = AutoCSer.ORM.Member.JsonSerialize(value.VerifyValueField);
            array[index++] = AutoCSer.ORM.Member.JsonSerialize(value.VerifyValueProperty);
            AutoCSer.ORM.CustomColumn.ToArray<VerifyData>.Write(value.CustomVerifyDataField, array, ref index);
            AutoCSer.ORM.CustomColumn.ToArray<VerifyData>.Write(value.CustomVerifyDataProperty, array, ref index);
        }
        public static void Copy(TableModelReaderIL source, TableModelReaderIL destination, MemberMap<TableModelReaderIL> memberMap)
        {
            destination.NullField = source.NullField;
            destination.NullProperty = source.NullProperty;
            destination.VerifyValueField = source.VerifyValueField;
            destination.VerifyValueProperty = source.VerifyValueProperty;
        }
        public static void ConcatCondition(CharStream charStream, TableModelReaderIL value, TableWriter tableWriter, MemberMap<TableModelReaderIL> memberMap)
        {
            int columnIndex = 0;
            bool isFirst = true;
            ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);

            if (MemberMap<TableModelReaderIL>.IsMember(memberMap, 0))
            {
                ConnectionCreator.WriteConcatCondition(charStream, tableWriter, columnIndex, isFirst);
                ConnectionCreator.Convert(charStream, value.Field);
                isFirst = false;
            }
            ++columnIndex;

            ConnectionCreator.Convert(charStream, value.Property);

            ConnectionCreator.Convert(charStream, value.NullField);
            ConnectionCreator.Convert(charStream, value.NullProperty);

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty));

            if (MemberMap<TableModelReaderIL>.IsMember(memberMap, 0))
            {
                columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.ConcatCondition(charStream, value.CustomVerifyDataField, tableWriter, columnIndex, isFirst);
                isFirst = false;
            }
            else columnIndex += 1;

            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.ConcatCondition(charStream, value.CustomVerifyDataProperty, tableWriter, columnIndex, isFirst);
        }
        public static void Update(CharStream charStream, TableModelReaderIL value, TableWriter tableWriter, MemberMap<TableModelReaderIL> memberMap)
        {
            int columnIndex = 0;
            bool isFirst = true;
            ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);

            if (MemberMap<TableModelReaderIL>.IsMember(memberMap, 0))
            {
                ConnectionCreator.WriteConcat(charStream, tableWriter, columnIndex, isFirst);
                ConnectionCreator.Convert(charStream, value.Field);
                isFirst = false;
            }
            ++columnIndex;

            ConnectionCreator.Convert(charStream, value.Property);
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Field));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Property));

            ConnectionCreator.Convert(charStream, value.NullField);
            ConnectionCreator.Convert(charStream, value.NullProperty);
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullField));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullProperty));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty.Verify()));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty.Verify()));

            if (MemberMap<TableModelReaderIL>.IsMember(memberMap, 0))
            {
                columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataField, tableWriter, columnIndex, isFirst);
                isFirst = false;
            }
            else columnIndex += 1;

            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataProperty, tableWriter, columnIndex, isFirst);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataField.Verify(), tableWriter, columnIndex, isFirst);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Update(charStream, value.CustomVerifyDataProperty.Verify(), tableWriter, columnIndex, isFirst);
        }
        public static void Insert(CharStream charStream, TableModelReaderIL value, TableWriter tableWriter)
        {
            int columnIndex = 0;
            ConnectionCreator connectionCreator = TableWriter.GetConnectionCreator(tableWriter);

            ConnectionCreator.Convert(charStream, value.Field);
            ConnectionCreator.Convert(charStream, value.Property);
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Field));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.Property));
            ++columnIndex;

            ConnectionCreator.Convert(charStream, value.NullField);
            ConnectionCreator.Convert(charStream, value.NullProperty);
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullField));
            ConnectionCreator.Convert(charStream, Member.Verify(tableWriter, columnIndex, value.NullProperty));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyDataProperty.Verify()));

            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueField.Verify()));
            ConnectionCreator.Convert(connectionCreator, charStream, Member.JsonSerialize(value.VerifyValueProperty.Verify()));

            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataField, tableWriter, columnIndex);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataProperty, tableWriter, columnIndex);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataField.Verify(), tableWriter, columnIndex);
            columnIndex = AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Insert(charStream, value.CustomVerifyDataProperty.Verify(), tableWriter, columnIndex);
        }
        public static void Reader(DbDataReader reader, TableModelReaderIL value, MemberMap<TableModelReaderIL> memberMap)
        {
            int index = 0;

            //value.Field = read(reader, index++);

            //value.Property = read(reader, index++);

            //value.NullableField = readNullable(reader, index++);

            //value.NullableProperty = readNullable(reader, index++);

            //value.NullField = AutoCSer.ORM.Member.ReadString(reader, index++);

            //value.NullProperty = AutoCSer.ORM.Member.ReadString(reader, index++);

            //value.JsonStringField = AutoCSer.ORM.Member.ReadJson<TableModelReaderIL>(reader, index++);

            //value.JsonStringProperty = AutoCSer.ORM.Member.ReadJson<TableModelReaderIL>(reader, index++);

            AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Read(reader, ref value.CustomVerifyDataField, ref index);

            VerifyData property = default(VerifyData);
            AutoCSer.ORM.CustomColumn.TableModel<VerifyData>.Read(reader, ref property, ref index);
            value.CustomVerifyDataProperty = property;
        }
    }
#endif
}
