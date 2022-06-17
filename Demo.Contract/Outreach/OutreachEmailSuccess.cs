using System;
using MassTransit;

namespace QES.Demo.Contract.Outreach
{
    public interface OutreachEmailSuccess : CorrelatedBy<Guid>
    {
        string AttestationResult { get; set; }
    }
}
