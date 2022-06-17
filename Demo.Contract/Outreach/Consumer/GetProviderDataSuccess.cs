using System;
using MassTransit;

namespace QES.Demo.Contract.Outreach.Consumer
{
    public interface GetProviderDataSuccess : CorrelatedBy<Guid>
    {
        string PhoneNumber { get; set; }
        string EmailAddress { get; set; }
    }
}
