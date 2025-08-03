using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 配置加载缓存
    /// </summary>
    internal static class Cache
    {
        /// <summary>
        /// 获取配置创建
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static Creator? GetCreator(Type type, string name)
#else
        internal static Creator GetCreator(Type type, string name)
#endif
        {
            var creator = default(Creator);
            return cache.TryGetValue(new HashKey<HashObject<System.Type>, string>(type, name), out creator) ? creator : null;
        }
        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置缓存名称</param>
        /// <returns>配置项数据</returns>
#if NetStandard21
        internal static ConfigObject? Get(Type type, string name)
#else
        internal static ConfigObject Get(Type type, string name)
#endif
        {
            var creator = default(Creator);
            return cache.TryGetValue(new HashKey<HashObject<System.Type>, string>(type, name), out creator) ? creator.Create() : null;
        }

        /// <summary>
        /// 配置集合 [类型+名称]
        /// </summary>
        private static readonly Dictionary<HashKey<HashObject<System.Type>, string>, Creator> cache = DictionaryCreator<HashKey<HashObject<System.Type>, string>>.Create<Creator>();
        /// <summary>
        /// 添加配置缓存
        /// </summary>
        /// <param name="type"></param>
        private static void append(Type type)
        {
            foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attribute = field.GetCustomAttribute(typeof(MemberAttribute), false);
                if (attribute != null)
                {
                    var objectType = ConfigObject.GetConfigObjectType(field.FieldType);
                    HashKey<HashObject<System.Type>, string> key = new HashKey<HashObject<System.Type>, string>(objectType ?? field.FieldType, ((MemberAttribute)attribute).GetCacheName(field.Name));
                    if (!cache.ContainsKey(key))
                    {
                        if (objectType == null) cache.Add(key, new FieldObjectCreator(field));
                        else cache.Add(key, new FieldCreator(field));
                    }
                }
            }
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (property.CanRead)
                {
                    var method = property.GetGetMethod(true);
                    if (method?.GetParameters().Length == 0)
                    {
                        var attribute = property.GetCustomAttribute(typeof(MemberAttribute), false);
                        if (attribute != null)
                        {
                            var objectType = ConfigObject.GetConfigObjectType(property.PropertyType);
                            HashKey<HashObject<System.Type>, string> key = new HashKey<HashObject<System.Type>, string>(objectType ?? property.PropertyType, ((MemberAttribute)attribute).GetCacheName(property.Name));
                            if (!cache.ContainsKey(key))
                            {
                                if (objectType == null) cache.Add(key, new PropertyObjectCreator(method));
                                else cache.Add(key, new PropertyCreator(method));
                            }
                        }
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
                        var attribute = method.GetCustomAttribute(typeof(MemberAttribute), false);
                        if (attribute != null)
                        {
                            returnType = returnType.GetGenericArguments()[0];
                            var objectType = ConfigObject.GetConfigObjectType(returnType);
                            HashKey<HashObject<System.Type>, string> key = new HashKey<HashObject<System.Type>, string>(objectType ?? returnType, ((MemberAttribute)attribute).GetCacheName(method.Name));
                            if (!cache.ContainsKey(key))
                            {
                                if (objectType == null) cache.Add(key, new MethodObjectCreator(method, returnType));
                                else cache.Add(key, new MethodCreator(method, returnType));
                            }
                        }
                    }
                }
            }
        }
        static Cache()
        {
            LeftArray<KeyValue<Type, Exception>> exceptionTypeArray = new LeftArray<KeyValue<Type, Exception>>(0);
            IRoot root = Common.Root;
            if (object.ReferenceEquals(root, Root.Null))
            {
                foreach (Type type in Assembly.GetEntryAssembly()?.GetTypes() ?? EmptyArray<Type>.Array)
                {
#if AOT
                    if (!type.IsGenericType && !type.IsAbstract && !type.IsInterface && type.IsClass && type.Name.EndsWith("Config", StringComparison.Ordinal) && typeof(IRoot).IsAssignableFrom(type))
#else
                    if (!type.IsGenericType && !type.IsAbstract && !type.IsInterface && type.IsClass && typeof(IRoot).IsAssignableFrom(type))
#endif
                    {
                        try
                        {
                            var value = Activator.CreateInstance(type);
                            if (value != null)
                            {
                                root = (IRoot)value;
                                break;
                            }
                        }
                        catch (Exception exception)
                        {
                            //Console.WriteLine(exception.ToString());
                            exceptionTypeArray.Add(new KeyValue<Type, Exception>(type, exception));
                        }
                    }
                }
            }
            if (!object.ReferenceEquals(root, Root.Null))
            {
                append(root.GetType());
                foreach (Type type in root.MainTypes) append(type);
                foreach (Type type in root.PublicTypes) append(type);
            }
            if (exceptionTypeArray.Length != 0) Root.OnLoadException(root, exceptionTypeArray).AutoCSerNotWait();
        }
    }
}
