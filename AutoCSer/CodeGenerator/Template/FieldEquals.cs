using System;

namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class FieldEquals : Pub
    {
        public unsafe partial class TypeName
        {
            #region PART CLASS
            /// <summary>
            /// Object comparison
            /// 对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            internal static bool @FieldEqualsMethodName(@CurrentType.FullName left, @CurrentType.FullName right)
            {
                return left.fieldEquals(right)/*IF:BaseType*/ && AutoCSer.FieldEquals.Comparor.CallEquals<@BaseType.FullName>(left, right)/*IF:BaseType*/;
            }
            /// <summary>
            /// Object comparison
            /// 对象对比
            /// </summary>
            /// <param name="__value__"></param>
            private bool fieldEquals(@CurrentType.FullName __value__)
            {
                #region LOOP Fields
                if(!AutoCSer.FieldEquals.Comparor.@EqualsMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(@MemberName, __value__.@MemberName)) return false;
                #endregion LOOP Fields
                return true;
            }
            /// <summary>
            /// Object comparison based on member bitmaps
            /// 基于成员位图的对象对比
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="memberMap"></param>
            /// <returns></returns>
            internal static bool @MemberMapFieldEqualsMethodName(@CurrentType.FullName left, @CurrentType.FullName right, AutoCSer.Metadata.MemberMap<@CurrentType.FullName> memberMap)
            {
                return left.fieldEquals(right, memberMap);
            }
            /// <summary>
            /// Object comparison based on member bitmaps
            /// 基于成员位图的对象对比
            /// </summary>
            /// <param name="__value__"></param>
            /// <param name="__memberMap__"></param>
            private bool fieldEquals(@CurrentType.FullName __value__, AutoCSer.Metadata.MemberMap<@CurrentType.FullName> __memberMap__)
            {
                #region LOOP MemberMapFields
                if (__memberMap__.IsMember(@MemberIndex) && !AutoCSer.FieldEquals.Comparor.@EqualsMethodName/*IF:GenericTypeName*/<@GenericTypeName>/*IF:GenericTypeName*/(@MemberName, __value__.@MemberName)) return false;
                #endregion LOOP MemberMapFields
                return true;
            }
            /// <summary>
            /// AOT code generation call activation reflection
            /// AOT 代码生成调用激活反射
            /// </summary>
            internal static void @FieldEqualsMethodName()
            {
                @CurrentType.FullName left = default(@CurrentType.FullName), right = default(@CurrentType.FullName);
                @FieldEqualsMethodName(left, right);
                @MemberMapFieldEqualsMethodName(left, right, null);
                AutoCSer.FieldEquals.Comparor.CallEquals<@CurrentType.FullName>(left, right);
            }
            #endregion PART CLASS
            private string MemberName = null;
            private AutoCSer.Metadata.MemberMapIndex<CurrentType.FullName> MemberIndex = default(AutoCSer.Metadata.MemberMapIndex<CurrentType.FullName>);
        }
    }
}
