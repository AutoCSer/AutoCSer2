using System;
/*decimal,NotNumber;DateTime,NotDateTime;TimeSpan,NotTimeSpan*/

namespace AutoCSer
{
    /// <summary>
    /// XML 反序列化
    /// </summary>
    public sealed unsafe partial class XmlDeserializer
    {
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        public void XmlDeserialize(ref decimal value)
        {
            getValue();
            if (State == AutoCSer.Xml.DeserializeStateEnum.Success)
            {
                if (valueSize != 0)
                {
                    if (XmlSerializer.CustomConfig.Deserialize(this, new AutoCSer.Memory.Pointer(valueStart, valueSize << 1), ref value))
                    {
                        getValueEnd();
                        return;
                    }
                }
                State = AutoCSer.Xml.DeserializeStateEnum.NotNumber;
            }
        }
    }
}
