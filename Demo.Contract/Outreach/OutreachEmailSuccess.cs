using System;
using MassTransit;

namespace Demo.Contract.Outreach
{
    public interface OutreachEmailSuccess : CorrelatedBy<Guid>
    {
        string AttestationResult { get; set; }
    }
}
