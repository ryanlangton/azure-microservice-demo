using System;
using MassTransit;

namespace QES.Demo.Contract.Outreach
{
    public interface StartOutreachPhoneCall : CorrelatedBy<Guid>
    {
    }
}
