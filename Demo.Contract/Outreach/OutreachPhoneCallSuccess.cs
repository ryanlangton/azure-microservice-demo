using System;
using MassTransit;

namespace QES.Demo.Contract.Outreach
{
    public interface OutreachPhoneCallSuccess : CorrelatedBy<Guid>
    {
        string AttestationResult { get; set; }
    }
}
