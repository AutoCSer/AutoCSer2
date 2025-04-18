using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class MemberCopy : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name="writeValue"></param>
            /// <param name="readValue"></param>
            internal static void @MemberCopyMethodName(ref @CurrentType.FullName writeValue, @CurrentType.FullName readValue)
            {
                writeValue.memberCopyFrom(readValue);
            }
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name="__value__"></param>
            private void memberCopyFrom(@CurrentType.FullName __value__)
            {
                #region LOOP Fields
                @MemberName = __value__.@MemberName;
                #endregion LOOP Fields
            }
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name="writeValue"></param>
            /// <param name="readValue"></param>
            /// <param name="memberMap"></param>
            internal static void @MemberMapCopyMethodName(ref @CurrentType.FullName writeValue, @CurrentType.FullName readValue, AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap)
            {
                writeValue.memberCopyFrom(readValue, memberMap);
            }
            /// <summary>
            /// 成员复制
            /// </summary>
            /// <param name="readValue"></param>
            /// <param name="memberMap"></param>
            private void memberCopyFrom(@CurrentType.FullName __value__, AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__)
            {
                #region LOOP Fields
                if (__memberMap__.IsMember(@MemberIndex)) @MemberName = __value__.@MemberName;
                #endregion LOOP Fields
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void @MemberCopyMethodName()
            {
                @CurrentType.FullName writeValue = default(@CurrentType.FullName), readValue = default(@CurrentType.FullName);
                @MemberCopyMethodName(ref writeValue, readValue);
                @MemberMapCopyMethodName(ref writeValue, readValue, null);
                AutoCSer.MemberCopy<@CurrentType.FullName>.Copy(ref writeValue, readValue);
            }
            #endregion PART CLASS
            private string MemberName;
            private AutoCSer.Metadata.MemberMapIndex<CurrentType.FullName> MemberIndex = new AutoCSer.Metadata.MemberMapIndex<FullName>(null);
        }
    }
}
