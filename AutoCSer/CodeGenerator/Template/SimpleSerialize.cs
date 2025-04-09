using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class SimpleSerialize : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static void @SimpleSerializeMethodName(AutoCSer.Memory.UnmanagedStream stream, ref @CurrentType.FullName value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
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
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            public static byte* @SimpleDeserializeMethodName(byte* start, ref @CurrentType.FullName value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private byte* simpleDeserialize(byte* __start__, byte* __end__)
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
                if (__start__ == null || __start__ > __end__) return null;
                #endregion LOOP FixedFields
                #region IF FixedFillSize
                __start__ += @FixedFillSize;
                #endregion IF FixedFillSize
                #region LOOP FieldArray
                #region IF IsString
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.@FieldName, __end__);
                #endregion IF IsString
                #region NOT IsString
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.@FieldName);
                #endregion NOT IsString
                if (__start__ == null || __start__ > __end__) return null;
                #endregion LOOP FieldArray
                return __start__;
            }
            #region NOTE
            private const int PrepSize = 0;
            private const int FixedFillSize = 0;
            private string FieldName;
            #endregion NOTE
            #endregion PART CLASS
        }
    }
    #region NOTE
    /// <summary>
    /// CSharp 模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        public unsafe partial class FullName
        {
            public static implicit operator int(FullName value) { return 0; }
            public static implicit operator FullName(int value) { return null; }
            public static implicit operator string(FullName value) { return null; }
            public void simpleSerialize(object value) { }
            public byte* simpleDeserialize(params byte*[] values) { return null; }
        }
        public partial class UnderlyingType : Pub
        {
        }
        public partial class MemberType : Pub
        {
        }
    }
    #endregion NOTE
}
