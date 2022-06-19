using System;
using MassTransit;

namespace Demo.Contract.Outreach
{
    public interface OutreachPhoneCallSuccess : CorrelatedBy<Guid>
    {
        string AttestationResult { get; set; }
    }
}
