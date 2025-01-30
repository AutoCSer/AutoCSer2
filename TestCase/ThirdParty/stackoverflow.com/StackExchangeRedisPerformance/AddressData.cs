using System;

namespace StackExchangeRedisPerformance
{
    /// <summary>
    /// Test data https://redis.io/docs/latest/integrate/redisom-for-net/
    /// </summary>
    public sealed class AddressData
    {
        public int StreetNumber { get; set; }

        public string? Unit { get; set; }

        public string? StreetName { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public AddressData Clone()
        {
            return (AddressData)MemberwiseClone();
        }
    }
}
