using System;
using MassTransit;

namespace Demo.Contract.Outreach
{
    public interface StartOutreachPhoneCall : CorrelatedBy<Guid>
    {
    }
}
