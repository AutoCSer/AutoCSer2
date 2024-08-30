using System;
using System.Text.RegularExpressions;

namespace AutoCSer.TestCase.CommonModel.TableModel.CustomColumn
{
    /// <summary>
    /// 电子邮箱（建议使用字段来描述持久化数据，同时也作为内部 RPC 服务接口传参）
    /// </summary>
    [AutoCSer.JsonSerialize(CustomReferenceTypes = new Type[0], DocumentType = typeof(string))]
    [AutoCSer.ORM.CustomColumn(NameConcatType = AutoCSer.ORM.CustomColumnNameConcatTypeEnum.Parent)]
    public struct Email : AutoCSer.Json.ICustomSerialize<Email>, AutoCSer.ORM.IVerify<Email>
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [AutoCSer.ORM.String(IsAscii = true, Size = 256)]
        [AutoCSer.RandomObject.Member(IsNullable = false)]
        private string email;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static implicit operator Email(string email)
        {
            if (!verifyRegex.IsMatch(email)) throw new InvalidCastException($"非法邮箱地址 {email}");
            return new Email { email = email };
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static implicit operator string(Email email) { return email.email; }

        /// <summary>
        /// 邮箱地址简单验证正则
        /// </summary>
        private static readonly Regex verifyRegex = new Regex(@"^[A-Z0-9a-z_\.]+@[A-Z0-9a-z_]+\.[A-Z0-9a-z]+$", RegexOptions.Compiled);
        /// <summary>
        /// 自定义数据验证，验证失败需要抛出异常
        /// </summary>
        /// <returns></returns>
        public Email Verify()
        {
            if (!verifyRegex.IsMatch(email)) throw new InvalidCastException($"非法邮箱地址 {email}");
            return this;
        }

        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.Json.ICustomSerialize<Email>.Serialize(JsonSerializer serializer)
        {
            serializer.CustomSerialize(email);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.Json.ICustomSerialize<Email>.Deserialize(JsonDeserializer deserializer)
        {
            if (deserializer.CustomDeserialize(ref email) && !verifyRegex.IsMatch(email)) deserializer.SetCustomError($"非法邮箱地址 {email}");
        }
    }
}
