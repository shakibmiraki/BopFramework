using System;


namespace Bop.Core.Domain.Customers
{
    public class CustomerToken : BaseEntity
    {

        public string AccessTokenHash { get; set; }

        public DateTime AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string RefreshTokenIdHashSource { get; set; }

        public DateTime RefreshTokenExpiresDateTime { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
