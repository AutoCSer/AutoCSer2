using System;

namespace RedisPerformance.Data
{
    /// <summary>
    /// 测试数据 https://redis.io/docs/latest/integrate/redisom-for-net/
    /// </summary>
    public sealed class Address
    {
        public int StreetNumber;

        public string? Unit;

        public string? StreetName;

        public string? City;

        public string? State;

        public string? PostalCode;

        public string? Country;

        public Address Clone()
        {
           return (Address)MemberwiseClone();
        }
    }
}
