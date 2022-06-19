using System;
using MassTransit;

namespace Demo.Contract.Outreach.Consumer
{
    public interface GetProviderData : CorrelatedBy<Guid>
    {
        int ProviderId { get; set; }
    }
}
