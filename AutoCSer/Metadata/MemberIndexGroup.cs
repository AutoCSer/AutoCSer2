using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员索引分组
    /// </summary>
    internal sealed class MemberIndexGroup
    {
        /// <summary>
        /// 公有字段
        /// </summary>
        internal readonly FieldIndex[] PublicFields;
        /// <summary>
        /// 非公有字段
        /// </summary>
        internal readonly FieldIndex[] NonPublicFields;
        /// <summary>
        /// 字段成员数量
        /// </summary>
        internal int FieldCount { get { return PublicFields.Length + NonPublicFields.Length; } }
        /// <summary>
        /// 公有属性
        /// </summary>
        internal readonly PropertyIndex[] PublicProperties;
        /// <summary>
        /// 非公有属性
        /// </summary>
        internal readonly PropertyIndex[] NonPublicProperties;
        ///// <summary>
        ///// 匿名字段
        ///// </summary>
        //internal readonly FieldIndex[] AnonymousFields;
        /// <summary>
        /// 所有成员数量
        /// </summary>
        internal readonly int MemberCount;
        /// <summary>
        /// 成员索引分组
        /// </summary>
        /// <param name="type"></param>
        private MemberIndexGroup(Type type)
        {
            if (type == typeof(object))
            {
                PublicFields = NonPublicFields = EmptyArray<FieldIndex>.Array;//AnonymousFields = 
                PublicProperties = NonPublicProperties = EmptyArray<PropertyIndex>.Array;
                return;
            }
            FieldInfo[] publicFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] nonPublicFields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            PropertyInfo[] publicProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] nonPublicProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
            int memberIndex = 0, index;
            var anonymousName = default(string);
            var anonymousField = default(FieldInfo);
            if (type.IsValueType)
            {
                PublicFields = publicFields.Length != 0 ? new FieldIndex[publicFields.Length] : EmptyArray<FieldIndex>.Array;
                index = 0;
                foreach (FieldInfo field in publicFields.sort(fieldCompare)) PublicFields[index++] = new FieldIndex(field, MemberFiltersEnum.PublicInstanceField, memberIndex++);
                LeftArray<FieldIndex> nonPublicFieldIndexs = new LeftArray<FieldIndex>(nonPublicFields.Length);
                var anonymousFields = default(Dictionary<string, FieldInfo>);
                foreach (FieldInfo field in nonPublicFields.sort(fieldCompare))
                {
                    if (field.getAnonymousName(out anonymousName))
                    {
                        if (anonymousFields == null) anonymousFields = DictionaryCreator<string>.Create<FieldInfo>();
                        anonymousFields.Add(anonymousName, field);
                    }
                    else nonPublicFieldIndexs.Add(new FieldIndex(field, MemberFiltersEnum.NonPublicInstanceField, memberIndex++));
                }
                NonPublicFields = nonPublicFieldIndexs.ToArray();
                PublicProperties = publicProperties.Length != 0 ? new PropertyIndex[publicProperties.Length] : EmptyArray<PropertyIndex>.Array;
                index = 0;
                foreach (PropertyInfo property in publicProperties.sort(propertyCompare))
                {
                    anonymousFields?.Remove(property.Name, out anonymousField);
                    PublicProperties[index++] = new PropertyIndex(property, MemberFiltersEnum.PublicInstanceProperty, memberIndex++, anonymousField);
                }
                NonPublicProperties = nonPublicProperties.Length != 0 ? new PropertyIndex[nonPublicProperties.Length] : EmptyArray<PropertyIndex>.Array;
                index = 0;
                foreach (PropertyInfo property in nonPublicProperties.sort(propertyCompare))
                {
                    anonymousFields?.Remove(property.Name, out anonymousField);
                    NonPublicProperties[index++] = new PropertyIndex(property, MemberFiltersEnum.NonPublicInstanceProperty, memberIndex++, anonymousField);
                }
                //if (anonymousFields == null || anonymousFields.Count == 0) AnonymousFields = EmptyArray<FieldIndex>.Array;
                //else
                //{
                //    AnonymousFields = new FieldIndex[anonymousFields.Count];
                //    index = 0;
                //    foreach (KeyValuePair<string, FieldInfo> field in anonymousFields) AnonymousFields[index++] = new FieldIndex(field.Value, MemberFiltersEnum.NonPublicInstanceField, memberIndex++, field.Key);
                //}
            }
            else
            {
                var group = default(MemberGroup);
                Dictionary<HashObject<Type>, MemberGroup> groups = DictionaryCreator.CreateHashObject<Type, MemberGroup>();
                foreach (FieldInfo field in publicFields)
                {
                    Type declaringType = field.DeclaringType.notNull().getGenericTypeDefinition();
                    if (!groups.TryGetValue(declaringType, out group)) groups.Add(declaringType, group = new MemberGroup());
                    group.PublicFields.Add(field);
                }
                LeftArray<KeyValue<string, FieldInfo>> anonymousFields = new LeftArray<KeyValue<string, FieldInfo>>(0);
                foreach (FieldInfo field in nonPublicFields)
                {
                    if (field.getAnonymousName(out anonymousName)) anonymousFields.Add(new KeyValue<string, FieldInfo>(anonymousName, field));
                    else
                    {
                        Type declaringType = field.DeclaringType.notNull().getGenericTypeDefinition();
                        if (!groups.TryGetValue(declaringType, out group)) groups.Add(declaringType, group = new MemberGroup());
                        group.NonPublicFields.Add(field);
                    }
                }
                foreach (PropertyInfo property in publicProperties.sort(propertyCompare))
                {
                    Type declaringType = property.DeclaringType.notNull().getGenericTypeDefinition();
                    if (!groups.TryGetValue(declaringType, out group)) groups.Add(declaringType, group = new MemberGroup());
                    group.PublicProperties.Add(property);
                }
                foreach (PropertyInfo property in nonPublicProperties)
                {
                    Type declaringType = property.DeclaringType.notNull().getGenericTypeDefinition();
                    if (!groups.TryGetValue(declaringType, out group)) groups.Add(declaringType, group = new MemberGroup());
                    group.NonPublicProperties.Add(property);
                }
                HashSet<string> names = HashSetCreator<string>.Create();
                LeftArray<FieldInfo> publicFieldArray = new LeftArray<FieldInfo>(0, publicFields), nonPublicFieldArray = new LeftArray<FieldInfo>(0, nonPublicFields);
                LeftArray<PropertyInfo> publicPropertyArray = new LeftArray<PropertyInfo>(0, publicProperties), nonPublicPropertyArray = new LeftArray<PropertyInfo>(0, nonPublicProperties);
                for (var baseType = type; baseType != null && baseType != typeof(object); baseType = baseType.BaseType)
                {
                    var declaringType = baseType.getGenericTypeDefinition();
                    if (groups.TryGetValue(declaringType, out group))
                    {
                        foreach (FieldInfo field in group.PublicFields)
                        {
                            if (names.Add(field.Name)) publicFieldArray.Add(field);
                        }
                        foreach (FieldInfo field in group.NonPublicFields)
                        {
                            if (names.Add(field.Name)) nonPublicFieldArray.Add(field);
                        }
                        foreach (PropertyInfo property in group.PublicProperties)
                        {
                            if (names.Add(property.Name)) publicPropertyArray.Add(property);
                        }
                        foreach (PropertyInfo property in group.NonPublicProperties)
                        {
                            if (names.Add(property.Name)) nonPublicPropertyArray.Add(property);
                        }
                    }
                }
                PublicFields = publicFieldArray.Length != 0 ? new FieldIndex[publicFieldArray.Length] : EmptyArray<FieldIndex>.Array;
                index = 0;
                publicFieldArray.Sort(fieldCompare);
                foreach (FieldInfo field in publicFieldArray) PublicFields[index++] = new FieldIndex(field, MemberFiltersEnum.PublicInstanceField, memberIndex++);
                NonPublicFields = nonPublicFieldArray.Length != 0 ? new FieldIndex[nonPublicFieldArray.Length] : EmptyArray<FieldIndex>.Array;
                index = 0;
                nonPublicFieldArray.Sort(fieldCompare);
                foreach (FieldInfo field in nonPublicFieldArray) NonPublicFields[index++] = new FieldIndex(field, MemberFiltersEnum.NonPublicInstanceField, memberIndex++);
                PublicProperties = publicPropertyArray.Length != 0 ? new PropertyIndex[publicPropertyArray.Length] : EmptyArray<PropertyIndex>.Array;
                index = 0;
                publicPropertyArray.Sort(propertyCompare);
                int anonymousFieldIndex;
                foreach (PropertyInfo property in publicPropertyArray)
                {
                    Type declaringType = property.DeclaringType.notNull();
                    anonymousField = null;
                    if (declaringType == type)
                    {
                        anonymousFieldIndex = 0;
                        foreach (KeyValue<string, FieldInfo> anonymousFieldName in anonymousFields)
                        {
                            if (anonymousFieldName.Key == property.Name)
                            {
                                anonymousField = anonymousFieldName.Value;
                                anonymousFields.UnsafeRemoveAtToEnd(anonymousFieldIndex);
                                break;
                            }
                            ++anonymousFieldIndex;
                        }
                    }
                    else
                    {
                        foreach (KeyValue<string, FieldInfo> anonymousFieldName in getDeclaredOnlyAnonymousFields(declaringType))
                        {
                            if (anonymousFieldName.Key == property.Name)
                            {
                                anonymousField = anonymousFieldName.Value;
                                break;
                            }
                        }
                    }
                    PublicProperties[index++] = new PropertyIndex(property, MemberFiltersEnum.PublicInstanceProperty, memberIndex++, anonymousField);
                }
                NonPublicProperties = nonPublicPropertyArray.Length != 0 ? new PropertyIndex[nonPublicPropertyArray.Length] : EmptyArray<PropertyIndex>.Array;
                index = 0;
                nonPublicPropertyArray.Sort(propertyCompare);
                foreach (PropertyInfo property in nonPublicPropertyArray)
                {
                    Type declaringType = property.DeclaringType.notNull();
                    anonymousField = null;
                    if (declaringType == type)
                    {
                        anonymousFieldIndex = 0;
                        foreach (KeyValue<string, FieldInfo> anonymousFieldName in anonymousFields)
                        {
                            if (anonymousFieldName.Key == property.Name)
                            {
                                anonymousField = anonymousFieldName.Value;
                                anonymousFields.UnsafeRemoveAtToEnd(anonymousFieldIndex);
                                break;
                            }
                            ++anonymousFieldIndex;
                        }
                    }
                    else
                    {
                        foreach (KeyValue<string, FieldInfo> anonymousFieldName in getDeclaredOnlyAnonymousFields(declaringType))
                        {
                            if (anonymousFieldName.Key == property.Name)
                            {
                                anonymousField = anonymousFieldName.Value;
                                break;
                            }
                        }
                    }
                    NonPublicProperties[index++] = new PropertyIndex(property, MemberFiltersEnum.NonPublicInstanceProperty, memberIndex++, anonymousField);
                }
                //AnonymousFields = anonymousFields.Length != 0 ? new FieldIndex[anonymousFields.Length] : EmptyArray<FieldIndex>.Array;
                //foreach (KeyValue<string, FieldInfo> field in anonymousFields) AnonymousFields[index++] = new FieldIndex(field.Value, MemberFiltersEnum.NonPublicInstanceField, memberIndex++, field.Key);
            }
            MemberCount = memberIndex;
        }
        /// <summary>
        /// 成员集合
        /// </summary>
        internal MemberIndexInfo[] GetAllMembers()
        {
            LeftArray<MemberIndexInfo> members = new LeftArray<MemberIndexInfo>(MemberCount);
            members.Add(PublicFields.toGeneric<MemberIndexInfo>());
            members.Add(NonPublicFields.toGeneric<MemberIndexInfo>());
            members.Add(PublicProperties.toGeneric<MemberIndexInfo>());
            members.Add(NonPublicProperties.toGeneric<MemberIndexInfo>());
            //members.Add(AnonymousFields.toGeneric<MemberIndexInfo>());
            return members.ToArray();
        }
        /// <summary>
        /// 获取成员索引集合
        /// </summary>
        /// <param name="isValue">成员匹配委托</param>
        /// <returns>成员索引集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private IEnumerable<MemberIndexInfo> get(Func<MemberIndexInfo, bool> isValue)
        {
            return ((IEnumerable<MemberIndexInfo>)PublicFields).Concat(NonPublicFields).Concat(PublicProperties).Concat(NonPublicProperties).Where(isValue);
        }
        /// <summary>
        /// 根据类型获取成员信息集合
        /// </summary>
        /// <param name="filter">选择类型</param>
        /// <param name="isFilter">是否完全匹配选择类型</param>
        /// <returns>成员信息集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal IEnumerable<MemberIndexInfo> Find(MemberFiltersEnum filter, bool isFilter = false)
        {
            return get(value => isFilter ? (value.MemberFilters & filter) == filter : ((value.MemberFilters & filter) != 0));
        }
        /// <summary>
        /// 获取字段集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberFilter">成员选择类型</param>
        /// <returns></returns>
        internal static LeftArray<FieldIndex> GetFields(Type type, MemberFiltersEnum memberFilter = MemberFiltersEnum.InstanceField)
        {
            if ((memberFilter & MemberFiltersEnum.PublicInstanceField) == 0)
            {
                if ((memberFilter & MemberFiltersEnum.NonPublicInstanceField) == 0) return new LeftArray<FieldIndex>(0);
                return new LeftArray<FieldIndex>(GetGroup(type).NonPublicFields);
            }
            else if ((memberFilter & MemberFiltersEnum.NonPublicInstanceField) == 0) return new LeftArray<FieldIndex>(GetGroup(type).PublicFields);
            MemberIndexGroup group = GetGroup(type);
            LeftArray<FieldIndex> fields = new LeftArray<FieldIndex>(group.PublicFields.Length + group.NonPublicFields.Length);
            fields.Add(group.PublicFields);
            fields.Add(group.NonPublicFields);
            return fields;
        }
        /// <summary>
        /// 获取字段集合（包括匿名字段）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberFilter">成员选择类型</param>
        /// <returns></returns>
        internal static LeftArray<FieldIndex> GetAnonymousFields(Type type, MemberFiltersEnum memberFilter = MemberFiltersEnum.InstanceField)
        {
            if ((memberFilter & MemberFiltersEnum.NonPublicInstanceField) != 0)
            {
                MemberIndexGroup group = GetGroup(type);
                LeftArray<FieldIndex> fields = new LeftArray<FieldIndex>(0);
                if ((memberFilter & MemberFiltersEnum.PublicInstanceField) != 0) fields.Add(group.PublicFields);
                fields.Add(group.NonPublicFields);
                foreach (PropertyIndex property in group.PublicProperties)
                {
                    if (property.AnonymousField != null) fields.Add(property.AnonymousField);
                }
                foreach (PropertyIndex property in group.NonPublicProperties)
                {
                    if (property.AnonymousField != null) fields.Add(property.AnonymousField);
                }
                //fields.Add(group.AnonymousFields);
                return fields;
            }
            return GetFields(type, memberFilter);
        }
        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberFilter">成员选择类型</param>
        /// <returns></returns>
        internal static LeftArray<PropertyIndex> GetProperties(Type type, MemberFiltersEnum memberFilter = MemberFiltersEnum.InstanceProperty)
        {
            //if (type != typeof(System.Numerics.Matrix3x2) && type != typeof(System.Numerics.Matrix4x4) && type != typeof(System.Numerics.Quaternion))
            {
                if ((memberFilter & MemberFiltersEnum.PublicInstanceProperty) == 0)
                {
                    if ((memberFilter & MemberFiltersEnum.NonPublicInstanceProperty) == 0) return new LeftArray<PropertyIndex>(0);
                    return new LeftArray<PropertyIndex>(GetGroup(type).NonPublicProperties);
                }
                else if ((memberFilter & MemberFiltersEnum.NonPublicInstanceProperty) == 0) return new LeftArray<PropertyIndex>(GetGroup(type).PublicProperties);
                MemberIndexGroup group = GetGroup(type);
                LeftArray<PropertyIndex> properties = new LeftArray<PropertyIndex>(group.PublicProperties.Length + group.NonPublicProperties.Length);
                properties.Add(group.PublicProperties);
                properties.Add(group.NonPublicProperties);
                return properties;
            }
            //return new LeftArray<PropertyIndex>(0);
        }
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int Compare(FieldInfo left, FieldInfo right)
        {
            return string.CompareOrdinal(left.Name, right.Name);
        }
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        private static readonly Func<FieldInfo, FieldInfo, int> fieldCompare = Compare;
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(PropertyInfo left, PropertyInfo right)
        {
            return string.CompareOrdinal(left.Name, right.Name);
        }
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        private static readonly Func<PropertyInfo, PropertyInfo, int> propertyCompare = compare;
        /// <summary>
        /// 成员索引分组缓存
        /// </summary>
        private static Dictionary<HashObject<Type>, MemberIndexGroup> groups = DictionaryCreator.CreateHashObject<Type, MemberIndexGroup>();
        /// <summary>
        /// 成员索引分组缓存访问锁
        /// </summary>
        private static readonly object groupLock = new object();
        /// <summary>
        /// 获取成员索引分组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static MemberIndexGroup GetGroup(Type type)
        {
            var group = default(MemberIndexGroup);
            Monitor.Enter(groupLock);
            try
            {
                if (!groups.TryGetValue(type, out group)) groups.Add(type, group = new MemberIndexGroup(type));
            }
            finally { Monitor.Exit(groupLock); }
            return group;
        }
        /// <summary>
        /// 匿名字段集合缓存
        /// </summary>
        private static Dictionary<HashObject<Type>, KeyValue<string, FieldInfo>[]> declaredOnlyAnonymousFields = DictionaryCreator.CreateHashObject<Type, KeyValue<string, FieldInfo>[]>();
        /// <summary>
        /// 匿名字段集合缓存访问锁
        /// </summary>
        private static readonly object declaredOnlyAnonymousFieldLock = new object();
        /// <summary>
        /// 获取匿名字段集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static KeyValue<string, FieldInfo>[] getDeclaredOnlyAnonymousFields(Type type)
        {
            var anonymousName = default(string);
            var anonymousFields = default(KeyValue<string, FieldInfo>[]);
            Monitor.Enter(declaredOnlyAnonymousFieldLock);
            try
            {
                if (!declaredOnlyAnonymousFields.TryGetValue(type, out anonymousFields))
                {
                    LeftArray<KeyValue<string, FieldInfo>> anonymousFieldArray = new LeftArray<KeyValue<string, FieldInfo>>(0);
                    foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        if (field.getAnonymousName(out anonymousName)) anonymousFieldArray.Add(new KeyValue<string, FieldInfo>(anonymousName, field));
                    }
                    declaredOnlyAnonymousFields.Add(type, anonymousFields = anonymousFieldArray.ToArray());
                }
            }
            finally { Monitor.Exit(declaredOnlyAnonymousFieldLock); }
            return anonymousFields;
        }
        /// <summary>
        /// 清除缓存信息
        /// </summary>
        private static void clearCache()
        {
            if (declaredOnlyAnonymousFields.Count != 0)
            {
                Monitor.Enter(declaredOnlyAnonymousFieldLock);
                try
                {
                    declaredOnlyAnonymousFields = DictionaryCreator.CreateHashObject<Type, KeyValue<string, FieldInfo>[]>();
                }
                finally { Monitor.Exit(declaredOnlyAnonymousFieldLock); }
            }
            if (groups.Count != 0)
            {
                Monitor.Enter(groupLock);
                try
                {
                    groups = DictionaryCreator.CreateHashObject<Type, MemberIndexGroup>();
                }
                finally { Monitor.Exit(groupLock); }
            }
        }

        static MemberIndexGroup()
        {
            int clearSeconds = AutoCSer.Common.Config.GetMemoryCacheClearSeconds();
            if (clearSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(clearCache, clearSeconds, Threading.SecondTimerKeepModeEnum.After, clearSeconds);
        }
    }
}
