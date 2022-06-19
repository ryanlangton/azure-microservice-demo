using System;
using MassTransit;

namespace Demo.Contract.Outreach
{
    public interface UpdateContactInformation : CorrelatedBy<Guid>
    {
        int ProviderId { get; set; }
        string PhoneNumber { get; set; }
        string EmailAddress { get; set; }
    }
}
