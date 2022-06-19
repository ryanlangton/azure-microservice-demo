using System;
using MassTransit;

namespace Demo.Contract.Outreach
{
    public interface BeginOutreach : CorrelatedBy<Guid>
    {
        int ProviderId { get; set; }
    }
}
