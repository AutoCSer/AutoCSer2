using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance.Data
{
    /// <summary>
    /// 测试数据 https://redis.io/docs/latest/integrate/redisom-for-net/
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class Address
    {
        public int StreetNumber;

        public string Unit;

        public string StreetName;

        public string City;

        public string State;

        public string PostalCode;

        public string Country;

        public Address Clone()
        {
            return (Address)MemberwiseClone();
        }
    }
}
