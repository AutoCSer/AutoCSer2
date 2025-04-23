using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 项目配置
    /// </summary>
    [Generator(Name = "项目配置", IsAuto = true)]
    internal partial class Configuration : AttributeGenerator<AutoCSer.CodeGenerator.ConfigurationAttribute>
    {
        /// <summary>
        /// 项目配置方法名称
        /// </summary>
        public string ConfigurationMethodName { get { return ConfigurationAttribute.ConfigurationMethodName; } }

        /// <summary>
        /// 创建配置对象类型集合
        /// </summary>
        public ExtensionType[] CreateTypes;
        /// <summary>
        /// 创建配置对象类型集合
        /// </summary>
        public ExtensionType[] CreateTaskTypes;
        /// <summary>
        /// 获取配置对象类型集合
        /// </summary>
        public ExtensionType[] GetTaskTypes;
        /// <summary>
        /// 安装下一个类型
        /// </summary>
        protected override Task nextCreate()
        {
            Type type = CurrentType.Type;
            if (type.IsAbstract || type.IsGenericTypeDefinition) return AutoCSer.Common.CompletedTask;
            HashSet<HashObject<Type>> createTypes = HashSetCreator.CreateHashObject<Type>(), createTaskTypes = HashSetCreator.CreateHashObject<Type>(), getTaskTypes = HashSetCreator.CreateHashObject<Type>();
            foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attribute = field.GetCustomAttribute(typeof(AutoCSer.Configuration.MemberAttribute), false);
                if (attribute != null && ConfigObject.GetConfigObjectType(field.FieldType) == null) createTypes.Add(field.FieldType);
            }
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (property.CanRead)
                {
                    var method = property.GetGetMethod(true);
                    if (method?.GetParameters().Length == 0)
                    {
                        var attribute = property.GetCustomAttribute(typeof(AutoCSer.Configuration.MemberAttribute), false);
                        if (attribute != null && ConfigObject.GetConfigObjectType(property.PropertyType) == null) createTypes.Add(property.PropertyType);
                    }
                }
            }
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!method.IsGenericMethodDefinition)
                {
                    Type returnType = method.ReturnType;
                    if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>) && method.GetParameters().Length == 0)
                    {
                        var attribute = method.GetCustomAttribute(typeof(AutoCSer.Configuration.MemberAttribute), false);
                        if (attribute != null)
                        {
                            returnType = returnType.GetGenericArguments()[0];
                            var objectType = ConfigObject.GetConfigObjectType(returnType);
                            if (objectType == null) createTaskTypes.Add(returnType);
                            else getTaskTypes.Add(objectType);
                        }
                    }
                }
            }
            if ((createTypes.Count | createTaskTypes.Count | getTaskTypes.Count) == 0) return AutoCSer.Common.CompletedTask;
            CreateTypes = createTypes.getArray(p => (ExtensionType)p.Value);
            CreateTaskTypes = createTaskTypes.getArray(p => (ExtensionType)p.Value);
            GetTaskTypes = getTaskTypes.getArray(p => (ExtensionType)p.Value);
            create(true);
            AotMethod.Append(CurrentType, ConfigurationMethodName);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 安装完成处理
        /// </summary>
        /// <returns></returns>
        protected override Task onCreated() { return AutoCSer.Common.CompletedTask; }
    }
}
