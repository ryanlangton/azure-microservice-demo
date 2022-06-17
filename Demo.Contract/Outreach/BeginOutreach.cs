using System;
using MassTransit;

namespace QES.Demo.Contract.Outreach
{
    public interface BeginOutreach : CorrelatedBy<Guid>
    {
        int ProviderId { get; set; }
    }
}
