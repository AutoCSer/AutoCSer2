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
    [Generator(Name = "简单序列化", IsAuto = true)]
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
            /// 是否检查结束位置
            /// </summary>
            public bool IsCheckEnd
            {
                get
                {
                    Type type = field.Field.FieldType;
                    return type == typeof(string) || type == typeof(byte[]);
                }
            }
        }

        /// <summary>
        /// 序列化方法名称
        /// </summary>
        public string SimpleSerializeMethodName { get { return SimpleSerializeAttribute.SimpleSerializeMethodName; } }
        /// <summary>
        /// 反序列化方法名称
        /// </summary>
        public string SimpleDeserializeMethodName { get { return SimpleSerializeAttribute.SimpleDeserializeMethodName; } }
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
        /// 类型名称
        /// </summary>
        public string TypeFullName;
        /// <summary>
        /// 是否生成序列化代码
        /// </summary>
        public bool IsSerialize;
        /// <summary>
        /// 是否生成反序列化代码
        /// </summary>
        public bool IsDeserialize;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            TypeFullName = CurrentType.FullName;
            IsSerialize = CurrentAttribute.IsSerialize;
            IsDeserialize = CurrentAttribute.IsDeserialize;
            if (nextCreate(true)) AotMethod.Append(CurrentType, SimpleSerializeMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="isOutput"></param>
        /// <returns></returns>
        private bool nextCreate(bool isOutput)
        {
            int memberCountVerify;
            FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetFields(CurrentType.Type, MemberFiltersEnum.PublicInstanceField), false, out memberCountVerify);
            foreach (FieldSize field in fields.FixedFields.Concat(fields.FieldArray))
            {
                if (!AutoCSer.SimpleSerialize.Serializer.IsType(field.Field.FieldType)) return false;
            }
            PrepSize = (fields.FixedSize + fields.FieldArray.Length * sizeof(int) + 3) & (int.MaxValue - 3);
            FixedFillSize = -fields.FixedSize & 3;
            FixedFields = fields.FixedFields.getArray(p => new SerializeField(p));
            FieldArray = fields.FieldArray.getArray(p => new SerializeField(p));
            create(isOutput);
            return true;
        }
        /// <summary>
        /// 生成自定义类型代码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeFullName"></param>
        /// <param name="isSerialize"></param>
        /// <param name="isDeserialize"></param>
        /// <returns></returns>
        internal string Create(Type type, string typeFullName, bool isSerialize, bool isDeserialize)
        {
            CurrentType = type;
            TypeFullName = typeFullName;
            IsSerialize = isSerialize;
            IsDeserialize = isDeserialize;
            _code_.Array.Length = 0;
            nextCreate(false);
            AotMethod.Append($"{typeFullName}.{SimpleSerializeMethodName}");
            return string.Concat(_code_.Array.ToArray());
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
