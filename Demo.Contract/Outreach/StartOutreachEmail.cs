using System;
using MassTransit;

namespace Demo.Contract.Outreach
{
    public interface StartOutreachEmail : CorrelatedBy<Guid>
    {
    }
}
