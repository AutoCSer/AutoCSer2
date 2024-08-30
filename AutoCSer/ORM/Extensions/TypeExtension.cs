using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM.Extensions
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// C#类型转SQL数据类型集合
        /// </summary>
        private static readonly Dictionary<HashObject<System.Type>, SqlDbType> CSharpToDbTypes;
        /// <summary>
        /// 根据C#类型获取SQL数据类型
        /// </summary>
        /// <param name="type">C#类型</param>
        /// <returns>SQL数据类型</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SqlDbType getDbType(this Type type)
        {
            SqlDbType value;
            return CSharpToDbTypes.TryGetValue(type.getNullableType() ?? type, out value) ? value : SqlDbType.NVarChar;
        }
        static TypeExtension()
        {
            #region C#类型转SQL数据类型集合
            CSharpToDbTypes = DictionaryCreator.CreateHashObject<System.Type, SqlDbType>();
            CSharpToDbTypes.Add(typeof(bool), SqlDbType.Bit);
            CSharpToDbTypes.Add(typeof(byte), SqlDbType.TinyInt);
            CSharpToDbTypes.Add(typeof(sbyte), SqlDbType.TinyInt);
            CSharpToDbTypes.Add(typeof(short), SqlDbType.SmallInt);
            CSharpToDbTypes.Add(typeof(ushort), SqlDbType.SmallInt);
            CSharpToDbTypes.Add(typeof(int), SqlDbType.Int);
            CSharpToDbTypes.Add(typeof(uint), SqlDbType.Int);
            CSharpToDbTypes.Add(typeof(long), SqlDbType.BigInt);
            CSharpToDbTypes.Add(typeof(ulong), SqlDbType.BigInt);
            CSharpToDbTypes.Add(typeof(decimal), SqlDbType.Decimal);
            CSharpToDbTypes.Add(typeof(float), SqlDbType.Real);
            CSharpToDbTypes.Add(typeof(double), SqlDbType.Float);
            CSharpToDbTypes.Add(typeof(string), SqlDbType.NVarChar);
            CSharpToDbTypes.Add(typeof(DateTime), SqlDbType.DateTime);
            CSharpToDbTypes.Add(typeof(TimeSpan), SqlDbType.Time);
            CSharpToDbTypes.Add(typeof(Guid), SqlDbType.UniqueIdentifier);
            //CSharpToDbTypes.Add(typeof(byte[]), SqlDbType.VarBinary);
            #endregion
        }
    }
}
