using AutoCSer.BinarySerialize;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    [Generator(Name = "简单序列化", IsAuto = true, IsAOT = true)]
    internal partial class SimpleSerialize : AttributeGenerator<AutoCSer.CodeGenerator.SimpleSerializeAttribute>
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        public sealed class SerializeField
        {
            /// <summary>
            /// 成员字段
            /// </summary>
            private AutoCSer.CodeGenerator.Metadata.FieldIndex field;
            /// <summary>
            /// 是否枚举类型
            /// </summary>
            public bool IsEnum
            {
                get { return field.Field.FieldType.IsEnum; }
            }
            /// <summary>
            /// 枚举数字类型
            /// </summary>
            public ExtensionType UnderlyingType
            {
                get { return System.Enum.GetUnderlyingType(field.Field.FieldType); }
            }
            /// <summary>
            /// 字段名称
            /// </summary>
            public string FieldName { get { return field.FieldName; } }
            /// <summary>
            /// 字段类型
            /// </summary>
            public ExtensionType MemberType { get { return field.Field.FieldType; } }
            /// <summary>
            /// 字段信息
            /// </summary>
            /// <param name="field"></param>
            public SerializeField(FieldSize field)
            {
                this.field = new AutoCSer.CodeGenerator.Metadata.FieldIndex(field.FieldIndex);
            }
            /// <summary>
            /// 是否字符串类型
            /// </summary>
            public bool IsString { get { return field.Field.FieldType == typeof(string); } }
        }

        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string SimpleSerializeMethodName { get { return AutoCSer.SimpleSerialize.Serializer.SimpleSerializeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string SimpleDeserializeMethodName { get { return AutoCSer.SimpleSerialize.Deserializer.SimpleDeserializeMethodName; } }
        /// <summary>
        /// 预申请字节数
        /// </summary>
        public int PrepSize;
        /// <summary>
        /// 固定字段对齐补全字节数
        /// </summary>
        public int FixedFillSize;
        /// <summary>
        /// 固定字段集合
        /// </summary>
        public SerializeField[] FixedFields;
        /// <summary>
        /// 非固定字段集合
        /// </summary>
        public SerializeField[] FieldArray;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            int memberCountVerify;
            FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetFields(CurrentType.Type, MemberFiltersEnum.PublicInstanceField), false, out memberCountVerify);
            foreach (FieldSize field in fields.FixedFields.Concat(fields.FieldArray))
            {
                if (!AutoCSer.SimpleSerialize.Serializer.IsType(field.Field.FieldType)) return AutoCSer.Common.CompletedTask;
            }
            PrepSize = (fields.FixedSize + fields.FieldArray.Length * sizeof(int) + 3) & (int.MaxValue - 3);
            FixedFillSize = -fields.FixedSize & 3;
            FixedFields = fields.FixedFields.getArray(p => new SerializeField(p));
            FieldArray = fields.FieldArray.getArray(p => new SerializeField(p));
            create(true);
            AotMethod.Append(CurrentType, SimpleSerializeMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
