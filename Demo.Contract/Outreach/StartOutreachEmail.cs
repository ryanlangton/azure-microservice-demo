using System;
using MassTransit;

namespace QES.Demo.Contract.Outreach
{
    public interface StartOutreachEmail : CorrelatedBy<Guid>
    {
    }
}
