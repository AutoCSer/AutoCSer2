using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Data.Common;

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct DataValue
    {
        /// <summary>
        /// 整数数据
        /// </summary>
        internal Bit128 Bit128;
        /// <summary>
        /// 字符串
        /// </summary>
        internal string String;
        /// <summary>
        /// 数据类型
        /// </summary>
        internal DataType DataType;
        /// <summary>
        /// 是否空值
        /// </summary>
        private bool isNull;

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <param name="dataType"></param>
        internal void Set(DbDataReader reader, int index, DataType dataType)
        {
            DataType = dataType;
            if (reader.IsDBNull(index)) isNull = true;
            else
            {
                switch (DataType = dataType)
                {
                    case DataType.String:
                        String = reader.GetString(index);
                        if (String == null) isNull = true;
                        return;
                    case DataType.Int: Bit128.Int = reader.GetInt32(index); return;
                    case DataType.Bool: Bit128.Bool = reader.GetBoolean(index); return;
                    case DataType.DateTime: Bit128.DateTime = reader.GetDateTime(index); return;
                    case DataType.TimeSpan: Bit128.TimeSpan = (TimeSpan)reader.GetValue(index); return;
                    case DataType.Long: Bit128.Long = reader.GetInt64(index); return;
                    case DataType.Decimal: Bit128.Decimal = reader.GetDecimal(index); return;
                    case DataType.Guid: Bit128.Guid = reader.GetGuid(index); return;
                    case DataType.Byte: Bit128.Byte = reader.GetByte(index); return;
                    case DataType.Short: Bit128.Short = reader.GetInt16(index); return;
                    case DataType.Float: Bit128.Float = reader.GetFloat(index); return;
                    case DataType.Double: Bit128.Double = reader.GetDouble(index); return;
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal unsafe uint Serialize(UnmanagedStream stream)
        {
            if (!isNull)
            {
                switch (DataType)
                {
                    case DataType.String:
                        switch (String.Length)
                        {
                            case 0: return 0x20U | (byte)DataType;
                            case 1:
                                uint code = (uint)String[0] - '0';
                                if (code < 0xd) return ((code + 3) << 4) | (byte)DataType;
                                break;
                        }
                        fixed (char* stringFixed = String) stream.Serialize(stringFixed, String.Length);
                        break;
                    case DataType.Int:
                        if ((uint)Bit128.Int < 0xe) return (((uint)Bit128.Int + 2) << 4) | (byte)DataType;
                        stream.Write(Bit128.Int);
                        break;
                    case DataType.Bool: return (Bit128.Bool ? 0x30U : 0x20U) | (byte)DataType;
                    case DataType.DateTime:
                        if (Bit128.DateTime == default(DateTime)) return 0x20U | (byte)DataType;
                        stream.Write(Bit128.DateTime);
                        break;
                    case DataType.TimeSpan:
                        if (Bit128.TimeSpan == default(TimeSpan)) return 0x20U | (byte)DataType;
                        stream.Write(Bit128.TimeSpan);
                        break;
                    case DataType.Long:
                        if ((ulong)Bit128.Long < 0xe) return (((uint)(ulong)Bit128.Long + 2) << 4) | (byte)DataType;
                        stream.Write(Bit128.Long);
                        break;
                    case DataType.Decimal:
                        uint value = (uint)(int)Bit128.Decimal;
                        if(value == Bit128.Decimal && value < 0xe) return ((value + 2) << 4) | (byte)DataType;
                        stream.Write(Bit128.Decimal);
                        break;
                    case DataType.Guid:
                        if (Bit128.Guid == default(Guid)) return 0x20U | (byte)DataType;
                        stream.Write(Bit128.Guid);
                        break;
                    case DataType.Byte:
                        if (Bit128.Byte < 0xe) return (((uint)Bit128.Byte + 2) << 4) | (byte)DataType;
                        stream.Write((int)Bit128.Byte);
                        break;
                    case DataType.Short:
                        if ((ushort)Bit128.Short < 0xe) return (((uint)(ushort)Bit128.Short + 2) << 4) | (byte)DataType;
                        stream.Write((int)Bit128.Short);
                        break;
                    case DataType.Float:
                        value = (uint)(int)Bit128.Float;
                        if (value == Bit128.Float && value < 0xe) return ((value + 2) << 4) | (byte)DataType;
                        stream.Write(Bit128.Float);
                        break;
                    case DataType.Double:
                        value = (uint)(int)Bit128.Double;
                        if (value == Bit128.Double && value < 0xe) return ((value + 2) << 4) | (byte)DataType;
                        stream.Write(Bit128.Double);
                        break;
                }
                return (byte)DataType;
            }
            return 0x10U | (byte)DataType;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal bool Deserialize(byte dataType, AutoCSer.BinaryDeserializer deserializer)
        {
            DataType = (DataType)(byte)(dataType & 0xf);
            switch (dataType >>= 4)
            {
                case 0:
                    switch (DataType)
                    {
                        case DataType.String: deserializer.Deserialize(ref String); break;
                        case DataType.Int: deserializer.Read(out Bit128.Int); break;
                        case DataType.Bool: goto ERROR;
                        case DataType.DateTime: deserializer.Read(out Bit128.DateTime); break;
                        case DataType.TimeSpan: deserializer.Read(out Bit128.TimeSpan); break;
                        case DataType.Long: deserializer.Read(out Bit128.Long); break;
                        case DataType.Decimal: deserializer.Read(out Bit128.Decimal); break;
                        case DataType.Guid: deserializer.Read(out Bit128.Guid); break;
                        case DataType.Byte:
                            int value;
                            deserializer.Read(out value);
                            if (value < byte.MinValue || value > byte.MaxValue) goto ERROR;
                            Bit128.Byte = (byte)value;
                            break;
                        case DataType.Short:
                            deserializer.Read(out value);
                            if (value < short.MinValue || value > short.MaxValue) goto ERROR;
                            Bit128.Short = (short)value;
                            break;
                        case DataType.Float: deserializer.Read(out Bit128.Float); break;
                        case DataType.Double: deserializer.Read(out Bit128.Double); break;
                    }
                    break;
                case 1: isNull = true; break;
                default:
                    switch (DataType)
                    {
                        case DataType.String: String = dataType == 2 ? string.Empty : ((char)(dataType + ('0' - 3))).ToString(); break;
                        case DataType.Int: Bit128.Int = dataType - 2; break;
                        case DataType.Bool:
                            switch (dataType - 2)
                            {
                                case 2 - 2: Bit128.Bool = false; break;
                                case 3 - 2: Bit128.Bool = true; break;
                                default: goto ERROR;
                            }
                            break;
                        case DataType.DateTime:
                        case DataType.TimeSpan:
                        case DataType.Guid:
                            if (dataType != 2) goto ERROR;
                            break;
                        case DataType.Long: Bit128.Long = dataType - 2; break;
                        case DataType.Decimal: Bit128.Decimal = dataType - 2; break;
                        case DataType.Byte: Bit128.Byte = (byte)(dataType - 2); break;
                        case DataType.Short: Bit128.Short = (short)(dataType - 2); break;
                        case DataType.Float: Bit128.Float = dataType - 2; break;
                        case DataType.Double: Bit128.Double = dataType - 2; break;
                    }
                    break;
            }
            if (DataType <= DataType.Double) return deserializer.State == BinarySerialize.DeserializeStateEnum.Success;
            ERROR:
            deserializer.SetCustomError(string.Empty);
            return false;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private int readInt()
        {
            switch (DataType)
            {
                case DataType.Long:
                    int value = (int)Bit128.Long;
                    if (value == Bit128.Decimal) return value;
                    throw new InvalidCastException($"{Bit128.Long} 转换 int 失败");
                case DataType.Int: return Bit128.Int;
                case DataType.Short: return Bit128.Short;
                case DataType.Byte: return Bit128.Byte;
                case DataType.Decimal:
                    value = (int)Bit128.Decimal;
                    if (value == Bit128.Decimal) return value;
                    throw new InvalidCastException($"{Bit128.Decimal} 转换 int 失败");
                case DataType.Double:
                    value = (int)Bit128.Double;
                    if (value == Bit128.Double) return value;
                    throw new InvalidCastException($"{Bit128.Double} 转换 int 失败");
                case DataType.Float:
                    value = (int)Bit128.Float;
                    if (value == Bit128.Float) return value;
                    throw new InvalidCastException($"{Bit128.Float} 转换 int 失败");
                case DataType.String:
                    if (int.TryParse(String, out value)) return value;
                    throw new InvalidCastException($"{String} 转换 int 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 int 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private long readLong()
        {
            switch (DataType)
            {
                case DataType.Long: return Bit128.Long;
                case DataType.Int: return Bit128.Int;
                case DataType.Short: return Bit128.Short;
                case DataType.Byte: return Bit128.Byte;
                case DataType.Decimal:
                    long value = (long)Bit128.Decimal;
                    if (value == Bit128.Decimal) return value;
                    throw new InvalidCastException($"{Bit128.Decimal} 转换 long 失败");
                case DataType.Double:
                    value = (long)Bit128.Double;
                    if (value == Bit128.Double) return value;
                    throw new InvalidCastException($"{Bit128.Double} 转换 long 失败");
                case DataType.Float:
                    value = (long)Bit128.Float;
                    if (value == Bit128.Float) return value;
                    throw new InvalidCastException($"{Bit128.Float} 转换 long 失败");
                case DataType.String:
                    if (long.TryParse(String, out value)) return value;
                    throw new InvalidCastException($"{String} 转换 long 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 long 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private short readShort()
        {
            switch (DataType)
            {
                case DataType.Long:
                    short value = (short)Bit128.Long;
                    if (value == Bit128.Long) return value;
                    throw new InvalidCastException($"{Bit128.Long} 转换 short 失败");
                case DataType.Int:
                    value = (short)Bit128.Int;
                    if (value == Bit128.Int) return value;
                    throw new InvalidCastException($"{Bit128.Int} 转换 short 失败");
                case DataType.Short: return Bit128.Short;
                case DataType.Byte: return Bit128.Byte;
                case DataType.Decimal:
                    value = (short)Bit128.Decimal;
                    if (value == Bit128.Decimal) return value;
                    throw new InvalidCastException($"{Bit128.Decimal} 转换 short 失败");
                case DataType.Double:
                    value = (short)Bit128.Double;
                    if (value == Bit128.Double) return value;
                    throw new InvalidCastException($"{Bit128.Double} 转换 short 失败");
                case DataType.Float:
                    value = (short)Bit128.Float;
                    if (value == Bit128.Float) return value;
                    throw new InvalidCastException($"{Bit128.Float} 转换 short 失败");
                case DataType.String:
                    if (short.TryParse(String, out value)) return value;
                    throw new InvalidCastException($"{String} 转换 short 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 short 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private byte readByte()
        {
            switch (DataType)
            {
                case DataType.Long:
                    byte value = (byte)Bit128.Long;
                    if (value == Bit128.Long) return value;
                    throw new InvalidCastException($"{Bit128.Long} 转换 byte 失败");
                case DataType.Int:
                    value = (byte)Bit128.Int;
                    if (value == Bit128.Int) return value;
                    throw new InvalidCastException($"{Bit128.Int} 转换 byte 失败");
                case DataType.Short:
                    value = (byte)Bit128.Short;
                    if (value == Bit128.Short) return value;
                    throw new InvalidCastException($"{Bit128.Short} 转换 byte 失败");
                case DataType.Byte: return Bit128.Byte;
                case DataType.Decimal:
                    value = (byte)Bit128.Decimal;
                    if (value == Bit128.Decimal) return value;
                    throw new InvalidCastException($"{Bit128.Decimal} 转换 byte 失败");
                case DataType.Double:
                    value = (byte)Bit128.Double;
                    if (value == Bit128.Double) return value;
                    throw new InvalidCastException($"{Bit128.Double} 转换 byte 失败");
                case DataType.Float:
                    value = (byte)Bit128.Float;
                    if (value == Bit128.Float) return value;
                    throw new InvalidCastException($"{Bit128.Float} 转换 byte 失败");
                case DataType.String:
                    if (byte.TryParse(String, out value)) return value;
                    throw new InvalidCastException($"{String} 转换 byte 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 byte 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private bool readBool()
        {
            if (DataType == DataType.Bool) return Bit128.Bool;
            throw new InvalidCastException($"{DataType} 转换 bool 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private DateTime readDateTime()
        {
            if (DataType == DataType.DateTime) return Bit128.DateTime;
            else if (DataType == DataType.String)
            {
                DateTime value;
                if (DateTime.TryParse(String, out value)) return value;
                throw new InvalidCastException($"{String} 转换 DateTime 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 DateTime 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private DateTimeOffset readDateTimeOffset()
        {
            if (DataType == DataType.DateTime) return Bit128.DateTime;
            else if (DataType == DataType.String)
            {
                DateTimeOffset value;
                if (DateTimeOffset.TryParse(String, out value)) return value;
                throw new InvalidCastException($"{String} 转换 DateTimeOffset 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 DateTimeOffset 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private TimeSpan readTimeSpan()
        {
            if (DataType == DataType.TimeSpan) return Bit128.TimeSpan;
            else if (DataType == DataType.String)
            {
                TimeSpan value;
                if (TimeSpan.TryParse(String, out value)) return value;
                throw new InvalidCastException($"{String} 转换 TimeSpan 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 TimeSpan 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private Guid readGuid()
        {
            if (DataType == DataType.Guid) return Bit128.Guid;
            else if (DataType == DataType.String)
            {
                Guid value;
                if (Guid.TryParse(String, out value)) return value;
                throw new InvalidCastException($"{String} 转换 Guid 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 Guid 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private decimal readDecimal()
        {
            switch (DataType)
            {
                case DataType.Long: return Bit128.Long;
                case DataType.Int: return Bit128.Int;
                case DataType.Short: return Bit128.Short;
                case DataType.Byte: return Bit128.Byte;
                case DataType.Decimal: return Bit128.Decimal;
                case DataType.Double:
                    decimal value = (decimal)Bit128.Double;
                    if ((double)value == Bit128.Double && value == (decimal)Bit128.Double) return value;
                    throw new InvalidCastException($"{Bit128.Double} 转换 decimal 失败");
                case DataType.Float:
                    value = (decimal)Bit128.Float;
                    if ((float)value == Bit128.Float && value == (decimal)Bit128.Float) return value;
                    throw new InvalidCastException($"{Bit128.Float} 转换 decimal 失败");
                case DataType.String:
                    if (decimal.TryParse(String, out value)) return value;
                    throw new InvalidCastException($"{String} 转换 decimal 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 decimal 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private double readDouble()
        {
            switch (DataType)
            {
                case DataType.Long:
                    double value = Bit128.Long;
                    if (value == Bit128.Long) return value;
                    throw new InvalidCastException($"{Bit128.Long} 转换 double 失败");
                case DataType.Int: return Bit128.Int;
                case DataType.Short: return Bit128.Short;
                case DataType.Byte: return Bit128.Byte;
                case DataType.Decimal:
                    value = (double)Bit128.Decimal;
                    if ((decimal)value == Bit128.Decimal && value == (double)Bit128.Decimal) return value;
                    throw new InvalidCastException($"{Bit128.Decimal} 转换 double 失败");
                case DataType.Double: return Bit128.Double;
                case DataType.Float:return Bit128.Float;
                case DataType.String:
                    if (double.TryParse(String, out value)) return value;
                    throw new InvalidCastException($"{String} 转换 double 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 double 失败");
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private float readFloat()
        {
            switch (DataType)
            {
                case DataType.Long:
                    float value = Bit128.Long;
                    if (value == Bit128.Long) return value;
                    throw new InvalidCastException($"{Bit128.Long} 转换 float 失败");
                case DataType.Int:
                    value = Bit128.Int;
                    if (value == Bit128.Int) return value;
                    throw new InvalidCastException($"{Bit128.Int} 转换 float 失败");
                case DataType.Short: return Bit128.Short;
                case DataType.Byte: return Bit128.Byte;
                case DataType.Decimal:
                    value = (float)Bit128.Decimal;
                    if ((decimal)value == Bit128.Decimal && value == (float)Bit128.Decimal) return value;
                    throw new InvalidCastException($"{Bit128.Decimal} 转换 float 失败");
                case DataType.Double:
                    value = (float)Bit128.Double;
                    if (value == Bit128.Double) return value;
                    throw new InvalidCastException($"{Bit128.Double} 转换 float 失败");
                case DataType.Float: return Bit128.Float;
                case DataType.String:
                    if (float.TryParse(String, out value)) return value;
                    throw new InvalidCastException($"{String} 转换 float 失败");
            }
            throw new InvalidCastException($"{DataType} 转换 float 失败");
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal string ReadString()
        {
            if (isNull) return null;
            switch (DataType)
            {
                case DataType.String: return String;
                case DataType.Int: return Bit128.Int.toString();
                case DataType.DateTime: return Bit128.DateTime.toString();
                case DataType.TimeSpan: return Bit128.TimeSpan.toString();
                case DataType.Long: return Bit128.Long.toString();
                case DataType.Decimal: return Bit128.Decimal.ToString();
                case DataType.Guid: return Bit128.Guid.ToString();
                case DataType.Byte: return Bit128.Byte.toString();
                case DataType.Short: return Bit128.Short.toString();
                case DataType.Float: return Bit128.Float.ToString();
                case DataType.Double: return Bit128.Double.ToString();
            }
            throw new InvalidCastException($"{DataType} 转换 long 失败");
        }
    }
}
