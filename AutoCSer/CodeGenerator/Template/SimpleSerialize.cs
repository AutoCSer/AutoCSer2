﻿using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class SimpleSerialize : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            #region IF IsSerialize
            /// <summary>
            /// Simple binary serialization
            /// 简单二进制序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void @SimpleSerializeMethodName(AutoCSer.Memory.UnmanagedStream stream, ref @TypeFullName value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// Simple binary serialization
            /// 简单二进制序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(@PrepSize))
                {
                    #region LOOP FixedFields
                    #region IF IsEnum
                    __stream__.Write((@UnderlyingType.FullName)/*NOTE*/(object)/*NOTE*/this.@FieldName);
                    #endregion IF IsEnum
                    #region NOT IsEnum
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.@FieldName);
                    #endregion NOT IsEnum
                    #endregion LOOP FixedFields
                    #region IF FixedFillSize
                    __stream__.TryMoveSize(@FixedFillSize);
                    #endregion IF FixedFillSize
                    #region LOOP FieldArray
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.@FieldName);
                    #endregion LOOP FieldArray
                }
            }
            #endregion IF IsSerialize
            #region IF IsDeserialize
            /// <summary>
            /// Simple binary deserialization
            /// 简单二进制反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* @SimpleDeserializeMethodName(byte* start, ref @TypeFullName value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// Simple binary deserialization
            /// 简单二进制反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                #region LOOP FixedFields
                #region IF IsEnum
                @UnderlyingType.FullName @FieldName = 0;
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref @FieldName);
                this.@FieldName = (@MemberType.FullName)/*NOTE*/(object)/*NOTE*/@FieldName;
                #endregion IF IsEnum
                #region NOT IsEnum
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.@FieldName);
                #endregion NOT IsEnum
                #endregion LOOP FixedFields
                #region IF FixedFillSize
                __start__ += @FixedFillSize;
                #endregion IF FixedFillSize
                #region IF FixedFields.Length
                if (__start__ == null || __start__ > __end__) return null;
                #endregion IF FixedFields.Length
                #region LOOP FieldArray
                #region IF IsCheckEnd
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.@FieldName, __end__);
                #endregion IF IsCheckEnd
                #region NOT IsCheckEnd
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.@FieldName);
                #endregion NOT IsCheckEnd
                if (__start__ == null || __start__ > __end__) return null;
                #endregion LOOP FieldArray
                return __start__;
            }
            #endregion IF IsDeserialize
            /// <summary>
            /// AOT code generation call activation reflection
            /// AOT 代码生成调用激活反射
            /// </summary>
            internal unsafe static void @SimpleSerializeMethodName()
            {
                @TypeFullName value = default(@TypeFullName);
                #region IF IsSerialize
                @SimpleSerializeMethodName(null, ref value);
                #endregion IF IsSerialize
                #region IF IsDeserialize
                @SimpleDeserializeMethodName(null, ref value, null);
                #endregion IF IsDeserialize
                AutoCSer.AotReflection.NonPublicMethods(typeof(@TypeFullName));
            }
            #endregion PART CLASS
            private const int PrepSize = 0;
            private const int FixedFillSize = 0;
            private string FieldName;
        }
    }
}
