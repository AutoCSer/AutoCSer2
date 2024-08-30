using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 自定义全局配置
    /// </summary>
    public class CustomConfig : AutoCSer.TextSerialize.CustomConfig
    {
        /// <summary>
        /// 根据类型获取 XML 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.XmlSerializer，第二参数类型为具体数据类型，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual AutoCSer.TextSerialize.SerializeDelegate GetCustomSerializeDelegate(Type type)
        {
            AutoCSer.TextSerialize.SerializeDelegate serializeDelegate;
            if (getCustomSerializeDelegate(type, out serializeDelegate)) return serializeDelegate;
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return new AutoCSer.TextSerialize.SerializeDelegate(CustomSerializeGenericType.Get(type).SerializeDelegate, getCustomSerializeReferenceTypes<XmlSerializeAttribute>(type));
            }
            return default(AutoCSer.TextSerialize.SerializeDelegate);
        }

        /// <summary>
        /// 写入时间值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(XmlSerializer serializer, DateTime value)
        {
            serializer.SerializeDateTime(value);
            return 0;
        }
        /// <summary>
        /// 写入时间值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(XmlSerializer serializer, TimeSpan value)
        {
            serializer.SerializeTimeSpan(value);
            return 0;
        }
        /// <summary>
        /// 自定义序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int NotSupport<T>(XmlSerializer serializer, T value)
        {
            return 0;
        }

        /// <summary>
        /// 根据类型获取 XML 自定义类型(比如泛型)序列化函数，必须是静态方法，第一个参数类型为 AutoCSer.XmlDeserializer，第二参数类型为具体数据类型 ref，返回值类型为 void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual Delegate GeteCustomDeserializDelegate(Type type)
        {
            Delegate deserializDelegate;
            if (geteCustomDeserializDelegate(type, out deserializDelegate)) return deserializDelegate;

            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return CustomSerializeGenericType.Get(type).DeserializeDelegate;
            }
            return null;
        }
        /// <summary>
        /// 自定义反序列化浮点数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref float value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && float.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化浮点数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref double value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && double.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化小数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref decimal value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && decimal.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref DateTime value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && DateTime.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Deserialize(XmlDeserializer deserializer, AutoCSer.Memory.Pointer buffer, ref TimeSpan value)
        {
            var stringBuffer = deserializer.GetStringBuffer(ref buffer);
            return stringBuffer.Length != 0 && TimeSpan.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool NotSupport<T>(XmlDeserializer deserializer, ref T value)
        {
            value = default(T);
            deserializer.IgnoreValue();
            return true;
        }
    }
}
