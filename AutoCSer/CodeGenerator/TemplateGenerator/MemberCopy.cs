using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 成员复制
    /// </summary>
    [Generator(Name = "成员复制", IsAuto = true)]
    internal partial class MemberCopy : AttributeGenerator<AutoCSer.CodeGenerator.MemberCopyAttribute>
    {
        /// <summary>
        /// 成员信息
        /// </summary>
        public sealed class CopyMember
        {
            /// <summary>
            /// 成员名称
            /// </summary>
            public readonly string MemberName;
            /// <summary>
            /// 成员编号
            /// </summary>
            public readonly int MemberIndex;
            /// <summary>
            /// 成员信息
            /// </summary>
            /// <param name="field"></param>
            public CopyMember(AutoCSer.Metadata.FieldIndex field)
            {
                MemberIndex = field.MemberIndex;
                MemberName = field.AnonymousName;
            }
        }

        /// <summary>
        /// 成员复制方法名称
        /// </summary>
        public string MemberCopyMethodName { get { return MemberCopyAttribute.MemberCopyMethodName; } }
        /// <summary>
        /// 成员复制方法名称
        /// </summary>
        public string MemberMapCopyMethodName { get { return MemberCopyAttribute.MemberMapCopyMethodName; } }
        /// <summary>
        /// 固定序列化成员
        /// </summary>
        public CopyMember[] Fields;

        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            LeftArray<AutoCSer.Metadata.FieldIndex> fields = AutoCSer.Metadata.MemberIndexGroup.GetAnonymousFields(type);
            Fields = fields.getArray(p => new CopyMember(p));
            create(true);
            AotMethod.Append(CurrentType, MemberCopyMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
